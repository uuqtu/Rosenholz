using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{
    public class F16
    {
        private string _keyword;
        private string _label;
        private string _purpose;
        private F16F22Reference _f16f22reference;
        public static F16Storage Storage = new F16Storage();

        public F16()
        {
            
        }

        public F16(string keyword, string label, string purpose, string reference)
        {
            Keyword = keyword;
            Label = label;
            Purpose = purpose;
            F16F22Reference = new F16F22Reference(reference); 
        }

        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = value;
                OnPropertyChanged(nameof(Keyword));
            }
        }
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                OnPropertyChanged(nameof(Label));
            }
        }
        public string Purpose
        {
            get
            {
                return _purpose;
            }
            set
            {
                _purpose = value;
                OnPropertyChanged(nameof(Purpose));
            }
        }
        public F16F22Reference F16F22Reference
        {
            get
            {
                return _f16f22reference;
            }
            set
            {
                _f16f22reference = value;
                OnPropertyChanged(nameof(F16F22Reference));
            }
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
