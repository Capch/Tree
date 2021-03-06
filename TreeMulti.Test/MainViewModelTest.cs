﻿using System;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TreeMulti.Interfaces;
using TreeMulti.Model;
using TreeMulti.ViewModel;

namespace TreeMulti.Test
{
    [TestFixture]
    public class MainViewModelTest
    {

        private MainViewModel _mainViewModel;
        private Mock<AddViewModel> _addEditVmMock;
        private static readonly object[] _changedNodes =
        {
            new Node1(),
            new Node2(),
            new GroupNode(),
            new Node1("changed", "changed", "changed"),
            new Node2("changed", "changed", "changed", "changed"),
            new GroupNode("changed", "changed")

        };

        private void MainViewModelMockInitialize(Node inputCreateEditNode, bool expectedAddEditVmDialogResult = true,
            NodeTypes expectedInputCreateEditNodeType = NodeTypes.Node1)
        {
            if (!inputCreateEditNode.IsNotEmpty()) inputCreateEditNode.SetDefault();

            var mockRepository = new Mock<ITreeRepository>();
            mockRepository.Setup(a => a.GetTree()).Returns(new ObservableCollection<Data.Node>()
            {
                new Data.Node1(), new Data.Node2(), new Data.GroupNode()
                {
                    Children = new List<Data.Node>
                    {
                        new Data.Node1(),
                        new Data.Node2()

                    }
                }
            });

            _addEditVmMock = new Mock<AddViewModel>(inputCreateEditNode);
            _addEditVmMock.SetupProperty(x => x.NewNode, inputCreateEditNode);
            _addEditVmMock.SetupProperty(x => x.Result, expectedAddEditVmDialogResult);

            var mockDialogTrue = new Mock<IDialogService>();
            mockDialogTrue.Setup(x => x.ShowDialog(_addEditVmMock.Object)).Returns(expectedAddEditVmDialogResult);

            var mainViewModelMock = new Mock<MainViewModel>(mockRepository.Object, mockDialogTrue.Object,
                new Func<Model.Node, AddViewModel>(x => _addEditVmMock.Object));

            mainViewModelMock.Setup(x => x.CreateNodeForAddEditVm(expectedInputCreateEditNodeType)).Returns(inputCreateEditNode);

            _mainViewModel = mainViewModelMock.Object;
            _mainViewModel.SelectedItems = new List<object>();
        }

        [Test]
        public void DeleteCommand_BadInput_DoesNotDelete()
        {
            MainViewModelMockInitialize(new Node1());
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.DeleteCommand.Execute(_mainViewModel.SelectedItems);

            Assert.IsTrue(_mainViewModel.Tree.Count == count);
        }

        [Test]
        public void DeleteCommand_SingleSelect_Delete()
        {
            MainViewModelMockInitialize(new Node1());
            var count = _mainViewModel.Tree.Count;
            var selectedOne = new List<object>
            {
                _mainViewModel.Tree.First()
            };

            _mainViewModel.DeleteCommand.Execute(selectedOne);

            Assert.IsTrue(_mainViewModel.Tree.Count == count - selectedOne.Count);
        }

        [Test]
        public void DeleteCommand_SelectedAll_Delete()
        {
            MainViewModelMockInitialize(new Node1());
            var count = _mainViewModel.Tree.Count;
            var selectedAll = new List<object>(_mainViewModel.Tree);

            _mainViewModel.DeleteCommand.Execute(selectedAll);

            Assert.IsTrue(_mainViewModel.Tree.Count == count - selectedAll.Count);
        }
        
        [Test]
        public void AddCommand_SelectedNull_AddToRoot([Values] NodeTypes nodeTypes)
        {
            MainViewModelMockInitialize(new Node1(), true, nodeTypes);
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.AddCommand.Execute(nodeTypes);

            Assert.IsTrue(_mainViewModel.Tree.Count == count + 1);
        }

        [Test]
        public void AddCommand_Cancelled_TreeNotChanged([Values] NodeTypes nodeTypes)
        {
            MainViewModelMockInitialize(new Node1(), false, nodeTypes);
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.AddCommand.Execute(nodeTypes);

            Assert.IsFalse(_mainViewModel.Tree.Count == count + 1);
        }

        [Test]
        public void AddCommand_SelectedGroup_AddToGroup([Values] NodeTypes nodeTypes)
        {
            MainViewModelMockInitialize(new Node1(), true, nodeTypes);
            var group = (Model.GroupNode)_mainViewModel.Tree.First(x => x is Model.GroupNode);
            var count = group.Children.Count;
            _mainViewModel.SelectedItems = new List<object>()
            {
                group
            };

            _mainViewModel.AddCommand.Execute(nodeTypes);

            Assert.IsTrue(group.Children.Count == count + 1);
        }

        [Test]
        public void AddCommand_SelectedNode_AddToParent([Values] NodeTypes nodeTypes)
        {
            MainViewModelMockInitialize(new Node1(), true, nodeTypes);
            var group = (Model.GroupNode)_mainViewModel.Tree.First(x => x is Model.GroupNode);
            var selectedChildInGroup = group.Children.First();
            var count = group.Children.Count;
            _mainViewModel.SelectedItems = new List<object>()
            {
                selectedChildInGroup
            };

            _mainViewModel.AddCommand.Execute(nodeTypes);

            Assert.IsTrue(group.Children.Count == count + 1);
        }
        
        [Test, TestCaseSource(nameof(_changedNodes))]
        public void EditCommand_SelectedNode_Changed(Node changeNode)
        {
            MainViewModelMockInitialize(changeNode, true);
            var selectedNode = _mainViewModel.Tree.First();
            var select = new List<object>
            {
                selectedNode
            };
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.EditCommand.Execute(select);

            var nodeAfterEdit = _mainViewModel.Tree.First();
            Assert.AreEqual(nodeAfterEdit, changeNode);
            Assert.IsTrue(_mainViewModel.Tree.Count == count);
        }

        [Test, TestCaseSource(nameof(_changedNodes))]
        public void EditCommand_UserCancelled_NotChanged(Node changeNode)
        {
            MainViewModelMockInitialize(changeNode, false);
            var selectedNode = _mainViewModel.Tree.First();
            var select = new List<object>
            {
                selectedNode
            };
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.EditCommand.Execute(select);

            var nodeAfterEdit = _mainViewModel.Tree.First();
            Assert.AreNotEqual(nodeAfterEdit, changeNode);
            Assert.IsTrue(_mainViewModel.Tree.Count == count);
        }

    }
}




