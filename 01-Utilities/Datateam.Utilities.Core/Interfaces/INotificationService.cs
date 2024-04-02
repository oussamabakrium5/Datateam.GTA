namespace Datateam.Utilities
{
	public interface INotificationService
	{
		Task<bool> SendNotification(string from, string to, string message, NotificationType notificationType );
	}
}
