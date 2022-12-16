using Npgsql;
using WebApplication2._0.Options.FolderForMocks.Abstractions;

namespace WebApplication2._0.Options.FolderForMocks
{
	public class Command : ICommand
	{
		private readonly NpgsqlCommand _npgsqlCommand;
		public ICollection<NpgsqlParameter> Parameters => _npgsqlCommand.Parameters;

		ICollection<NpgsqlParameter> ICommand.Parameters => _npgsqlCommand.Parameters;

		public Command(NpgsqlCommand npgsqlCommand)
		{
			_npgsqlCommand = npgsqlCommand;
		}

		public int ExecuteNonQuery()
		{
			return _npgsqlCommand.ExecuteNonQuery();
		}

		public NpgsqlDataReader ExecuteReader()
		{
			return _npgsqlCommand.ExecuteReader();
		}

		public async Task<ISQLReader> ExecuteReaderAsync()
		{
			return new SQLReader(await _npgsqlCommand.ExecuteReaderAsync());
		}
	}
}
