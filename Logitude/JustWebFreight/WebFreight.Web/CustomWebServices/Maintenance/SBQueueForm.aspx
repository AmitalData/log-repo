<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SBQueueForm.aspx.cs" Inherits="WebFreight.Web.CustomWebServices.Maintenance.SBQueueForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
        Logitude.Customs.BL.Messaging.Customs.SBQMessageService.CreateBasic<Logitude.Customs.BL.Messaging.Customs.CustomsCommandEnum>(
                           Logitude.Customs.BL.Messaging.Customs.CustomsCommandEnum.CustomsCommandAnalyzeResponseWR,
                           208,
                           "2750",
                           "86c9679c-9b80-4ee7-8539-112414decd64");
         %>
    </div>
    </form>
</body>
</html>
