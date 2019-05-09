using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TreeMulti.Test
{
    [TestFixture]
    public class MappingTest
    {

        [Test]
        public void MapModelToData_ModelToData_ResultData()
        {
            var collection = new ObservableCollection<Model.Node>();

            var result = collection.ToData();

            Assert.IsTrue(result is IEnumerable<Data.Node>);
        }

        [Test]
        public void MapModelToData_DataToModel_ResultModel()
        {
            var collection = new ObservableCollection<Data.Node>();

            var result = collection.ToModel();

            Assert.IsTrue(result is IEnumerable<Model.Node>);
        }

        [Test]
        public void MapModelToData_IncorrectInput_ReturnNull()
        {
            List<Model.Node> collection1 = null;
            List<Data.Node> collection2 = null;
            
            Assert.IsTrue(collection1.ToData() ==null);
            Assert.IsTrue(collection2.ToModel()==null);
        }

    }
}