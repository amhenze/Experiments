using Npgsql;
using WebApplication2._0.Options.FolderForMocks.Abstractions;

namespace WebApplication2._0.Options.FolderForMocks
{
	public class Command : ICommand
	{
		private readonly NpgsqlCommand npgsqlCommand;
		public NpgsqlParameterCollection Parameters => npgsqlCommand.Parameters;
		public Command(string? command, IConnection? connection)
		{
			npgsqlCommand = new NpgsqlCommand(command, connection.GetConnection());
		}

		public int ExecuteNonQuery()
		{
			return npgsqlCommand.ExecuteNonQuery();
		}

		public NpgsqlDataReader ExecuteReader()
		{
			return npgsqlCommand.ExecuteReader();
		}

		public async Task<ISQLReader> ExecuteReaderAsync()
		{
			return new SQLReader(await npgsqlCommand.ExecuteReaderAsync());
		}
	}
}
