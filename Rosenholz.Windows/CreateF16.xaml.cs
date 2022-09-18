using Rosenholz.Model;
using Rosenholz.Model.RomanNumerals;
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
    /// Interaction logic for CreateF16.xaml
    /// </summary>
    public partial class CreateF16 : Window
    {
        private string _f16f22ReferenceToSet;
        private string _keywordToSet;
        private string _labelToSet;
        private string _purposeToSet;
        private int _f16Count;

        public string F16f22ReferenceToSet
        {
            get
            {
                return _f16f22ReferenceToSet;
            }
            set
            {
                _f16f22ReferenceToSet = value;
                OnPropertyChanged(nameof(F16f22ReferenceToSet));
            }
        }

        public string KeywordToSet
        {
            get
            {
                return _keywordToSet;
            }
            set
            {
                _keywordToSet = value;
                OnPropertyChanged(nameof(KeywordToSet));
            }
        }

        public string LabelToSet
        {
            get
            {
                return _labelToSet;
            }
            set
            {
                _labelToSet = value;
                OnPropertyChanged(nameof(LabelToSet));
            }
        }

        public string PurposeToSet
        {
            get
            {
                return _purposeToSet;
            }
            set
            {
                _purposeToSet = value;
                OnPropertyChanged(nameof(PurposeToSet));
            }
        }


        private string _currentF22Reference = "";
        public string CurrentF22Reference
        {
            get
            {
                return _currentF22Reference;
            }
            set
            {
                _currentF22Reference = value;
                OnPropertyChanged(nameof(CurrentF22Reference));
            }
        }

        public string ProposedF22Reference
        {
            get
            {
                //Passiert nur beim ersten Anlegen eines Elements
                if (_f16Count == 0)
                    CurrentF22Reference = $"{Roman.ToRoman(int.Parse(Settings.Settings.Instance.Position))}_000_00";
                var a = F16F22Reference.NextF22(CurrentF22Reference);
                F16f22ReferenceToSet = a;
                return a;
            }
        }
        public CreateF16(string currentF22, int count)
        {
            InitializeComponent();
            DataContext = this;
            CurrentF22Reference = currentF22;
            _f16Count = count;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private RelayCommand _saveNewF16;
        public RelayCommand SaveNewF16
        {
            get
            {
                if (_saveNewF16 == null)
                {
                    _saveNewF16 = new RelayCommand(
                        (parameter) => SaveNewF16Execute(parameter),
                        (parameter) => CanExecuteSaveNewF16(parameter)
                    );
                }
                return _saveNewF16;
            }
        }

        private bool CanExecuteSaveNewF16(object parameter)
        {
            return !string.IsNullOrWhiteSpace(F16f22ReferenceToSet) &&
                   !string.IsNullOrWhiteSpace(KeywordToSet) &&
                   !string.IsNullOrWhiteSpace(LabelToSet) &&
                   !string.IsNullOrWhiteSpace(PurposeToSet);
        }

        private void SaveNewF16Execute(object parameter)
        {
            var F16ToCreate = new F16(KeywordToSet, LabelToSet, PurposeToSet, F16f22ReferenceToSet);
            F16Storage.Instance.InsertData(F16ToCreate);
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
    }
}
