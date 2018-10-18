<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FBInfoPage.aspx.cs"  Inherits="WebFreight.Web.WebPages.FBInfoPage" %>

<%@ Register Assembly="Facebook.Web" Namespace="Facebook.Web.FbmlControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<asp:Label runat="server" text="Label"  ></asp:Label>
    <form id="form1" runat="server">

    <p>
       

</p>



    
    <asp:Label ID="Label1" runat="server" Text="Label" onload="Label1_Load" ></asp:Label>

    <cc1:Time ID="Time1" runat="server">
    </cc1:Time>
   
    <fb:iframe src="http://192.168.1.111/WebFreight.Web/Default.aspx"  style="width: 758px; height: 679px;" />

    
</form>


</html>

