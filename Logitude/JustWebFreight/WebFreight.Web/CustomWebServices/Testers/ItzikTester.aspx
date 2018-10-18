<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItzikTester.aspx.cs" Inherits="WebFreight.Web.CustomWebServices.Testers.ItzikTester" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="../Maintenance/SBQueueForm.aspx" >SBQueueForm</a>
        <a href="../Maintenance/SignServiceTester.aspx" >SignServiceTester</a>
        <%
            try
            {
                TestCancellCRS("9739afb1-4106-406c-ae81-461970ec71bf", new DateTime(2018,08,01,15,55,30));
            }
            catch (Exception  ee)
            {

                Response.Write(ee.ToString());
            }
             %>
        
        <h1 >Oracle time <% = this.OracleTime.GetValueOrDefault() %> </h1>

    </form>
</body>
</html>
