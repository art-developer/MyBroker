using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace MyBroker.Controller
{
    public class MailController
    {
        private string _from;
        private string _smtpHost;
        private string _smtpUserName;
        private string _smtpPassword;
        private string _to;

        public MailController(string from, string smtpHost, string smtpUserName, string smtpPassword,string to)
        {
            _from = from;
            _smtpHost = smtpHost;
            _smtpPassword = smtpPassword;
            _smtpUserName = smtpUserName;
            _to = to;
        }

        public void Send(string subject, string body,Dictionary<string, string> images)
        {
            if(string.IsNullOrEmpty(_smtpHost))
                throw new InvalidOperationException("Невозможно отправить сообщение - не указан smtpHost");
            if (string.IsNullOrEmpty(_smtpUserName))
                throw new InvalidOperationException("Невозможно отправить сообщение - не указан smtpUserName");
            if (string.IsNullOrEmpty(_smtpPassword))
                throw new InvalidOperationException("Невозможно отправить сообщение - не указан smtpPassword");
            if (string.IsNullOrEmpty(_from))
                throw new InvalidOperationException("Невозможно отправить сообщение - не указан from");


            if (string.IsNullOrEmpty(_to))
                throw new ArgumentNullException("to");

            string[] recipients = _to.Split(';');

            foreach (string recipient in recipients)
            {
                if (string.IsNullOrEmpty(recipient.Trim()))
                    continue;
                MailMessage mess = new MailMessage(_from, recipient);
                mess.Subject = subject;
                

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body.Replace(Environment.NewLine,"</br>"), null, "text/html");
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");

                //create the LinkedResource (embedded image)
                foreach (string key in images.Keys)
                {
                    string fileName = images[key];
                    LinkedResource logo = new LinkedResource(fileName);
                    logo.ContentId = key;
                    //add the LinkedResource to the appropriate view
                    htmlView.LinkedResources.Add(logo);
                }

                mess.AlternateViews.Add(plainView);
                mess.AlternateViews.Add(htmlView);

                SmtpClient client = new SmtpClient(_smtpHost,587);
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(_smtpUserName, _smtpPassword);
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = SMTPUserInfo;
                client.Send(mess);
            }
        }

        public void Send(string subject, string body)
        {
            Send(subject, body, new Dictionary<string, string>());
        }
    }
}
