using Npgsql;
using WebApplication2._0.Options.FolderForMocks.Abstractions;

namespace WebApplication2._0.Options.FolderForMocks
{
	public class SQLReader : ISQLReader
	{
		public NpgsqlDataReader Reader { get; set; }

		public object this[string name] 
		{
			get => Reader[name];
		}

		public SQLReader(NpgsqlDataReader reader)
		{
			Reader = reader;
		}

		public Task<bool> ReadAsync()
		{
			return Reader.ReadAsync();
		}

		public async ValueTask DisposeAsync()
		{
			await Reader.DisposeAsync().ConfigureAwait(false);
		}
	}
}
