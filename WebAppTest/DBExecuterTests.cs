using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2._0.DataBaseWorker;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Managers;
using WebApplication2._0.Models;
using WebApplication2._0.Options.FolderForMocks;
using WebApplication2._0.Options.FolderForMocks.Abstractions;

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


		[TestInitialize]
        public void Initialize()
        {
			//_dbExecuterTest = new Mock<IDBExecuter>();
            _loggerMock = new Mock<ILogger<DBExecuter>>();
			_connectionMock = new Mock<IConnection>();
			_commandMock = new Mock<ICommand>();
			_commandFactoryMock = new Mock<ICommandFactory>();
			//connectionMock = new NpgsqlConnection("Host=pg_container;Username=root;Password=root;Port=5432;Database=test_db");

			_dbExecuterTest = new DBExecuter(_connectionMock.Object, _loggerMock.Object, _commandMock.Object,_commandFactoryMock.Object);
		}

        [TestMethod]
		//[ExpectedException(typeof(InvalidOperationException))]
		public void DBExecuter_ExecuteNonQuery()
        {
			//arrange 
			_commandFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<IConnection>())).Returns(_commandMock.Object);
			object[] param = {1,1,2};
			_commandMock.Setup(x => x.ExecuteNonQuery()).Returns(3);
			//act
			_dbExecuterTest.ExecuteNonQuery("asds",param);
			//assert
			_commandMock.Verify(x => x.ExecuteNonQuery(), Times.Once);
        }

		//[TestMethod]
		//public void

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void MinPoolSizeLargeThanPoolSizeLimit()
		{
			var conn = new NpgsqlConnection(_connectionMock + ";MinPoolSize=1025;");
			conn.Open();
			conn.Close();
		}
	}
}
