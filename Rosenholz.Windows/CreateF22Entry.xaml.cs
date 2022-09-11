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
    /// Interaction logic for CreateF22.xaml
    /// </summary>
    public partial class CreateF22Entry : Window
    {
        private string _aUReferenceCurrent;
        private string _f16ReferenceCurrent;

        public string AUReferenceCurrent
        {
            get
            { return _aUReferenceCurrent; }
            set
            {
                _aUReferenceCurrent = value;
                OnPropertyChanged(nameof(AUReferenceCurrent));
            }
        }

        public string F16ReferenceCurrent
        {
            get
            { return _f16ReferenceCurrent; }
            set
            {
                _f16ReferenceCurrent = value;
                OnPropertyChanged(nameof(F16ReferenceCurrent));
            }
        }


        public string ProposedAuReference
        {
            get
            {
                var a = AUReference.NextAU(AUReferenceCurrent);
                return a;
            }
        }
        public CreateF22Entry(F16F22Reference currentF16Reference)
        {
            InitializeComponent();
            DataContext = this;

            var Items = F22.Storage.ReadData();

            F16ReferenceCurrent = currentF16Reference.F22String;
            AUReferenceCurrent = Items[0].AUReference.AUReferenceString;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
