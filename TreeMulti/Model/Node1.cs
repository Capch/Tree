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

    }
}
