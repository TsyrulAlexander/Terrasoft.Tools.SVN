using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using Terrasoft.Tools.Svn;
using Terrasoft.Tools.Svn.Properties;

namespace Terrasoft.Tools.SVN
{
    public static class BugReporter
    {
        public static void SendBugReport(object value, Type type) {
            var smtpClient = new SmtpClient("smtp.tscrm.com", 25);
            byte[] passBytes = Convert.FromBase64String(Resources.ResourceManager.GetString("SMTPCRD") ??
                                                        throw new ArgumentNullException(
                                                            nameof(Resources.ResourceManager)
                                                        )
            );
            string password = Encoding.UTF8.GetString(passBytes);
            ICredentialsByHost credentials = new NetworkCredential("BPMonlineBuild", password, "TSCRM");
            smtpClient.Credentials = credentials;
            var message = new MailMessage();
            var toMailAddress = new MailAddress("e.androsov@bpmonline.com", "Eugene Androsov");
            message.To.Add(toMailAddress);
            var fromMailAddress = new MailAddress("bpmonlinebuild@bpmonline.com", "Terrasoft.Tools.Svn");

            using (MemoryStream stream = GetJsonStream(value)) {
                var attachment = new Attachment(stream, type.FullName);
                message.Attachments.Add(attachment);
                message.From = fromMailAddress;
                try {
                    smtpClient.Send(message);
                } catch (SmtpException smtpException) {
                    Logger.Error(smtpException.Message, smtpException.StackTrace);
                }
            }
        }

        private static MemoryStream GetJsonStream(object value) {
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            string jsonObject = SerializeObjectToJsonString(value);
            streamWriter.Write(jsonObject);
            streamWriter.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        ///     Сериализация объекта в JSON строку
        /// </summary>
        /// <param name="value">Объект сериализации</param>
        /// <returns></returns>
        private static string SerializeObjectToJsonString(object value) {
            var javaScriptSerializer = new JavaScriptSerializer();
            string jsonObject = javaScriptSerializer.Serialize(value);
            return jsonObject;
        }
    }
}