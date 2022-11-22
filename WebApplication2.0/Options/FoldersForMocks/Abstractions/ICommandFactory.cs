namespace WebApplication2._0.Options.FolderForMocks.Abstractions
{
	public interface ICommandFactory
	{
		ICommand Create(string command, IConnection connection);
	}
}
