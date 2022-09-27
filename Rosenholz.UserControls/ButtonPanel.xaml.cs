using Rosenholz.Model;
using Rosenholz.ViewModel;
using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        #region Open Folder
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
        #endregion
        #region Create New PowerPoint

        private RelayCommand _createNewPowerPointCommand;
        public RelayCommand CreateNewPowerPointCommand
        {
            get
            {
                if (_createNewPowerPointCommand == null)
                {
                    _createNewPowerPointCommand = new RelayCommand(
                        (parameter) => CreateNewPowerPointExecute(),
                        (parameter) => CanEcexuteCreateNewPowerPoint()
                    );
                }
                return _createNewPowerPointCommand;
            }
        }



        private bool CanEcexuteCreateNewPowerPoint()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewPowerPointExecute()
        {
            InputBox wdow = new InputBox("Geben Sie einen Titel für die Datei ein.");
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{wdow.InputString}.pptx");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "powerpoint.pptx"),
                             t);
                    Process.Start(t);
                }
            }
        }
        #endregion

        #region Create New Excel

        private RelayCommand _createNewExcelCommand;
        public RelayCommand CreateNewExcelCommand
        {
            get
            {
                if (_createNewExcelCommand == null)
                {
                    _createNewExcelCommand = new RelayCommand(
                        (parameter) => CreateNewExcelExecute(),
                        (parameter) => CanEcexuteCreateNewExcel()
                    );
                }
                return _createNewExcelCommand;
            }
        }



        private bool CanEcexuteCreateNewExcel()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewExcelExecute()
        {
            InputBox wdow = new InputBox("Geben Sie einen Titel für die Datei ein.");
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{wdow.InputString}.xlsx");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "excel.xlsx"),
                             t);
                    Process.Start(t);
                }
            }
        }
        #endregion

        #region Create New Project

        private RelayCommand _createNewProjectCommand;
        public RelayCommand CreateNewProjectCommand
        {
            get
            {
                if (_createNewProjectCommand == null)
                {
                    _createNewProjectCommand = new RelayCommand(
                        (parameter) => CreateNewProjectExecute(),
                        (parameter) => CanEcexuteCreateNewProject()
                    );
                }
                return _createNewProjectCommand;
            }
        }



        private bool CanEcexuteCreateNewProject()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewProjectExecute()
        {
            InputBox wdow = new InputBox("Geben Sie einen Titel für die Datei ein.");
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{wdow.InputString}.mpp");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "Project.mpp"),
                             t);
                    Process.Start(t);
                }
            }
        }
        #endregion

        #region Create New Visio

        private RelayCommand _createNewVisioCommand;
        public RelayCommand CreateNewVisioCommand
        {
            get
            {
                if (_createNewVisioCommand == null)
                {
                    _createNewVisioCommand = new RelayCommand(
                        (parameter) => CreateNewVisioExecute(),
                        (parameter) => CanEcexuteCreateNewVisio()
                    );
                }
                return _createNewVisioCommand;
            }
        }

      
        private bool CanEcexuteCreateNewVisio()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewVisioExecute()
        {
            InputBox wdow = new InputBox("Geben Sie einen Titel für die Datei ein.");
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{wdow.InputString}.vsdx");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "visio.vsdx"),
                             t);
                    Process.Start(t);
                }
            }
        }
        #endregion

        #region Create New TextFile

        private RelayCommand _createNewTextFileCommand;
        public RelayCommand CreateNewTextFileCommand
        {
            get
            {
                if (_createNewTextFileCommand == null)
                {
                    _createNewTextFileCommand = new RelayCommand(
                        (parameter) => CreateNewTextFileExecute(),
                        (parameter) => CanEcexuteCreateNewTextFile()
                    );
                }
                return _createNewTextFileCommand;
            }
        }


        private bool CanEcexuteCreateNewTextFile()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewTextFileExecute()
        {
            InputBox wdow = new InputBox("Geben Sie einen Titel für die Datei ein.");
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{wdow.InputString}.txt");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "textfile.txt"),
                             t);
                    Process.Start(t);
                }
            }
        }
        #endregion

        #region Create New SQLFile

        private RelayCommand _createNewSQLFileCommand;
        public RelayCommand CreateNewSQLFileCommand
        {
            get
            {
                if (_createNewSQLFileCommand == null)
                {
                    _createNewSQLFileCommand = new RelayCommand(
                        (parameter) => CreateNewSQLFileExecute(),
                        (parameter) => CanEcexuteCreateNewSQLFile()
                    );
                }
                return _createNewSQLFileCommand;
            }
        }


        private bool CanEcexuteCreateNewSQLFile()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewSQLFileExecute()
        {
            InputBox wdow = new InputBox("Geben Sie einen Titel für die Datei ein.");
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{wdow.InputString}.sql");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "sqlfile.sql"),
                             t);
                    Process.Start(t);
                }
            }
        }
        #endregion

        #region Create New RFile

        private RelayCommand _createNewRFileCommand;
        public RelayCommand CreateNewRFileCommand
        {
            get
            {
                if (_createNewRFileCommand == null)
                {
                    _createNewRFileCommand = new RelayCommand(
                        (parameter) => CreateNewRFileExecute(),
                        (parameter) => CanEcexuteCreateNewRFile()
                    );
                }
                return _createNewRFileCommand;
            }
        }


        private bool CanEcexuteCreateNewRFile()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewRFileExecute()
        {
            InputBox wdow = new InputBox("Geben Sie einen Titel für die Datei ein.");
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{wdow.InputString}.R");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "RFile.R"),
                             t);
                    Process.Start(t);
                }
            }
        }
        #endregion






        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
