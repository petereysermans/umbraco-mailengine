using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using umbraco.cms.businesslogic.member;
namespace Nibble.Umb.MailEngine.usercontrols
{
    public partial class testing : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Member change = Member.GetMemberFromEmail(Request[Key]);

                if (change != null)
                {
                   
                    change.getProperty(PropertyAlias).Value = 1;
                    change.Save();
                    change.XmlGenerate(new System.Xml.XmlDocument());
                }
            }
        }

        private string _Key;

        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

        private string _PropertyAlias;

        public string PropertyAlias
        {
            get
            {
                return _PropertyAlias;
            }
            set
            {
                _PropertyAlias = value;
            }
        }
    }
}