using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Resources;
using System.Security;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Terrasoft.Tools.Svn.Properties;

namespace Terrasoft.Tools.Svn
{
    internal static class BugReporter
    {
        public static void SendBugReport(object value, Type type)
        {
            if (type is null) {
                return;
            }

            using (SmtpClient smtpClient = new SmtpClient("smtp.tscrm.com", 25)) {
                using (SecureString password = new SecureString()) {
                    GetSecurePassword(password);
                    const string domain = "TSCRM";
                    const string userName = "BPMonlineBuild";
                    ICredentialsByHost credentials = new NetworkCredential(userName, password, domain);
                    smtpClient.Credentials = credentials;
                    using (MailMessage message = new MailMessage()) {
                        const string toAddress = "e.androsov@bpmonline.com";
                        const string displayName = "Eugene Androsov";
                        MailAddress toMailAddress = new MailAddress(toAddress, displayName);
                        message.To.Add(toMailAddress);
                        const string fromAddress = "bpmonlinebuild@bpmonline.com";
                        const string subject = "Terrasoft.Tools.Svn";
                        MailAddress fromMailAddress = new MailAddress(fromAddress, subject);

                        using (MemoryStream stream = GetJsonStream(value)) {
                            Attachment attachment = new Attachment(stream, type.FullName + ".json");
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

        private static void GetSecurePassword(SecureString securePassword)
        {
            ResourceManager resourceManager = Resources.ResourceManager;

            string encodedHash = resourceManager?.GetString("SMTPCRD", CultureInfo.CurrentCulture);

            if (encodedHash is null) {
                return;
            }

            byte[] passBytes = Convert.FromBase64String(encodedHash);
            char[] hash = Encoding.UTF8.GetChars(passBytes);
            //var securePassword = new SecureString();
            foreach (char c in hash) {
                securePassword.AppendChar(c);
            }
        }

        private static MemoryStream GetJsonStream(object value)
        {
            MemoryStream stream = new MemoryStream();
            using (StreamWriter streamWriter = new StreamWriter(stream)) {
                string jsonObject = SerializeObjectToJsonString(value);
                streamWriter.Write(jsonObject);
                streamWriter.Flush();
            }

            stream.Position = 0;
            return stream;
        }

        /// <summary>
        ///     Сериализация объекта в JSON строку
        /// </summary>
        /// <param name="value">Объект сериализации</param>
        /// <returns></returns>
        private static string SerializeObjectToJsonString(object value)
        {
            JsonSerializerSettings converters = new JsonSerializerSettings();
            JsonConverter converter = new StringEnumConverter(new DefaultNamingStrategy(), false);
            converters.Converters.Add(converter);
            return JsonConvert.SerializeObject(value, Formatting.Indented, converters);
            /*
            var javaScriptSerializer = new JavaScriptSerializer();
            string jsonObject = javaScriptSerializer.Serialize(value);
            return jsonObject;*/
        }
    }
}