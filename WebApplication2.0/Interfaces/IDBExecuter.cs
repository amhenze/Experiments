namespace WebApplication2._0.Interfaces
{
    public interface IDBExecuter
    {
        void ExecuteNonQuery(string sql, object[] parameters);
        Task<List<T>> ExecuteReader<T>(string sql, object[] parameters) where T : class, new();
		void Dispose();
	}
}
