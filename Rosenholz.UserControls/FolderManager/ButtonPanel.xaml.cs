using Rosenholz.Extensions;
using Rosenholz.Model;
using Rosenholz.Sniper;
using Rosenholz.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;
using Path = System.IO.Path;
using RelayCommand = Rosenholz.Model.RelayCommand;

namespace Rosenholz.UserControls
{
    /// <summary>
    /// Interaction logic for ButtonPanel.xaml
    /// </summary>
    public delegate void TaskCreationRequired(string currentFolder);
    public partial class ButtonPanel : UserControl, INotifyPropertyChanged
    {
        public event TaskCreationRequired TaskCreationRequiredEvent;
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
            Process.Start(new ProcessStartInfo(CurrentFolder) { UseShellExecute = true });
        }
        #endregion

        #region Copy AU Foldername
        private RelayCommand _copyAuFolderNameCommand;
        public RelayCommand CopyAuFolderNameCommand
        {
            get
            {
                if (_copyAuFolderNameCommand == null)
                {
                    _copyAuFolderNameCommand = new RelayCommand(
                        (parameter) => CopyAuFolderNameExecute(),
                        (parameter) => CanEcexuteCopyAuFolderName()
                    );
                }
                return _copyAuFolderNameCommand;
            }
        }

        private bool CanEcexuteCopyAuFolderName()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CopyAuFolderNameExecute()
        {
            var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
            string t = "";

            if (rslt.Result)
            {
                t = $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__";
            }
            else
            {
                t = $"{DateTime.Now.ToString("MM_dd")}__";
            }
            Clipboard.SetText(t);
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
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__{wdow.InputString}.pptx");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "powerpoint.pptx"),
                             t);
                    Process.Start(new ProcessStartInfo(t) { UseShellExecute = true });
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
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__{wdow.InputString}.xlsx");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "excel.xlsx"),
                             t);
                    Process.Start(new ProcessStartInfo(t) { UseShellExecute = true });
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
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__{wdow.InputString}.mpp");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "Project.mpp"),
                             t);
                    Process.Start(new ProcessStartInfo(t) { UseShellExecute = true });
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
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__{wdow.InputString}.vsdx");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "visio.vsdx"),
                             t);
                    Process.Start(new ProcessStartInfo(t) { UseShellExecute = true });
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
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__{wdow.InputString}.txt");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "textfile.txt"),
                             t);
                    Process.Start(new ProcessStartInfo(t) { UseShellExecute = true });
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
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__{wdow.InputString}.sql");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "sqlfile.sql"),
                             t);
                    Process.Start(new ProcessStartInfo(t) { UseShellExecute = true });
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
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string proj = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}.Rproj");
                    if (!File.Exists(proj))
                    {
                        File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "rproject.Rproj"),
                            proj);
                    }

                    string t = System.IO.Path.Combine(CurrentFolder, $"{rslt.Value}_{DateTime.Now.ToString("MM_dd")}__{wdow.InputString}.R");
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "RFile.R"),
                             t);
                    Process.Start(new ProcessStartInfo(proj) { UseShellExecute = true });
                }
            }
        }
        #endregion

        #region CopyLinkClipboard New RFile

        private RelayCommand _copyLinkToClipboardCommand;
        public RelayCommand CopyLinkToClipboardCommand
        {
            get
            {
                if (_copyLinkToClipboardCommand == null)
                {
                    _copyLinkToClipboardCommand = new RelayCommand(
                        (parameter) => CopyLinkToClipboardExecute(),
                        (parameter) => CanEcexuteCopyLinkToClipboard()
                    );
                }
                return _copyLinkToClipboardCommand;
            }
        }


        private bool CanEcexuteCopyLinkToClipboard()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CopyLinkToClipboardExecute()
        {
            Clipboard.SetText(CurrentFolder);
            Thread.Sleep(500);
        }
        #endregion

        #region Create New Task

        private RelayCommand _createNewTaskCommand;
        public RelayCommand CreateNewTaskCommand
        {
            get
            {
                if (_createNewTaskCommand == null)
                {
                    _createNewTaskCommand = new RelayCommand(
                        (parameter) => CreateNewTaskExecute(),
                        (parameter) => CanEcexuteCreateNewTask()
                    );
                }
                return _createNewTaskCommand;
            }
        }


        private bool CanEcexuteCreateNewTask()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewTaskExecute()
        {
            TaskCreationRequiredEvent?.Invoke(CurrentFolder);
        }
        #endregion

        #region Create New Link

        private RelayCommand _createNewLinkCommand;
        public RelayCommand CreateNewLinkCommand
        {
            get
            {
                if (_createNewLinkCommand == null)
                {
                    _createNewLinkCommand = new RelayCommand(
                        (parameter) => CreateNewLinkExecute(),
                        (parameter) => CanEcexuteCreateNewLink()
                    );
                }
                return _createNewLinkCommand;
            }
        }


        private bool CanEcexuteCreateNewLink()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateNewLinkExecute()
        {
            LinkCaputreBox link = new LinkCaputreBox("Geben Sie Benennung und Link ein");
            link.ShowDialog();

            if (string.IsNullOrWhiteSpace(link?.InputString))
                return;


            using (StreamWriter writer = new StreamWriter(Path.Combine(CurrentFolder, string.Join("_", link.InputString.Split(Path.GetInvalidFileNameChars())) + ".url")))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + link.LinkString);
                writer.Flush();
            }
        }
        #endregion

        #region Create Tex

        private RelayCommand _createTexCommand;
        public RelayCommand CreateTexCommand
        {
            get
            {
                if (_createTexCommand == null)
                {
                    _createTexCommand = new RelayCommand(
                        (parameter) => CreateTexExecute(),
                        (parameter) => CanEcexuteCreateTex()
                    );
                }
                return _createTexCommand;
            }
        }


        private bool CanEcexuteCreateTex()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CreateTexExecute()
        {
            Rosenholz.Extensions.InputBox wdow = new Rosenholz.Extensions.InputBox("Geben Sie einen Titel für die Datei ein.", true);
            wdow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(wdow.InputString))
            {
                var rslt = Model.AUReference.GetAUStringFromPath(CurrentFolder);
                if (rslt.Result)
                {
                    string dir1 = System.IO.Path.Combine(CurrentFolder, wdow.InputString);
                    string dir2 = System.IO.Path.Combine(CurrentFolder, wdow.InputString, "bib");

                    if (!Directory.Exists(dir2))
                        Directory.CreateDirectory(dir2);

                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "Tex", "fphw.cls"), Path.Combine(dir1, "fphw.cls"));
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "Tex", "litverz.bib"), Path.Combine(dir1, "litverz.bib"));
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "Tex", "main.tex"), Path.Combine(dir1, "main.tex"));
                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "Tex", "main_vorlage.tex"), Path.Combine(dir1, "main_vorlage.tex"));

                    File.Copy(System.IO.Path.Combine(Settings.Settings.Instance.AppBaseLocation, "Templates", "Tex", "bib", "bib.ctv6"), Path.Combine(dir2, "main_vorlage.tex"));



                    Process.Start(new ProcessStartInfo(Path.Combine(dir1, "main.tex")) { UseShellExecute = true });
                }
            }
        }
        #endregion

        #region Capture Screen

        private RelayCommand _catureScreenCommand;
        public RelayCommand CaptureScreenCommand
        {
            get
            {
                if (_catureScreenCommand == null)
                {
                    _catureScreenCommand = new RelayCommand(
                        (parameter) => CaptureScreenCommandExecute(),
                        (parameter) => CanCaptureScreenCommand()
                    );
                }
                return _catureScreenCommand;
            }
        }


        private bool CanCaptureScreenCommand()
        {
            return !string.IsNullOrWhiteSpace(CurrentFolder);
        }

        public void CaptureScreenCommandExecute()
        {
            using (ScreenshotWindow window = new ScreenshotWindow(CurrentFolder))
            {
                window.ShowDialog();
                //Swindow.Save();
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





