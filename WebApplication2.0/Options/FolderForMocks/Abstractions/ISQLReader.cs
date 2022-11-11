using Npgsql;

namespace WebApplication2._0.Options.FolderForMocks.Abstractions
{
	public interface ISQLReader
	{
		NpgsqlDataReader Reader { get; set; }
		public object this[string name] { get; }
		Task<bool> ReadAsync();
		ValueTask DisposeAsync();
	}
}
