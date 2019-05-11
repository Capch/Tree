using System.Collections.Generic;

namespace TreeMulti.Data
{
    public class GroupNode : Node
    {
        public GroupNode()
        {
            Children=new List<Node>();
        }

        public GroupNode(string name, string comment) : base(name, comment)
        {
            Children = new List<Node>();
        }

        public IEnumerable<Node> Children { get; set; }

    }
}
