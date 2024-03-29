﻿using Rosenholz.Model;
using Rosenholz.Model.RomanNumerals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Rosenholz.ViewModel
{
    public delegate void F22ContextChanged(F16F22Reference reference);
    public delegate void F16EntryRequired(string currentF22, List<F16> list);
    public class F16ViewModel : ViewModelBase
    {
        private ObservableCollection<F16> _f16List;
        private string _textFilter;
        private string _reference;
        private string _latestItem;
        private F16 _currentF16Selected = null;
        public event F22ContextChanged F22ContextChangeEvent;
        public event F16EntryRequired F16EntryRequiredEvent;
        private ListCollectionView _f16CollectionView;
        public ICollectionView F16CollectionView
        {
            get { return this._f16CollectionView; }
        }

        public F16ViewModel()
        {
            F16ViewModel.ArchiveF16Execute();
        }

        public string TextFilter
        {
            get { return this._textFilter; }
            set
            {
                _textFilter = value;
                OnPropertyChanged(nameof(TextFilter));

                //https://stackoverflow.com/questions/15473048/create-a-textboxsearch-to-filter-from-listview-wpf
                if (String.IsNullOrEmpty(value))
                    F16CollectionView.Filter = null;
                else
                    F16CollectionView.Filter = new Predicate<object>(o => ((F16)o).Keyword?.ToLower()?.Contains(value.ToLower()) == true ||
                                                                       ((F16)o).Label?.ToLower()?.Contains(value.ToLower()) == true ||
                                                                       ((F16)o).Purpose?.ToLower()?.Contains(value.ToLower()) == true);
            }
        }



        public string Reference
        {
            get
            {
                return _reference;
            }
            set
            {
                _reference = value;
                OnPropertyChanged(nameof(Reference));
            }
        }

        public F16 CurrentF16Selected
        {
            get
            {
                return _currentF16Selected;
            }
            set
            {
                _currentF16Selected = value;
                if (_currentF16Selected != null)
                    F22ContextChangeEvent?.Invoke(_currentF16Selected.F16F22Reference);
                else
                    F22ContextChangeEvent?.Invoke(new F16F22Reference("I_000_00"));
                OnPropertyChanged(nameof(CurrentF16Selected));

#warning WriteF16ItemsCommandExecute auskommentiert.
                //try
                //{
                //    WriteF16ItemsCommandExecute();
                //}
                //catch
                //{

                //}
            }
        }


        public string LatestItem
        {
            get
            {
                return _latestItem;
            }
            set
            {
                _latestItem = value;
                OnPropertyChanged(nameof(LatestItem));
            }
        }

        private void UpdateLatestItem()
        {
            string retval = $"I_000_0";
            var elements = F16Storage.Instance.ReadData();
            if (elements.Count > 0)
            {
                LatestItem = F16Storage.Instance.ReadData().ElementAt(0).F16F22Reference.F22String;
            }
        }


        public ObservableCollection<F16> F16Items
        {
            get { return _f16List; }
            set
            {
                _f16List = value;
                OnPropertyChanged(nameof(F16Items));
            }
        }


        public void LoadF16Items()
        {
            var a = F16Storage.Instance.ReadData();
            F16Items = new ObservableCollection<F16>(a);
            _f16CollectionView = new ListCollectionView(F16Items);
            OnPropertyChanged(nameof(F16CollectionView));
            UpdateLatestItem();
        }

        #region Show

        private RelayCommand _writeF16ItemsCommand
            ;
        public RelayCommand WriteF16ItemsCommand
        {
            get
            {
                if (_writeF16ItemsCommand == null)
                {
                    _writeF16ItemsCommand = new RelayCommand(
                        (parameter) => WriteF16ItemsCommandExecute(),
                        (parameter) => CanExecuteWriteF16ItemsCommand(parameter)
                    );
                }
                return _writeF16ItemsCommand;
            }
        }

        public static void WriteF16ItemsCommandExecute()
        {

            var items = F16Storage.Instance.ReadData();
            string dir = Settings.Settings.Instance.F16SubLocation;


            string literals = "_ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            foreach (char literal in literals)
            {
                string path = Path.Combine(dir, literal.ToString());
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            foreach (var item in items)
            {
                string fileName = item.Keyword;
                string label = item.Label;
                string purpose = item.Purpose;
                string f22 = item.F16F22Reference.F22String;

                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }

                string[] lines = { $"Stichwort: {fileName}", $"Genaue Bezeichnung: {label}", $"Gegenstand: {purpose}", $"F22: {f22}" };

                var character = fileName[0];

                if (!Directory.Exists(Path.Combine(dir, character.ToString())))
                    Directory.CreateDirectory(Path.Combine(dir, character.ToString()));

                File.WriteAllLines(Path.Combine(dir, character.ToString(), $"{fileName}.txt"), lines);
            }
        }
        public bool CanExecuteWriteF16ItemsCommand(object parameter)
        {
            //            var text = (string)parameter;
            //#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
            //            var val = F16Items.ToList().Any(s => s.F16F22Reference.F22String.Equals(parameter));
            //#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast

            return true;
        }

        #endregion


        private RelayCommand _addF16Command;
        public RelayCommand AddF16Command
        {
            get
            {
                if (_addF16Command == null)
                {
                    _addF16Command = new RelayCommand(
                        (parameter) => AddF16Execute(parameter),
                        (parameter) => true
                    );
                }
                return _addF16Command;
            }
        }

        public void AddF16Execute(object parameter)
        {
            var currentF22 = (string)parameter;

            F16EntryRequiredEvent?.Invoke(currentF22, F16Items.ToList());

            LoadF16Items();
        }

        private RelayCommand _archiveF16Command;
        public RelayCommand ArchiveF16Command
        {
            get
            {
                if (_archiveF16Command == null)
                {
                    _archiveF16Command = new RelayCommand(
                        (parameter) => ArchiveF16Execute(),
                        (parameter) => true
                    );
                }
                return _archiveF16Command;
            }
        }

        public static void ArchiveF16Execute()
        {
            string dir = Settings.Settings.Instance.F16SubLocation;

            if (!Directory.Exists(Path.Combine(dir, "_archive")))
                Directory.CreateDirectory(Path.Combine(dir, "_archive"));

            string location = Path.Combine(dir, "_archive", $"f16_{DateTime.Now.ToFileTimeUtc()}.zip");

            string src = Path.Combine(Settings.Settings.Instance.F16SubLocation, Settings.Settings.Instance.F16FileName);
            string dest = Path.Combine(Settings.Settings.Instance.F16SubLocation, "_archive", $"{DateTime.Now.ToFileTimeUtc()}_{Settings.Settings.Instance.F16FileName}");

            File.Copy(src, dest);

            //using (ZipArchive zip = ZipFile.Open(location, ZipArchiveMode.Create))
            //{
            //    try
            //    {
            //        zip.CreateEntryFromFile(Settings.Settings.Instance.F16SubLocation, Settings.Settings.Instance.F16FileName);
            //    }
            //    catch (Exception ex)
            //    {

            //        MessageBox.Show(ex.Message);
            //    }
            //}
        }
    }
}

