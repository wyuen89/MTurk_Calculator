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

        public String id { get; set; }

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
        public String date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
                canExecute = FieldsFilled();
                saveCommand.RaiseCanExecuteChanged();
            }
        }

        public String status { get; set; }
        public String[] statusValues { get; set; }
        public DateTime selectedDate { get; set; }

        public RelayCommand<IClosableDialog> cancelCommand { get; set; }
        public RelayCommand<IClosableDialog> saveCommand { get; set; }

        public bool canExecute { get; set; }

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

        private void Cancel(IClosableDialog window)
        {
            window.Close(false);
        }

        private void Save(IClosableDialog window)
        {
            bool success = DbUtility.update(id, date, requester, name, amount, bonus, status);
            window.Close(success);
        }

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
