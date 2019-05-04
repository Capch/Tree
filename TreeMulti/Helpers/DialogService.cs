using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using TreeMulti.Interfaces;

namespace TreeMulti
{
    class DialogService : IDialogService
    {
        private readonly Dictionary<Type, FrameworkElement> _dictionary = new Dictionary<Type, FrameworkElement>();

        public void RegisterView(FrameworkElement view, Type viewModelType)
        {
            try
            {
                _dictionary.Add(viewModelType, view);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public bool? ShowDialog(ViewModelBase viewModel)
        {
            bool? result = null;
            if (_dictionary.TryGetValue(viewModel.GetType(), out var view))
            {
                view.DataContext = viewModel;
                var window = new DialogBase(view)
                {
                    Title = (view as IWindowConfiguration)?.GetWindowTitle(),
                    Height = (view as IWindowConfiguration).GetWindowSize().Height,
                    Width = (view as IWindowConfiguration).GetWindowSize().Width
                };
                viewModel.RequestClose += (sender, e) => window.Close();
                result = window.ShowDialog();
            }
            return result;
        }
    }
}