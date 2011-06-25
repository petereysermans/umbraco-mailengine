using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using umbraco.cms.businesslogic.member;

namespace Nibble.Umb.MailEngine.usercontrols
{
    public partial class passwordrecovery : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
        {
            Library.SendMailFromPageToMember("test@nibble.be",
                Member.GetMemberFromLoginName(PasswordRecovery1.UserName),
                "your password",
                1056);

            e.Cancel = true;

        }
    }
}