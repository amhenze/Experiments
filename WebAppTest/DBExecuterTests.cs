using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2._0.DataBaseWorker;
using WebApplication2._0.Entities;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Managers;
using WebApplication2._0.Models;
using WebApplication2._0.Options.FolderForMocks;
using WebApplication2._0.Options.FolderForMocks.Abstractions;
using WebAppTest.Class;

namespace WebAppTest
{
	[TestClass]
	public class DBExecuterTests
	{
		private IDBExecuter _dbExecuterTest;
		private Mock<ILogger<DBExecuter>> _loggerMock;
		private Mock<IConnection> _connectionMock;
		private Mock<ICommand> _commandMock;
		private Mock<ICommandFactory> _commandFactoryMock;
		private Mock<ISQLReader> _sqlReaderMock;
		private List<NpgsqlParameter> _parameters;


		[TestInitialize]
		public void Initialize()
		{
			//_dbExecuterTest = new Mock<IDBExecuter>();
			_loggerMock = new Mock<ILogger<DBExecuter>>();
			_connectionMock = new Mock<IConnection>();
			_commandMock = new Mock<ICommand>();

			_parameters = new List<NpgsqlParameter>();
			_commandMock.SetupAllProperties();
			_commandMock.Setup(x => x.Parameters).Returns(_parameters);

			_commandFactoryMock = new Mock<ICommandFactory>();
			_sqlReaderMock = new Mock<ISQLReader>();
			//connectionMock = new NpgsqlConnection("Host=pg_container;Username=root;Password=root;Port=5432;Database=test_db");

			_dbExecuterTest = new DBExecuter(_connectionMock.Object, _loggerMock.Object, _commandMock.Object, _commandFactoryMock.Object);
		}

