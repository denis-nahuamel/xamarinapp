
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace xamainazureapp.ViewModels
{
   public class Personal: INotifyPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;
        public bool _varon { get; set; }
        public bool Varon { get; set; }
        public bool varon
        {
            get
            {
                return Varon;
            }
            set
            {
                if (Varon != value)
                {
                    if (value != _varon)//ha cambiado 
                    {

                        if (value == true)
                        {
                            _mujer = false;
                            mujer = false;
                        }
                        else
                        {
                            _mujer = true;
                            mujer = true;
                        }
                        Varon = value;
                        _varon = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("varon"));


                    }
                    else
                    {
                        Varon = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("varon"));
                    }
                }
            }
        }
        public bool _mujer { get; set; }
        public bool Mujer { get; set; }
        public bool mujer
        {
            get
            {
                return Mujer;
            }
            set
            {
                if (Mujer != value)
                {
                    if (value != _mujer)//ha cambiado 
                    {

                        if (value == true)
                        {
                            _varon = false;
                            varon = false;
                        }
                        else
                        {
                            _varon = true;
                            varon = true;
                        }
                        Mujer = value;
                        _mujer = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("mujer"));


                    }
                    else
                    {
                        Mujer = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("mujer"));
                    }
                }
            }
        }
        public Personal()
        {
           /* varon = true;
            _varon = true¨*/
        }
    }
}
