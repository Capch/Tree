using System.ComponentModel;
using System.Runtime.CompilerServices;
using TreeMulti.Annotations;

namespace TreeMulti.Model
{
    public abstract class Node : INotifyPropertyChanged
    {
        private string _name;
        private string _comment;
        private Node _parent;
        public event PropertyChangedEventHandler PropertyChanged;

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
                if (_name == value)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment == value)
                {
                    return;
                }
                _comment = value;
                OnPropertyChanged();
            }
        }

        public Node Parent
        {
            get => _parent;
            set
            {
                if (_parent == value)
                {
                    return;
                }
                _parent = value;
                OnPropertyChanged();
            }
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual bool IsNotEmpty()
        {
            return (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Comment));
        }

        public override string ToString()
        {
            return Name.ToString()+Comment.ToString();
        }

    }
}
