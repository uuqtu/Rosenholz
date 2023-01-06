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
        private static string MainSettingsName = "settings.ini";
        private static string SettingsFileEnding = ".ini";

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
            var dir = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"*{SettingsFileEnding}");

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

        private string _completionOfAssignmentsSubLocation;
        public string CompletionOfAssignmentsSubLocation
        {
            get => _completionOfAssignmentsSubLocation;
            set => SetField(ref _completionOfAssignmentsSubLocation, value, nameof(CompletionOfAssignmentsSubLocation));
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

        private string _completionOfAssignmentsFileName;
        public string CompletionOfAssignmentsFileName
        {
            get => _completionOfAssignmentsFileName;
            set => SetField(ref _completionOfAssignmentsFileName, value, nameof(CompletionOfAssignmentsFileName));
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

            InitializeIniFileAndWrite(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{str}{SettingsFileEnding}"));

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
                IniReadValues(parameter.ToString());
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
                IniWriteValues(parameter.ToString());
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
                IniWriteValues(parameter.ToString());
                File.Delete(MainSettingsName);
                IniWriteValues(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MainSettingsName));

                DialogResult = true;

                this.Close();
            }
        }
        #endregion
        /// <summary>
        /// Initializes the Ini File
        /// </summary>
        /// <param name="str"></param>
        private static void InitializeIniFileAndWrite(string patToIniFile)
        {
            IniFile var = new IniFile(patToIniFile);
            var.IniWriteValue(Admin, nameof(Position), 2);
            var.IniWriteValue(Storage, nameof(StorageBaseLocation), System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%desktop%"), "MFS"));
            #region File Path
            var.IniWriteValue(FilePath, nameof(F22SubLocation), "ZVK");
            var.IniWriteValue(FilePath, nameof(F16SubLocation), "ZPK");
            var.IniWriteValue(FilePath, nameof(TaskSubLocation), "ZTV");
            var.IniWriteValue(FilePath, nameof(AUSubLocation), "ZAV");
            var.IniWriteValue(FilePath, nameof(MemorexSubLocation), "ZLV");
            var.IniWriteValue(FilePath, nameof(CompletionOfAssignmentsSubLocation), "CoA");
            #endregion
            #region File Name
            var.IniWriteValue(FileName, nameof(F22FileName), "f22.db");
            var.IniWriteValue(FileName, nameof(F16FileName), "f16.db");
            var.IniWriteValue(FileName, nameof(TasksFileName), "tasks.db");
            var.IniWriteValue(FileName, nameof(TaskItemsFileName), "taskitems.db");
            var.IniWriteValue(FileName, nameof(TaskLinkFileName), "linkedtaskitems.db");
            var.IniWriteValue(FileName, nameof(MemorexFileName), "memorex.db");
            var.IniWriteValue(FileName, nameof(CompletionOfAssignmentsFileName), "CoA.db");
            #endregion
        }
        /// <summary>
        /// Writes Values to the selected Ini FIle
        /// </summary>
        /// <param name="patToIniFile"></param>
        private void IniWriteValues(string patToIniFile)
        {
            IniFile var = new IniFile(patToIniFile);
            var.IniWriteValue(Admin, nameof(Position), Position);
            var.IniWriteValue(Storage, nameof(StorageBaseLocation), StorageBaseLocation);
            #region FilePaths
            var.IniWriteValue(FilePath, nameof(F22SubLocation), F22SubLocation);
            var.IniWriteValue(FilePath, nameof(F16SubLocation), F16SubLocation);
            var.IniWriteValue(FilePath, nameof(TaskSubLocation), TaskSubLocation);
            var.IniWriteValue(FilePath, nameof(AUSubLocation), AUSubLocation);
            var.IniWriteValue(FilePath, nameof(CompletionOfAssignmentsSubLocation), CompletionOfAssignmentsSubLocation);
            var.IniWriteValue(FilePath, nameof(MemorexSubLocation), MemorexSubLocation);
            #endregion
            #region FileName
            var.IniWriteValue(FileName, nameof(F22FileName), F22FileName);
            var.IniWriteValue(FileName, nameof(F16FileName), F16FileName);
            var.IniWriteValue(FileName, nameof(TasksFileName), TasksFileName);
            var.IniWriteValue(FileName, nameof(TaskItemsFileName), TaskItemsFileName);
            var.IniWriteValue(FileName, nameof(TaskLinkFileName), TaskLinkFileName);
            var.IniWriteValue(FileName, nameof(MemorexFileName), MemorexFileName);
            var.IniWriteValue(FileName, nameof(CompletionOfAssignmentsFileName), CompletionOfAssignmentsFileName);
            #endregion
        }
        /// <summary>
        /// Reads the values from the Ini File
        /// </summary>
        /// <param name="parameter"></param>
        private void IniReadValues(string parameter)
        {
            IniFile var = new IniFile(parameter);
            Position = var.IniReadValue(Admin, nameof(Position));
            StorageBaseLocation = var.IniReadValue(Storage, nameof(StorageBaseLocation));
            #region FilePath
            F22SubLocation = var.IniReadValue(FilePath, nameof(F22SubLocation));
            F16SubLocation = var.IniReadValue(FilePath, nameof(F16SubLocation));
            TaskSubLocation = var.IniReadValue(FilePath, nameof(TaskSubLocation));
            AUSubLocation = var.IniReadValue(FilePath, nameof(AUSubLocation));
            MemorexSubLocation = var.IniReadValue(FilePath, nameof(MemorexSubLocation));
            CompletionOfAssignmentsSubLocation = var.IniReadValue(FilePath, nameof(CompletionOfAssignmentsSubLocation));
            #endregion
            #region FileName
            F22FileName = var.IniReadValue(FileName, nameof(F22FileName));
            F16FileName = var.IniReadValue(FileName, nameof(F16FileName));
            TasksFileName = var.IniReadValue(FileName, nameof(TasksFileName));
            TaskItemsFileName = var.IniReadValue(FileName, nameof(TaskItemsFileName));
            TaskLinkFileName = var.IniReadValue(FileName, nameof(TaskLinkFileName));
            MemorexFileName = var.IniReadValue(FileName, nameof(MemorexFileName));
            CompletionOfAssignmentsFileName = var.IniReadValue(FileName, nameof(CompletionOfAssignmentsFileName));
            #endregion
        }
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
