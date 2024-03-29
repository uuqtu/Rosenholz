﻿using Rosenholz.Extensions;
using System;

using System.ComponentModel;
using System.IO;

namespace Rosenholz.Extensions

{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : System.Windows.Window, INotifyPropertyChanged
    {
        private bool _cleanForFilename = false;
        private string _inputString = "";
        public string InputString
        {
            get
            {
                return _inputString;
            }
            set
            {
                if (!_cleanForFilename)
                    _inputString = value;
                else
                    _inputString = string.Join("_", value.Split(Path.GetInvalidFileNameChars()));
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

        public InputBox(string labelText, bool cleanForFileName)
        {
            LabelText = labelText;
            InitializeComponent();
            DataContext = this;
            _cleanForFilename = cleanForFileName;
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
