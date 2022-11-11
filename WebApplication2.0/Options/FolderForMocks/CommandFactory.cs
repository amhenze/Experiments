using WebApplication2._0.Options.FolderForMocks.Abstractions;

namespace WebApplication2._0.Options.FolderForMocks
{
	public class CommandFactory : ICommandFactory
	{
		public ICommand Create(string command, IConnection connection)
		{
			return new Command(command, connection);
		}
	}
}
