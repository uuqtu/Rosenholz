using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfTutorialStep1
{
    public class vm : INotifyPropertyChanged
    {
        Microsoft.Win32.OpenFileDialog mDlgOpen = new Microsoft.Win32.OpenFileDialog();
        Microsoft.Win32.SaveFileDialog mDlgSave = new Microsoft.Win32.SaveFileDialog();
        System.IO.StreamReader reader = null;

        private string _textBoxContent;
        private string _statusBar;
        private bool _isReadOnly = false;
        private string _filePath = "";
        private RelayCommand _clearCommand;
        private RelayCommand _openCommand;
        private RelayCommand _readOnlyModeCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _saveAsCommand;

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

        public vm(string filePath = "")
        {
            FilePath = filePath;

            if (filePath == "")
                return;

            if (!File.Exists(filePath))
                File.Create(filePath);

            reader = new System.IO.StreamReader(filePath);
            TextBoxContent = reader.ReadToEnd();
            reader.Close();
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
                        (parameter) => !IsReadOnly
                    );
                }
                return _clearCommand;
            }
        }
        public void ClearCommandExecute(object window)
        {
            TextBoxContent = "";
            StatusBar = "Ready";
        }
        #endregion

        #region OpenCommand : Löscht die Inhalte im Texteditor
        public RelayCommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(
                        (parameter) => OpenCommandExecute(parameter),
                        (parameter) => true
                    );
                }
                return _openCommand;
            }
        }
        public void OpenCommandExecute(object window)
        {
            if (mDlgOpen.ShowDialog() == true)
            {


                StatusBar = "Read " + mDlgOpen.FileName;
            }
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
        }
        #endregion

        #region SaveCommand : Versetzt in ReadONly Mode
        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        (parameter) => SaveCommandCommandExecute(parameter),
                        (parameter) => true
                    );
                }
                return _saveCommand;
            }
        }
        public void SaveCommandCommandExecute(object window)
        {
            SaveFileAs();
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
                        (parameter) => true
                    );
                }
                return _saveAsCommand;
            }
        }
        public void SaveAsCommandCommandExecute(object window)
        {
            SaveFileAs();
        }
        #endregion

        private void SaveFileAs()
        {
            if (mDlgSave.ShowDialog() == true)
                SaveFile();
            else
                StatusBar = "Text not saved to file.";
        }

        private void SaveFile()
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(mDlgSave.FileName);
            writer.Write(TextBoxContent);
            writer.Close();
            StatusBar = "Wrote " + TextBoxContent.Length.ToString() + " chars in " + mDlgSave.FileName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
