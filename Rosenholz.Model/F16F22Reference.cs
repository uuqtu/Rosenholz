using Rosenholz.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Rosenholz.Model
{
    public class F16F22Reference : IComparable<F16F22Reference>, IEquatable<F16F22Reference>
    {
        private int _positionCounter;
        private int _itemCounter;
        private int _year;

        public F16F22Reference(string f16f22)
        {
            string[] splitted = f16f22.Split('_');
            PositionCounter = Roman.From(splitted[0]);
            ItemCounter = int.Parse(splitted[1]);
            Year = int.Parse(splitted[2]);
        }

        public int PositionCounter
        {
            get
            {
                return _positionCounter;
            }
            set
            {
                _positionCounter = value;
                OnPropertyChanged(nameof(PositionCounter));
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

        public string PositionCounterString => Roman.To(PositionCounter);
        public string ItemCounterString => ItemCounter.ToString("D3");
        public string YearString => Year.ToString();
        public string F22String => GetF22String();

        public string GetF22ReferenceString()
        {
            return F16F22Reference.ConvertToF22Reference(PositionCounter, ItemCounter, Year);
        }

        public string GetF22String()
        {
            return F16F22Reference.ConvertToF22Reference(PositionCounterString, ItemCounterString, YearString);
        }

        public static string ConvertToF22Reference(int position, int itemNumber, int year)
        {
            return position + "_" + itemNumber + "_" + year;
        }

        public static string ConvertToF22Reference(string position, string itemNumber, string year)
        {
            return position + "_" + itemNumber + "_" + year;
        }

        public static bool IsF22Reference(object candidate)
        {
            F16F22Reference obj = candidate as F16F22Reference;

            string f22string = obj.F22String;

            bool match = Regex.IsMatch(f22string, "");

            return match;
        }

        public static string NextF22(string f22)
        {
            
            F16F22Reference actual = new F16F22Reference(f22);

            string position = Roman.To(actual.PositionCounter);
            string item;
            string year;

            if(actual.Year == int.Parse(DateTime.Now.ToString("yy")))
            {
                 item = (actual.ItemCounter + 1).ToString("D3");
                 year = DateTime.Now.ToString("yy");
            }
            else if(actual.Year < int.Parse(DateTime.Now.ToString("yy")))
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

            return F16F22Reference.ConvertToF22Reference(position, item, year);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }




        public override bool Equals(object obj)
        {
            F16F22Reference t = obj as F16F22Reference;
            return (this.PositionCounterString == t.PositionCounterString) &&
                   (this.ItemCounterString == t.ItemCounterString) &&
                   (this.YearString == t.YearString);
        }

        /// <summary>
        /// Kleiner als 0 (null)	Diese Instanz befindet sich in der Sortierreihenfolge vor other.
        /// Zero	                Diese Instanz tritt in der Sortierreihenfolge an der gleichen Position wie other auf.
        /// Größer als 0 (null)	    Diese Instanz folgt in der Sortierreihenfolge auf other.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int CompareTo(F16F22Reference other)
        {
            bool? YearOfReferenceGreater = null;
            bool? ItemCounterReferenceGreter = null;
            bool? PositionCounterGreater = null;

            if (this.Year < other.Year)
                YearOfReferenceGreater = true;
            else if (this.Year > other.Year)
                YearOfReferenceGreater = false;
            else
                YearOfReferenceGreater = null;

            if (this.ItemCounter < other.ItemCounter)
                ItemCounterReferenceGreter = true;
            else
                ItemCounterReferenceGreter = false;

            if (this.PositionCounter < other.PositionCounter)
                PositionCounterGreater = true;
            else if (this.PositionCounter > other.PositionCounter)
                PositionCounterGreater = false;
            else
                PositionCounterGreater = null;


            if (PositionCounterGreater == true && ItemCounterReferenceGreter == true && YearOfReferenceGreater == true)
                return -2;
            if (PositionCounterGreater == true && ItemCounterReferenceGreter == true && YearOfReferenceGreater == false)
                throw new InvalidOperationException("Geht nicht, weil das Jahr einer früheren Position nicht größer sein kann als das Jahr der aktuellen Position.");
            if (PositionCounterGreater == true && ItemCounterReferenceGreter == false && YearOfReferenceGreater == true)
                return -1;
            if (PositionCounterGreater == true && ItemCounterReferenceGreter == false && YearOfReferenceGreater == false)
                throw new InvalidOperationException("Geht nicht, weil das Jahr einer früheren Position nicht größer sein kann als das Jahr der aktuellen Position.");
            if (PositionCounterGreater == false && ItemCounterReferenceGreter == true && YearOfReferenceGreater == true)
                throw new InvalidOperationException("Geht nicht, weil das Jahr einer früheren Position nicht größer sein kann als das Jahr der aktuellen Position.");
            if (PositionCounterGreater == false && ItemCounterReferenceGreter == true && YearOfReferenceGreater == false)
                return 1;
            if (PositionCounterGreater == false && ItemCounterReferenceGreter == false && YearOfReferenceGreater == true)
                throw new InvalidOperationException("Geht nicht, weil das Jahr einer früheren Position nicht größer sein kann als das Jahr der aktuellen Position.");
            if (PositionCounterGreater == false && ItemCounterReferenceGreter == false && YearOfReferenceGreater == false)
                return 1;
            // Gleiche Werte bei Position
            if (PositionCounterGreater == null && ItemCounterReferenceGreter == true && YearOfReferenceGreater == true)
                return -1;
            if (PositionCounterGreater == null && ItemCounterReferenceGreter == true && YearOfReferenceGreater == false)
                return 1;
            if (PositionCounterGreater == null && ItemCounterReferenceGreter == false && YearOfReferenceGreater == true)
                return -1;
            if (PositionCounterGreater == null && ItemCounterReferenceGreter == false && YearOfReferenceGreater == false)
                return 1;
            // Gleiche Werte bei Jahr
            if (PositionCounterGreater == true && ItemCounterReferenceGreter == true && YearOfReferenceGreater == null)
                return -1;
            if (PositionCounterGreater == false && ItemCounterReferenceGreter == true && YearOfReferenceGreater == null)
                return 1;
            if (PositionCounterGreater == true && ItemCounterReferenceGreter == false && YearOfReferenceGreater == null)
                return -1;
            if (PositionCounterGreater == false && ItemCounterReferenceGreter == false && YearOfReferenceGreater == null)
                return 1;
            // Gleiche Werte bei Position und Jahr
            if (PositionCounterGreater == null && ItemCounterReferenceGreter == true && YearOfReferenceGreater == null)
                return -1;
            if (PositionCounterGreater == null && ItemCounterReferenceGreter == false && YearOfReferenceGreater == null)
                return 1;

            throw new ArgumentException("Same Object can not occur!");
        }

        public bool Equals(F16F22Reference other)
        {
            return (this.PositionCounterString == other.PositionCounterString) &&
                   (this.ItemCounterString == other.ItemCounterString) &&
                   (this.YearString == other.YearString);
        }

        public override int GetHashCode()
        {
            int hashCode = -1868597832;
            hashCode = hashCode * -1521134295 + _positionCounter.GetHashCode();
            hashCode = hashCode * -1521134295 + _itemCounter.GetHashCode();
            hashCode = hashCode * -1521134295 + _year.GetHashCode();
            hashCode = hashCode * -1521134295 + PositionCounter.GetHashCode();
            hashCode = hashCode * -1521134295 + ItemCounter.GetHashCode();
            hashCode = hashCode * -1521134295 + Year.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PositionCounterString);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ItemCounterString);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(YearString);
            return hashCode;
        }
    }
}
