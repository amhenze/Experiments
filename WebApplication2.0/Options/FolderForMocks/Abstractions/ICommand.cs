using Npgsql;

namespace WebApplication2._0.Options.FolderForMocks.Abstractions
{
	public interface ICommand
	{
		NpgsqlParameterCollection Parameters { get; }
		public int ExecuteNonQuery();
		public NpgsqlDataReader ExecuteReader();
		Task<ISQLReader> ExecuteReaderAsync();
	}
}
