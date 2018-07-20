using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace MTurk_Calc
{
    class HitInfoViewModel : BaseViewModel
    {

        private String _requester;
        private String _name;
        private String _amount;
        private String _bonus;
        private String _date;

        /// <summary>
        /// This HIT's ID.
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// This HIT's requester.
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
                saveCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// This HIT's name.
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
                saveCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// This HIT's base payment amount.
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
                saveCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// This HIT's bonus payment amount.
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
                saveCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// This HIT's date.
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
                saveCommand.RaiseCanExecuteChanged();
                this.OnPropertyChanged("date");
            }
        }

        /// <summary>
        /// The HIT's saved status.
        /// </summary>
        public String status { get; set; }

        /// <summary>
        /// The Possible status values.
        /// </summary>
        public String[] statusValues { get; set; }

        /// <summary>
        /// The HIT's saved date.
        /// </summary>
        public DateTime selectedDate { get; set; }

        /// <summary>
        /// Command for a cancel button.
        /// </summary>
        public RelayCommand<IClosableDialog> cancelCommand { get; set; }

        /// <summary>
        /// Command for a button to save the inputted data.
        /// </summary>
        public RelayCommand<IClosableDialog> saveCommand { get; set; }

        /// <summary>
        /// Tells save command if it can execute or not.
        /// </summary>
        public bool canExecute { get; set; }

        /// <summary>
        /// Constructor for HitInfoViewModel
        /// </summary>
        /// <param name="id">The ID of the HIT.</param>
        /// <param name="date">The date of the HIT.</param>
        /// <param name="requester">The requester of the HIT.</param>
        /// <param name="name">The name of the HIT.</param>
        /// <param name="amount">The base amount for the HIT.</param>
        /// <param name="bonus">The bonus amount for the HIT.</param>
        /// <param name="status">The status of the hit; pending, approved, or paid.</param>
        public HitInfoViewModel(String id, String date, String requester, String name, String amount, String bonus, String status)
        {
            this.id = id;
            this._date = date;
            this._requester = requester;
            this._name = name;
            this._amount = amount;
            this._bonus = bonus;
            this.status = status;
            this.statusValues = Enum.GetNames(typeof(Status));
            this.selectedDate = DateTime.Parse(date);

            canExecute = FieldsFilled();

            cancelCommand = new RelayCommand<IClosableDialog>(param => Cancel(param));
            saveCommand = new RelayCommand<IClosableDialog>(param => Save(param), param => canExecute);
        }

        /// <summary>
        /// Closes the window without doing anything.
        /// </summary>
        /// <param name="window">The window that the current view is held in.</param>
        private void Cancel(IClosableDialog window)
        {
            window.Close(false);
        }

        /// <summary>
        /// Updates the respective row in the database.
        /// </summary>
        /// <param name="window">The window that the current view is held in.</param>
        private void Save(IClosableDialog window)
        {
            bool success = DbUtility.update(id, date, requester, name, amount, bonus, status);
            window.Close(success);
        }

        /// <summary>
        /// Checks if any of the fields are empty strings.
        /// </summary>
        /// <returns>True if no field is an empty string, false otherwise</returns>
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
