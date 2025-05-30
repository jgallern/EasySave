namespace Core.Model
{
	public interface IBackUpType
	{
		IJobs job { get; set; }
		Task ExecuteAsync(CancellationToken cancellationToken);
	}
}