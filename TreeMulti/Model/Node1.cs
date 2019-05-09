namespace TreeMulti.Model
{
    public class Node1 : Node
    {
        private string _comment2;

        public Node1()
        {
            
        }

        public Node1(string name, string comment, string comment2) :base(name,comment)
        {
            Comment2 = comment2;
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

        public override bool IsNotEmpty()
        {
            return (base.IsNotEmpty() && !string.IsNullOrEmpty(Comment2));
        }

        public override void SetDefault()
        {
            this.Name = "Node1 Name";
            this.Comment = "Some comment";
            this.Comment2 = "Some comment2";
        }

        public override object Clone()
        {
            return new Node1(this.Name, this.Comment, this.Comment2) { Parent = this.Parent };
        }

    }
}
