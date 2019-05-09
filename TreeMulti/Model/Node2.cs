namespace TreeMulti.Model
{
    public class Node2 : Node
    {
        private string _comment2;
        private string _comment3;

        public Node2()
        {
            
        }

        public Node2(string name, string comment, string comment2, string comment3) 
            :base(name, comment)
        {
            Comment2 = comment2;
            Comment3 = comment3;
        }

        public string Comment2
        {
            get => _comment2;
            set
            {
                if (_comment2 == value)
                {
                    return;
                }
                _comment2 = value;
                OnPropertyChanged();
            }
        }

        public string Comment3
        {
            get => _comment3;
            set
            {
                if (_comment3 == value)
                {
                    return;
                }
                _comment3 = value;
                OnPropertyChanged();
            }
        }

        public override bool IsNotEmpty()
        {
            return (base.IsNotEmpty() && !string.IsNullOrEmpty(Comment2) 
                                      && !string.IsNullOrEmpty(Comment3));
        }

        public override void SetDefault()
        {
            this.Name = "Node1 Name";
            this.Comment = "Some comment";
            this.Comment2 = "Some comment2";
            this.Comment3 = "Some comment3";
        }

        public override object Clone()
        {
            return new Node2(this.Name, this.Comment, this.Comment2, this.Comment3) {Parent = this.Parent};
        }

    }
}
