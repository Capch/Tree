using NUnit.Framework;
using TreeMulti.Model;
using TreeMulti.ViewModel;

namespace TreeMulti.Test
{
    [TestFixture]
    public class AddViewModelTest
    {
        [Test]
        public void AddViewModel_NullInput_NullOutput()
        {
            var viewmodel = new AddViewModel(null);

            Assert.IsTrue(viewmodel.NewNode== null);
            Assert.IsTrue(viewmodel.OutNode== null);
        }

        [Test]
        public void AddViewModel_NotNullInput_NotNullOutput()
        {
            var node = new Node1();

            var viewmodel = new AddViewModel(node);
            node.SetDefault();

            Assert.IsTrue(viewmodel.NewNode != null);
            Assert.IsTrue(viewmodel.NewNode.IsNotEmpty());
            Assert.IsTrue(viewmodel.OutNode == null);
        }

        [Test]
        public void Result_ConfirmPressed_NotNullOutput()
        {
            var node = new Node1();

            var viewmodel = new AddViewModel(node);
            node.SetDefault();
            viewmodel.AddCommand.Execute(new object());

            Assert.IsTrue(viewmodel.OutNode != null);
            Assert.IsTrue(viewmodel.OutNode.IsNotEmpty());
        }

        [Test]
        public void Result_CancelPressed_NotNullOutput()
        {
            var node = new Node1();

            var viewmodel = new AddViewModel(node);
            node.SetDefault();
            viewmodel.CancelCommand.Execute(new object());

            Assert.IsTrue(viewmodel.OutNode == null);

        }

    }
}