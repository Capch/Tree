using System.Collections.ObjectModel;
using System.Linq;

namespace TreeMulti.Model
{
    public class GroupNode : Node
    {

        private ObservableCollectionEx<Node> _children;

        public GroupNode()
        {
            Children = new ObservableCollectionEx<Node>();
        }

        public GroupNode(string name, string comment) : base(name, comment)
        {
            Children = new ObservableCollectionEx<Node>();
        }
        
        public void AddChild(Node node)
        {
            node.Parent = this;
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

        public ObservableCollectionEx<Node> Children
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
            Children = new ObservableCollectionEx<Node>(
                Children.OrderBy(x => x.GetType().Name)
                                  .ThenBy(x => x.Name));
        }

    }
}
