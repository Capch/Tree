using System.Collections.Generic;
using System.Collections.ObjectModel;
using TreeMulti.Helpers;

namespace TreeMulti.Model
{
    public static class InitXml
    {
        public static void Init()
        {
            var node = new Node1("node1", "comment1", "comment2");
            var node2 = new Node2("node2", "comment1", "comment2", "comment3");
            var grpNode = new GroupNode("Group1", "group1Comment");
            grpNode.AddChild(node);
            grpNode.AddChild(node2);
            var node3 = new Node1("node3", "comment1", "comment2");
            var node4 = new Node2("node4", "comment1", "comment2", "comment3");
            var grpNode2 = new GroupNode("Group2", "group2Comment");
            var node5 = new Node1("node5", "comment1", "comment2");
            var node6 = new Node2("node6", "comment1", "comment2", "comment3");
            var grpNode3 = new GroupNode("Group3", "group3Comment");
            grpNode3.AddChild(node5);
            grpNode3.AddChild(node6);
            grpNode2.AddChild(grpNode3);
            grpNode2.AddChild(node3);
            grpNode2.AddChild(node4);
            var grpNode4 = new GroupNode("Group4", "group4Comment");
            var node7 = new Node1("node7", "comment1", "comment2");
            var node8 = new Node2("node8", "comment1", "comment2", "comment3");
            var node9 = new Node1("node9", "comment1", "comment2");
            var node10 = new Node2("node10", "comment1", "comment2", "comment3");
            grpNode4.AddChildren(new List<Node>{node7,node8,node9,node10});
            var a = new List<Node> { grpNode, grpNode2, grpNode4 };

            new TreeXmlRepository().SetTree(a);
        }
    }
}
