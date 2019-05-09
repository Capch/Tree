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

        [SetUp]
        public void Init()
        {
            var mock = new Mock<ITreeRepository>();
            mock.Setup(a => a.GetTree()).Returns(new ObservableCollection<Node>());
            _addViewModel = new MainViewModel(mock.Object);
        }

        public void AddCommand_SelectedNothing_Added()
        {
            _addViewModel.SelectedItems = new List<object>();
            var count = _addViewModel.Tree.Count;

            _addViewModel.AddCommand.Execute(NodeTypes.Node1);

            Assert.IsTrue(_addViewModel.Tree.Count == count + 1);
        }

    }
}