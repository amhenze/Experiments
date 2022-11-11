using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using NuGet.Protocol.Plugins;
using NUnit.Framework;
using System.Text.RegularExpressions;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Managers;
using WebApplication2._0.Models;


namespace WebAppTest
{
	[TestClass]
	public class GenerateManagerTests
	{
		private GenerateManager generateManagerTest;
		private Mock<IRecordManager> recordManagerMock;
		private Mock<ICollectionManager> collectionManagerMock;
		private Mock<ILogger<GenerateManager>> loggerMock;
		private Mock<IRandomize> randomizeMock;

		string ConnectionString = "Host=pg_container;Username=root;Password=root;Port=5432;Database=test_db";

		[TestInitialize]
		public void Initialize()
		{
			recordManagerMock = new Mock<IRecordManager>();
			collectionManagerMock = new Mock<ICollectionManager>();
			loggerMock = new Mock<ILogger<GenerateManager>>();
			randomizeMock = new Mock<IRandomize>();


			generateManagerTest = new GenerateManager(collectionManagerMock.Object, recordManagerMock.Object, loggerMock.Object, randomizeMock.Object);
		}

		[TestMethod]
		public void GenerateRecords_BasicScenario_CallsRecordManager()
		{
			//arrange 
			CollectionModel collectionModel = new CollectionModel() { CollectionId = 1 };
			randomizeMock.Setup(x => x.GetRandomInt(1, 100, 1000)).Returns(2);
			var i = 100;
			randomizeMock.Setup(x => x.GetRandomInt(1, 0, 100000)).Returns(() => i++);
			randomizeMock.Setup(x => x.GetRandomString()).Returns("123");

			//act
			generateManagerTest.GenerateRecords(collectionModel);

			//assert
			recordManagerMock.Verify(x => x.Create(It.IsAny<RecordModel>()), Times.Exactly(2));
			recordManagerMock.Verify(x => x.Create(It.Is<RecordModel>(o => o.Number == 100 && o.Letter == "123" && o.CollectionId == 1)), Times.Once);
			recordManagerMock.Verify(x => x.Create(It.Is<RecordModel>(o => o.Number == 101 && o.Letter == "123" && o.CollectionId == 1)), Times.Once);
		}

	}
}