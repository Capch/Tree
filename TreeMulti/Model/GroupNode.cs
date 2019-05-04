using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using TreeMulti.Helpers;

namespace TreeMulti.Model
{
    [Serializable]
    public class GroupNode : Node
    {
        private ObservableCollection<Node> _children;

        public GroupNode()
        {
            Children = new ObservableCollection<Node>();
            Children.CollectionChanged += ChildrenOnCollectionChanged;
        }
        public GroupNode(string name, string comment) : base(name, comment)
        {
            Children = new ObservableCollection<Node >();
            Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Children));
        }

        [OnDeserialized]
        public void OnSerializedMethod(StreamingContext context)
        {
            if (Children != null)
                foreach (var item in Children)
                {
                    item.Parent = this;
                }
        }
        public void AddChild(Node node)
        {
            node.Parent = this;
            Children.Add(node);
        }
        public void AddChildren(List<Node> nodes)
        {
            foreach (var item in nodes)
            {
                Children.Add(item);
            }
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
                if (Equals(value, _children))
                {
                    return;
                }
                _children = value;
                OnPropertyChanged();
            }
            
        }
    }
}
