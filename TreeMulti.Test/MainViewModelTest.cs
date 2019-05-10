using System;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TreeMulti.Data;
using TreeMulti.Interfaces;
using TreeMulti.ViewModel;

namespace TreeMulti.Test
{
    [TestFixture]
    public class MainViewModelTest
    {
        private MainViewModel _mainViewModel;
        private List<object> _selectedItems;

        [SetUp]
        public void Init()
        {
            var mockRepository = new Mock<ITreeRepository>();
            mockRepository.Setup(a => a.GetTree()).Returns(new ObservableCollection<Node>() { new Node1(), new Node2() });

            var node = new Model.Node1();
            var addEditVmMock = new Mock<AddViewModel>(node);


            var mockDialogTrue = new Mock<IDialogService>();
            mockDialogTrue.Setup(x => x.ShowDialog(addEditVmMock.Object)).Returns(true);

            var mainViewModelMock = new Mock<MainViewModel>(mockRepository.Object, mockDialogTrue.Object, new Func<Model.Node, AddViewModel>(x => addEditVmMock.Object));

            mainViewModelMock.Setup(x => x.CreateNodeForAddEditVM(NodeTypes.Node1)).Returns(node);

            _mainViewModel = mainViewModelMock.Object;
            _selectedItems = new List<object>();
        }

        [Test]
        public void DeleteCommand_BadInput_DoesNotDelete()
        {
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.DeleteCommand.Execute(_selectedItems);

            Assert.IsTrue(_mainViewModel.Tree.Count == count);
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(null));
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(new List<object> { new object(), new object() }));
        }

        [Test]
        public void DeleteCommand_SingleSelect_Delete()
        {
            var count = _mainViewModel.Tree.Count;
            var selectedOne = new List<object> { _mainViewModel.Tree.First() };
            _mainViewModel.DeleteCommand.Execute(selectedOne);

            Assert.IsTrue(_mainViewModel.Tree.Count == count - selectedOne.Count);
        }

        [Test]
        public void DeleteCommand_SelectedAll_Delete()
        {
            var count = _mainViewModel.Tree.Count;
            var selectedAll = new List<object>(_mainViewModel.Tree);
            _mainViewModel.DeleteCommand.Execute(selectedAll);

            Assert.IsTrue(_mainViewModel.Tree.Count == count - selectedAll.Count);
        }

        [Test]
        public void EditCommand_InvalidInputs_ReturnedWithoutEx()
        {
            var selectedMany = new List<object> { new object(), new object() };
            var selectedNothing = new List<object>();

            Assert.DoesNotThrow(() => _mainViewModel.EditCommand.Execute(selectedMany));
            Assert.DoesNotThrow(() => _mainViewModel.EditCommand.Execute(selectedNothing));
            Assert.DoesNotThrow(() => _mainViewModel.EditCommand.Execute(null));
        }

        [Test]
        public void AddCommand_InvalidInputs_ReturnedWithoutEx()
        {
            Assert.DoesNotThrow(() => _mainViewModel.AddCommand.Execute(new object()));
            Assert.DoesNotThrow(() => _mainViewModel.AddCommand.Execute(null));
        }

        [Test]
        public void AddCommand_SelectedNull_AddToRoot()
        {
            var count = _mainViewModel.Tree.Count;
            _selectedItems.Add(_mainViewModel.Tree.First());
            _mainViewModel.AddCommand.Execute(NodeTypes.Node1);

            Assert.IsTrue(_mainViewModel.Tree.Count != count);
        }

    }
}