		[TestMethod]
		public void DBExecuter_ExecuteNonQuery()
		{
			//arrange 

			_commandFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<IConnection>())).Returns(_commandMock.Object);
			object[] param = { 1, 1, 2 };
			_commandMock.Setup(x => x.ExecuteNonQuery()).Returns(3);
			//act
			_dbExecuterTest.ExecuteNonQuery("asds", param);
			//assert
			Assert.AreEqual(3, _parameters.Count);
			Assert.AreEqual(param[0], _parameters[0].Value);
			Assert.AreEqual("param1", _parameters[0].ParameterName);
			Assert.AreEqual(param[1], _parameters[1].Value);
			Assert.AreEqual("param2", _parameters[1].ParameterName);
			Assert.AreEqual(param[2], _parameters[2].Value);
			Assert.AreEqual("param3", _parameters[2].ParameterName);
			_commandMock.Verify(x => x.ExecuteNonQuery(), Times.Once);
		}

		[TestMethod]
		public async Task DBExecuter_ExecuteReader_AllAtributes()
		{
			int timesCalled = -1;
			//arrange 
			_commandFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<IConnection>())).Returns(_commandMock.Object);
			object[] param = { 1, 1, 2 };
			int recordCount = 0;
			int collectionCount = 0;
			int numberCount = 0;
			int letterCount = 0;
			_sqlReaderMock.SetupGet(x => x["record_id"])
				.Callback(() => ++recordCount)
				.Returns(() => recordCount);
			_sqlReaderMock.SetupGet(x => x["collection_id"])
				.Callback(() => ++collectionCount)
				.Returns(() => collectionCount);
			_sqlReaderMock.SetupGet(x => x["number"])
				.Callback(() => ++numberCount)
				.Returns(() => numberCount);
			_sqlReaderMock.SetupGet(x => x["letter"])
				.Callback(() => ++letterCount)
				.Returns(() => $"{letterCount}");
			//_sqlReaderMock.SetupGet
			_commandMock.Setup(x => x.ExecuteReaderAsync()).ReturnsAsync(_sqlReaderMock.Object);
			_sqlReaderMock.Setup(x => x.ReadAsync())
				.Callback(() => ++timesCalled)
				.ReturnsAsync(() =>
				{
					if (timesCalled == 3)
					{
						return false;
					}
					return true;
				});
			//act
			List<RecordEntity> list = await _dbExecuterTest.ExecuteReader<RecordEntity>("asds", param);
			//assert
			Assert.AreEqual(3, _parameters.Count);
			Assert.AreEqual(param[0], _parameters[0].Value);
			Assert.AreEqual("param1", _parameters[0].ParameterName);
			Assert.AreEqual(param[1], _parameters[1].Value);
			Assert.AreEqual("param2", _parameters[1].ParameterName);
			Assert.AreEqual(param[2], _parameters[2].Value);
			Assert.AreEqual("param3", _parameters[2].ParameterName);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0].CollectionId);
			Assert.AreEqual(1, list[0].RecordId);
			Assert.AreEqual(1, list[0].Number);
			Assert.AreEqual("1", list[0].Letter);
			Assert.AreEqual(2, list[1].CollectionId);
			Assert.AreEqual(2, list[1].RecordId);
			Assert.AreEqual(2, list[1].Number);
			Assert.AreEqual("2", list[1].Letter);
			Assert.AreEqual(3, list[2].CollectionId);
			Assert.AreEqual(3, list[2].RecordId);
			Assert.AreEqual(3, list[2].Number);
			Assert.AreEqual("3", list[2].Letter);
		}

		[TestMethod]
		public async Task DBExecuter_ExecuteReader_NoAtributes()
		{
			int timesCalled = -1;
			Exception ex = null;
			//arrange 
			_commandFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<IConnection>())).Returns(_commandMock.Object);
			object[] param = { 1, 1, 2 };
			_commandMock.Setup(x => x.ExecuteReaderAsync()).ReturnsAsync(_sqlReaderMock.Object);
			_sqlReaderMock.Setup(x => x.ReadAsync())
				.Callback(() => ++timesCalled)
				.ReturnsAsync(() =>
				{
					if (timesCalled == 3)
					{
						return false;
					}
					return true;
				});
			//act
			ex = await Assert.ThrowsExceptionAsync<Exception>(() => _dbExecuterTest.ExecuteReader<TestPropertyClassNoAtribute>("asds", param));
			//assert
			Assert.AreEqual("Model with no atributes", ex.Message);
		}

		[TestMethod]
		public void DBExecuter_ExecuteReader_SomeAtributes()
		{
			int timesCalled = -1;
			//arrange 
			_commandFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<IConnection>())).Returns(_commandMock.Object);
			object[] param = { 1, 1, 2 };
			int recordCount = 0;
			int collectionCount = 0;
			_sqlReaderMock.SetupGet(x => x["record_id"])
				.Callback(() => ++recordCount)
				.Returns(() => recordCount);
			_sqlReaderMock.SetupGet(x => x["collection_id"])
				.Callback(() => ++collectionCount)
				.Returns(() => collectionCount);

			_commandMock.Setup(x => x.ExecuteReaderAsync()).ReturnsAsync(_sqlReaderMock.Object);
			_sqlReaderMock.Setup(x => x.ReadAsync())
				.Callback(() => ++timesCalled)
				.ReturnsAsync(() =>
				{
					if (timesCalled == 3)
					{
						return false;
					}
					return true;
				});
			//act
			List<TestPropertyClassWithAtribute> list = _dbExecuterTest.ExecuteReader<TestPropertyClassWithAtribute>("asds", param).Result;
			//assert
			Assert.AreEqual(3, _parameters.Count);
			Assert.AreEqual(param[0], _parameters[0].Value);
			Assert.AreEqual("param1", _parameters[0].ParameterName);
			Assert.AreEqual(param[1], _parameters[1].Value);
			Assert.AreEqual("param2", _parameters[1].ParameterName);
			Assert.AreEqual(param[2], _parameters[2].Value);
			Assert.AreEqual("param3", _parameters[2].ParameterName);
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0].CollectionId);
			Assert.AreEqual(1, list[0].RecordId);
			Assert.AreEqual(2, list[1].CollectionId);
			Assert.AreEqual(2, list[1].RecordId);
			Assert.AreEqual(3, list[2].CollectionId);
			Assert.AreEqual(3, list[2].RecordId);
		}
	}
}
