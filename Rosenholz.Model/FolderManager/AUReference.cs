﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Rosenholz.Model
{
    public class AUReference : IComparable<AUReference>, IEquatable<AUReference>
    {
        private string _initializer;
        private int _itemCounter;
        private int _year;

        public AUReference(string au)
        {
            string[] splitted = au.Split('_');
            Initializer = splitted[0];
            ItemCounter = int.Parse(splitted[1]);
            Year = int.Parse(splitted[2]);
        }

        public string Initializer
        {
            get
            {
                return _initializer;
            }
            set
            {
                _initializer = value;
                OnPropertyChanged(nameof(Initializer));
            }
        }

        public int ItemCounter
        {
            get
            {
                return _itemCounter;
            }
            set
            {
                _itemCounter = value;
                OnPropertyChanged(nameof(ItemCounter));
            }
        }

        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        public string ItemCounterString => ItemCounter.ToString("D3");
        public string YearString => Year.ToString();
        public string AUReferenceString => GetAUString();

        public static string GetMetadataF1622Reference(string auref)
        {
            var elem = Rosenholz.Model.F22Storage.Instance.ReadData();
            var tst = elem.First(x => x.AUReference.AUReferenceString.Equals(auref));
            return tst.F16F22Reference.F22String;
        }


        public string GetAUReferenceString()
        {
            return AUReference.ConvertToAUReference(Initializer, ItemCounter, Year);
        }

        public string GetAUString()
        {
            return AUReference.ConvertToAUReference(Initializer, ItemCounterString, YearString);
        }

        public static string ConvertToAUReference(string initializer, int itemNumber, int year)
        {
            return initializer + "_" + itemNumber + "_" + year;
        }

        public static string ConvertToAUReference(string initializer, string itemNumber, string year)
        {
            return initializer + "_" + itemNumber + "_" + year;
        }

        public static string NextAU(string au)
        {
            AUReference actual = new AUReference(au);

            string[] splitted = actual.AUReferenceString.Split('_');


            string initializer = "AU";
            string item = splitted[1];
            string year = splitted[2];

            if (actual.Year == int.Parse(DateTime.Now.ToString("yy")))
            {
                item = (actual.ItemCounter + 1).ToString("D3");
                year = DateTime.Now.ToString("yy");
            }
            else if (actual.Year < int.Parse(DateTime.Now.ToString("yy")))
            {
                item = "001";
                year = DateTime.Now.ToString("yy");
            }
            else
            {
                item = "???";
                year = "??";
                MessageBox.Show($"Das aktuelle Jahr ist {int.Parse(DateTime.Now.ToString("yy"))}, ich weiß nicht welches Jahr ich nun nehmen soll," +
                                $" da da aktelle Jahr der neusten F22-Referenz in der Zukunft liegt {actual.Year}.");
            }

            return AUReference.ConvertToAUReference(initializer, item, year);
        }

        public static bool IsAUReference(object candidate)
        {
            AUReference obj = candidate as AUReference;

            string austring = obj.AUReferenceString;

            bool match = Regex.IsMatch(austring, "^[A][U][_]\\d\\d\\d[_]\\d\\d$");

            return match;
        }

        public static (bool Result, string Value) GetAUStringFromPath(string path)
        {
            string pattern = @"[A][U]_[0-9]{3,4}_\d\d";
            Match match = Regex.Match(path, pattern);

            return (match.Success, match.Value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(AUReference other)
        {
            bool? ItemCounterReferenceGreter = null;
            bool? YearOfReferenceGreater = null;

            if (this.Year < other.Year)
                YearOfReferenceGreater = true;
            else if (this.Year > other.Year)
                YearOfReferenceGreater = false;
            else
                YearOfReferenceGreater = null;

            if (this.ItemCounter < other.ItemCounter)
                ItemCounterReferenceGreter = true;
            else if (this.ItemCounter > other.ItemCounter)
                ItemCounterReferenceGreter = false;
            else
                ItemCounterReferenceGreter = null;

            if (YearOfReferenceGreater == true && ItemCounterReferenceGreter == true)
                return -1;
            if (YearOfReferenceGreater == true && ItemCounterReferenceGreter == false)
                return -1;
            if (YearOfReferenceGreater == true && ItemCounterReferenceGreter == null)
                return -1;
            if (YearOfReferenceGreater == false && ItemCounterReferenceGreter == true)
                return 1;
            if (YearOfReferenceGreater == false && ItemCounterReferenceGreter == false)
                return 1;
            if (YearOfReferenceGreater == false && ItemCounterReferenceGreter == null)
                return 1;
            if (YearOfReferenceGreater == null && ItemCounterReferenceGreter == true)
                return -1;
            if (YearOfReferenceGreater == null && ItemCounterReferenceGreter == false)
                return 1;
            if (YearOfReferenceGreater == null && ItemCounterReferenceGreter == null)
                return 0;

            throw new ArgumentException("Fehler beim Vergleich");

        }

        public bool Equals(AUReference other)
        {
            return (this.Initializer == other.Initializer) &&
                   (this.ItemCounterString == other.ItemCounterString) &&
                   (this.YearString == other.YearString);
        }
    }
}
