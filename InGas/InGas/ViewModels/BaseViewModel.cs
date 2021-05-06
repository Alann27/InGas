using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InGas.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected BaseViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
