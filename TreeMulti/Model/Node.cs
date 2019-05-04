using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TreeMulti.Annotations;

namespace TreeMulti.Model
{
    [Serializable, XmlInclude(typeof(Node)), XmlInclude(typeof(Node1)), XmlInclude(typeof(Node2)), XmlInclude(typeof(GroupNode))]
    public abstract class Node : INotifyPropertyChanged
    {
        protected Node()
        {

        }

        protected Node(string name, string comment)
        {
            Name = name;
            Comment = comment;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                if (value == _comment)
                {
                    return;
                }
                _comment = value;
                OnPropertyChanged();
            }
        }

        [NonSerialized]
        public Node Parent;

        private string _name;
        private string _comment;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
