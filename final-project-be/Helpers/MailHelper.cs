using MailKit.Net.Smtp;

using MailKit.Security;
using MimeKit;
// using MailKit.Net.Smtp;


namespace final_project_be.Helpers
{
    public static class MailHelper
    {
        public static async Task Send(string toEmail, string subject, string messageText)
        {
            /* configuration */
            var message = new  MimeMessage();
            message.From.Add( new MailboxAddress("No Reply", "expertcookingtrial@gmail.com"));
            message.To.Add(new MailboxAddress("User", toEmail));
            message.Subject = subject;
            /* configuration */

            /* masukin message text */
            message.Body = new TextPart("plain")
            {
                Text = messageText
            };
            /* masukin message text */

            /* kirim emailnya */
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("expertcookingtrial@gmail.com", "fupiywbgrcymkqvq");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            /* kirim emailnya */
        }

        // internal static Task Send(string email, string v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}