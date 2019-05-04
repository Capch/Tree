using System;
using System.Windows;

namespace TreeMulti.Interfaces
{
    public interface IDialogService
    {
        void RegisterView(FrameworkElement view, Type viewModelType);
        bool? ShowDialog(ViewModelBase viewModel);
    }
}
