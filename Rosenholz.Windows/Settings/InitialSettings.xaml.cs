using Rosenholz.Model;
using Rosenholz.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Path = System.IO.Path;

namespace Rosenholz.Windows
{
    /// <summary>
    /// Interaktionslogik für InitialSettings.xaml
    /// </summary>
    public partial class InitialSettings : Window, INotifyPropertyChanged
    {
        private ObservableCollection<string> _iniFiles;
        private static string FilePath = "FilePath";
        private static string FileName = "FileName";
        private static string Storage = "Storage";
        private static string Admin = "Admin";
        private static string Organization = "Organization";
        private static string MainSettingsName = "settings.ini";

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

            // Exclude the MainSettingsName since it will be rewritten with the information selected.
            IniFiles = new ObservableCollection<string>(dir.ToList().Where(s => !s.Contains(MainSettingsName)));
        }


        #region Administrative 

        private string _storageBaseLocation;
        public string StorageBaseLocation
        {
            get => _storageBaseLocation;
            set => SetField(ref _storageBaseLocation, value, nameof(StorageBaseLocation));
        }

        private string _position;
        public string Position
        {
            get => _position;
            set => SetField(ref _position, value, nameof(Position));
        }
        #endregion

        #region SubLocations

        private string _f16SubLocation;
        public string F16SubLocation
        {
            get => _f16SubLocation;
            set => SetField(ref _f16SubLocation, value, nameof(F16SubLocation));
        }

        private string _f22SubLocation;
        public string F22SubLocation
        {
            get => _f22SubLocation;
            set => SetField(ref _f22SubLocation, value, nameof(F22SubLocation));
        }

        private string _taskSubLocation;
        public string TaskSubLocation
        {
            get => _taskSubLocation;
            set => SetField(ref _taskSubLocation, value, nameof(TaskSubLocation));
        }

        private string _aUSubLocation;
        public string AUSubLocation
        {
            get => _aUSubLocation;
            set => SetField(ref _aUSubLocation, value, nameof(AUSubLocation));
        }

        private string _completionOfAssignmentsLocation;
        public string CompletionOfAssignmentsLocation
        {
            get => _completionOfAssignmentsLocation;
            set => SetField(ref _completionOfAssignmentsLocation, value, nameof(CompletionOfAssignmentsLocation));
        }

        private string _memorexSubLocation;
        public string MemorexSubLocation
        {
            get => _memorexSubLocation;
            set => SetField(ref _memorexSubLocation, value, nameof(MemorexSubLocation));
        }
        #endregion

        #region Filenames

        private string _f16FileName;
        public string F16FileName
        {
            get => _f16FileName;
            set => SetField(ref _f16FileName, value, nameof(F16FileName));
        }

        private string _f22FileName;
        public string F22FileName
        {
            get => _f22FileName;
            set => SetField(ref _f22FileName, value, nameof(F22FileName));
        }

        private string _tasksFileName;
        public string TasksFileName
        {
            get => _tasksFileName;
            set => SetField(ref _tasksFileName, value, nameof(TasksFileName));
        }

        private string _taskItemsFileName;
        public string TaskItemsFileName
        {
            get => _taskItemsFileName;
            set => SetField(ref _taskItemsFileName, value, nameof(TaskItemsFileName));
        }

        private string _taskLinkFileName;
        public string TaskLinkFileName
        {
            get => _taskLinkFileName;
            set => SetField(ref _taskLinkFileName, value, nameof(TaskLinkFileName));
        }

