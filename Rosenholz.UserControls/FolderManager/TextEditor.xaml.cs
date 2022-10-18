using Microsoft.Win32;
using Rosenholz.Model;
using Rosenholz.ViewModel;
using Rosenholz.ViewModel.TextEditor;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rosenholz.UserControls
{
    /// <summary>
    /// Interaction logic for TextEditor.xaml
    /// https://wpf-tutorial.com/rich-text-controls/how-to-creating-a-rich-text-editor/
    /// </summary>
    public partial class TextEditorUserControl : UserControl, INotifyPropertyChanged
    {
        public TextEditorViewModelInline vmo { get; set; } = null;
        private string _currentFolder = "";



        public string CurrentFolder
        {
            get { return _currentFolder; }
            set { _currentFolder = value; OnPropertyChanged(nameof(CurrentFolder)); }
        }


        public TextEditorUserControl()
        {
            InitializeComponent();
            vmo = new TextEditorViewModelInline(true, false, true);
            this.DataContext = vmo;
        }


        public void LoadFile(string path)
        {
            vmo.FilePath = path;
            vmo.LoadFileInEditor();
        }


        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            vmo?.Save();
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
