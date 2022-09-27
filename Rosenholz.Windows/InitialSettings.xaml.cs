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
    /// Interaktionslogik für InitialSettings.xaml
    /// </summary>
    public partial class InitialSettings : Window, INotifyPropertyChanged
    {
        public InitialSettings()
        {
            InitializeComponent();
            DataContext = this;
        }


        public string Position
        {
            get
            {
                return Settings.Settings.Instance.Position;
            }
            set
            {
                Settings.Settings.Instance.Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public string F22Location
        {
            get { return Settings.Settings.Instance.F22Location; }
            set
            {
                Settings.Settings.Instance.F22Location = value;
                OnPropertyChanged(nameof(F22Location));
            }
        }

        public string F16Location
        {
            get { return Settings.Settings.Instance.F16Location; }
            set
            {
                Settings.Settings.Instance.F16Location = value;
                OnPropertyChanged(nameof(F16Location));
            }
        }

        public string BasePath
        {
            get { return Settings.Settings.Instance.BasePath; }
            set
            {
                Settings.Settings.Instance.BasePath = value;
                OnPropertyChanged(nameof(BasePath));
            }
        }

        public string FileTypeFilters
        {
            get { return Settings.Settings.Instance.FileTypeFilters; }
            set
            {
                Settings.Settings.Instance.FileTypeFilters = value;
                OnPropertyChanged(nameof(FileTypeFilters));
            }
        }

        public string TaskLocation
        {
            get { return Settings.Settings.Instance.TaskLocation; }
            set
            {
                Settings.Settings.Instance.TaskLocation = value;
                OnPropertyChanged(nameof(TaskLocation));
            }
        }

        public string TaskItemLocation
        {
            get { return Settings.Settings.Instance.TaskItemLocation; }
            set
            {
                Settings.Settings.Instance.TaskItemLocation = value;
                OnPropertyChanged(nameof(TaskItemLocation));
            }
        }

        public string AppBaseLocation
        {
            get { return Settings.Settings.Instance.AppBaseLocation; }
            set
            {
                Settings.Settings.Instance.AppBaseLocation = value;
                OnPropertyChanged(nameof(AppBaseLocation));
            }
        }

        

        private RelayCommand _closeSettingsCommand;
        public RelayCommand CloseSettingsCommand
        {
            get
            {
                if (_closeSettingsCommand == null)
                {
                    _closeSettingsCommand = new RelayCommand(
                        (parameter) => CloseSettingsExecute(),
                        (parameter) => CanEcexuteCloseSettings()
                    );
                }
                return _closeSettingsCommand;
            }
        }

        


        private bool CanEcexuteCloseSettings()
        {
            return true;
        }

        public void CloseSettingsExecute()
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
