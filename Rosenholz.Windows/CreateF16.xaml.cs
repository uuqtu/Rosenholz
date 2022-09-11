using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CreateF16.xaml
    /// </summary>
    public partial class CreateF16 : Window
    {
        private string _currentF22Reference = "";
        public string CurrentF22Reference
        {
            get
            {
                return _currentF22Reference;
            }
            set
            {
                _currentF22Reference = value;
                OnPropertyChanged(nameof(CurrentF22Reference));
            }
        }

        public string ProposedF22Reference
        {
            get
            {
                var a = F16F22Reference.NextF22(CurrentF22Reference);
                return a;
            }
        }
        public CreateF16(string currentF22)
        {
            InitializeComponent();
            DataContext = this;
            CurrentF22Reference = currentF22;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
