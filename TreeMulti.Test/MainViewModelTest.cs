using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using TreeMulti.ViewModel;
using Moq;
using TreeMulti.Data;
using TreeMulti.Interfaces;

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
            var mock = new Mock<ITreeRepository>();
            mock.Setup(a => a.GetTree()).Returns(new ObservableCollection<Node>() { new Node1(), new Node2() });
            _mainViewModel = new MainViewModel(mock.Object);
            _selectedItems = new List<object>();
        }

        [Test]
        public void DeleteCommand_BadInput_DoesNotDelete()
        {
            var count = _mainViewModel.Tree.Count;

            _mainViewModel.DeleteCommand.Execute(_selectedItems);

            Assert.IsTrue(_mainViewModel.Tree.Count == count);
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(null));
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(new List<object> {new object(), new object()}));
        }

        [Test]
        public void DeleteCommand_SingleSelect_Delete()
        {
            var count = _mainViewModel.Tree.Count;
            var selectedOne = new List<object> {_mainViewModel.Tree.First()};
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
            var selectedMany = new List<object> {new object(), new object()};
            var selectedNothing = new List<object>();

            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(selectedMany));
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(selectedNothing));
            Assert.DoesNotThrow(() => _mainViewModel.DeleteCommand.Execute(null));
        }

        //[Test]
        //public void AddCommand_SelectedNull_AddToRoot()
        //{
        //    var mock = new Mock<IDialogService>();
        //    mock.Setup(a => a.ShowDialog(new AddViewModel(null))).Returns(true);
        //    var m = mock.Object;
        //    _mainViewModel.AddCommand.Execute(NodeTypes.Node1);


        //}

    }
}




