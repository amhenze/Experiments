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
        private ICommand _command;
        private readonly ILogger<DBExecuter> _logger;
        private readonly ICommandFactory _commandFactory;

        public DBExecuter(IConnection connect, ILogger<DBExecuter> logger, ICommand command, ICommandFactory commandFactory)
        {
            _connect = connect;
            _command = command;
            _logger = logger;
            _commandFactory = commandFactory;
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
            ICommand cmd = _commandFactory.Create(sql, _connect);
            AddParameters(cmd, parameters);
            cmd.ExecuteNonQuery();
        }


        public async Task<List<T>> ExecuteReader<T>(string sql, object[] parameters) where T : class, new()
        {
            List<T> dataList = new List<T>();
            ICommand cmd = _commandFactory.Create(sql, _connect);
            AddParameters(cmd, parameters);
            System.Reflection.PropertyInfo[]? properties = typeof(T).GetProperties();
            bool noAtribute = true;
            await using ISQLReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var Row = new T();
                foreach (System.Reflection.PropertyInfo property in properties)
                {
                    var attribute = property.GetCustomAttributes(typeof(FieldNameAttribute), true).FirstOrDefault() as FieldNameAttribute;
                    if (attribute != null)
                    {
                        noAtribute = false;
                        var value = reader[attribute.FieldName];
                        property.SetValue(Row, value);
                    }
                    if (noAtribute)
                        throw new Exception("Model with no atributes");
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
