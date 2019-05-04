using System;

namespace TreeMulti.Interfaces
{
    internal interface ICloseable
    {
        event EventHandler<EventArgs> RequestClose;
    }
}