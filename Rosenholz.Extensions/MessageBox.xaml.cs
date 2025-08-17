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
    /// Interaktionslogik für MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        public bool? DialogResult = null;

        private string _outputString = "";
        public string OutputString
        {
            get
            {
                return _outputString;
            }
            set
            {
                _outputString = value;
                OnPropertyChanged(nameof(OutputString));
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

        public MessageBox(string labelText, string outputText)
        {
            LabelText = labelText;
            OutputString = outputText;
            InitializeComponent();
            DataContext = this;
        }


        private RelayCommand _accept;
        //change to new RelayCommand to avoid confusion with the base class possible (remove new keyword) 17.08.2025
        public RelayCommand Accept
        {
            get
            {
                if (_accept == null)
                {
                    _accept = new RelayCommand(
                        (parameter) => CloseExecute(true),
                        (parameter) => true
                    );
                }
                return _accept;
            }
        }

        private RelayCommand _deny;
        //change to new RelayCommand to avoid confusion with the base class possible (remove new keyword) 17.08.2025
        public RelayCommand Deny
        {
            get
            {
                if (_deny == null)
                {
                    _deny = new RelayCommand(
                        (parameter) => CloseExecute(false),
                        (parameter) => true
                    );
                }
                return _deny;
            }
        }

        public void CloseExecute(bool parameter)
        {
            DialogResult = parameter;
            this.Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