        private string _memorexFileName;
        public string MemorexFileName
        {
            get => _memorexFileName;
            set => SetField(ref _memorexFileName, value, nameof(MemorexFileName));
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
                        (parameter) => { DialogResult = false; this.Close(); },
                        (parameter) => true
                    );
                }
                return _closeSettingsCommand;
            }
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
            Rosenholz.Extensions.InputBox box = new Rosenholz.Extensions.InputBox($"Wie soll die SettingsDatei heißen? Es wir vorne der Computername angehangen {Environment.MachineName + "_"}", true);
            box.ShowDialog();

            var str = Environment.MachineName + "_" + box.InputString;
            if (string.IsNullOrWhiteSpace(str))
                return;

            IniFile var = new IniFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{str}.ini"));
            var.IniWriteValue(Admin, nameof(Position), 2);
            var.IniWriteValue(Storage, nameof(StorageBaseLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS"));
            #region File Path
            var.IniWriteValue(FilePath, nameof(F22SubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZVK"));
            var.IniWriteValue(FilePath, nameof(F16SubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZPK"));
            var.IniWriteValue(FilePath, nameof(TaskSubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZTV"));
            var.IniWriteValue(FilePath, nameof(AUSubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZAV"));
            var.IniWriteValue(FilePath, nameof(MemorexSubLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "ZLV"));
            var.IniWriteValue(FilePath, nameof(CompletionOfAssignmentsLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS", "CoA"));
            #endregion
            #region File Name
            var.IniWriteValue(FileName, nameof(F22FileName), "f22.db");
            var.IniWriteValue(FileName, nameof(F16FileName), "f16.db");
            var.IniWriteValue(FileName, nameof(TasksFileName), "tasks.db");
            var.IniWriteValue(FileName, nameof(TaskItemsFileName), "taskitems.db");
            var.IniWriteValue(FileName, nameof(TaskLinkFileName), "linkedtaskitems.db");
            var.IniWriteValue(FileName, nameof(MemorexFileName), "memorex.db");            
            #endregion

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
                Position                        = var.IniReadValue("Admin", nameof(Position));
                StorageBaseLocation             = var.IniReadValue("Storage", nameof(StorageBaseLocation));
                #region FilePath
                F22SubLocation                  = var.IniReadValue("FilePath", nameof(F22SubLocation));
                F16SubLocation                  = var.IniReadValue("FilePath", nameof(F16SubLocation));
                TaskSubLocation                 = var.IniReadValue("FilePath", nameof(TaskSubLocation));
                AUSubLocation                   = var.IniReadValue("FilePath", nameof(AUSubLocation));
                MemorexSubLocation              = var.IniReadValue("FilePath", nameof(MemorexSubLocation));
                CompletionOfAssignmentsLocation = var.IniReadValue("FilePath", nameof(CompletionOfAssignmentsLocation));
                #endregion
                #region FileName
                F22FileName                     = var.IniReadValue("FileName", nameof(F22FileName));
                F16FileName                     = var.IniReadValue("FileName", nameof(F16FileName));
                TasksFileName                   = var.IniReadValue("FileName", nameof(TasksFileName));
                TaskItemsFileName               = var.IniReadValue("FileName", nameof(TaskItemsFileName));
                TaskLinkFileName                = var.IniReadValue("FileName", nameof(TaskLinkFileName));
                MemorexFileName                 = var.IniReadValue("FileName", nameof(MemorexFileName));
                #endregion

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
                var.IniWriteValue("Organization", "MemorexSubLocation", MemorexSubLocation);
                var.IniWriteValue("Organization", "CoALocation", CompletionOfAssignmentsLocation);
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
                var.IniWriteValue("FileName", nameof(MemorexFileName), MemorexFileName);
                var.IniWriteValue("FilePath", nameof(CompletionOfAssignmentsLocation), CompletionOfAssignmentsLocation);
                var.IniWriteValue("FilePath", nameof(MemorexSubLocation), MemorexSubLocation);

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
                settings.IniWriteValue("FileName", nameof(MemorexFileName), MemorexFileName);
                settings.IniWriteValue("FilePath", nameof(CompletionOfAssignmentsLocation), CompletionOfAssignmentsLocation);
                settings.IniWriteValue("FilePath", nameof(MemorexSubLocation), MemorexSubLocation);

                DialogResult = true;

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
                        (parameter) =>
                        {
                            DialogResult = true; this.Close();
                        },
                        (parameter) => File.Exists(MainSettingsName)
                    );
                }
                return _penDefaultCommand;
            }
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

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
