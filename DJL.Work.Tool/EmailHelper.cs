using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DJL.Work.Tool
{
    public static class EmailHelper
    {
        //利用QQ的stmp服务器发送邮件
        private readonly static string SmtpServer = "smtp.qq.com";//邮件服务器
        private readonly static int SmtpServerPort = 25;
        //  如果是设置POP3和SMTP的SSL加密方式，则端口如下：
        //  POP3服务器（端口995）；
        //  SMTP服务器（端口465或587）。
        private readonly static bool SmtpEnableSsl = true;//不加密,注意设置QQ邮箱的 pop3/stmp 服务开启
        private readonly static string SmtpUsername = "394922860@qq.com";
        private readonly static string SmtpDisplayName = "394922860@qq.com";
        private readonly static string SmtpPassword = "btuzeaqzzepgbijf";//授权码 btuzeaqzzepgbijf

        /// <summary>
        /// 发送邮件到指定收件人
        /// </summary>
        /// <remarks>
        ///  2013-11-18 18:55 Created By iceStone
        /// </remarks>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">主题</param>
        /// <param name="mailBody">正文内容(支持HTML)</param>
        /// <param name="copyTos">抄送地址列表</param>
        /// <returns>是否发送成功</returns>
        public static bool Send(string to, string subject, string mailBody, params string[] copyTos)
        {
            return Send(new[] { to }, subject, mailBody, copyTos, new string[] { }, MailPriority.Normal);
        }

        /// <summary>
        /// 发送邮件到指定收件人
        /// </summary>
        /// <remarks>
        ///  2013-11-18 18:55 Created By iceStone
        /// </remarks>
        /// <param name="tos">收件人地址列表</param>
        /// <param name="subject">主题</param>
        /// <param name="mailBody">正文内容(支持HTML)</param>
        /// <param name="ccs">抄送地址列表</param>
        /// <param name="bccs">密件抄送地址列表</param>
        /// <param name="priority">此邮件的优先级</param>
        /// <param name="attachments">附件列表</param>
        /// <returns>是否发送成功</returns>
        /// <exception cref="System.ArgumentNullException">attachments</exception>
        public static bool Send(string[] tos, string subject, string mailBody, string[] ccs, string[] bccs, MailPriority priority, params Attachment[] attachments)
        {
            if (attachments == null) throw new ArgumentNullException("attachments");
            if (tos.Length == 0) return false;
            //创建Email实体
            var message = new MailMessage();
            message.From = new MailAddress(SmtpUsername, SmtpDisplayName);
            message.Subject = subject;
            message.Body = mailBody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Priority = priority;
            //插入附件
            foreach (var attachment in attachments)
            {
                message.Attachments.Add(attachment);
            }
            //插入收件人地址,抄送地址和密件抄送地址
            foreach (var to in tos.Where(c => !string.IsNullOrEmpty(c)))
            {
                message.To.Add(new MailAddress(to));
            }
            foreach (var cc in ccs.Where(c => !string.IsNullOrEmpty(c)))
            {
                message.CC.Add(new MailAddress(cc));
            }
            foreach (var bcc in bccs.Where(c => !string.IsNullOrEmpty(c)))
            {
                message.CC.Add(new MailAddress(bcc));
            }
            //创建SMTP客户端
            var client = new SmtpClient(SmtpServer, SmtpServerPort);
            client.EnableSsl = SmtpEnableSsl;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(SmtpUsername, SmtpPassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;          
            client.Send(message);
            return true;
        }
    }
}
