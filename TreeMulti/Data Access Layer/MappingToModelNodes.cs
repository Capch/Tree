using System;
using System.Collections.Generic;
using System.Linq;
using TreeMulti.Annotations;

namespace TreeMulti
{
    public static partial class MappingNodes
    {
        public static IEnumerable<Model.Node> ToModel(this IEnumerable<Data.Node> dataTree)
        {
            return dataTree?.Select(item => item.ConvertToModel()).ToList();
        }

        private static Model.Node ConvertToModel(this Data.Node dataNode)
        {
            switch (dataNode)
            {
                case Data.Node1 node1:
                    return node1.ConvertToModel();
                case Data.Node2 node2:
                    return node2.ConvertToModel();
                case Data.GroupNode node:
                    return node.ConvertToModel();
                default:
                    return null;
            }
        }

        private static Model.Node1 ConvertToModel([NotNull] this Data.Node1 dataNode)
        {
            if (dataNode == null)
            {
                throw new ArgumentNullException(nameof(dataNode));
            }
            return new Model.Node1(dataNode.Name, dataNode.Comment, dataNode.Comment2);
        }

        private static Model.Node2 ConvertToModel([NotNull] this Data.Node2 dataNode)
        {
            if (dataNode == null)
            {
                throw new ArgumentNullException(nameof(dataNode));
            }
            return new Model.Node2(dataNode.Name, dataNode.Comment, dataNode.Comment2, dataNode.Comment3);
        }

        private static Model.GroupNode ConvertToModel([NotNull] this Data.GroupNode dataNode)
        {
            if (dataNode == null)
            {
                throw new ArgumentNullException(nameof(dataNode));
            }
            var groupData = new Model.GroupNode(dataNode.Name, dataNode.Comment);

            var children = new ObservableCollectionEx<Model.Node>();

            foreach (var child in dataNode.Children)
            {
                var tempNode = child.ConvertToModel();
                tempNode.Parent = groupData;
                children.Add(tempNode);
            }

            groupData.Children = children;
            return groupData;
        }

    }
}