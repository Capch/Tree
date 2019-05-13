using System;
using System.Windows.Input;
using TreeMulti.Interfaces;
using TreeMulti.Model;
// ReSharper disable VirtualMemberCallInConstructor

namespace TreeMulti.ViewModel
{
    public class AddViewModel : ViewModelBase
    {

        private Node _newNode;

        public AddViewModel(Node item = null)
        {
            AddCommand = new Command(AddNode, IsFieldNotEmpty);
            CancelCommand = new Command(Cancel);

            NewNode = (Node)item?.Clone();

            if (NewNode != null && !NewNode.IsNotEmpty())
            {
                NewNode.SetDefault();
                Mode = WindowMode.Add;
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public virtual Node NewNode
        {
            get => _newNode;
            set
            {
                _newNode = value;
                OnPropertyChanged();
            }
        }

        public WindowMode Mode { get; set; } = WindowMode.Edit;

        private bool IsFieldNotEmpty(object arg)
        {
            return NewNode.IsNotEmpty();
        }

        private void AddNode(object obj)
        {
            Result = true;
            OnRequestClose();
        }

        private void Cancel(object obj)
        {
            Result = false;
            OnRequestClose();
        }

    }
}
