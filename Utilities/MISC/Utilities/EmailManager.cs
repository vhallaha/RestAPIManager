using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;

namespace Utilities
{
    /// <summary>
    /// Email Manager
    /// </summary>
    public class EmailManager
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="oEmail"></param>
        /// <returns></returns>
        public static bool SendEmail(Email oEmail)
        {
            MailMessage oMail = new MailMessage();

            try
            {
                oMail.Subject = oEmail.Subject;
                oMail.From = new MailAddress(oEmail.From);

                // Check if the user is trying to send an email to multiple users
                if (!String.IsNullOrEmpty(oEmail.Delimiter.ToString().Trim()) &&
                                          oEmail.To.Contains(oEmail.Delimiter))
                {
                    var oTo = oEmail.To.Split(oEmail.Delimiter);

                    foreach (string to in oTo)
                    {
                        if (!string.IsNullOrWhiteSpace(to))
                            oMail.To.Add(new MailAddress(to));
                    }
                }
                else
                {
                    oMail.To.Add(new MailAddress(oEmail.To));
                }

                oMail.Body = oEmail.Body;
                oMail.IsBodyHtml = true;

                // Create an SMTP Client to send the mail
                SmtpClient oClient = new SmtpClient();

                if (oEmail.SMTPHost != "localhost")
                {
                    oClient.Credentials = new System.Net.NetworkCredential(oEmail.SMTPUser, oEmail.SMTPPassword);
                    oClient.EnableSsl = oEmail.EnableSSL;
                }

                oClient.Host = oEmail.SMTPHost;
                oClient.Send(oMail);

                return true;
            }
            catch
            {
                // Log Error
            }

            return false;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="oEmail"></param>
        /// <param name="bEnableThreading"></param>
        /// <returns></returns>
        public static bool SendEmail(Email oEmail, bool bEnableThreading)
        {
            MailMessage oMail = new MailMessage();

            try
            {
                oMail.Subject = oEmail.Subject;
                oMail.From = new MailAddress(oEmail.From);

                // Check if the user is trying to send an email to multiple users
                if (!String.IsNullOrEmpty(oEmail.Delimiter.ToString().Trim()) &&
                                          oEmail.To.Contains(oEmail.Delimiter))
                {
                    var oTo = oEmail.To.Split(oEmail.Delimiter);

                    foreach (string to in oTo)
                    {
                        oMail.To.Add(new MailAddress(to));
                    }
                }
                else
                {
                    oMail.To.Add(new MailAddress(oEmail.To));
                }

                oMail.Body = oEmail.Body;
                oMail.IsBodyHtml = true;

                // Create an SMTP Client to send the mail
                SmtpClient oClient = new SmtpClient();
                oClient.Credentials = new System.Net.NetworkCredential(oEmail.SMTPUser, oEmail.SMTPPassword);
                oClient.Host = oEmail.SMTPHost;
                oClient.EnableSsl = oEmail.EnableSSL;
                oClient.Port = oEmail.SMTPPort;

                if (bEnableThreading)
                {
                    ThreadStart threadStart = delegate { oClient.Send(oMail); };
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                }
                else
                    oClient.Send(oMail);

                return true;
            }
            catch (Exception)
            {
                // Track Error
            }

            return false;
        }
    }

    /// <summary>
    /// Email Object
    /// </summary>
    public class Email
    {
        //----------------------------------
        // Email Information
        //----------------------------------

        /// <summary>
        /// To
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// From
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; }

        //----------------------------------
        // Credentials
        //----------------------------------

        /// <summary>
        /// SMTP User
        /// </summary>
        public string SMTPUser { get; set; }

        /// <summary>
        /// SMTP Password
        /// </summary>
        public string SMTPPassword { get; set; }

        /// <summary>
        /// SMTP Host
        /// </summary>
        public string SMTPHost { get; set; }

        /// <summary>
        /// SMTP Port
        /// </summary>
        public int SMTPPort { get; set; }

        //----------------------------------
        // Options
        //----------------------------------

        /// <summary>
        /// Delimiter
        /// </summary>
        public char Delimiter { get; set; }

        /// <summary>
        /// Enable SSL
        /// </summary>
        public bool EnableSSL { get; set; }
    }
}