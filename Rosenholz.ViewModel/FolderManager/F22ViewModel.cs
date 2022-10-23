using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rosenholz.ViewModel
{
    public delegate void AUContextChanged(AUReference reference);
    public delegate Tuple<bool, AUReference> F22EntryRequired(F16F22Reference reference);

    public class F22ViewModel : ViewModelBase
    {
        private ObservableCollection<F22> _f22List;
        private string _textFilter;
        private F16F22Reference _currentF16Reference;
        public event AUContextChanged AUContextChangeEvent;
        public event F22EntryRequired F22EntryRequiredEvent;

        private F22 _currentF22Selected = null;
        private ListCollectionView _f22CollectionView;
        public ICollectionView F22CollectionView
        {
            get { return this._f22CollectionView; }
        }


        public F22ViewModel()
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
                    F22CollectionView.Filter = null;
                else
                    F22CollectionView.Filter = new Predicate<object>(o => ((F22)o).Dossier.Split(',').Where(s => s.ToLower().Contains(value.ToLower())).Count() > 0 ||
                                                                          ((F22)o).Pseudonym.Contains(value.ToLower()));
            }
        }

        public ObservableCollection<F22> F22Items
        {
            get { return _f22List; }
            set
            {
                _f22List = value;
                OnPropertyChanged(nameof(F22Items));
            }
        }

        /// <summary>
        /// Holds the Reference F22 which is contained in the parent F16
        /// </summary>
        public F16F22Reference CurrentF16Reference
        {
            get { return _currentF16Reference; }
            set
            {
                _currentF16Reference = value;
                OnPropertyChanged(nameof(CurrentF16Reference));
            }
        }
        public F22 CurrentF22Selected
        {
            get
            {
                return _currentF22Selected;
            }
            set
            {
                _currentF22Selected = value;
                //Notwendig, da nach einem Speichervorgang die Liste neu geladen wird und damit die Referenz auf das Current Element verloren geht.
                if (_currentF22Selected != null)
                    AUContextChangeEvent.Invoke(_currentF22Selected.AUReference);
                //Löse dann ein Event mit null aus, dass sich auch die TreeView zuruecksetzen kann
#warning in der Treeview dann auch berücksichtigen, dass das passieren kann.
                else
                    AUContextChangeEvent.Invoke(null);

                OnPropertyChanged(nameof(CurrentF22Selected));
                try
                {
                    WriteF22ItemsCommandExecute(null);
                }
                catch
                {

                }
            }
        }

        public void SelectItems(F16F22Reference reference)
        {
            var tmp = reference;
            var a = F22Storage.Instance.SelectData(reference);
            F22Items = new ObservableCollection<F22>(a);
            _f22CollectionView = new ListCollectionView(F22Items);
            OnPropertyChanged(nameof(F22CollectionView));
        }

        private RelayCommand _addF22EntryCommand;
        public RelayCommand AddF22EntryCommand
        {
            get
            {
                if (_addF22EntryCommand == null)
                {
                    _addF22EntryCommand = new RelayCommand(
                        (parameter) => AddF22EntryExecute(),
                        (parameter) => CanEcexuteF22EntryAdd(parameter)
                    );
                }
                return _addF22EntryCommand;
            }
        }

        private bool CanEcexuteF22EntryAdd(object parameter)
        {
            return !string.IsNullOrWhiteSpace(CurrentF16Reference?.F22String) &&
                   !(CurrentF16Reference?.F22String?.Contains("I_000_0") == true);
        }

        public void AddF22EntryExecute()
        {
            if (CurrentF16Reference != null)
            {
                var result = F22EntryRequiredEvent.Invoke(CurrentF16Reference);

                if (result.Item1 == false)
                {
                    SelectItems(CurrentF16Reference);
                    AUContextChangeEvent(result.Item2);
                }
            }
        }

        private RelayCommand _writeF22ItemsCommand
          ;
        public RelayCommand WriteF22ItemsCommand
        {
            get
            {
                if (_writeF22ItemsCommand == null)
                {
                    _writeF22ItemsCommand = new RelayCommand(
                        (parameter) => WriteF22ItemsCommandExecute(parameter),
                        (parameter) => CanExecuteWriteF22ItemsCommand(parameter)
                    );
                }
                return _writeF22ItemsCommand;
            }
        }

        public void WriteF22ItemsCommandExecute(object parameter)
        {
            var text = (string)parameter;
            var items = F22Storage.Instance.ReadData();
            string dir = Settings.Settings.Instance.F22SubLocation;

            var positions = items.Select(p => p.F16F22Reference.PositionCounterString).Distinct().ToList();
            var references = items.Select(f => f.F16F22Reference.F22String).Distinct().ToList();

            foreach (var position in positions)
            {
                foreach (var reference in references)
                {
                    var refe = new F16F22Reference(reference);
                    string path = Path.Combine(dir, position, refe.YearString);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
            }


            foreach (var reference in references)
            {
                var refe = new F16F22Reference(reference);
                var elements = F22Storage.Instance.ReadAUElements(refe.F22String);

                List<string> stringList = new List<string>();

                foreach (var element in elements)
                {
                    stringList.Add($"{element.AUReference.AUReferenceString}: {element.Pseudonym}");
                }

                File.WriteAllLines(Path.Combine(dir, refe.PositionCounterString, refe.YearString, $"{refe.F22String}.txt"), stringList.ToArray());
            }
        }

        public bool CanExecuteWriteF22ItemsCommand(object parameter)
        {
            //            var text = (string)parameter;
            //#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
            //            var val = F16Items.ToList().Any(s => s.F16F22Reference.F22String.Equals(parameter));
            //#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast

            return true;
        }

        private RelayCommand _archiveF22Command;
        public RelayCommand ArchiveF22Command
        {
            get
            {
                if (_archiveF22Command == null)
                {
                    _archiveF22Command = new RelayCommand(
                        (parameter) => ArchiveF16Execute(parameter),
                        (parameter) => true
                    );
                }
                return _archiveF22Command;
            }
        }

        public void ArchiveF16Execute(object parameter)
        {
            var text = (string)parameter;
            string dir = Settings.Settings.Instance.F22SubLocation;

            if (!Directory.Exists(Path.Combine(dir, "_archive")))
                Directory.CreateDirectory(Path.Combine(dir, "_archive"));

            string location = Path.Combine(dir, "_archive", $"f22_{DateTime.Now.ToFileTimeUtc()}.zip");

            using (ZipArchive zip = ZipFile.Open(location, ZipArchiveMode.Create))
            {
                try
                {
                    zip.CreateEntryFromFile(Settings.Settings.Instance.F22SubLocation, Settings.Settings.Instance.F22FileName);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
