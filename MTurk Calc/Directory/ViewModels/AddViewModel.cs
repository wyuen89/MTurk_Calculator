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

        public String selected { get; set; }
        public String[] status { get; set; }

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

        public RelayCommand<IClosableDialog> cancelCommand { get; set; }
        public RelayCommand<IClosableDialog> addCommand { get; set; }
        public bool canExecute { get; set; }

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

        private void Add(IClosableDialog window)
        {
            window.Close(true);
            DbUtility.add(DateTime.Parse(date).ToString("MM/dd/yyyy"), requester, name, amount, bonus, selected);

        }

        private void Cancel(IClosableDialog window)
        {
            window.Close(false);
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
