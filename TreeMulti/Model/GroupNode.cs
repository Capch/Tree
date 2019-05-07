using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using TreeMulti.Helpers;

namespace TreeMulti.Model
{
    public class GroupNode : Node
    {
        private ObservableCollection<Node> _children;

        public GroupNode(string name, string comment) : base(name, comment)
        {
            Children = new ObservableCollection<Node>();
            Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Children));
        }

        public void AddChild(Node node)
        {
            node.Parent = this;
            Children.Add(node);
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
    }
}
