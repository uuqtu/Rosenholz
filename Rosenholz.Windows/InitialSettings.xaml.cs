using Rosenholz.Model;
using Rosenholz.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        private ObservableCollection<string> _iniFiles;
        public ObservableCollection<string> IniFiles { get { return _iniFiles; } set { _iniFiles = value; OnPropertyChanged(nameof(IniFiles)); } }
        public InitialSettings()
        {
            GetIniFiles();
            SelectedIniFileItem = IniFiles[0];

            InitializeComponent();
            DataContext = this;
        }


        public void GetIniFiles()
        {
            var dir = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.ini");

            IniFiles = new ObservableCollection<string>(dir.ToList());
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

        private RelayCommand _createNewSettingsCommand;
        public RelayCommand CreateNewSettingsCommand
        {
            get
            {
                if (_createNewSettingsCommand == null)
                {
                    _createNewSettingsCommand = new RelayCommand(
                        (parameter) => CreateNewSettingsExecute(),
                        (parameter) => CanEcexuteCreateNewSettings()
                    );
                }
                return _createNewSettingsCommand;
            }
        }




        private bool CanEcexuteCreateNewSettings()
        {
            return true;
        }

        public void CreateNewSettingsExecute()
        {
            InputBox box = new InputBox("Wie soll die SettingsDatei heißen?");
            box.ShowDialog();
            var str = box.InputString;
            IniFile var = new IniFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{str}.ini"));
            GetIniFiles();
        }


        private string _selectedIniFileItem;
        public string SelectedIniFileItem
        {
            get
            {
                return _selectedIniFileItem;
            }
            set
            {
                var param = _selectedIniFileItem = value;
                Settings.Settings.Location = param;
                OnPropertyChanged(nameof(SelectedIniFileItem));
                Position = Position;
                F22Location = F22Location;
                F16Location = F16Location;
                BasePath = BasePath;
                FileTypeFilters = FileTypeFilters;
                TaskLocation = TaskLocation;
                TaskItemLocation = TaskItemLocation;
                AppBaseLocation = AppBaseLocation;
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
