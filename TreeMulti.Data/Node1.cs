namespace TreeMulti.Data
{
    public class Node1 : Node
    {

        public Node1() { }

        public Node1(string name, string comment, string comment2) : base(name, comment)
        {
            Comment2 = comment2;
        }

        public string Comment2 { get; set; }

    }
}
