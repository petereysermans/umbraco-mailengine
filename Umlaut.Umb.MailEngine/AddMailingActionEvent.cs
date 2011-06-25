using System;
using System.Collections.Generic;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.cms.presentation.Trees;
using umbraco.interfaces;
namespace Nibble.Umb.MailEngine
{
    public class AddMailingActionEvent : ApplicationBase
    {
        public AddMailingActionEvent()
        {
            BaseContentTree.BeforeNodeRender += new BaseTree.BeforeNodeRenderEventHandler(BaseContentTree_BeforeNodeRender);
        }

        void BaseContentTree_BeforeNodeRender(ref XmlTree sender, ref XmlTreeNode node, EventArgs e)
        {

            if (node.NodeType == "content")
            {
                try
                {
                    int index = node.Menu.FindIndex(delegate(IAction a) { return a.Alias == "publish"; }) + 1;
                    node.Menu.Insert(index, MailingAction.Instance);
                }
                catch { }
            }
        }
    }
}
