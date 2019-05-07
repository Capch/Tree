using System;
using System.Collections.Generic;
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
                    Height = ((IWindowConfiguration) view).GetWindowSize().Height,
                    Width = ((IWindowConfiguration) view).GetWindowSize().Width
                };
                viewModel.RequestClose += (sender, e) => window.Close();
                result = window.ShowDialog();
            }
            return result;
        }
    }
}