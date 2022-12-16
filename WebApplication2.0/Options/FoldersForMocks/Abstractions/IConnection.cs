using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2._0.Options.FolderForMocks.Abstractions
{
	public interface IConnection
	{
		ICommand CreateCommand(string command);
		public void Open();
		public void Close();
	}
}
