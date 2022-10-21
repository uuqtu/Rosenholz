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

namespace Rosenholz.Extensions
{
    /// <summary>
    /// Interaktionslogik für LinkCaputreBox.xaml
    /// </summary>
    public partial class LinkCaputreBox : Window, INotifyPropertyChanged
    {
        private string _inputString = "";
        public string InputString
        {
            get
            {
                return _inputString;
            }
            set
            {
                _inputString = value;
                OnPropertyChanged(nameof(InputString));
            }
        }

        private string _linktString = "";
        public string LinkString
        {
            get
            {
                return _linktString;
            }
            set
            {
                _linktString = value;
                OnPropertyChanged(nameof(LinkString));
            }
        }

        private string _labelText;
        public string LabelText
        {
            get
            {
                return _labelText;
            }
            set
            {
                _labelText = value;
                OnPropertyChanged(nameof(LabelText));
            }
        }

        public LinkCaputreBox(string labelText)
        {
            LabelText = labelText;
            InitializeComponent();
            DataContext = this;
        }


        private RelayCommand _close;
        public new RelayCommand Close
        {
            get
            {
                if (_close == null)
                {
                    _close = new RelayCommand(
                        (parameter) => CloseExecute(parameter),
                        (parameter) => true
                    );
                }
                return _close;
            }
        }

        public void CloseExecute(object parameter)
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
