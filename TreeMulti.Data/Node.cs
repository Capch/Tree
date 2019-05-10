namespace TreeMulti.Data
{
    public abstract class Node 
    {
        protected Node() { }

        protected Node(string name, string comment)
        {
            Name = name;
            Comment = comment;
        }

        public string Name { get; set; }
        public string Comment { get; set; }
        public Node Parent { get; set; }

    }
}
