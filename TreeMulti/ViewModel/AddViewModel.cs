using System;
using System.Windows.Input;
using TreeMulti.Interfaces;
using TreeMulti.Model;

namespace TreeMulti.ViewModel
{
    public class AddViewModel : ViewModelBase
    {
        private Node _newNode;

        public Node NewNode
        {
            get => _newNode;
            set
            {
                _newNode = value;
                OnPropertyChanged();
            }
        }
        public AddViewModel(Node item = null)
        {
            AddCommand = new Command(AddNode);
            NewNode = item;
        }

        public ICommand AddCommand { get; set; }

        private void AddNode(object obj)
        {

            OnRequestClose();
        }
    }
}
