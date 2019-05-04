using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeMulti.Interfaces
{
    public class ViewModelBase : INotifyPropertyChanged, ICloseable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    
        public event EventHandler<EventArgs> RequestClose;

        protected virtual void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
