using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeMulti.Interfaces
{
    public abstract class ViewModelBase : INotifyPropertyChanged, ICloseable, IDisposable
    {
        public event EventHandler<EventArgs> RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool Result { set; get; } = false;

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public virtual void OnClosing(object sender, EventArgs e)
        {
            
        }

        protected virtual void OnRequestClose()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Dispose()
        {
            
        }

    }
}
