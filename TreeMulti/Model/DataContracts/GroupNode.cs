using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace TreeMulti.Model.DataContracts
{
    [Serializable]
    public class GroupNode : Model.Node
    {
        private ObservableCollection<Model.Node> _children;

        public GroupNode()
        {
            Children = new ObservableCollection<Model.Node>();
            Children.CollectionChanged += ChildrenOnCollectionChanged;
        }
        public GroupNode(string name, string comment) : base(name, comment)
        {
            Children = new ObservableCollection<Model.Node >();
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
        public void AddChild(Model.Node node)
        {
            node.Parent = this;
            Children.Add(node);
        }
        public void AddChildren(List<Model.Node> nodes)
        {
            foreach (var item in nodes)
            {
                Children.Add(item);
            }
        }
        public void Delete(Model.Node node)
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

        public ObservableCollection<Model.Node> Children
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
