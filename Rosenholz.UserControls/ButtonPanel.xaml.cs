using Rosenholz.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rosenholz.UserControls
{
    /// <summary>
    /// Interaction logic for ButtonPanel.xaml
    /// </summary>
    public partial class ButtonPanel : UserControl, INotifyPropertyChanged
    {
        public ButtonPanel()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string _currentFolder;
        public string CurrentFolder
        {
            get { return _currentFolder; }
            set
            {
                _currentFolder = value;
                OnPropertyChanged(nameof(CurrentFolder));
            }
        }

        private RelayCommand _openFolderCommand;
        public RelayCommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(
                        (parameter) => OpenFolderExecute(),
                        (parameter) => CanEcexuteOpenFolder()
                    );
                }
                return _openFolderCommand;
            }
        }

        private bool CanEcexuteOpenFolder()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void OpenFolderExecute()
        {
            Process.Start(CurrentFolder);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
