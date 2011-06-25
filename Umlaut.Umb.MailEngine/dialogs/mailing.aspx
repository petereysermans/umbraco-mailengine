<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mailing.aspx.cs" Inherits="Nibble.Umb.MailEngine.dialogs.mailing" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
    <link rel="Stylesheet" href="/umbraco_client/ui/default.css" type="text/css" />
    <link rel='stylesheet' href='/umbraco_client/propertypane/style.css' />

    <script language="javascript">
        function OnProgress(progressBar) {
            var extraData = progressBar.getExtraData();
            if (extraData) {
                //The following code demonstrates how to update
                //client side DHTML element based on the value
                //RunTask passed to us with e.UpdateProgress
                var div = document.getElementById("divStatus");
                div.innerHTML = extraData;
            }
        }
    </script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
    <script type="text/javascript" src="progress_script.js"></script>

    <style type="text/css">
        .propertyItemheader{width: 160px !Important;}
    </style>

</head>
<body class="umbracoDialog" style="margin: 15px 10px 0px 10px;">
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <eo:ScriptManager ID="ScriptManager2" runat="server">
    </eo:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
     
        <asp:Panel ID="pnlError" runat="server" CssClass="error" Visible="false">
            <p>The document needs to be published</p>
        </asp:Panel>
        
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="error" ValidationGroup="send"/>
        
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFrom" ErrorMessage="From Emailaddress is required" Display="None" ValidationGroup="send"></asp:RequiredFieldValidator>
        
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFrom"
            ErrorMessage="Valid From Emailaddress is required" Display="None" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="send"></asp:RegularExpressionValidator>
        
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSubject" ErrorMessage="Subject is required" Display="None" ValidationGroup="send"></asp:RequiredFieldValidator>
        
        
         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTo" ErrorMessage="To Emailaddress is required" Display="None" Enabled="false" ValidationGroup="send"></asp:RequiredFieldValidator>
         
         <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtTo"
            ErrorMessage="Valid To Emailaddress is required" Display="None" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Enabled="false" ValidationGroup="send"></asp:RegularExpressionValidator>
         
        <h2 class="propertypaneTitel"> <img src="../images/umbraco/newsletter.gif" /> <asp:Literal ID="Literal1" runat="server"></asp:Literal></h2>

        <div class="propertypane" style=''>
            <div>

               
               
                <div class="propertyItem" style=''>
                <div class="propertyItemheader">From emailaddress: </div> 
                <div class="propertyItemContent"><asp:TextBox ID="txtFrom" runat="server"  style="width:252px;"></asp:TextBox> <br /> <small>emailaddress </small></div> 
               
                </div>
                
                <div class="propertyItem" style=''>

                <div class="propertyItemheader">From display name: </div> 
                <div class="propertyItemContent"><asp:TextBox ID="txtFromName" runat="server" style="width:252px;" ></asp:TextBox> <br /> <small>the display name assiciated with the address</small></div> 
               </div>
               
                <div class="propertyItem" style=''>
                <div class="propertyItemheader">Subject: </div> 
                <div class="propertyItemContent"> <asp:TextBox ID="txtSubject" runat="server" style="width:252px;"></asp:TextBox> <br /> <small>subject of the mail</small></div> 
                
                <div class="propertyItem" style=''>
                <div class="propertyItemheader">Type: </div> 
                 <div class="propertyItemContent">
                    <asp:RadioButton ID="rbSingle" runat="server" Text="Single" AutoPostBack="true" 
                            oncheckedchanged="rbSingle_CheckedChanged" GroupName="type"/><br />
                    <asp:RadioButton ID="rbMass" runat="server" Text="Mass" AutoPostBack="true" 
                            oncheckedchanged="rbMass_CheckedChanged" GroupName="type"/>
                            
                            <br /> <small>use single for testing, mass for mailings</small>
                 </div>
                 
  
                        
                </div>
            </div>
        </div>
               <div class='propertyPaneFooter'>-</div>
       </div>
       


        <asp:Panel ID="pnlSingle" runat="server" Visible="false" >
            <h2 class="propertypaneTitel">Send single mail</h2>

            <div class="propertypane" style=''>
            <div>
                    <div class="propertyItem" style=''>
                    <div class="propertyItemheader">To emailaddress: </div> 
                    <div class="propertyItemContent"><asp:TextBox ID="txtTo" runat="server" style="width:252px;"></asp:TextBox>
                    <br /> <small>data will be merged if member with email is found</small>
                    </div>
                    
                    </div>
                    <div class="propertyItem" style=''>
                     <div class="propertyItemheader">&nbsp; </div> 
                     <div class="propertyItemContent">
                    <asp:Button ID="btnSendSingle" runat="server" Text="Send" onclick="btnSend_Click" ValidationGroup="send"/>
                    </div></div>
           </div>
           <div class='propertyPaneFooter'>-</div>
           </div>
       </asp:Panel>

      <asp:Panel ID="pnlMass" runat="server" Visible="false"  >
         <h2 class="propertypaneTitel">Send mass mail</h2>

         <div class="propertypane" style=''>
         
         <div>
              
                   
                     <div class="propertyItem" style=''>
                    <div class="propertyItemheader">MemberGroup:  </div> 
                     <div class="propertyItemContent">
                    <asp:DropDownList ID="ddmemberGroup" runat="server" 
                        onselectedindexchanged="ddmemberGroup_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    </div>
                    
                    <div class="propertyItem" style=''>
                    <div class="propertyItemheader">Filter:  <br /><small>Alias1@value1,val2&Alias2@val3</small></div> 
                     <div class="propertyItemContent">
                         <asp:TextBox ID="txtFilter" runat="server" TextMode="MultiLine"></asp:TextBox>
                         <br />
                         <small>
                             <asp:LinkButton ID="btnRecalc" runat="server" 
                             onclick="btnRecalc_Click">Recalculate</asp:LinkButton></small>
                    </div>
                    </div>
                    
                                      
                     <div class="propertyItem" style=''>
                     <div class="propertyItemheader">&nbsp; </div> 
                     <div class="propertyItemContent">
                    <asp:Button ID="btnSendMass" runat="server" Text="Send" 
                        ValidationGroup="send" OnClick="btnSendMass_Click" />  <small><asp:Literal ID="lblMemberCount" runat="server"></asp:Literal></small>
                        </div></div>
                        
                        <div style="clear:both;">
            
             
            
              
             </div>
                        
        </div>
        <div class='propertyPaneFooter'>-</div>
        </div>
        
      </asp:Panel>


     <asp:Panel ID="pnlStatus" runat="server" Visible="false"  >
         <h2 class="propertypaneTitel">Status</h2>
         
         <div class="propertypane" style=''>
                   <div class="propertyItem" style=''>
                    <div class="propertyItemheader">Progress:  </div> 
                     <div class="propertyItemContent" style="visibility:hidden;">
                             <eo:ProgressBar runat="server" id="ProgressBar1" ShowPercentage="True" 
                                 BorderColor="Gray" BorderWidth="1px" Height="16px"
		                         IndicatorColor="Gainsboro" IndicatorIncrement="1"
	                             Width="250px" 
	                            ClientSideOnValueChanged="OnProgress"></eo:ProgressBar>
                    </div>
                    </div>
                    
                    <div class="propertyItem" style=''>
                    <div class="propertyItemheader">Info:  </div> 
                     <div class="propertyItemContent">
                              <div id="divStatus">
                                Waiting...
                            </div>
                    </div>
                    </div>
                    
         <div>
         
         </div>
          <div class='propertyPaneFooter'>-</div>
         </div>
    </asp:Panel>
      
       <!-- <a href="#" target="_blank">open documentation</a> -->
        
     </ContentTemplate>
    </asp:UpdatePanel>
    
    </form>
</body>
</html>
