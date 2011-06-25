using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace Nibble.Umb.MailEngine.dialogs
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class MailProgress : WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] HelloWorld()
        {
            AsyncProgress p = (AsyncProgress) HttpContext.Current.Application[typeof(mailing).ToString() + ".sendMass"];
            if (p == null) return new string[] { 0.ToString(), "Waiting..." };
            return new string[] {p.progress().ToString(),p.message()};
        }
    }
}
