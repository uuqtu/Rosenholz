using Rosenholz.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rosenholz.ViewModel.Memorex
{
    public class CategoryViewModel : ViewModelBase
    {
        public event EventHandler CategoryViewModelChanged;
        public ICommand AddCategoryCommand { get; set; }

        private string _category = String.Empty;

        public string Category
        {
            get => _category;
            set => SetField(ref _category, value);
        }

        public CategoryViewModel()
        {
            AddCategoryCommand = new RelayCommand<object>(ExecuteAdd, CanExecuteAdd);
        }

        [DebuggerStepThrough]
        private bool CanExecuteAdd(object obj)
        {
            if (Category != null)
                if (Category != String.Empty)
                    return true;
            return false;
        }

        private void ExecuteAdd(object obj)
        {
            Model.Storage.MemorexStorage.Instance.InserCategoryIfNotExist(Category);
            Category = String.Empty;
            CategoryViewModelChanged?.Invoke(null, null);
        }
    }
}
