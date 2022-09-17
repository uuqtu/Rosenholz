using Microsoft.Win32;
using Rosenholz.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rosenholz.UserControls
{
    /// <summary>
    /// Interaction logic for TextEditor.xaml
    /// https://wpf-tutorial.com/rich-text-controls/how-to-creating-a-rich-text-editor/
    /// </summary>
    public partial class TextEditor : UserControl, INotifyPropertyChanged
    {
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

        public TextEditor()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            DataContext = this;
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        public void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(CurrentFolder == null)
                rtbEditor.Document.Blocks.Clear();

            if (CurrentFolder?.Contains(Settings.Settings.Instance.BasePath) != true)
                return;

            string noteDirectory = System.IO.Path.Combine(CurrentFolder, "_notes");
            string notePath = System.IO.Path.Combine(noteDirectory, "main.rft");

            if (File.Exists(notePath))
            {
                FileStream fileStream = new FileStream(notePath, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();
                fileStream.Dispose();
            }
            else
            {
                if (!Directory.Exists(noteDirectory))
                    Directory.CreateDirectory(noteDirectory);

                FileStream fileStream = new FileStream(notePath, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
                fileStream.Dispose();
            }

            //    OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            //if (dlg.ShowDialog() == true)
            //{
            //    FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
            //    TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            //    range.Load(fileStream, DataFormats.Rtf);
            //}
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CurrentFolder?.Contains(Settings.Settings.Instance.BasePath) != true)
                return;

            string noteDirectory = System.IO.Path.Combine(CurrentFolder, "_notes");
            string notePath = System.IO.Path.Combine(noteDirectory, "main.rft");

            if (!Directory.Exists(noteDirectory))
                Directory.CreateDirectory(noteDirectory);

            FileStream fileStream = new FileStream(notePath, FileMode.Create);
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();
            fileStream.Dispose();

            //SaveFileDialog dlg = new SaveFileDialog();
            //dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            //if (dlg.ShowDialog() == true)
            //{
            //    FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
            //    TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            //    range.Save(fileStream, DataFormats.Rtf);
            //}
        }

        private ViewModel.RelayCommand _addTask;
        public ViewModel.RelayCommand AddTask
        {
            get
            {
                if (_addTask == null)
                {
                    _addTask = new RelayCommand(
                        (parameter) => ExecuteAddTask(),
                        (parameter) => true
                    ); ;
                }
                return _addTask;
            }
        }

        private void ExecuteAddTask()
        {
            Save_Executed(null, null);

            string noteDirectory = System.IO.Path.Combine(CurrentFolder, "_notes");
            string notePath = System.IO.Path.Combine(noteDirectory, "main.rft");

            if (File.Exists(notePath))
            {
                FileStream fileStream = new FileStream(notePath, FileMode.Open);

                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
#warning Das geht so nicht.
                range.Text += Environment.NewLine + "<T:,D:>";
                range.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Save_Executed(null, null);
        }
    }
}
