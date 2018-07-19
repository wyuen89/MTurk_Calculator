using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace MTurk_Calc
{
    class HitExpandViewModel : BaseViewModel
    {
        public HitInfoViewModel _selected { get; set; }
        public int _index { get; set; }

        public ObservableCollection<HitInfoViewModel> hitList { get; set; }
        public String header { get; set; }

        public RelayCommand editCommand { get; set; }
        public RelayCommand deleteCommand { get; set; }

        public double total { get; set; }
        public double pending { get; set; }

        public HitInfoViewModel selected
        {
            get
            {
                return _selected;
            }

            set
            {
                _selected = value;
            }
        }

        public int index
        {
            get
            {
                return _index;
            }

            set
            {
                _index = value;
            }
        }

        public HitExpandViewModel(String header, ObservableCollection<HitInfoViewModel> hitlist)
        {
            index = -1;
            this.header = header;
            this.hitList = hitlist;
            editCommand = new RelayCommand(edit);
            deleteCommand = new RelayCommand(delete);
            total = 0.00;
            pending = 0.00;
        }

        public void edit()
        {
            Console.WriteLine(selected.id);
            HitInfoViewModel info = new HitInfoViewModel(selected.id, selected.date, selected.requester, selected.name, selected.amount, selected.bonus, "Pending");
            EditWindow edit = new EditWindow();
            edit.DataContext = info;

            if (edit.ShowDialog() == true)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<String>("new");
            }
                
        }

        public void delete()
        {
            DbUtility.delete(selected.id);
        }
    }
}
