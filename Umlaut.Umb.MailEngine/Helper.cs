using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using umbraco.cms.businesslogic.property;
using umbraco.cms.businesslogic.member;
using umbraco.presentation.umbracobase.library;

namespace Nibble.Umb.MailEngine
{
    static class Helper
    {
        
        /// <summary>
        /// Builds the mail.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public static MailMessage BuildMail(string subject, string body)
        {
            return BuildMail(Member.GetCurrentMember(), subject, body);

            //MailMessage message = new MailMessage();
            //message.Subject = memberMergeString(Member.GetCurrentMember(), subject, false);
            //message.IsBodyHtml = true;
            //HtmlBody htmlbody = BuildBody(memberMergeString(Member.GetCurrentMember(),body,true));

            //AlternateView item = AlternateView.CreateAlternateViewFromString(htmlbody.body, null, "text/html");

            //foreach (LinkedResource resource in htmlbody.linkedRessources.Values)
            //{
            //    item.LinkedResources.Add(resource);
            //}

            //message.AlternateViews.Add(item);

            //return message;
        }

        /// <summary>
        /// Builds the mail.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        /// 
        public static MailMessage BuildMail(Member member, string subject, string body)
        {
            return BuildMail(member, subject, body, AssembleResources(body));
        }
        public static MailMessage BuildMail(Member member, string subject, string body, Dictionary<string, LinkedResource> imagedictionary)
        {

            MailMessage message = new MailMessage();
            message.Subject = memberMergeString(member, subject, false);
            message.IsBodyHtml = false;
            message.Body = "You need an HTML capable mail client to see this message.";

            //AlternateView plain = AlternateView.CreateAlternateViewFromString("You need an HTML capable mail client to see this message.", null, "text/plain");
            //plain.ContentType = new System.Net.Mime.ContentType("text/plain");
            //plain.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

            //message.AlternateViews.Add(plain);


            HtmlBody htmlbody = BuildBody(memberMergeString(member, body, true),imagedictionary);


            AlternateView item = AlternateView.CreateAlternateViewFromString(htmlbody.body, System.Text.Encoding.GetEncoding("iso-8859-1"), "text/html");
            item.ContentType = new System.Net.Mime.ContentType("text/html");

            foreach (LinkedResource resource in htmlbody.linkedRessources.Values)
            {
                item.LinkedResources.Add(resource);
            }

            message.AlternateViews.Add(item);

            return message;
        }

        /// <summary>
        /// merges memberdata.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <param name="text">The text.</param>
        /// <param name="htmlLineBreaks">if set to <c>true</c> [HTML line breaks].</param>
        /// <returns></returns>
        public static string memberMergeString(Member m, string text, bool htmlLineBreaks)
        {
            if (m == null)
            {
                return text;
            }
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            string str = text;
            foreach (Property property in m.getProperties)
            {
                string alias = "";
                string str3 = "";
                alias = property.PropertyType.Alias;
                str3 = m.getProperty(alias).Value.ToString();
                if (htmlLineBreaks)
                {
                    str3 = str3.Replace("\n", "<br>");
                }
                string str4 = "[" + alias + "]";
                if (!(string.IsNullOrEmpty(str4) || string.IsNullOrEmpty(str3)))
                {
                    str = str.Replace(str4, str3);
                }
            }
            
            return str.Replace("[memberId]", m.Id.ToString()).Replace("[memberEmail]", m.Email).Replace("[memberLoginName]", m.LoginName).Replace("[memberPassword]", m.Password).Replace("[memberName]", m.Text).Replace("[memberText]", m.Text);
        }
        private static string _serverName = null;
        public static string serverName
        {
            get
            {
                if (_serverName == null)
                    _serverName  = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                return _serverName;
            }
        }

