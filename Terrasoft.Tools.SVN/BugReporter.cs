using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Security;
using System.Text;
using System.Web.Script.Serialization;
using Terrasoft.Tools.Svn.Properties;

namespace Terrasoft.Tools.Svn
{
    internal static class BugReporter
    {
        public static void SendBugReport(object value, Type type) {
            if (type == null) {
                return;
            }

            using (var smtpClient = new SmtpClient("smtp.tscrm.com", 25)) {
                using (var password = new SecureString()) {
                    GetSecurePassword(password);
                    ICredentialsByHost credentials = new NetworkCredential(
                        "BPMonlineBuild",
                        password, "TSCRM"
                    );
                    smtpClient.Credentials = credentials;
                    using (var message = new MailMessage()) {
                        var toMailAddress = new MailAddress("e.androsov@bpmonline.com", "Eugene Androsov");
                        message.To.Add(toMailAddress);
                        var fromMailAddress = new MailAddress("bpmonlinebuild@bpmonline.com", "Terrasoft.Tools.Svn");

                        using (MemoryStream stream = GetJsonStream(value)) {
                            var attachment = new Attachment(stream, type.FullName + ".json");
                            message.Attachments.Add(attachment);
                            message.From = fromMailAddress;
                            try {
                                smtpClient.Send(message);
                            } catch (SmtpException smtpException) {
                                Logger.Error(smtpException.Message, smtpException.StackTrace);
                            }
                        }
                    }
                }
            }
        }

        private static void GetSecurePassword(SecureString securePassword) {
            ResourceManager resourceManager = Resources.ResourceManager;

            string encodedHash = resourceManager?.GetString("SMTPCRD");

            if (encodedHash == null) {
                return;
            }

            byte[] passBytes = Convert.FromBase64String(encodedHash);
            char[] hash = Encoding.UTF8.GetChars(passBytes);
            //var securePassword = new SecureString();
            foreach (char c in hash) {
                securePassword.AppendChar(c);
            }
        }

        private static MemoryStream GetJsonStream(object value) {
            using (var stream = new MemoryStream()) {
                var streamWriter = new StreamWriter(stream);
                string jsonObject = SerializeObjectToJsonString(value);
                streamWriter.Write(jsonObject);
                streamWriter.Flush();
                stream.Position = 0;
                return stream;
            }
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