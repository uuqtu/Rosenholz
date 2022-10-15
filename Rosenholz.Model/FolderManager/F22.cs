using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{
    public class F22
    {
        private AUReference _aUReference;
        private F16F22Reference _f16f22reference;
        private string _pseudonym;
        private string _created;
        private string _dossier;
        private string _link;       

        public F22()
        {

        }

        public F22(string aUReference, string f16f22reference, string pseudonym, string created, string dossier, string link)
        {
            AUReference = new AUReference(aUReference);
            F16F22Reference = new F16F22Reference(f16f22reference);
            Pseudonym = pseudonym;
            Created = created;
            Dossier = dossier;
            Link = link;
        }
        public AUReference AUReference
        {
            get
            {
                return _aUReference;
            }
            set
            {
                _aUReference = value;
                OnPropertyChanged(nameof(AUReference));
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

        public string Pseudonym
        {
            get
            {
                return _pseudonym;
            }
            set
            {
                _pseudonym = value;
                OnPropertyChanged(nameof(Pseudonym));
            }
        }

        public string Created
        {
            get
            {
                return _created;
            }
            set
            {
                _created = value;
                OnPropertyChanged(nameof(Created));
            }
        }

        public string Dossier
        {
            get
            {
                return _dossier;
            }
            set
            {
                _dossier = value;
                OnPropertyChanged(nameof(Dossier));
            }
        }

        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
                OnPropertyChanged(nameof(Link));
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
