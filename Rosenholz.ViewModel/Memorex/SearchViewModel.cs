using Rosenholz.Model;
using Rosenholz.Model.Memorex;
using Rosenholz.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Rosenholz.ViewModel.Memorex
{
    public class SearchViewModel : ViewModelBase
    {
        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OpenCommand { get; set; }

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
            OpenCommand = new RelayCommand<object>(ExecuteOpenCommand, CanOpenCommand);
        }

        #region Open
        private bool CanOpenCommand(object obj)
        {
            if (SelectedElement == null)
                return false;
            else if (String.IsNullOrEmpty(SelectedElement.Link))
                return false;
            else
                return true;
        }

        private void ExecuteOpenCommand(object obj)
        {
            var element = SelectedElement.Link;
            Process.Start("explorer", element);
        }
        #endregion 

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

        public void LoadItems()
        {
            var a = Rosenholz.Model.Storage.MemorexStorage.Instance.ReadData();
            KnowledgeElements = new ObservableCollection<KnowledgeElement>(a);
            _knowledgeElementCollectionView = new ListCollectionView(KnowledgeElements);
            OnPropertyChanged(nameof(KnowledgeElements));
            OnPropertyChanged(nameof(KnowledgeElementCollectionView));
        }
        #endregion

        #region ClearCommand

        private void ExecuteClear(object obj)
        {
            KnowledgeElements.Clear();
            TextFilter = String.Empty;
            SelectedElement = null;
            LoadItems();
            OnPropertyChanged(nameof(KnowledgeElements));
            OnPropertyChanged(nameof(KnowledgeElementCollectionView));
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
            Rosenholz.Extensions.MessageBox box = new Extensions.MessageBox("Info", $"Eintrag mit den Suchworten \"{SelectedElement?.Searchwords}\" und Link \"{SelectedElement?.Link}\" wirklich löschen?");
            box.ShowDialog();

            ; if (box.DialogResult == true)
            {
                Model.Storage.MemorexStorage.Instance.DeleteEntry(SelectedElement.Guid);
            }

            LoadItems();
            OnPropertyChanged(nameof(KnowledgeElements));
            OnPropertyChanged(nameof(KnowledgeElementCollectionView));
        }

        #endregion
    }
}
