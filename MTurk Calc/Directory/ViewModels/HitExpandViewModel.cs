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
        private HitInfoViewModel _selected { get; set; }
        private int _index { get; set; }

        /// <summary>
        /// A collection of HITs for this date as HitInfoViewModels.
        /// </summary>
        public ObservableCollection<HitInfoViewModel> hitList { get; set; }

        /// <summary>
        /// The date for this collection of HITs
        /// </summary>
        public String header { get; set; }

        /// <summary>
        /// Command to edit a HIT.
        /// </summary>
        public RelayCommand editCommand { get; set; }

        /// <summary>
        /// Command to delete a HIT.
        /// </summary>
        public RelayCommand deleteCommand { get; set; }

        /// <summary>
        /// Total amount for this group of HITs.
        /// </summary>
        public double total { get; set; }

        /// <summary>
        /// Total pending amount for this group of HITs.
        /// </summary>
        public double pending { get; set; }

        /// <summary>
        /// The selected HitInfoViewModel
        /// </summary>
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

        /// <summary>
        /// The index of the selected HitInfoViewModel
        /// </summary>
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

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="header">The date for this group of HITs.</param>
        /// <param name="hitlist">Collection of HITs as HitInfoViewModels</param>
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

        /// <summary>
        /// Opens an edit window for the selected object.
        /// </summary>
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

        /// <summary>
        /// Deletes the selected HIT from the database.
        /// </summary>
        public void delete()
        {
            DbUtility.delete(selected.id);
        }
    }
}
