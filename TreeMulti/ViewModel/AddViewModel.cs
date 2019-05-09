using System.Windows.Input;
using TreeMulti.Interfaces;
using TreeMulti.Model;

namespace TreeMulti.ViewModel
{
    public class AddViewModel : ViewModelBase
    {

        private Node _newNode;
        private Node _outNode;

        public AddViewModel(Node item = null)
        {
            AddCommand = new Command(AddNode, IsFieldNotEmpty);
            CancelCommand = new Command(Cancel);

            NewNode = (Node)item?.Clone();
            OutNode = null;

            if (NewNode != null && !NewNode.IsNotEmpty())
            {
                NewNode.SetDefault();
                Mode = "Add";
                OnPropertyChanged(nameof(NewNode));
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public Node NewNode
        {
            get => _newNode;
            set
            {
                _newNode = value;
                OnPropertyChanged();
            }
        }
        public Node OutNode
        {
            get => _outNode;
            set
            {
                _outNode = value;
                OnPropertyChanged();
            }
        }

        public string Mode { get; set; } = "Edit";

        private bool IsFieldNotEmpty(object arg)
        {
            return NewNode.IsNotEmpty();
        }

        private void AddNode(object obj)
        {
            OutNode = NewNode.IsNotEmpty() ? NewNode : null;
            OnRequestClose();
        }

        private void Cancel(object obj)
        {
            OutNode = null;
            OnRequestClose();
        }

    }
}
