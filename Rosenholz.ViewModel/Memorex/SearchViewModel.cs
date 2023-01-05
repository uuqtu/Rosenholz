using Rosenholz.Model.Memorex;
using Rosenholz.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Rosenholz.Model;
using System.Windows.Data;
using System.ComponentModel;

namespace Rosenholz.ViewModel.Memorex
{
    public class SearchViewModel : ViewModelBase
    {
        public static EventHandler SearchViewModelChanged;
        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private KnowledgeElement _selectedElement = null;
        private ObservableCollection<KnowledgeElement> _knowledgeElements;
        private ListCollectionView _knowledgeElementCollectionView;
        private string _textFilter;

        public KnowledgeElement SelectedElement
        {
            get => _selectedElement;
            set
            {
                SetField(ref _selectedElement, value);
                Clipboard.SetText(value?.Link ?? "");
            }
        }

        public ObservableCollection<KnowledgeElement> KnowledgeElements { get { return _knowledgeElements; } set { _knowledgeElements = value; OnPropertyChanged(nameof(KnowledgeElements)); } }

        public ICollectionView KnowledgeElementCollectionView
        {
            get { return this._knowledgeElementCollectionView; }
        }


        public SearchViewModel()
        {
            ClearCommand = new RelayCommand<object>(ExecuteClear);
            DeleteCommand = new RelayCommand<object>(ExecuteDeleteCommand, CanDeleteCommand);
        }
        #region SearchCommand

        public string TextFilter
        {
            get { return this._textFilter; }
            set
            {
                _textFilter = value;
                OnPropertyChanged(nameof(TextFilter));

                //https://stackoverflow.com/questions/15473048/create-a-textboxsearch-to-filter-from-listview-wpf
                if (String.IsNullOrEmpty(value))
                    KnowledgeElementCollectionView.Filter = null;
                else
                    KnowledgeElementCollectionView.Filter = new Predicate<object>(o => ((KnowledgeElement)o).Searchwords?.ToLower()?.Contains(value.ToLower()) == true);
            }
        }

        public void LoadItems(Rosenholz.Model.TaskState state)
        {
            var a = Rosenholz.Model.Storage.MemorexStorage.Instance.ReadData();
            KnowledgeElements = new ObservableCollection<KnowledgeElement>(a);
            _knowledgeElementCollectionView = new ListCollectionView(KnowledgeElements);
            OnPropertyChanged(nameof(KnowledgeElements));
        }
        #endregion

        #region ClearCommand

        private void ExecuteClear(object obj)
        {
            KnowledgeElements.Clear();
            OnPropertyChanged(nameof(KnowledgeElements));
            TextFilter = String.Empty;
            SelectedElement = null;
            SearchViewModelChanged?.Invoke(null, null);
        }
        #endregion

        #region DeleteCommand

        [DebuggerStepThrough]
        private bool CanDeleteCommand(object obj)
        {
            return SelectedElement != null;
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var rslt = System.Windows.MessageBox.Show($"Eintrag mit den Suchworten \n\"{SelectedElement?.Searchwords}\" \nund Link \n\"{SelectedElement?.Link}\" \n wirklich löschen?", "", MessageBoxButton.YesNo);
            if (rslt == MessageBoxResult.Yes)
            {
                Model.Storage.MemorexStorage.Instance.DeleteEntry(SelectedElement.Guid);
            }
        }

        #endregion
    }
}
