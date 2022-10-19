using Rosenholz.Extensions;
using System;

using System.ComponentModel;


namespace Rosenholz.Extensions

{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : System.Windows.Window, INotifyPropertyChanged
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

        public InputBox(string labelText)
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
