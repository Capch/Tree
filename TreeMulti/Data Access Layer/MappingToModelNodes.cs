using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TreeMulti.Annotations;

namespace TreeMulti
{
    public static partial class MappingNodes
    {
        public static IEnumerable<Model.Node> ToModel(this IEnumerable<Data.Node> dataTree)
        {
            return dataTree.Select(item => item.ConvertToModel()).ToList();
        }
        public static Model.Node ConvertToModel(this Data.Node dataNode)
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
        public static Model.Node1 ConvertToModel([NotNull] this Data.Node1 dataNode)
        {
            if (dataNode == null) throw new ArgumentNullException(nameof(dataNode));
            return new Model.Node1(dataNode.Name, dataNode.Comment, dataNode.Comment2)
            {
                Parent = dataNode.Parent.ConvertToModel()
            };
        }
        public static Model.Node2 ConvertToModel([NotNull] this Data.Node2 dataNode)
        {
            if (dataNode == null) throw new ArgumentNullException(nameof(dataNode));
            return new Model.Node2(dataNode.Name, dataNode.Comment, dataNode.Comment2, dataNode.Comment3)
            {
                Parent = dataNode.Parent.ConvertToModel()
            };
        }
        public static Model.GroupNode ConvertToModel([NotNull] this Data.GroupNode dataNode)
        {
            if (dataNode == null) throw new ArgumentNullException(nameof(dataNode));
            var groupData = new Model.GroupNode(dataNode.Name, dataNode.Comment)
            {
                Parent = dataNode.Parent.ConvertToModel()
            };

            var children = new ObservableCollection<Model.Node>();

            foreach (var child in dataNode.Children)
            {
                children.Add(child.ConvertToModel());
            }

            groupData.Children = children;
            return groupData;
        }
    }
}