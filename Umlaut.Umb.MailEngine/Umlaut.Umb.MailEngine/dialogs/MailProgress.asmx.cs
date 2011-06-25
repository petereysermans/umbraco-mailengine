using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace Nibble.Umb.MailEngine.dialogs
{
    /// <summary>
    /// Summary description for MailProgress
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MailProgress : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object[] HelloWorld()
        {
            AsyncProgress p = (AsyncProgress) HttpContext.Current.Application[typeof(mailing).ToString() + ".sendMass"];
            if (p == null) return new object[] { 0, "Waiting..." };
            return new object[] {p.progress(),p.message()};
        }
    }
}
