using System;
using System.Collections.Generic;
using System.Web;
using umbraco.interfaces;

namespace Nibble.Umb.MailEngine
{
    public class MailingAction : IAction
    {
        private static readonly MailingAction _instance = new MailingAction();

        [Obsolete("Use the singleton instantiation instead of a constructor")]
        public MailingAction() { }

        public static MailingAction Instance
        {
            get { return _instance; }
        }

        #region IAction Members

		public char Letter
		{
			get
			{
				
				return 'x';
			}
		}

		public string JsFunctionName
		{
			get
			{
				// TODO:  Add ActionNew.JsFunctionName getter implementation
                return "openModal('dialogs/mailing.aspx?id=' + nodeID, 'Send mails', 550, 480);";
			}
		}

		public string JsSource
		{
			get
			{
				// TODO:  Add ActionNew.JsSource getter implementation
                return "";
			}
		}

		public string Alias
		{
			get
			{
				// TODO:  Add ActionNew.Alias getter implementation
				return "send as mail";
			}
		}

		public string Icon
		{
			get
			{
				// TODO:  Add ActionNew.Icon getter implementation
                return "umbraco/newsletter.gif";
			}
		}

		public bool ShowInNotifier
		{
			get
			{
				// TODO:  Add ActionNew.ShowInNotifier getter implementation
				return true;
			}
		}
		public bool CanBePermissionAssigned
		{
			get
			{
				// TODO:  Add ActionNew.ShowInNotifier getter implementation
				return true;
			}
		}
		#endregion
    }
}
