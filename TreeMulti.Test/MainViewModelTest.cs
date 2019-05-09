using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private MainViewModel _addViewModel;
        private List<object> _selectedItems;

        [SetUp]
        public void Init()
        {
            var mock = new Mock<ITreeRepository>();
            mock.Setup(a => a.GetTree()).Returns(new ObservableCollection<Node>());
            _addViewModel = new MainViewModel(mock.Object);
            _selectedItems = new List<object>();
        }

        [Test]
        public void DeleteCommand_SelectedNothing_Added()
        {
            var count = _addViewModel.Tree.Count;

            _addViewModel.DeleteCommand.Execute(_addViewModel.SelectedItems);

            Assert.IsTrue(_addViewModel.Tree.Count == count);
        }


    }
}