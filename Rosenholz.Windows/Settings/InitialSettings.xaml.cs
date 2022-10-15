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
        private string _selectedIniPath;
        public string SelectedIniPath
        {
            get { return _selectedIniPath; }
            set
            {
                _selectedIniPath = value;
                OnPropertyChanged(nameof(SelectedIniPath));
            }
        }
        public InitialSettings()
        {
            GetIniFiles();
            InitializeComponent();
            DataContext = this;
        }


        public void GetIniFiles()
        {
            var dir = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.ini");


            IniFiles = new ObservableCollection<string>(dir.ToList().Where(s => !s.Contains("settings.ini")));
        }

        private string _storageBaseLocation;
        public string StorageBaseLocation
        {
            get { return _storageBaseLocation; }
            set
            {
                _storageBaseLocation = value;
                OnPropertyChanged(nameof(StorageBaseLocation));
            }
        }

        private string _position;
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private string _f22SubLocation;
        public string F22SubLocation
        {
            get { return _f22SubLocation; }
            set
            {
                _f22SubLocation = value;
                OnPropertyChanged(nameof(F22SubLocation));
            }
        }


        private string _f16SubLocation;
        public string F16SubLocation
        {
            get { return _f16SubLocation; }
            set
            {
                _f16SubLocation = value;
                OnPropertyChanged(nameof(F16SubLocation));
            }
        }




        private string _taskSubLocation;
        public string TaskSubLocation
        {
            get { return _taskSubLocation; }
            set
            {
                _taskSubLocation = value;
                OnPropertyChanged(nameof(TaskSubLocation));
            }
        }

        private string _aUSubLocation;
        public string AUSubLocation
        {
            get { return _aUSubLocation; }
            set
            {
                _aUSubLocation = value;
                OnPropertyChanged(nameof(AUSubLocation));
            }
        }

        #region Filenames

        private string _f22FileName;
        public string F22FileName
        {
            get { return _f22FileName; }
            set
            {
                _f22FileName = value;
                OnPropertyChanged(nameof(F22FileName));
            }
        }

        private string _f16FileName;
        public string F16FileName
        {
            get { return _f16FileName; }
            set
            {
                _f16FileName = value;
                OnPropertyChanged(nameof(F16FileName));
            }
        }

        private string _tasksFileName;
        public string TasksFileName
        {
            get { return _tasksFileName; }
            set
            {
                _tasksFileName = value;
                OnPropertyChanged(nameof(TasksFileName));
            }
        }


        private string _taskItemsFileName;
        public string TaskItemsFileName
        {
            get { return _taskItemsFileName; }
            set
            {
                _taskItemsFileName = value;
                OnPropertyChanged(nameof(TaskItemsFileName));
            }
        }

        private string _taskLinkFileName;
        public string TaskLinkFileName
        {
            get { return _taskLinkFileName; }
            set
            {
                _taskLinkFileName = value;
                OnPropertyChanged(nameof(TaskLinkFileName));
            }
        }

        #endregion




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


        #region CreateNewSettingsCommand
        private RelayCommand _createNewSettingsCommand;
        public RelayCommand CreateNewSettingsCommand
        {
            get
            {
                if (_createNewSettingsCommand == null)
                {
                    _createNewSettingsCommand = new RelayCommand(
                        (parameter) => CreateNewSettingsExecute(),
                        (parameter) => true
                    );
                }
                return _createNewSettingsCommand;
            }
        }
        public void CreateNewSettingsExecute()
        {
            InputBox box = new InputBox("Wie soll die SettingsDatei heißen?");
            box.ShowDialog();
            var str = box.InputString;
            IniFile var = new IniFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{str}.ini"));
            var.IniWriteValue("Admin", nameof(Position), 2);
            var.IniWriteValue("Storage", nameof(StorageBaseLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS"));
            var.IniWriteValue("FilePath", nameof(F22SubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZVK"));
            var.IniWriteValue("FilePath", nameof(F16SubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZPK"));
            var.IniWriteValue("FilePath", nameof(TaskSubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZTV"));
            var.IniWriteValue("FilePath", nameof(AUSubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZAV"));
            var.IniWriteValue("FileName", nameof(F22FileName), "f22.db");
            var.IniWriteValue("FileName", nameof(F16FileName), "f16.db");
            var.IniWriteValue("FileName", nameof(TasksFileName), "tasks.db");
            var.IniWriteValue("FileName", nameof(TaskItemsFileName), "taskitems.db");
            var.IniWriteValue("FileName", nameof(TaskLinkFileName), "linkedtaskitems.db");

            GetIniFiles();
        }
        #endregion

        #region LoadSettingsCommand
        private RelayCommand _loadSettingsCommand;
        public RelayCommand LoadSettingsCommand
        {
            get
            {
                if (_loadSettingsCommand == null)
                {
                    _loadSettingsCommand = new RelayCommand(
                        (parameter) => LoadSettingsCommandExecute(parameter),
                        (parameter) => !String.IsNullOrWhiteSpace(SelectedIniPath)
                    );
                }
                return _loadSettingsCommand;
            }
        }
        public void LoadSettingsCommandExecute(object parameter)
        {
            if (parameter is string)
            {
                IniFile var = new IniFile(parameter.ToString());
                Position = var.IniReadValue("Admin", nameof(Position));
                StorageBaseLocation = var.IniReadValue("Storage", nameof(StorageBaseLocation));
                F22SubLocation = var.IniReadValue("FilePath", nameof(F22SubLocation));
                F16SubLocation = var.IniReadValue("FilePath", nameof(F16SubLocation));
                TaskSubLocation = var.IniReadValue("FilePath", nameof(TaskSubLocation));
                AUSubLocation = var.IniReadValue("FilePath", nameof(AUSubLocation));
                F22FileName = var.IniReadValue("FileName", nameof(F22FileName));
                F16FileName = var.IniReadValue("FileName", nameof(F16FileName));
                TasksFileName = var.IniReadValue("FileName", nameof(TasksFileName));
                TaskItemsFileName = var.IniReadValue("FileName", nameof(TaskItemsFileName));
                TaskLinkFileName = var.IniReadValue("FileName", nameof(TaskLinkFileName));
                GetIniFiles();
            }
        }
        #endregion

        #region SaveSettingsCommand
        private RelayCommand _saveSettingsCommand;
        public RelayCommand SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null)
                {
                    _saveSettingsCommand = new RelayCommand(
                        (parameter) => SaveSettingsCommandExecute(parameter),
                        (parameter) => !String.IsNullOrWhiteSpace(SelectedIniPath)
                    );
                }
                return _saveSettingsCommand;
            }
        }
        public void SaveSettingsCommandExecute(object parameter)
        {
            if (parameter is string)
            {
                IniFile var = new IniFile(parameter.ToString());
                var.IniWriteValue("Admin", "Position", Position);
                var.IniWriteValue("Organization", "F22SubLocation", F22SubLocation);
                var.IniWriteValue("Organization", "F16SubLocation", F16SubLocation);
                var.IniWriteValue("Organization", "AUSubLocation", AUSubLocation);
                var.IniWriteValue("Organization", "TaskSubLocation", TaskSubLocation);
                var.IniWriteValue("Storage", "StorageBaseLocation", StorageBaseLocation);
                GetIniFiles();
            }
        }
        #endregion

        #region SetSettingsAndOpenCommand
        private RelayCommand _setSettingsAndOpenCommand;
        public RelayCommand SetSettingsAndOpenCommand
        {
            get
            {
                if (_setSettingsAndOpenCommand == null)
                {
                    _setSettingsAndOpenCommand = new RelayCommand(
                        (parameter) => SetSettingsAndOpenCommandExecute(parameter),
                        (parameter) => !String.IsNullOrWhiteSpace(SelectedIniPath)
                    );
                }
                return _setSettingsAndOpenCommand;
            }
        }
        public void SetSettingsAndOpenCommandExecute(object parameter)
        {
            if (parameter is string)
            {
                IniFile var = new IniFile(parameter.ToString());
                var.IniWriteValue("Admin", nameof(Position), Position);
                var.IniWriteValue("Storage", nameof(StorageBaseLocation), StorageBaseLocation);
                var.IniWriteValue("FilePath", nameof(F22SubLocation), F22SubLocation);
                var.IniWriteValue("FilePath", nameof(F16SubLocation), F16SubLocation);
                var.IniWriteValue("FilePath", nameof(TaskSubLocation), TaskSubLocation);
                var.IniWriteValue("FilePath", nameof(AUSubLocation), AUSubLocation);
                var.IniWriteValue("FileName", nameof(F22FileName), F22FileName);
                var.IniWriteValue("FileName", nameof(F16FileName), F16FileName);
                var.IniWriteValue("FileName", nameof(TasksFileName), TasksFileName);
                var.IniWriteValue("FileName", nameof(TaskItemsFileName), TaskItemsFileName);
                var.IniWriteValue("FileName", nameof(TaskLinkFileName), TaskLinkFileName);

                File.Delete("settings.ini");

                IniFile settings = new IniFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini"));
                settings.IniWriteValue("Admin", nameof(Position), Position);
                settings.IniWriteValue("Storage", nameof(StorageBaseLocation), StorageBaseLocation);
                settings.IniWriteValue("FilePath", nameof(F22SubLocation), F22SubLocation);
                settings.IniWriteValue("FilePath", nameof(F16SubLocation), F16SubLocation);
                settings.IniWriteValue("FilePath", nameof(TaskSubLocation), TaskSubLocation);
                settings.IniWriteValue("FilePath", nameof(AUSubLocation), AUSubLocation);
                settings.IniWriteValue("FileName", nameof(F22FileName), F22FileName);
                settings.IniWriteValue("FileName", nameof(F16FileName), F16FileName);
                settings.IniWriteValue("FileName", nameof(TasksFileName), TasksFileName);
                settings.IniWriteValue("FileName", nameof(TaskItemsFileName), TaskItemsFileName);
                settings.IniWriteValue("FileName", nameof(TaskLinkFileName), TaskLinkFileName);

                this.Close();
            }
        }
        #endregion

        #region OpenDefaultCommand
        private RelayCommand _penDefaultCommand;
        public RelayCommand OpenDefaultCommand
        {
            get
            {
                if (_penDefaultCommand == null)
                {
                    _penDefaultCommand = new RelayCommand(
                        (parameter) => OpenDefaultCommandExecute(parameter),
                        (parameter) => File.Exists("settings.ini")
                    );
                }
                return _penDefaultCommand;
            }
        }
        public void OpenDefaultCommandExecute(object parameter)
        {
            this.Close();
        }
        #endregion



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSettingsCommandExecute(SelectedIniPath);
        }
    }
}
