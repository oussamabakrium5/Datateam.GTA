
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;

namespace Datateam.Utilities
{
	public class NotificationService : INotificationService
	{
		public async Task<bool> SendNotification(string from, string to, string message, NotificationType notificationType)
		{

			switch (notificationType)
			{
				case NotificationType.Email:
					return await SendEmailNotification(from, to, message);
				case NotificationType.WhatsApp:
					return await SendWhatsAppNotification(to, message); // WhatsApp notification logic
				case NotificationType.SMS:
					return await SendSMSNotification(to, message); // SMS notification logic
				default:
					throw new ArgumentException("Invalid NotificationType provided");
			}

			//throw new NotImplementedException();
		}

		private async Task<bool> SendEmailNotification(string from, string to, string message)
		{
			var client = new SmtpClient("smtp.office365.com", 587)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(from, "your password")
			};

			try
			{
				await client.SendMailAsync(new MailMessage(from, to, " ", message));
				return true;
			}
			catch (Exception ex)
			{
				// Email sending failed
				return false;
			}

		}

		private async Task<bool> SendWhatsAppNotification(string to, string message)
		{
			/*https://faq.whatsapp.com/695500918177858*/
			return true;
		}

		private async Task<bool> SendSMSNotification(string to, string message)
		{
			string apikey = "";
			string senders = "Datateam";

			string baseUrl = "https://api.textlocal.in/send/";

			try
			{
				HttpWebRequest objrequest = (HttpWebRequest)WebRequest.Create(baseUrl);
				objrequest.Method = "POST";
				objrequest.ContentType = "application/x-www-form-urlencoded";

				objrequest.Headers.Add("apikey", apikey);
				objrequest.Headers.Add("numbers", to);
				objrequest.Headers.Add("message", message);
				objrequest.Headers.Add("senders", senders);

				using (StreamWriter mywriter = new StreamWriter(objrequest.GetRequestStream()))
				{
					await mywriter.FlushAsync();
				}

				HttpWebResponse response = (HttpWebResponse)await objrequest.GetResponseAsync();
				return response.StatusCode == HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				return false;
			}

		}
	}
}
