using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.ComponentModel;

namespace SimpleEmailLab1
{
    class Program
    {
        static string userToken = new Guid().ToString();
        static string EMAIL_ADDRESS = "dummyemailaddress";
        static SmtpClient client = new SmtpClient("smtp.gmail.com")
        {
            Port = 25,
            EnableSsl = true,
            Credentials = new NetworkCredential(EMAIL_ADDRESS, "******"),  //change this info!
        };
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the command line email client!");
            var loop = true;
            while (loop)
            {
                Console.WriteLine("\nNew Message");
                Console.Write("To: ");
                var to = Console.ReadLine();

                Console.Write("Subject: ");
                var subject = Console.ReadLine();

                Console.Write("Path to text file of body: ");
                var filepath = Console.ReadLine();
                StreamReader reader;
                if (!string.IsNullOrEmpty(filepath))
                {
                    reader = File.OpenText(filepath);
                    var body = reader.ReadToEnd();
                    Console.WriteLine($"Body Text: \n{body}");
                    Console.Write("Are you sure you want to send? (Y/N): ");
                    var key = Console.ReadKey().Key;
                    if (key != ConsoleKey.Y)
                    {
                        continue;
                    }

                    var message = new MailMessage()
                    {
                        Subject = subject,
                        From = new MailAddress(EMAIL_ADDRESS),
                        Body = body,
                    };
                    message.To.Add(to);

                    // Link event handler
                    client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);

                    // Send email
                    client.Send(message);
                    Console.Write("\nWould you like to send another email? (Y/N): ");
                    key = Console.ReadKey().Key;
                    if (key != ConsoleKey.Y)
                    {
                        loop = false;
                    }
                }
                Console.WriteLine("invalid path to file text! quitting program.....");
                loop = false;

            }
        }

        static void client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Encourage quick failing
            if (e.Cancelled == true)
            {
                Console.Error.WriteLine("Message was cancelled");
                return;
            }

            if (e.Error != null)
            {
                Console.Error.WriteLine($"An error occured: {e.Error.Message}");
                return;
            }
            Console.WriteLine("Email sent!");

        }
    }
}