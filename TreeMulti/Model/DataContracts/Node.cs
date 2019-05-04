using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using TreeMulti.Annotations;

namespace TreeMulti.Model.DataContracts
{
    [Serializable, XmlInclude(typeof(Node)), XmlInclude(typeof(Node1)), XmlInclude(typeof(Node2)), XmlInclude(typeof(GroupNode))]
    public abstract class Node 
    {
        protected Node()
        {

        }

        protected Node(string name, string comment)
        {
            Name = name;
            Comment = comment;
        }

        public string Name { get; set; }
        public string Comment { get; set; }

        [NonSerialized]
        public Node Parent;

    }
}
