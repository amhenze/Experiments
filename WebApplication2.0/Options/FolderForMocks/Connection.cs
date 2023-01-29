using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2._0.Options.FolderForMocks.Abstractions;

namespace WebApplication2._0.Options.FolderForMocks
{
	public class Connection : IConnection
	{
		private readonly NpgsqlConnection npgsqlConnection;

		public Connection(NpgsqlConnection npgsqlConnection)
		{
			this.npgsqlConnection = npgsqlConnection;
		}

		public void Open()
		{
			npgsqlConnection.Open();
		}
		public void Close()
		{
			npgsqlConnection.Close();
		}
		public ICommand CreateCommand(string command)
		{
			return new Command(new NpgsqlCommand (command, npgsqlConnection));
		}
	}
}
