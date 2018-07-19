using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MTurk_Calc
{
    public static class DbUtility
    {
        /// <summary>
        /// The path to the database.
        /// </summary>
        private static String path = System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\App_Data\\test.db";

        /// <summary>
        /// The connection to the SQLite database to be used.
        /// </summary>
        private static SQLiteConnection conn = new SQLiteConnection("Data Source=" + path +";Version=3;");

        /// <summary>
        /// Creates tables if the tables do not already exist in the database. Tables are only created if this is a newly created database.
        /// </summary>
        public static void createTables()
        {
            String createRequesters = "CREATE TABLE IF NOT EXISTS requesters (requesterID integer PRIMARY KEY, name varchar(20) NOT NULL, UNIQUE(name));";
            String createStatus = "CREATE TABLE IF NOT EXISTS status (statusID integer PRIMARY KEY, status varchar(10) NOT NULL, UNIQUE(status));";
            String createHitTable = "CREATE TABLE IF NOT EXISTS hits (hitID integer PRIMARY KEY, " +
                                                                     "date text NOT NULL, " +
                                                                     "requesterID integer, " +
                                                                     "hitName varchar(255) NOT NULL, " +
                                                                     "amount decimal(5,2) NOT NULL, " +
                                                                     "bonus decimal(5,2) NOT NULL, " +
                                                                     "statusID integer, " +
                                                                     "FOREIGN KEY(requesterID) references requesters(requesterID), " +
                                                                     "FOREIGN KEY(statusID) REFERENCES status(statusID)" +
                                                                     ");";
            String populateStatus = "INSERT OR IGNORE INTO status(status) VALUES (\"Pending\"), (\"Approved\"), (\"Paid\");";

            conn.Open();

            SQLiteCommand command = new SQLiteCommand(createRequesters, conn);
            command.ExecuteNonQuery();

            command = new SQLiteCommand(createStatus, conn);
            command.ExecuteNonQuery();

            command = new SQLiteCommand(createHitTable, conn);
            command.ExecuteNonQuery();

            command = new SQLiteCommand(populateStatus, conn);
            command.ExecuteNonQuery();

            command.Dispose();
            conn.Close();


        }

        /// <summary>
        /// Adds a new HIT to the database along with the requester if it isn't already exist in it's respective table.
        /// </summary>
        /// <param name="date">The date of the HIT</param>
        /// <param name="requester">The requester of the HIT</param>
        /// <param name="hitName">The HIT's name</param>
        /// <param name="amount">The base amount paid by the HIT</param>
        /// <param name="bonus">The bonus payment of the HIT</param>
        /// <param name="status">Whether or not the HIT has been approved, paid, or is pending</param>
        /// <returns>Returns true if the HIT was successfully added to the database, false otherwise.</returns>
        public static bool add(String date, String requester, String hitName, String amount, String bonus, String status)
        {
            String insertRequester = "INSERT OR IGNORE INTO requesters(name) VALUES (@requester);";
            String insertHIT = "INSERT INTO hits(date, requesterID, hitName, amount, bonus, statusID) " +
                               "VALUES(@date, (SELECT requesterID FROM requesters WHERE name = @requester), @hitName, @amount, @bonus, " +
                                     "(SELECT statusID FROM status WHERE status = @status));";

            SQLiteTransaction transaction = null;
            SQLiteCommand command = null;

            bool success = true;

            conn.Open();
            try
            {
                transaction = conn.BeginTransaction();

                //inserts requester into requester table if it doesn't already exist
                command = new SQLiteCommand(insertRequester);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@requester", requester));
                command.Transaction = transaction;
                command.ExecuteNonQuery();

                command = new SQLiteCommand(insertHIT);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@date", date));
                command.Parameters.Add(new SQLiteParameter("@requester", requester));
                command.Parameters.Add(new SQLiteParameter("@hitName", hitName));
                command.Parameters.Add(new SQLiteParameter("@amount", amount));
                command.Parameters.Add(new SQLiteParameter("@bonus", bonus));
                command.Parameters.Add(new SQLiteParameter("@status", status));
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch(Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    Console.WriteLine(e.ToString());
                }

                success = false;
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();

                if (command != null)
                    command.Dispose();

                conn.Close();
            }

            return success;
        }

        /// <summary>
        /// Returns an ArrayList of HitInfo objects representing all the rows in the database.
        /// </summary>
        /// <returns>Returns an ArrayList of HitInfo objects representing all the rows in the database.</returns>
        public static ArrayList getHITs()
        {
            String query = "SELECT hits.hitID AS id, hits.date AS \'Date\', requesters.name AS Requester, hits.hitName AS Name, hits.amount as Amount, hits.bonus AS Bonus, status.status AS Status " +
                           "FROM requesters JOIN hits ON requesters.requesterID = hits.requesterID " +
                           "JOIN status ON status.statusID = hits.statusID ORDER BY hits.date DESC;";

            conn.Open();

            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            ArrayList ret = new ArrayList();

            while (reader.Read())
            {
                ret.Add(new HitInfo() { id = reader.GetInt32(0).ToString(),
                                        date = reader.GetString(1),
                                        requester = reader.GetString(2),
                                        name = reader.GetString(3),
                                        amt = reader.GetDecimal(4).ToString("#0.00"),
                                        bonus = reader.GetDecimal(5).ToString("#0.00"),
                                        status =reader.GetString(6)});
            }

            command.Dispose();
            reader.Close();
            conn.Close();

            return ret;
        }

        /// <summary>
        /// Deletes the row with the given HIT ID from the database.
        /// </summary>
        /// <param name="hitId">The HIT ID of the row to be deleted.</param>
        /// <returns>Returns true if the HIT was successfully deleted, false otherwise.</returns>
        public static bool delete(String hitId)
        {
            String sql = "DELETE FROM hits WHERE hitID = @id;";

            SQLiteTransaction transaction = null;
            SQLiteCommand command = null;

            bool success = true;

            conn.Open();

            try
            {
                transaction = conn.BeginTransaction();
                command = new SQLiteCommand(sql);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@id", hitId));
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    Console.WriteLine(e.ToString());
                }

                success = false;
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();

                if (command != null)
                    command.Dispose();

                conn.Close();
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<String>("deleted");
            return success;

        }

        /// <summary>
        /// Updates the row with the corresponding HIT ID with the param values.
        /// </summary>
        /// <param name="hitId">The ID of the HIT to be updated.</param>
        /// <param name="date">The updated date.</param>
        /// <param name="requester">The updated requester name.</param>
        /// <param name="hitName">The updated HIT name.</param>
        /// <param name="amount">The updated base amount of the HIT.</param>
        /// <param name="bonus">The updated bonus amount of the HIT.</param>
        /// <param name="status">the updated status of the HIT.</param>
        /// <returns>Returns true if the hit was successfully updated, false otherwise.</returns>
        public static bool update(String hitId, String date, String requester, String hitName, String amount, String bonus, String status)
        {
            //Adds updated requester name to table 'requesters' in case it it doesn't already exist.
            String insertRequester = "INSERT OR IGNORE INTO requesters(name) VALUES (@requester);";
            String sql = "UPDATE hits " +
                         "SET date = @date, requesterID = (SELECT requesterID FROM requesters WHERE name = @requester), hitName = @hitName, amount = @amount, bonus = @bonus, " +
                         "statusID = (SELECT statusID FROM status WHERE status = @status) " +
                         "WHERE hitID = @hitId;";

            SQLiteTransaction transaction = null;
            SQLiteCommand command = null;

            bool success = true;

            conn.Open();

            try
            {
                transaction = conn.BeginTransaction();
                command = new SQLiteCommand(insertRequester);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@requester", requester));
                command.Transaction = transaction;
                command.ExecuteNonQuery();

                command = new SQLiteCommand(sql);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@hitId", hitId));
                command.Parameters.Add(new SQLiteParameter("@date", date));
                command.Parameters.Add(new SQLiteParameter("@requester", requester));
                command.Parameters.Add(new SQLiteParameter("@hitName", hitName));
                command.Parameters.Add(new SQLiteParameter("@amount", amount));
                command.Parameters.Add(new SQLiteParameter("@bonus", bonus));
                command.Parameters.Add(new SQLiteParameter("@status", status));
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch(Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("HIT was not updated");
                }

                success = false;
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();

                if (command != null)
                    command.Dispose();

                conn.Close();
            }

            return success;
        }
    }
}
