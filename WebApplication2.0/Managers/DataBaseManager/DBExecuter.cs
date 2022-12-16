using Npgsql;
using WebApplication2._0.Attributes;
using WebApplication2._0.Interfaces;
using WebApplication2._0.Options.FolderForMocks;
using WebApplication2._0.Options.FolderForMocks.Abstractions;

namespace WebApplication2._0.DataBaseWorker
{
    public class DBExecuter : IDBExecuter
    {
        private IConnection _connect;
        private readonly ILogger<DBExecuter> _logger;

        public DBExecuter(IConnection connect, ILogger<DBExecuter> logger)
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
            ICommand cmd = _connect.CreateCommand(sql);
            AddParameters(cmd, parameters);
            cmd.ExecuteNonQuery();
        }


        public async Task<List<T>> ExecuteReader<T>(string sql, object[] parameters) where T : class, new()
        {
            List<T> dataList = new List<T>();
            ICommand cmd = _connect.CreateCommand(sql);
            AddParameters(cmd, parameters);
            System.Reflection.PropertyInfo[]? properties = typeof(T).GetProperties();
            bool noAttribute = true;
            Dictionary<System.Reflection.PropertyInfo, FieldNameAttribute> attributeDictionary =
                new Dictionary<System.Reflection.PropertyInfo, FieldNameAttribute>();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(FieldNameAttribute), true).FirstOrDefault() as FieldNameAttribute;
                if (attribute != null)
                    noAttribute = false;
                attributeDictionary.Add(property, attribute);
            }
            if (noAttribute) // возвращает list с пустой строкой
            {
                var Row = new T();
                dataList.Add(Row);
                return dataList;
            }
            await using ISQLReader reader = await cmd.ExecuteReaderAsync();
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

        public ICommand AddParameters(ICommand cmd, object[] parameters)
		{
            int i = 1;
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(new NpgsqlParameter($"param{i}", parameter));
                i++;
            }
            return cmd;
		}

    }
}
