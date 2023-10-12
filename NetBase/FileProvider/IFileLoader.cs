namespace NetBase.FileProvider
{
	public interface IFileLoader
	{
		string Load(string path);
		string[] GetFiles();
	}
}
