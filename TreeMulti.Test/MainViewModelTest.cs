using System;
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

        public void MainViewModelInitialize(Node node, bool dialogResult)
        {
            if(!node.IsNotEmpty()) node.SetDefault();

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

            _addEditVmMock = new Mock<AddViewModel>(node);
            _addEditVmMock.SetupProperty(x => x.NewNode, node);
            _addEditVmMock.SetupProperty(x => x.Result, dialogResult);

            var mockDialogTrue = new Mock<IDialogService>();
            mockDialogTrue.Setup(x => x.ShowDialog(_addEditVmMock.Object)).Returns(dialogResult);

            var mainViewModelMock = new Mock<MainViewModel>(mockRepository.Object, mockDialogTrue.Object,
                new Func<Model.Node, AddViewModel>(x => _addEditVmMock.Object));

            mainViewModelMock.Setup(x => x.CreateNodeForAddEditVm(NodeTypes.Node1)).Returns(node);

            _mainViewModel = mainViewModelMock.Object;
            _mainViewModel.SelectedItems = new List<object>();
        }

        [Test]
        public void DeleteCommand_BadInput_DoesNotDelete()
        {
            MainViewModelInitialize(new Node1(), true);
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.DeleteCommand.Execute(_mainViewModel.SelectedItems);

            Assert.IsTrue(_mainViewModel.Tree.Count == count);
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(null));
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(new List<object>
            {
                new object(), new object()
            }));
        }

        [Test]
        public void DeleteCommand_SingleSelect_Delete()
        {
            MainViewModelInitialize(new Node1(), true);
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
            MainViewModelInitialize(new Node1(), true);
            var count = _mainViewModel.Tree.Count;
            var selectedAll = new List<object>(_mainViewModel.Tree);

            _mainViewModel.DeleteCommand.Execute(selectedAll);

            Assert.IsTrue(_mainViewModel.Tree.Count == count - selectedAll.Count);
        }
        
        [Test]
        public void AddCommand_InvalidInputs_ReturnedWithoutEx()
        {
            MainViewModelInitialize(new Node1(), true);
            Assert.DoesNotThrow(() => _mainViewModel.AddCommand.Execute(new object()));
            Assert.DoesNotThrow(() => _mainViewModel.AddCommand.Execute(null));
        }

        [Test]
        public void AddCommand_SelectedNull_AddToRoot()
        {
            MainViewModelInitialize(new Node1(), true);
            var count = _mainViewModel.Tree.Count;
            
            _mainViewModel.AddCommand.Execute(NodeTypes.Node1);

            Assert.IsTrue(_mainViewModel.Tree.Count == count + 1);
        }

        [Test]
        public void AddCommand_Cancelled_TreeNotChanged()
        {
            MainViewModelInitialize(new Node1(), false);
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.AddCommand.Execute(NodeTypes.Node1);

            Assert.IsFalse(_mainViewModel.Tree.Count == count + 1);
        }

        [Test]
        public void AddCommand_SelectedGroup_AddToGroup()
        {
            MainViewModelInitialize(new Node1(), true);
            var group = (Model.GroupNode)_mainViewModel.Tree.First(x => x is Model.GroupNode);
            var count = group.Children.Count;
            _mainViewModel.SelectedItems = new List<object>()
            {
                group
            };

            _mainViewModel.AddCommand.Execute(NodeTypes.Node1);

            Assert.IsTrue(group.Children.Count == count + 1);
        }

        [Test]
        public void AddCommand_SelectedNode_AddToParent()
        {
            MainViewModelInitialize(new Node1(), true);
            var group = (Model.GroupNode)_mainViewModel.Tree.First(x => x is Model.GroupNode);
            var selectedChildInGroup = group.Children.First();
            var count = group.Children.Count;
            _mainViewModel.SelectedItems = new List<object>()
            {
                selectedChildInGroup
            };

            _mainViewModel.AddCommand.Execute(NodeTypes.Node1);

            Assert.IsTrue(group.Children.Count == count + 1);
        }

        [Test]
        public void EditCommand_InvalidInputs_ReturnedWithoutEx()
        {
            MainViewModelInitialize(new Node1(), true);
            var selectedMany = new List<object> { new object(), new object() };
            var selectedNothing = new List<object>();

            Assert.DoesNotThrow(() => _mainViewModel.EditCommand.Execute(selectedMany));
            Assert.DoesNotThrow(() => _mainViewModel.EditCommand.Execute(selectedNothing));
            Assert.DoesNotThrow(() => _mainViewModel.EditCommand.Execute(null));
        }

        [Test]
        public void EditCommand_SelectedNode_Changed()
        {
            var changeNode = new Node1("changed", "changed", "changed");
            MainViewModelInitialize(changeNode, true);
            var selectedNode = _mainViewModel.Tree.First();
            var select = new List<object>();
            select.Add(selectedNode);
            var count = _mainViewModel.Tree.Count;
            
            _mainViewModel.EditCommand.Execute(select);

            var nodeAfterEdit = _mainViewModel.Tree.First();
            Assert.AreEqual(nodeAfterEdit, changeNode);
            Assert.IsTrue(_mainViewModel.Tree.Count == count);
        }

        [Test]
        public void EditCommand_UserCancelled_NotChanged()
        {
            var changeNode = new Node1("changed", "changed", "changed");
            MainViewModelInitialize(changeNode, false);
            var selectedNode = _mainViewModel.Tree.First();
            var select = new List<object>();
            select.Add(selectedNode);
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.EditCommand.Execute(select);

            var nodeAfterEdit = _mainViewModel.Tree.First();
            Assert.AreNotEqual(nodeAfterEdit, changeNode);
            Assert.IsTrue(_mainViewModel.Tree.Count == count);
        }
    }
}




