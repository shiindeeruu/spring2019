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
        static void Main(string[] args)
        {
           
            Console.WriteLine("Welcome to the command line email client!");
            var loop = true;
            while (loop)
            {
                Console.WriteLine("\nNew Message");
                Console.Write("To: ");
                string to = Console.ReadLine();

                Console.Write("Subject: ");
                string subject = Console.ReadLine();

                Console.Write("Body: ");
                string body = Console.ReadLine();
                
                if (!string.IsNullOrEmpty(body))
                {
                    Console.WriteLine($"Body Text: \n{body}");
                    Console.Write("Are you sure you want to send? (Y/N): ");
                    var key = Console.ReadKey().Key;
                    if (key != ConsoleKey.Y)
                    {
                        continue;
                    }

                  
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("bhuvanagopal@gmail.com");
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                  
                    SmtpServer.Port = 25;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("bhuvanagopal", "*******");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    Console.WriteLine("done!");
                    // Link event handler
                    SmtpServer.SendCompleted += new SendCompletedEventHandler(SmtpServer_SendCompleted);

                    Console.Write("\nWould you like to send another email? (Y/N): ");
                    key = Console.ReadKey().Key;
                    if (key != ConsoleKey.Y)
                    {
                        loop = false;
                    }
                }

            }
            
        }

        static void SmtpServer_SendCompleted(object sender, AsyncCompletedEventArgs e)
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