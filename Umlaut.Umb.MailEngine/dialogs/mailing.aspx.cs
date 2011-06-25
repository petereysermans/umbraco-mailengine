using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.web;
using umbraco.cms.businesslogic.member;
using System.Reflection;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Threading;
using System.Net.Mail;

namespace Nibble.Umb.MailEngine.dialogs
{
    public partial class mailing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = int.Parse(Request["id"]);
                Document doc = new Document(id);

                if (!doc.Published)
                {
                    rbMass.Enabled = false;
                    rbSingle.Enabled = false;
                    txtFrom.Enabled = false;
                    txtSubject.Enabled = false;
                    txtFromName.Enabled = false;

                    pnlError.Visible = true;
                }
                txtSubject.Text = doc.Text;

                Literal1.Text = string.Format("Mail document '{0}'",doc.Text);

                //mass
                this.ddmemberGroup.Items.Add(new ListItem("No specific", "0"));
                foreach (MemberGroup group in MemberGroup.GetAll)
                {
                    this.ddmemberGroup.Items.Add(new ListItem(group.Text, group.Id.ToString()));
                }
                if (this.ddmemberGroup.Items.Count > 1) this.ddmemberGroup.SelectedIndex = 1;
               
                updateCount();

                HttpContext.Current.Application.Remove(typeof(mailing).ToString() + ".sendMass");

            }
            //Assembly assExecuting = Assembly.GetExecutingAssembly();
            //Stream resourceStream = assExecuting.GetManifestResourceStream(typeof(mailing), "progress_script.js");
            //String script;
            //using (StreamReader reader = new StreamReader(resourceStream, Encoding.ASCII))
            //    script = reader.ReadToEnd();

            //Page.ClientScript.RegisterStartupScript(typeof(mailing), "progressasync", script);

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            //send single

            Library.SendMailFromPage(txtFrom.Text, txtTo.Text, txtSubject.Text, int.Parse(Request["id"]));
        }

        protected void rbSingle_CheckedChanged(object sender, EventArgs e)
        {
            pnlSingle.Visible = true;
            pnlMass.Visible = false;
            pnlStatus.Visible = false;
            RequiredFieldValidator3.Enabled = true;
            RegularExpressionValidator2.Enabled = true;
            
        }

        protected void rbMass_CheckedChanged(object sender, EventArgs e)
        {
            pnlMass.Visible = true;
            pnlStatus.Visible = true;
            pnlSingle.Visible = false;
            RequiredFieldValidator3.Enabled = false;
            RegularExpressionValidator2.Enabled = false;
        }

        protected void btnSendMass_Click(object sender, EventArgs e)
        {
            //needs to be async 

            //send mass

            int MemberGroupID = Convert.ToInt32(ddmemberGroup.SelectedValue);
            MemberGroup group = null;
            if (MemberGroupID  != 0)
            {
                group = new MemberGroup(MemberGroupID );
            }
            int PageId = int.Parse(Request["id"]);
            string body = umbraco.library.RenderTemplate(PageId);

            HttpContext.Current.Application.Add(typeof(mailing).ToString() + ".sendMass", new MailTask(Helper.FilterMembers(group, txtFilter.Text), txtFrom.Text, txtSubject.Text, PageId, body));

        }
        class MailTask : AsyncProgress
        {
            Thread background;
            List<Member> members;
            int _progress;
            string _message;
            string fromText;
            string subjectText;
            int pageid;
            string body;
            Dictionary<string, LinkedResource> imagedictionary;

            public MailTask(List<Member> members, string fromText, string subjectText, int pageid, string body)
            {
                this.members = members;
                this.fromText = fromText;
                this.subjectText = subjectText;
                this.pageid = pageid;
                this.body = body;
                this.imagedictionary = Helper.AssembleResources(body);
                Thread background = new Thread(backgroundProcess);
                background.Start();
            }
            public void backgroundProcess()
            {
                lock (this)
                {
                    _progress = 0;
                    _message = "Starting...";
                }
                
                foreach (Member receiver in members)
                {
                    try
                    {
                        Library.SendMailFromPageToMember(fromText, receiver, subjectText, pageid, body, imagedictionary);
                        lock (this)
                        {
                            _progress += 1;
                            _message = "Sent " + _progress.ToString() + " emails.";
                        }
                    }
                    catch(Exception ex)
                    {
                        _message = "Exception " + ex.Message;
                    }
                }
                lock (this) _message = "Sending done!";

            }
            #region AsyncProgress Members

            int AsyncProgress.progress()
            {
                int r = 0;
                lock (this) r = _progress;
                return r;
            }


            string AsyncProgress.message()
            {
                string s;
                lock (this) s = _message;
                return s;
            }

            #endregion
        }


        private void updateCount()
        {
            btnSendMass.Enabled = true;

            int MemberGroupID = Convert.ToInt32(ddmemberGroup.SelectedValue);
            MemberGroup group = null;
            if (MemberGroupID != 0)
            {
                group = new MemberGroup(MemberGroupID);
            }
            int count = Helper.FilterMembers(group,txtFilter.Text).Count;

            ProgressBar1.Maximum = count;
            lblMemberCount.Text = string.Format("this configuration will send to {0} members", count);

            if (count == 0)
                btnSendMass.Enabled = false;
        }

        protected void ddmemberGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateCount();
        }

        protected void btnRecalc_Click(object sender, EventArgs e)
        {
            updateCount();
        }

       
        protected void ProgressBar1_RunTask(object sender, EO.Web.ProgressTaskEventArgs e)
        {
            
            //e.UpdateProgress(0, "Sending started");

            //int MemberGroupID = Convert.ToInt32(ddmemberGroup.SelectedValue);
            //MemberGroup group = null;
            //if (MemberGroupID != 0)
            //{
            //    group = new MemberGroup(MemberGroupID);
            //}
            //int i = 1;
            //foreach (Member receiver in Helper.FilterMembers(group, txtFilter.Text))
            //{
            //    Library.SendMailFromPageToMember(txtFrom.Text, receiver, txtSubject.Text, int.Parse(Request["id"]));

            //    e.UpdateProgress(i, "Sending to " + receiver.Email);
            //    System.Threading.Thread.Sleep(100);
            //    i++;
            //}
            //e.UpdateProgress(ProgressBar1.Maximum, "Sending done!");
        }
    }
}
