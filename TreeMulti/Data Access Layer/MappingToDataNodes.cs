using System;
using System.Collections.Generic;
using System.Linq;
using TreeMulti.Annotations;

namespace TreeMulti
{
    public static partial class MappingNodes
    {

        public static List<Data.Node> ToData(this IEnumerable<Model.Node> modelTree)
        {
            return modelTree.Select(item => item.ConvertToData()).ToList();
        }

        public static Data.Node ConvertToData(this Model.Node modelNode)
        {
            switch (modelNode)
            {
                case Model.Node1 node1:
                    return node1.ConvertToData();
                case Model.Node2 node2:
                    return node2.ConvertToData();
                case Model.GroupNode node:
                    return node.ConvertToData();
                default:
                    return null;
            }
        }

        public static Data.Node1 ConvertToData([NotNull] this Model.Node1 modelNode)
        {
            if (modelNode == null) throw new ArgumentNullException(nameof(modelNode));
            return new Data.Node1(modelNode.Name, modelNode.Comment, modelNode.Comment2);
        }

        public static Data.Node2 ConvertToData([NotNull] this Model.Node2 modelNode)
        {
            if (modelNode == null) throw new ArgumentNullException(nameof(modelNode));
            return new Data.Node2(modelNode.Name, modelNode.Comment, modelNode.Comment2, modelNode.Comment3);
        }

        public static Data.GroupNode ConvertToData([NotNull] this Model.GroupNode modelNode)
        {
            if (modelNode == null) throw new ArgumentNullException(nameof(modelNode));
            var groupData = new Data.GroupNode(modelNode.Name, modelNode.Comment);
            var children = new List<Data.Node>();
            foreach (var child in modelNode.Children)
            {
                var tempNode = child.ConvertToData();
                tempNode.Parent = groupData;
                children.Add(tempNode);
            }

            groupData.Children = children;
            return groupData;
        }

    }
}