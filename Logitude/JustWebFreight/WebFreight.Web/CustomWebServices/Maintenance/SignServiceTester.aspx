<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignServiceTester.aspx.cs" Inherits="WebFreight.Web.CustomWebServices.Maintenance.SignServiceTester" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="ButtonSignQPersonal" runat="server" Text="SignQueueByPersonId Signalr" OnClick="ButtonSignQPersonal_Click1" ></asp:Button>
        <asp:Button ID="ButtonSignQCompany" runat="server" Text="SignQueueByCustomsAgentId Signalr" OnClick="ButtonSignQCompany_Click1" ></asp:Button>
        <asp:Label >Person/CompanyID</asp:Label> <asp:TextBox ID="TextBoxId" runat="server">123456789</asp:TextBox>
    </div>
        <div>
            <asp:Button ID="ButtonGetSignQueueList" runat="server" Text="GetSignQueueList (tenant)" OnClick="ButtonGetSignQueueList_Click" ></asp:Button>
            <asp:Button ID="ButtonSubscribeSignServer" runat="server" Text="SubscribeSignServer (tenant)" OnClick="ButtonSubscribeSignServer_Click" ></asp:Button>
            
            <asp:Label >Tenant</asp:Label> 
            <asp:TextBox ID="TextBoxTenant" runat="server">208</asp:TextBox>
        </div>

        
        <div>
            <asp:Label >Result/Log </asp:Label>
            <asp:TextBox ID="TextBoxResult" runat="server" MaxLength="60000" Height="378px" Width="613px"  TextMode="multiline" ></asp:TextBox>
        </div>


    </form>
</body>
</html>
