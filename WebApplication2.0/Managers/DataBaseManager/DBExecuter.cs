using Npgsql;
using WebApplication2._0.Attributes;
using WebApplication2._0.Interfaces;

namespace WebApplication2._0.DataBaseWorker
{
    public class DBExecuter : IDBExecuter
    {
        private NpgsqlConnection _connect;
        private readonly ILogger<DBExecuter> _logger;

        public DBExecuter(NpgsqlConnection connect, ILogger<DBExecuter> logger)
        {
            _connect = connect;
            _logger = logger;
            _connect.Open();
        }

        ~DBExecuter()
        {
            try
            {
                _connect.Close();
            }
            catch
            {
                _logger.LogError("Соединение не существует");
            }
        }

        public void ExecuteNonQuery(string sql, object[] parameters)
        {
            int i = 1;
            using var cmd = new NpgsqlCommand(sql, _connect);
            foreach (var parameter in parameters)
            {
                cmd.Parameters.AddWithValue($"param{i}", parameter);
                i++;
            }
            cmd.ExecuteNonQuery();
        }

        //cmd.ExecuteScalar();

        public async Task<List<T>> ExecuteReader<T>(string sql, object[] parameters) where T : class , new()
        {
            List<T> dataList = new List<T>();
            await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _connect);
            int i = 1;
            foreach (var parameter in parameters)
            {
                cmd.Parameters.AddWithValue($"param{i}", parameter);
                i++;// вынести
            }
            System.Reflection.PropertyInfo[]? properties = typeof(T).GetProperties();
            Dictionary<System.Reflection.PropertyInfo, FieldNameAttribute> attributeDictionary =
                new Dictionary<System.Reflection.PropertyInfo, FieldNameAttribute>();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(FieldNameAttribute), true).FirstOrDefault() as FieldNameAttribute;
                attributeDictionary.Add(property, attribute);
            }

            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var Row = new T();
                foreach (System.Reflection.PropertyInfo property in properties)
                {
                    var attribute = attributeDictionary[property];
                    if (attribute != null)
                    {
                        var value = reader[attribute.FieldName];
                        property.SetValue(Row, value);
                    }
                }
                    dataList.Add(Row);
            }
            return dataList;
        }

    }
}
