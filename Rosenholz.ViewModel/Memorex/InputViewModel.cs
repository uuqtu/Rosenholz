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
    public class InputViewModel : ViewModelBase
    {
        public static EventHandler InputViewModelChanged;
        public ICommand AddCommand { get; set; }
        private string _searchwords = String.Empty;
        private string _link = String.Empty;
        private string _category = String.Empty;
        public string Link
        {
            get => _link;
            set => SetField(ref _link, value);
        }
        public string Searchwords
        {
            get => _searchwords;
            set => SetField(ref _searchwords, value);
        }
        public string Category
        {
            get => _category;
            set => SetField(ref _category, value);
        }

        public IList<string> PossibleCategories
        {
            get
            {
                return Rosenholz.Model.Storage.MemorexStorage.Instance.ReadCategoryData();
            }
        }


        public InputViewModel()
        {
            AddCommand = new RelayCommand<object>(ExecuteAdd, CanExecuteAdd);
        }

        [DebuggerStepThrough]
        private bool CanExecuteAdd(object obj)
        {
            if (Link != null && Searchwords != null && Category != null)
                if (Link != String.Empty && Searchwords != String.Empty && Category != String.Empty)
                    return true;
            return false;
        }

        private void ExecuteAdd(object obj)
        {
            Rosenholz.Model.Storage.MemorexStorage.Instance.InsertData(new Model.Memorex.KnowledgeElement(Link, Searchwords, Category));
            Link = String.Empty;
            Searchwords = String.Empty;
            Category = String.Empty;
        }
    }
}
