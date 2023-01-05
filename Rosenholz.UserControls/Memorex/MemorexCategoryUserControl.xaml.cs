using Rosenholz.ViewModel.Memorex;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rosenholz.UserControls
{
    /// <summary>
    /// Interaktionslogik für CategoryView.xaml
    /// </summary>
    public partial class MemorexCategoryUserControl : UserControl
    {
        public CategoryViewModel CategoryVieModelObject { get; set; } = new CategoryViewModel();

        public MemorexCategoryUserControl()
        {
            InitializeComponent();
        }

        private void CategoryViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = CategoryViewControl;
        }
    }
}

