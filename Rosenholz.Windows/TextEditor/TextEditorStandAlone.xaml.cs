using Rosenholz.ViewModel.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Rosenholz.Windows.TextEditor
{
    /// <summary>
    /// Interaktionslogik für TextEditor.xaml
    /// </summary>
    public partial class TextEditorStandAlone : Window, INotifyPropertyChanged
    {
        public TextEditorViewModelStandAlone vmo { get; set; } = null;
        private string _currentFolder = "";



        public string CurrentFolder
        {
            get { return _currentFolder; }
            set { _currentFolder = value; OnPropertyChanged(nameof(CurrentFolder)); }
        }


        public TextEditorStandAlone(string path)
        {
            InitializeComponent();
            vmo = new TextEditorViewModelStandAlone(path, true, false);
            this.DataContext = vmo;

            vmo.LoadFileInEditor();
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(vmo.FilePath))
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
