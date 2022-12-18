using Rosenholz.Model;
using Rosenholz.Model.CompletionOfAssignments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Rosenholz.Model.Storage;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Rosenholz.ViewModel.CompletionOfAssignments
{
    public delegate void CoAContextChanged(CoA coA);
    public delegate void CoAEntryRequired(string currentCoA, List<CoA> list);
    public class CoAViewModel : ViewModelBase
    {
        private ObservableCollection<CoA> _coAList;
        private string _textFilter;
        private string _reference;
        private string _latestItem;
        private CoA _currentCoASelected = null;
        public event CoAContextChanged CoAContextChangeEvent;
        public event CoAEntryRequired CoAEntryRequiredEvent;
        private ListCollectionView _coACollectionView;
        public ICollectionView CoACollectionView
        {
            get { return this._coACollectionView; }
        }

        public CoAViewModel()
        {

        }

        public string TextFilter
        {
            get { return this._textFilter; }
            set
            {
                _textFilter = value;
                OnPropertyChanged(nameof(TextFilter));

                //https://stackoverflow.com/questions/15473048/create-a-textboxsearch-to-filter-from-listview-wpf
                if (String.IsNullOrEmpty(value))
                    CoACollectionView.Filter = null;
                else
                    CoACollectionView.Filter = new Predicate<object>(o => ((CoA)o).Description?.ToLower()?.Contains(value.ToLower()) == true ||
                                                                       ((CoA)o).TaskName?.ToLower()?.Contains(value.ToLower()) == true);
            }
        }


        public CoA CurrentCoASelected
        {
            get
            {
                return _currentCoASelected;
            }
            set
            {
                _currentCoASelected = value;
                if (_currentCoASelected != null)
                    CoAContextChangeEvent?.Invoke(_currentCoASelected);
                else
                    CoAContextChangeEvent?.Invoke(new CoA());
                OnPropertyChanged(nameof(CurrentCoASelected));

                try
                {
                    WriteCoAItemsCommandExecute(null);
                }
                catch
                {

                }
            }
        }


        private void UpdateLatestItem()
        {
            string retval = $"I_000_0";
            var elements = CoAStorage.Instance.ReadData();
            if (elements.Count > 0)
            {
                CurrentCoASelected = CoAStorage.Instance.ReadData().ElementAt(0);
            }
        }


        public ObservableCollection<CoA> CoAItems
        {
            get { return _coAList; }
            set
            {
                _coAList = value;
                OnPropertyChanged(nameof(CoAItems));
            }
        }


        public void LoadCoAItems()
        {
            var a = CoAStorage.Instance.ReadData();
            CoAItems = new ObservableCollection<CoA>(a);
            _coACollectionView = new ListCollectionView(CoAItems);
            OnPropertyChanged(nameof(CoACollectionView));
            UpdateLatestItem();
        }

        #region Show

        private RelayCommand _writeCoAItemsCommand
            ;
        public RelayCommand WriteCoAItemsCommand
        {
            get
            {
                if (_writeCoAItemsCommand == null)
                {
                    _writeCoAItemsCommand = new RelayCommand(
                        (parameter) => WriteCoAItemsCommandExecute(parameter),
                        (parameter) => CanExecuteWriteCoAItemsCommand(parameter)
                    );
                }
                return _writeCoAItemsCommand;
            }
        }

        public void WriteCoAItemsCommandExecute(object parameter)
        {
            var text = (string)parameter;
            var items = CoAStorage.Instance.ReadData();
            string dir = Settings.Settings.Instance.CompletionOfAssignmentsLocation;

            foreach (var item in items)
            {
                string taskName = item.TaskName;
                DateTime endDate = item.EndDate;
                string timeEstimation = item.TimeEstimation;
                string description = item.Description;
                string location = item.Location;
                bool state = item.State;

                string fileName = taskName;

                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }

                string[] lines = { $"TaskName: {taskName}", $"EndDate: {endDate.ToString()}", $"Time-Estimation: {timeEstimation}", $"Description: {description}", $"Location: {location}", $"State: {state}" };

                if (!Directory.Exists(Path.Combine(dir, "Items")))
                    Directory.CreateDirectory(Path.Combine(dir, "Items"));

                File.WriteAllLines(Path.Combine(dir, "Items", $"{fileName}.txt"), lines);
            }
        }
        public bool CanExecuteWriteCoAItemsCommand(object parameter)
        {

            return true;
        }

        #endregion


        private RelayCommand _addCoACommand;
        public RelayCommand AddCoACommand
        {
            get
            {
                if (_addCoACommand == null)
                {
                    _addCoACommand = new RelayCommand(
                        (parameter) => AddCoAExecute(parameter),
                        (parameter) => true
                    );
                }
                return _addCoACommand;
            }
        }

        public void AddCoAExecute(object parameter)
        {
            var currentF22 = (string)parameter;

            CoAEntryRequiredEvent?.Invoke(currentF22, CoAItems.ToList());

            LoadCoAItems();
        }
    }
}
