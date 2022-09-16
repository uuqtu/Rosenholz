using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Rosenholz.Windows
{

    /// <summary>
    /// Interaction logic for CreateF22.xaml
    /// </summary>
    public partial class CreateF22Entry : Window
    {
        private string _aUReferenceCurrent;
        private string _f16F22ReferenceCurrent;
        private string _auReferenceToSet;
        private string _pseudonymToSet;
        private string _createdToSet;
        private string _linkToset;
        private string _dossierToSet;

        public CreateF22Entry(F16F22Reference currentF16Reference)
        {
            InitializeComponent();
            DataContext = this;
            CreatetToSet = DateTime.Now.ToString();

            var Items = F22.Storage.ReadData();

            F16F22ReferenceCurrent = currentF16Reference.F22String;
            
            //Damit es auch bei Erstanlage funktioniert.
            if (Items?.Count != 0)
                AUReferenceCurrent = Items[0].AUReference.AUReferenceString;
            else
                AUReferenceCurrent = $"AU_000_{int.Parse(DateTime.Now.ToString("yy"))}";            
        }

        public string DossierToSet
        {
            get
            { return _dossierToSet; }
            set
            {
                _dossierToSet = value;
                OnPropertyChanged(nameof(DossierToSet));
            }
        }

        public string LinkToSet
        {
            get
            { return _linkToset; }
            internal set
            {
                _linkToset = value;
                OnPropertyChanged(nameof(LinkToSet));
            }
        }

        public string CreatetToSet
        {
            get
            { return _createdToSet; }
            set
            {
                _createdToSet = value;
                OnPropertyChanged(nameof(CreatetToSet));
            }
        }

        public string PseudonymToSet
        {
            get
            { return _pseudonymToSet; }
            set
            {
                _pseudonymToSet = value;
                OnPropertyChanged(nameof(PseudonymToSet));
            }
        }

        public string AUReferenceToSet
        {
            get
            { return _auReferenceToSet; }
            set
            {
                _auReferenceToSet = value;
                OnPropertyChanged(nameof(AUReferenceToSet));
            }
        }

        public string AUReferenceCurrent
        {
            get
            { return _aUReferenceCurrent; }
            set
            {
                _aUReferenceCurrent = value;
                OnPropertyChanged(nameof(AUReferenceCurrent));
            }
        }

        public string F16F22ReferenceCurrent
        {
            get
            { return _f16F22ReferenceCurrent; }
            set
            {
                _f16F22ReferenceCurrent = value;
                OnPropertyChanged(nameof(F16F22ReferenceCurrent));
            }
        }


        public string ProposedAuReference
        {
            get
            {
                var a = AUReference.NextAU(AUReferenceCurrent);
                AUReferenceToSet = a;
                LinkToSet = FolderManager.Instance.GetAUFolder(AUReferenceToSet);
                return a;
            }
        }


        private RelayCommand _saveNewF22Entry;
        public RelayCommand SaveNewF22Entry
        {
            get
            {
                if (_saveNewF22Entry == null)
                {
                    _saveNewF22Entry = new RelayCommand(
                        (parameter) => SaveNewF22EntryExecute(parameter),
                        (parameter) => CanExecuteSaveNewF22Entry(parameter)
                    );
                }
                return _saveNewF22Entry;
            }
        }

        private bool CanExecuteSaveNewF22Entry(object parameter)
        {
            return !string.IsNullOrWhiteSpace(AUReferenceToSet) &&
                   !string.IsNullOrWhiteSpace(F16F22ReferenceCurrent) &&
                   !string.IsNullOrWhiteSpace(PseudonymToSet) &&
                   !string.IsNullOrWhiteSpace(CreatetToSet) &&
                   !string.IsNullOrWhiteSpace(LinkToSet) &&
                   !string.IsNullOrWhiteSpace(DossierToSet);
        }

        private void SaveNewF22EntryExecute(object parameter)
        {
            var F16ToCreate = new F22(AUReferenceToSet, F16F22ReferenceCurrent, PseudonymToSet, CreatetToSet, DossierToSet, LinkToSet);
            F22.Storage.InsertData(F16ToCreate);
            FolderManager.Instance.CreateAUFolder(F16ToCreate.AUReference.AUReferenceString);

            this.Close();
        }

        private RelayCommand _abort;
        public RelayCommand Abort
        {
            get
            {
                if (_abort == null)
                {
                    _abort = new RelayCommand(
                        (parameter) => AbortExecute(parameter),
                        (parameter) => CanExecuteAbort(parameter)
                    );
                }
                return _abort;
            }
        }

        private bool CanExecuteAbort(object parameter)
        {
            return true;
        }

        private void AbortExecute(object parameter)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
