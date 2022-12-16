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
		private IDBExecuter _dbExecuter;
		private Mock<ILogger<DBExecuter>> _loggerMock;
		private Mock<IConnection> _connectionMock;
		private Mock<ICommand> _commandMock;
		private Mock<ISQLReader> _sqlReaderMock;
		private List<NpgsqlParameter> _parameters;
		private string sqlConnection = "asds";
		private object[] param = { 1, 1, 2 };


		[TestInitialize]
		public void Initialize()
		{
			_loggerMock = new Mock<ILogger<DBExecuter>>();
			_connectionMock = new Mock<IConnection>();
			_commandMock = new Mock<ICommand>();

			_parameters = new List<NpgsqlParameter>();
			_commandMock.SetupAllProperties();
			_commandMock.Setup(x => x.Parameters).Returns(_parameters);
			_connectionMock.Setup(x => x.CreateCommand(It.IsAny<string>())).Returns(_commandMock.Object);

			_sqlReaderMock = new Mock<ISQLReader>();
			_commandMock.Setup(x => x.ExecuteReaderAsync()).ReturnsAsync(_sqlReaderMock.Object);
			_dbExecuter = new DBExecuter(_connectionMock.Object, _loggerMock.Object);
		}

		[TestMethod]
		public void DBExecuter_ExecuteNonQuery()
		{
			//arrange 
			_commandMock.Setup(x => x.ExecuteNonQuery()).Returns(3);
			//act
			_dbExecuter.ExecuteNonQuery(sqlConnection, param);
			//assert
			AssertParam(param);
			_commandMock.Verify(x => x.ExecuteNonQuery(), Times.Once);
		}

		[TestMethod]
		public async Task DBExecuter_ExecuteReader_AllAttributes()
		{
			//arrange 
			SqlReaderSetupIndexCallback();
			SqlReaderSetupCallback();
			//act
			List<RecordEntity> list = await _dbExecuter.ExecuteReader<RecordEntity>(sqlConnection, param);
			//assert
			AssertParam(param);
			Assert.AreEqual(3, list.Count);
			for(int i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(i+1, list[i].CollectionId);
				Assert.AreEqual(i+1, list[i].RecordId);
				Assert.AreEqual(i+1, list[i].Number);
				Assert.AreEqual($"{i+1}", list[i].Letter);
			}
		}

		[TestMethod]
		public async Task DBExecuter_ExecuteReader_NoAttributes()
		{
			//arrange 
			//act
			List<TestPropertyClassNoAttribute> list = await _dbExecuter.ExecuteReader<TestPropertyClassNoAttribute>(sqlConnection, param);
			//assert
			AssertParam(param);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(0, list[0].CollectionId);
			Assert.AreEqual(0, list[0].RecordId);
			Assert.AreEqual(0, list[0].Number);
			Assert.AreEqual(null, list[0].Letter);
		}

		[TestMethod]
		public async Task DBExecuter_ExecuteReader_SomeAttributes()
		{
			//arrange 
			SqlReaderSetupIndexCallback();
			SqlReaderSetupCallback();
			//act
			List<TestPropertyClassWithAttribute> list = await _dbExecuter.ExecuteReader<TestPropertyClassWithAttribute>("asds", param);
			//assert
			AssertParam(param);
			Assert.AreEqual(3, list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(i + 1, list[i].CollectionId);
				Assert.AreEqual(i + 1, list[i].RecordId);
			}
			_connectionMock.Verify(x => x.Open(), Times.Once());
			//_connectionMock.Verify(x => x.Close(), Times.Once());
		}

		public void SqlReaderSetupCallback()
		{
			int timesCalled = -1;
			_sqlReaderMock.Setup(x => x.ReadAsync())
				.Callback(() => ++timesCalled)
				.ReturnsAsync(() => timesCalled != 3);
		}

		public void SqlReaderSetupIndexCallback()
		{
			string[] attribute = { "record_id", "collection_id", "number" };
			int letterCount = 0;
			for (int i = 0; i < attribute.Length; i++)
			{
				int count = 0;
				_sqlReaderMock.SetupGet(x => x[attribute[i]])
					.Callback(() => ++count)
					.Returns(() => count);
			}
			_sqlReaderMock.SetupGet(x => x["letter"])
				.Callback(() => ++letterCount)
				.Returns(() => $"{letterCount}");
		}

		public void AssertParam(object[] param)
		{
			Assert.AreEqual(3, _parameters.Count);
			for (int i = 0; i < _parameters.Count; i++)
			{
				Assert.AreEqual(param[i], _parameters[i].Value);
				Assert.AreEqual($"param{i+1}", _parameters[i].ParameterName);
			}
			_connectionMock.Verify(x => x.CreateCommand("asds"), Times.Once);
		}
	}
}
