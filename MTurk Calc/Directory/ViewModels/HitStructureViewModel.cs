using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace MTurk_Calc
{
    class HitStructureViewModel : BaseViewModel
    {
        ObservableCollection<HitExpandViewModel> _hitExpands { get; set; }

        public double allTotal { get; set; }
        public double allPending { get; set; }

        public ObservableCollection<HitExpandViewModel> hitExpands
        {
            get
            {
                return _hitExpands;
            }

            set
            {
                _hitExpands = value;
                this.OnPropertyChanged("hitExpands");
            }
        }
        public RelayCommand addCommand { get; set; }

        public HitStructureViewModel()
        {
            allTotal = 0.0;
            allPending = 0.0;

            InitializeDb();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<String>(this, (param) => Refresh());

            hitExpands = CreateHitExpands();
            addCommand = new RelayCommand(SpawnAddViewModel);

        }

        private void InitializeDb()
        {
            DbUtility.createTables();
        }

        private void SpawnAddViewModel()
        {
            var add = new AddWindow();

            if (add.ShowDialog() == true)
            {
                Refresh();
            }

            else
            {
            }
        }

        private ObservableCollection<HitExpandViewModel> CreateHitExpands()
        {
            ObservableCollection<HitExpandViewModel> ret = new ObservableCollection<HitExpandViewModel>();
            var allHITs = HitStructure.getHITs();

            HitExpandViewModel currExpand = null;
            String prevDate = "";

            for (int i = 0; i < allHITs.Count; i++)
            {
                String date = ((HitInfo)allHITs[i]).date;

                HitInfo hit = (HitInfo)allHITs[i];

                HitInfoViewModel info = new HitInfoViewModel(hit.id, hit.date, hit.requester, hit.name, hit.amt, hit.bonus, hit.status);

                if (!prevDate.Equals(date))
                {
                    if (currExpand != null)
                    {
                        ret.Add(currExpand);
                    }

                    currExpand = new HitExpandViewModel(date, new ObservableCollection<HitInfoViewModel>());
                    currExpand.hitList.Add(info);
                    prevDate = date;

                    //adds to collection if this is the last iteration of loop
                    if (i == allHITs.Count - 1)
                    {
                        ret.Add(currExpand);
                    }
                }

                else
                {
                    currExpand.hitList.Add(info);
                }

                currExpand.total += Double.Parse(hit.amt);
                currExpand.total += Double.Parse(hit.bonus);
                allTotal += Double.Parse(hit.amt);
                allTotal += Double.Parse(hit.bonus);

                if (hit.status.Equals(Status.Pending.ToString()))
                {
                    currExpand.pending += Double.Parse(hit.amt);
                    allPending += Double.Parse(hit.amt);
                }
            }

            return ret;
        }

        private void Refresh()
        {
            Console.WriteLine("Refresh called");
            hitExpands = CreateHitExpands();
        }
    }
}
