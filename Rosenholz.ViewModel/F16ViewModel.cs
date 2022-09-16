using Rosenholz.Model;
using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rosenholz.ViewModel
{
    public delegate void F22ContextChanged(F16F22Reference reference);
    public class F16ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<F16> _f16List;
        private string _reference;
        private string _latestItem;
        private F16 _currentF16Selected = null;
        public event F22ContextChanged F22ContextChangeEvent;


        public F16ViewModel()
        {

        }


        public string Reference
        {
            get
            {
                return _reference;
            }
            set
            {
                _reference = value;
                OnPropertyChanged(nameof(Reference));
            }
        }

        public F16 CurrentF16Selected
        {
            get
            {
                return _currentF16Selected;
            }
            set
            {
                _currentF16Selected = value;
                if (_currentF16Selected != null)
                    F22ContextChangeEvent?.Invoke(_currentF16Selected.F16F22Reference);
                else
                    F22ContextChangeEvent?.Invoke(new F16F22Reference("I_000_00"));
                OnPropertyChanged(nameof(CurrentF16Selected));
            }
        }


        public string LatestItem
        {
            get
            {
                return _latestItem;
            }
            set
            {
                _latestItem = value;
                OnPropertyChanged(nameof(LatestItem));
            }
        }

        private void UpdateLatestItem()
        {
            string retval = $"I_000_00";
            var elements = F16.Storage.ReadData();
            if (elements.Count > 0)
            {
                LatestItem = F16.Storage.ReadData().ElementAt(0).F16F22Reference.F22String;
            }
        }


        public ObservableCollection<F16> F16Items
        {
            get { return _f16List; }
            set
            {
                _f16List = value;
                OnPropertyChanged(nameof(F16Items));
            }
        }


        public void LoadF16Items()
        {
            var a = F16.Storage.ReadData();
            F16Items = new ObservableCollection<F16>(a);
            UpdateLatestItem();
        }

        #region Show

        private RelayCommand _showF22;
        public RelayCommand ShowF22
        {
            get
            {
                if (_showF22 == null)
                {
                    _showF22 = new RelayCommand(
                        (parameter) => ShowF22Command(parameter),
                        (parameter) => IsValidF22(parameter)
                    );
                }
                return _showF22;
            }
        }

        public void ShowF22Command(object parameter)
        {
            var text = (string)parameter;

        }

        public bool IsValidF22(object parameter)
        {
            var text = (string)parameter;
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
            var val = F16Items.ToList().Any(s => s.F16F22Reference.F22String.Equals(parameter));
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast

            return val;
        }

        #endregion


        private RelayCommand _addF16Command;
        public RelayCommand AddF16Command
        {
            get
            {
                if (_addF16Command == null)
                {
                    _addF16Command = new RelayCommand(
                        (parameter) => AddF16Execute(parameter),
                        (parameter) => true
                    );
                }
                return _addF16Command;
            }
        }

        public void AddF16Execute(object parameter)
        {
            var text = (string)parameter;
            CreateF16 model = new CreateF16(text, F16Items.Count);
            model.ShowDialog();
            LoadF16Items();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