        public static Dictionary<string, LinkedResource> AssembleResources(string body)
        {
            Dictionary<string, LinkedResource> dictionary = new Dictionary<string, LinkedResource>();

            string str = serverName;
            string pattern = "href=\"?([^\\\"' >]+)|src=\\\"?([^\\\"' >]+)|background=\\\"?([^\\\"' >]+)";

            MatchCollection matchs = Regex.Matches(body, pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            foreach (Match match in matchs)
            {
                if (match.Groups.Count <= 0)
                {
                    continue;
                }
                if ((((match.Groups[1].Value.ToLower().IndexOf("http://") == -1) && (match.Groups[2].Value.ToLower().IndexOf("http://") == -1)) && (match.Groups[1].Value.ToLower().IndexOf("mailto:") == -1)) && (match.Groups[2].Value.ToLower().IndexOf("mailto:") == -1))
                {
                    if (match.Groups[1].Value == "")
                    {
                        string str4 = "jpg,jpeg,gif,png";
                        string image = match.Groups[2].Value;
                        if (image == "")
                        {
                            image = match.Groups[3].Value;
                        }
                        string oldValue = image;
                        string str7 = image.Split(new char[] { char.Parse(".") })[image.Split(new char[] { char.Parse(".") }).Length - 1].ToLower();
                        if (str4.IndexOf(str7) != -1)
                        {
                            string contentId = Guid.NewGuid().ToString();
                            LinkedResource resource = CreateLinkedResource(image, str7, contentId);
                            if (resource != null)
                            {
                                if (!dictionary.ContainsKey(image))
                                {
                                    dictionary.Add(image, resource);
                                }
                            }
                       }
                    }
                }
            }
            return dictionary;
        }
        private static HtmlBody BuildBody(string body)
        {
            return BuildBody(body,AssembleResources(body));
        }
        /// <summary>
        /// Builds the body.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        private static HtmlBody BuildBody(string body, Dictionary<string, LinkedResource> imagedictionary)
        {

            Dictionary<string, LinkedResource> dictionary = new Dictionary<string, LinkedResource>();

            string str = serverName;
            string pattern = "href=\"?([^\\\"' >]+)|src=\\\"?([^\\\"' >]+)|background=\\\"?([^\\\"' >]+)";

            MatchCollection matchs = Regex.Matches(body, pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            foreach (Match match in matchs)
            {
                if (match.Groups.Count <= 0)
                {
                    continue;
                }
                if ((((match.Groups[1].Value.ToLower().IndexOf("http://") == -1) && (match.Groups[2].Value.ToLower().IndexOf("http://") == -1) && (match.Groups[3].Value.ToLower().IndexOf("http://") == -1)) && (match.Groups[1].Value.ToLower().IndexOf("mailto:") == -1)) && (match.Groups[2].Value.ToLower().IndexOf("mailto:") == -1))
                {
                    if (match.Groups[1].Value != "")
                    {
                        if (match.Groups[0].Value.ToLower() == "href=\"/")
                        {
                            if (match.Groups[1].Value.IndexOf("?") == -1)
                            {
                                body = body.Replace(match.Groups[0].Value + "\"", "href=\"" + str + match.Groups[1].Value + "\"");
                            }
                            else
                            {
                                body = body.Replace(match.Groups[0].Value + "\"", "href=\"" + str + match.Groups[1].Value + "\"");
                            }
                        }
                        else if (match.Groups[1].Value.IndexOf("?") == -1)
                        {
                            body = body.Replace("href=\"" + match.Groups[1].Value + "\"", "href=\"" + str + match.Groups[1].Value + "\"");
                        }
                        else
                        {
                            body = body.Replace("href=\"" + match.Groups[1].Value + "\"", "href=\"" + str + match.Groups[1].Value + "\"");
                        }
                    }
                    else
                    {
                        string str4 = "jpg,jpeg,gif,png";
                        string image = match.Groups[2].Value;
                        if (image == "")
                        {
                            image = match.Groups[3].Value;
                        }
                        string oldValue = image;
                        string str7 = image.Split(new char[] { char.Parse(".") })[image.Split(new char[] { char.Parse(".") }).Length - 1].ToLower();
                        if (str4.IndexOf(str7) != -1)
                        {
                            LinkedResource resource = imagedictionary[image];
                            string contentId = resource.ContentId;
                            if (resource != null)
                            {
                                if (!dictionary.ContainsKey(contentId))
                                {
                                    dictionary.Add(contentId, resource);
                                }
                                body = body.Replace(image, "cid:" + contentId);
                            }
                            else
                            {
                                body = body.Replace(oldValue, str + image);
                            }
                        }
                        else
                        {
                            body = body.Replace(oldValue, str + image);
                        }
                    }
                }
            }
            HtmlBody body2 = new HtmlBody();
            body2.body = body;
            body2.linkedRessources = dictionary;
            return body2;


        }

        /// <summary>
        /// Creates the linked resource.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="ext">The ext.</param>
        /// <param name="contentId">The content id.</param>
        /// <returns></returns>
        private static LinkedResource CreateLinkedResource(string image, string ext, string contentId)
        {
            string path = HttpContext.Current.Server.MapPath(image);
            if (!File.Exists(path))
            {
                return null;
            }
            LinkedResource resource = new LinkedResource(path, "image/" + ext.ToLower().Replace("jpg", "jpeg"));
            resource.ContentId = contentId;
            return resource;
        }

        private static string GenerateHash(string input)
        {
            string inputForSHA1 = string.Format("{0}{1}", input, "T77dRUchAMEvApa4");

            //string inputForSHA1 = string.Format("{0}{1}", input, Constants.Hash.SHA1.Activation);

            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(inputForSHA1, "SHA1");
        }


        public static List<Member> FilterMembers(MemberGroup Group, string filter)
        {
            Member[] getAll = Member.GetAll;
            List<Member> list = new List<Member>();

            foreach (Member member in getAll)
            {
                if (!string.IsNullOrEmpty(member.Email) && ((Group == null) || member.Groups.ContainsKey(Group.Id)))
                {
                    if (filter.Length == 0)
                    {
                        list.Add(member);
                    }
                    else
                    {
                        bool flag = true;
                        foreach (PropertyFilter propfilter in FiltersFromString(filter))
                        {
                            Property property = member.getProperty(propfilter.Field);
                            if (property == null)
                            {
                                break;
                            }
                            string str = property.Value.ToString();
                            if (Array.IndexOf<string>(propfilter.Values, str) == -1)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            list.Add(member);
                        }
                    }
                }
            }

            return list;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HtmlBody
        {
            public Dictionary<string, LinkedResource> linkedRessources;
            public string body;
        }

        public static List<PropertyFilter> FiltersFromString(string filterstring)
        {
            List<PropertyFilter> list = new List<PropertyFilter>();
            if (!string.IsNullOrEmpty(filterstring))
            {
                string[] strArray = filterstring.Split("&".ToCharArray());
                foreach (string str in strArray)
                {
                    PropertyFilter item = new PropertyFilter();
                    string[] strArray2 = str.Split("@".ToCharArray());
                    item.Field = strArray2[0];
                    item.Values = strArray2[1].Split(",".ToCharArray());
                    list.Add(item);
                }
            }
            return list;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PropertyFilter
        {
            public string Field;
            public string[] Values;
        }

    }
}
