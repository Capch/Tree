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
            NewNode = (Node) item?.Clone();
            OutNode = null;
        }

        public ICommand AddCommand { get; set; }

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

        private bool IsFieldNotEmpty(object arg)
        {
            return NewNode.IsNotEmpty();
        }

        private void AddNode(object obj)
        {
            OutNode = NewNode;
            OnRequestClose();
        }

    }
}
