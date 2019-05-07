using System.Collections.ObjectModel;
using System.Linq;

namespace TreeMulti.Model
{
    public class GroupNode : Node
    {
        private ObservableCollection<Node> _children;

        public GroupNode(string name, string comment) : base(name, comment)
        {
            Children = new ObservableCollection<Node>();
        }
        
        public void AddChild(Node node)
        {
            Children.Add(node);
            SortChildren();
        }

        public void Delete(Node node)
        {
            foreach (var item in Children)
            {
                if (item == node)
                {
                    Children.Remove(item);
                    break;
                }
                if (item is GroupNode groupNode)
                {
                    groupNode.Delete(node);
                }
            }
        }

        public ObservableCollection<Node> Children
        {
            get => _children;
            set
            {
                if (_children == value)
                {
                    return;
                }
                _children = value;
                OnPropertyChanged();
            }
        }

        public void SortChildren()
        {
            Children = new ObservableCollection<Node>(Children.OrderBy(x=>x.GetType().FullName));
        }
    }
}
