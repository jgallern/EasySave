namespace Model
{
	public interface IBackUpType
	{
		string Name { get; }
		string FileSource{ get; }
		string FileTarget {  get; }
		void Execute();
	}
}