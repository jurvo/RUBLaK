using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;


namespace RUBLaK
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args == null || args.Length == 0) return;
			if (Properties.Settings1.Default.loginID == "" || Properties.Settings1.Default.password == "" || args[0] == "update")
			{
				Console.WriteLine("Bitte geben Sie ihre Login-ID und ihr Passwort an:");
				Console.Write("Login ID: ");
				Properties.Settings1.Default.loginID = Console.ReadLine();
				Console.Write("Passwort: ");
				Properties.Settings1.Default.password = Console.ReadLine();
				Properties.Settings1.Default.Save();
				Console.WriteLine("Erfolgreich gespeichert!");
			}
			if (args[0] == "reset")
			{
				Properties.Settings1.Default.Reset();
				Console.WriteLine("Erfolgreich zurückgesetzt!");
			}
			if (args[0] == "Login" || args[0] == "Logout")
			{
				string action = args[0];
				string ip = GetLocalIPAddress();
				string username = args.Length == 3 ? args[1] : Properties.Settings1.Default.loginID;
				string password = args.Length == 3 ? args[2] : Properties.Settings1.Default.password;

				using (WebClient client = new WebClient())
				{
					byte[] response = client.UploadValues("https://login.rz.ruhr-uni-bochum.de/cgi-bin/laklogin", new NameValueCollection()
					{
						{ "code", "1" },
						{ "loginid",  username},
						{ "password",  password},
						{ "ipaddr", ip },
						{ "action", action }
					});
					string s = Encoding.UTF8.GetString(response);
					if (s.Contains("Authentisierung gelungen"))
						Console.WriteLine("Authentisierung gelungen!");
					else if (s.Contains("Authentisierung fehlgeschlagen"))
						Console.WriteLine("Authentisierung fehlgeschlagen!");
					else if (s.Contains("Logout"))
						Console.WriteLine("Logout erfolgreich!");
				}
			}
			System.Threading.Thread.Sleep(1000);
		}

		public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					if (ip.ToString().StartsWith("10."))
					{
						return ip.ToString();
					}
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}
	}
}
