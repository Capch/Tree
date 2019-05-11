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
            Children.Sort();
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

        public override void SetDefault()
        {
            this.Name = "Group Name";
            this.Comment = "Some comment";
        }

        public override object Clone()
        {
            return new GroupNode(this.Name, this.Comment) { Parent = this.Parent, Children = this.Children};
        }

    }
}
