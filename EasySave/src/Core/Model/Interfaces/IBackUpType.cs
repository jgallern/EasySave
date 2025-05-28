namespace Core.Model
{
	public interface IBackUpType
	{
		IJobs job { get; set; }
		void Execute();
	}
}