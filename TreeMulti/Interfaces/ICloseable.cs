using System;

namespace TreeMulti.Interfaces
{
    public interface ICloseable
    {
        event EventHandler<EventArgs> RequestClose;
    }
}