using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
	public class DebugMailService : IMailService
	{
		public void SendMail(string to, string from, string subject, string body)
		{
			//Simulates the email service
			Debug.WriteLine($"Sending Mail:  To: {to}  From: {from}  Subject: {subject}  Message: {body}");

		}
	}
}
