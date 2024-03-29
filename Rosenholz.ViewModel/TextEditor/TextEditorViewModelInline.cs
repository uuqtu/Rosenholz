﻿using Markdig;
using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Rosenholz.ViewModel.TextEditor
{
    public class TextEditorViewModelInline : ViewModelBase
    {
        Microsoft.Win32.OpenFileDialog mDlgOpen = new Microsoft.Win32.OpenFileDialog();
        Microsoft.Win32.SaveFileDialog mDlgSave = new Microsoft.Win32.SaveFileDialog();


        private string _textBoxContent;
        private string _statusBar;
        private bool _isReadOnly = false;
        private string _filePath = "";
        private bool _emptyEditorIsDisabled = true;
        private double _selectedFontSize = 13;
        private RelayCommand _clearCommand;
        private RelayCommand openMarkupCommand;
        private RelayCommand _readOnlyModeCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _saveAsCommand;
        private RelayCommand _openMarkDownHelpCommand;

        public string StatusBar
        {
            get { return _statusBar; }
            set { _statusBar = value; OnPropertyChanged(nameof(StatusBar)); }
        }
        public string TextBoxContent
        {
            get { return _textBoxContent; }
            set { _textBoxContent = value; OnPropertyChanged(nameof(TextBoxContent)); }
        }
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged(nameof(FilePath)); }
        }
        public bool EmptyEditorIsDisabled
        {
            get { return _emptyEditorIsDisabled; }
            set { _emptyEditorIsDisabled = value; OnPropertyChanged(nameof(EmptyEditorIsDisabled)); }
        }

        public double SelectedFontSize
        {
            get { return _selectedFontSize; }
            set { _selectedFontSize = value; OnPropertyChanged(nameof(SelectedFontSize)); }
        }

        public List<double> FontSizes { get; } = new List<double>() { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };


        public TextEditorViewModelInline(bool defaultReadOnly = true, bool emptyEditorIsDisabled = true)
        {
            IsReadOnly = defaultReadOnly;
            EmptyEditorIsDisabled = emptyEditorIsDisabled;
        }


        public void LoadFileInEditor()
        {
            IsReadOnly = true;

            if (!File.Exists(FilePath))
                File.Create(FilePath).Close();

            var dir = Path.GetDirectoryName(FilePath);

            var file = Path.GetFileNameWithoutExtension(FilePath);

            File.Copy(FilePath, Path.Combine(dir, $"{file}_{DateTime.Now.ToFileTimeUtc()}.txt"));

            using (System.IO.StreamReader reader = new System.IO.StreamReader(FilePath))
            {
                TextBoxContent = reader.ReadToEnd();
                reader.Close();
            }
        }


        #region ClearCommand : Löscht die Inhalte im Texteditor
        public RelayCommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand(
                        (parameter) => ClearCommandExecute(parameter),
                        (parameter) => !IsReadOnly && false
                    );
                }
                return _clearCommand;
            }
        }
        public void ClearCommandExecute(object window)
        {
            var answ = MessageBox.Show("Clear?", "", MessageBoxButton.YesNo);
            if (answ == MessageBoxResult.Yes)
            {
                TextBoxContent = "";
                StatusBar = "Ready";
            }
            else
            {
                StatusBar = "Clear aborded";
            }
        }
        #endregion

        #region OpenMarkupCommand :Öffnet im Texteditor
        public RelayCommand OpenMarkupCommand
        {
            get
            {
                if (openMarkupCommand == null)
                {
                    openMarkupCommand = new RelayCommand(
                        (parameter) => OpenMarkupCommandExecute(parameter),
                        (parameter) => TextBoxContent != null && FilePath != null && FilePath?.Length > 0
                    );
                }
                return openMarkupCommand;
            }
        }
        public void OpenMarkupCommandExecute(object window)
        {
            Save();

            var result = Markdown.ToHtml(TextBoxContent);

            var filename = Path.GetFileNameWithoutExtension(FilePath);
            var dir = Path.GetDirectoryName(FilePath);

            string fileLocalization = Path.Combine(dir, $"{filename}.htm");

            File.WriteAllText(fileLocalization, @result);

            Process.Start(new ProcessStartInfo(fileLocalization) { UseShellExecute = true });

            StatusBar = "Mardown file rendered and displayed";

        }
        #endregion

        #region ReadOnlyMode : Versetzt in ReadONly Mode
        public RelayCommand ReadOnlyModeCommand
        {
            get
            {
                if (_readOnlyModeCommand == null)
                {
                    _readOnlyModeCommand = new RelayCommand(
                        (parameter) => ReadOnlyModeCommandExecute(parameter),
                        (parameter) => true
                    );
                }
                return _readOnlyModeCommand;
            }
        }
        public void ReadOnlyModeCommandExecute(object result)
        {
            //IsReadOnly = !(bool)result;
            StatusBar = (IsReadOnly ? "En" : "Dis") + "abled read only";
            Save();
        }
        #endregion

        #region SaveCommand : Speichert die Datei
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        (parameter) => SaveCommandCommandExecute(parameter),
                        (parameter) => TextBoxContent != null && FilePath != null && FilePath?.Length > 0
                    );
                }
                return _saveCommand;
            }
        }
        public void SaveCommandCommandExecute(object window)
        {
            Save(false);
        }
        #endregion

        #region SaveAsCommand : Versetzt in ReadONly Mode
        public RelayCommand SaveAsCommand
        {
            get
            {
                if (_saveAsCommand == null)
                {
                    _saveAsCommand = new RelayCommand(
                        (parameter) => SaveAsCommandCommandExecute(parameter),
                        (parameter) => TextBoxContent != null && FilePath != null && FilePath?.Length > 0
                    );
                }
                return _saveAsCommand;
            }
        }
        public void SaveAsCommandCommandExecute(object window)
        {
            Save(true);
        }
        #endregion

        #region OpenMarkDownHelpCommand : Öfffnet die Hilfe PDF zu Markdown
        public RelayCommand OpenMarkDownHelpCommand
        {
            get
            {
                if (_openMarkDownHelpCommand == null)
                {
                    _openMarkDownHelpCommand = new RelayCommand(
                        (parameter) => OpenMarkDownHelpCommandExecute(parameter),
                        (parameter) => true
                    );
                }
                return _openMarkDownHelpCommand;
            }
        }
        public void OpenMarkDownHelpCommandExecute(object window)
        {
            Process.Start(new ProcessStartInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Documents", "markdig_cheatsheet.pdf")) { UseShellExecute = true });
        }
        #endregion


        public void Save(bool foreSaveas = false)
        {
            if (!foreSaveas)
            {
                if (File.Exists(FilePath))
                {
                    SaveFile();
                    StatusBar = $"[{DateTime.Now.ToString("HH:MM:ss")}] - File saved @ {FilePath}";
                }
                else
                    StatusBar = "Text not saved to file.";
            }
            else if (File.Exists(FilePath))
            {
                if (mDlgSave.ShowDialog() == true)
                {
                    FilePath = mDlgSave.FileName;
                    SaveFile();
                    StatusBar = $"[{DateTime.Now.ToString("HH:MM:ss")}] - File saved @ {FilePath}";
                }
                else
                    StatusBar = "Text not saved to file.";
            }
        }



        private void SaveFile()
        {
            var dir = Path.GetDirectoryName(FilePath);
            var file = Path.GetFileNameWithoutExtension(FilePath);
            File.Copy(FilePath, Path.Combine(dir, $"{file}_{DateTime.Now.ToFileTimeUtc()}.txt"));

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(FilePath))
            {
                writer.Write(TextBoxContent);
                writer.Close();
            }

            StatusBar = "Wrote " + TextBoxContent?.Length.ToString() + " chars in " + FilePath;

            CleanUpNotes(dir);
        }

        private static void CleanUpNotes(string dir)
        {
            try
            {
                var files = Directory.GetFiles(dir);

                string parent = Directory.GetParent(dir).Parent.FullName;
                string archivLocation = Path.Combine(parent, "Archiv");

                if (Directory.Exists(parent))
                    Directory.CreateDirectory(archivLocation);

                string[] types = { "main_", "extranotes1_", "extranotes2_", "extranotes3_" };

                foreach (var type in types)
                {
                    int numbre = files.Where(s => s.Contains(type)).Count();

                    if (numbre > 50)
                    {
                        var toKeep = files.Where(s => Path.GetFileNameWithoutExtension(s).Contains("main_")).OrderByDescending(t => DateTime.FromFileTimeUtc(long.Parse(Path.GetFileNameWithoutExtension(t).Split('_')[1]))).Take(100).ToList();
                        var toDelete = files.Where(s => s.Contains("main_")).Where(t => !toKeep.Contains(t)).ToList();

                        foreach (var item in toDelete)
                        {
                            if (Path.GetFileName(item) != "main.txt" &&
                                Path.GetFileName(item) != "extranotes1.txt" &&
                                Path.GetFileName(item) != "extranotes2.txt" &&
                                Path.GetFileName(item) != "extranotes3.txt")
                            {
                                string archiveFileName = $"{Model.AUReference.GetAUStringFromPath(item).Value}_{Path.GetFileName(item)}";
                                File.Move(item, Path.Combine(archivLocation, archiveFileName));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
