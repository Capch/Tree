using System.Collections.Generic;

namespace TreeMulti.Data
{
    public class GroupNode : Node
    {
        public GroupNode()
        {

        }

        public GroupNode(string name, string comment) : base(name, comment)
        {
        }

        public IEnumerable<Node> Children { get; set; }

    }
}
