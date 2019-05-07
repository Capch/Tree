namespace TreeMulti.Model
{
    public class Node2 : Node
    {
        private string _comment2;
        private string _comment3;

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
    }
}
