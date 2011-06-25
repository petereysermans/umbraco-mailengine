using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;

using umbraco.cms.businesslogic.member;
using System.IO;

namespace Nibble.Umb.MailEngine
{
    public class Library
    {

        ///// <summary>
        ///// Sends the mail from string.
        ///// </summary>
        ///// <param name="FromMail">From mail.</param>
        ///// <param name="ToMail">To mail.</param>
        ///// <param name="Subject">The subject.</param>
        ///// <param name="Body">The body.</param>
        ///// <param name="IsHtml">if set to <c>true</c> [is HTML].</param>
        //public static void SendMailFromString(string FromMail, string ToMail, string Subject, string Body, bool IsHtml)
        //{
        //    MailMessage mail = new MailMessage(FromMail.Trim(), ToMail.Trim());

        //    // populate the message
        //    mail.Subject = Helper.memberMergeString(Member.GetCurrentMember(),Subject,false);
        //    if (IsHtml)
        //        mail.IsBodyHtml = true;
        //    else
        //        mail.IsBodyHtml = false;

        //    mail.Body = Helper.memberMergeString(Member.GetCurrentMember(),Body,IsHtml);

        //    // send it
        //    SmtpClient smtpClient = new SmtpClient();
        //    smtpClient.Send(mail);
        //}

        ///// <summary>
        ///// Sends the mail from static template.
        ///// </summary>
        ///// <param name="FromMail">From mail.</param>
        ///// <param name="ToMail">To mail.</param>
        ///// <param name="Subject">The subject.</param>
        ///// <param name="StaticTemplate">The static template.</param>
        //public static void SendMailFromStaticTemplate(string FromMail, string ToMail, string Subject, string StaticTemplate)
        //{
        //    MailMessage mail = new MailMessage(FromMail.Trim(), ToMail.Trim());

        //    // populate the message
        //    mail.Subject = Helper.memberMergeString(Member.GetCurrentMember(), Subject, false);
 
        //    mail.IsBodyHtml = true;

        //    string body = string.Empty;

        //    if (File.Exists(HttpContext.Current.Server.MapPath(StaticTemplate)))
        //    {
        //        StreamReader sreader;

        //        sreader = File.OpenText(HttpContext.Current.Server.MapPath(StaticTemplate));
        //        body = sreader.ReadToEnd();

        //        sreader.Close();
        //    }

        //    mail.Body = Helper.memberMergeString(Member.GetCurrentMember(), body, true);

        //    // send it
        //    SmtpClient smtpClient = new SmtpClient();
        //    smtpClient.Send(mail);
        //}

        private static void SendMail(MailMessage message)
        {   
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(message);
        }

        /// <summary>
        /// Sends the mail from page.
        /// </summary>
        /// <param name="FromMail">From mail.</param>
        /// <param name="ToMail">To mail.</param>
        /// <param name="Subject">The subject.</param>
        /// <param name="PageId">The page id.</param>
        public static void SendMailFromPage(string FromMail, string ToMail, string Subject, int PageId)
        {
            string body = umbraco.library.RenderTemplate(PageId);

            MailMessage mail = Helper.BuildMail(Subject, body);



            
            mail.From = new MailAddress(FromMail.Trim());
            mail.To.Add(new MailAddress(ToMail.Trim()));

            SendMail(mail);
        }


        /// <summary>
        /// Sends the mail from page to member.
        /// </summary>
        /// <param name="FromMail">From mail.</param>
        /// <param name="member">The member.</param>
        /// <param name="Subject">The subject.</param>
        /// <param name="PageId">The page id.</param>
        /// 
        public static void SendMailFromPageToMember(string FromMail, Member member, string Subject, int PageId)
        {
            string body = umbraco.library.RenderTemplate(PageId);
            SendMailFromPageToMember(FromMail, member, Subject, PageId, body, Helper.AssembleResources(body));
        }

        public static void SendMailFromPageToMember(string FromMail, Member member, string Subject, int PageId, string body, Dictionary<string, LinkedResource> imagedictionary)
        {

            MailMessage mail = Helper.BuildMail(member,Subject, body, imagedictionary);

            mail.From = new MailAddress(FromMail.Trim());
            mail.To.Add(new MailAddress(member.Email));

            SendMail(mail);
        }
        
        /// <summary>
        /// Sends the mail from page and template.
        /// </summary>
        /// <param name="FromMail">From mail.</param>
        /// <param name="ToMail">To mail.</param>
        /// <param name="Subject">The subject.</param>
        /// <param name="PageId">The page id.</param>
        /// <param name="TemplateId">The template id.</param>
        public static void SendMailFromPageAndTemplate(string FromMail, string ToMail, string Subject, int PageId, int TemplateId)
        {
            string body = umbraco.library.RenderTemplate(PageId, TemplateId);

            MailMessage mail = Helper.BuildMail(Subject, body);

            mail.From = new MailAddress(FromMail.Trim());
            mail.To.Add(new MailAddress(ToMail.Trim()));

            SendMail(mail);
        }
    }
}
