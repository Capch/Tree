namespace TreeMulti.Data
{
    public class Node2 : Node
    {
        public Node2() { }

        public Node2(string name, string comment, string comment2, string comment3) : base(name, comment)
        {
            Comment2 = comment2;
            Comment3 = comment3;
        }

        public string Comment2 { get; set; }
        public string Comment3 { get; set; }

    }
}
