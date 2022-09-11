using Rosenholz.Model;
using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rosenholz.ViewModel
{
    public delegate void F22ContextChanged(F16F22Reference reference);
    public class F16ViewModel
    {
        private IList<F16> _f16List;
        private string _reference;
        private string _latestItem => F16Items[0].F16F22Reference.F22String;
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
                F22ContextChangeEvent.Invoke(_currentF16Selected.F16F22Reference);
                OnPropertyChanged(nameof(CurrentF16Selected));
            }
        }


        public string LatestItem
        {
            get
            {
                return _latestItem;
            }
        }



        public IList<F16> F16Items
        {
            get { return _f16List; }
            set { _f16List = value; }
        }


        public void LoadF16Items()
        {
            var a = F16.Storage.ReadData();

            F16Items = a;
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
            CreateF16 model = new CreateF16(text);
            model.ShowDialog();

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

