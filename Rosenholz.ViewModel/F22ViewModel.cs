using Rosenholz.Model;
using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.ViewModel
{
    public delegate void AUContextChanged(AUReference reference);
    public class F22ViewModel
    {
        private IList<F22> _f22List;
        private F16F22Reference _currentF16Reference;
        public event AUContextChanged AUContextChangeEvent;
        private F22 _currentF22Selected = null;


        public F22ViewModel()
        {

        }

        public IList<F22> F22Items
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
                AUContextChangeEvent.Invoke(_currentF22Selected.AUReference);
                OnPropertyChanged(nameof(CurrentF22Selected));
            }
        }

        public void SelectItems(F16F22Reference reference)
        {
            var a = F22.Storage.SelectData(reference);
            F22Items = a;
        }

        private RelayCommand _addF22EntryCommand;
        public RelayCommand AddF22EntryCommand
        {
            get
            {
                if (_addF22EntryCommand == null)
                {
                    _addF22EntryCommand = new RelayCommand(
                        (parameter) => AddF22EntryExecute(parameter),
                        (parameter) => CanEcexuteF22ntryAdd(parameter)
                    );
                }
                return _addF22EntryCommand;
            }
        }

        private bool CanEcexuteF22ntryAdd(object parameter)
        {
            return CurrentF16Reference != null;
        }

        public void AddF22EntryExecute(object parameter)
        {
            if (parameter is F22)
            {
                var text = parameter as F22;
                CreateF22Entry model = new CreateF22Entry(CurrentF16Reference);
                model.ShowDialog();
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
