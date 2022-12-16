using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Managers;
using WebApplication2._0.Models;

namespace WebAppTest
{
    [TestClass]
    public class GenerateManagerTests
    {
        private GenerateManager _generateManager;
        private Mock<IRecordManager> _recordManagerMock;
        private Mock<ICollectionManager> _collectionManagerMock;
        private Mock<ILogger<GenerateManager>> _loggerMock;
        private Mock<IRandomize> _randomizeMock;

        [TestInitialize]
        public void Initialize()
        {
            _recordManagerMock = new Mock<IRecordManager>();
            _collectionManagerMock = new Mock<ICollectionManager>();
            _loggerMock = new Mock<ILogger<GenerateManager>>();
            _randomizeMock = new Mock<IRandomize>();

            _generateManager = new GenerateManager(_collectionManagerMock.Object, _recordManagerMock.Object, _loggerMock.Object,_randomizeMock.Object);
        }
           
        [TestMethod]
        public void GenerateRecords_BasicScenario_CallsRecordManager()
        {
            //arrange 
            CollectionModel collectionModel = new CollectionModel() { CollectionId = 1};
            _randomizeMock.Setup(x => x.GetRandomInt(1, 100, 1000)).Returns(2);
            var i = 100;
            _randomizeMock.Setup(x => x.GetRandomInt(1, 0, 100000)).Returns(() => i++);
            _randomizeMock.Setup(x => x.GetRandomString()).Returns("123");

            //act
            _generateManager.GenerateRecords(collectionModel);

            //assert
            _recordManagerMock.Verify(x => x.Create(It.IsAny<RecordModel>()), Times.Exactly(2));
            _recordManagerMock.Verify(x => x.Create(It.Is<RecordModel>(o => o.Number == 100 && o.Letter=="123" && o.CollectionId == 1)), Times.Once);
            _recordManagerMock.Verify(x => x.Create(It.Is<RecordModel>(o => o.Number == 101 && o.Letter == "123" && o.CollectionId == 1)), Times.Once);
        }
    }
}