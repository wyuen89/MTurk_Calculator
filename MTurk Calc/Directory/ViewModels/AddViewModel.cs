using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Windows.Controls.Primitives;
using GalaSoft.MvvmLight.Command;

namespace MTurk_Calc
{
    class AddViewModel : BaseViewModel
    {
        private String _requester;
        private String _name;
        private String _amount;
        private String _bonus;
        private String _date;

        /// <summary>
        /// The status of the HIT.
        /// </summary>
        public String selected { get; set; }

        /// <summary>
        /// The possible statuses.
        /// </summary>
        public String[] status { get; set; }

        /// <summary>
        /// The HIT's requester's name.
        /// </summary>
        public String requester
        {
            get
            {
                return _requester;
            }

            set
            {
                _requester = value;
                canExecute = FieldsFilled();
                addCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The HIT's name.
        /// </summary>
        public String name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                canExecute = FieldsFilled();
                addCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The HIT's base payment amount.
        /// </summary>
        public String amount
        {
            get
            {
                return _amount;
            }

            set
            {
                _amount = value;
                canExecute = FieldsFilled();
                addCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The HIT's bonus payment amount.
        /// </summary>
        public String bonus
        {
            get
            {
                return _bonus;
            }

            set
            {
                _bonus = value;
                canExecute = FieldsFilled();
                addCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The HIT's date.
        /// </summary>
        public String date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = DateTime.Parse(value).ToShortDateString();
                canExecute = FieldsFilled();
                addCommand.RaiseCanExecuteChanged();
                this.OnPropertyChanged("date");
            }
        }

        /// <summary>
        /// Command to cancel adding a new HIT.
        /// </summary>
        public RelayCommand<IClosableDialog> cancelCommand { get; set; }

        /// <summary>
        /// Command to add a new HIT.
        /// </summary>
        public RelayCommand<IClosableDialog> addCommand { get; set; }

        /// <summary>
        /// Tells whether it's possible for addCommand to execute.
        /// </summary>
        public bool canExecute { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public AddViewModel()
        {
            _requester = "";
            _name = "";
            _amount = "";
            _bonus = "0.00";
            _date = System.DateTime.Now.ToShortDateString();

            status = Enum.GetNames(typeof(Status));
            selected = "Pending";

            canExecute = FieldsFilled();

            cancelCommand = new RelayCommand<IClosableDialog>(param => this.Cancel(param));
            addCommand = new RelayCommand<IClosableDialog>(param => this.Add(param), param => canExecute);
        }

        /// <summary>
        /// Adds a new HIT to the database.
        /// </summary>
        /// <param name="window"></param>
        private void Add(IClosableDialog window)
        {
            window.Close(true);
            DbUtility.add(DateTime.Parse(date).ToString("MM/dd/yyyy"), requester, name, amount, bonus, selected);

        }

        /// <summary>
        /// Closes current window without entering a new HIT to the database.
        /// </summary>
        /// <param name="window"></param>
        private void Cancel(IClosableDialog window)
        {
            window.Close(false);
        }

        /// <summary>
        /// Checks if all fields are filled in and not empty strings.
        /// </summary>
        /// <returns></returns>
        private bool FieldsFilled()
        {
            if (requester.Equals("") || name.Equals("") || amount.Equals("") || bonus.Equals("") || date.Equals(""))
            {
                return false;
            }

            return true;
        }
    }
}
