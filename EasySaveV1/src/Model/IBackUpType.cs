namespace BackUp.Model
{
	public interface IBackUpType
	{
		string Name { get; }
		string dirSource{ get; }
		string dirTarget {  get; }
		void Execute();
	}
}