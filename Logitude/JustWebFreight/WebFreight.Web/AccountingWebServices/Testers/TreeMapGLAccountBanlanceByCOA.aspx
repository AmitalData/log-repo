<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreeMapGLAccountBanlanceByCOA.aspx.cs" Inherits="WebFreight.Web.AccountingWebServices.Testers.TreeMapGLAccountBanlanceByCOA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        table, th, td {
   border: 1px solid black;
}
        .dropdown {
    position: relative;
    display: inline-block;
}

.dropdown-content {
    display: none;
    position: absolute;
    background-color: #f9f9f9;
    min-width: 160px;
    box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
    padding: 12px 16px;
    z-index: 1;
}

.dropdown:hover .dropdown-content {
    display: block;
}
    </style>
    <script src="<% =  Page.ResolveUrl("~/HtmlHelpers/JS/jquery.min.js") %>"></script>
    <script lang="javascript" >
        var _LastResponse;
        var urlBase = '';
        //http://accountingtest/accounting/api/authentication
        if (urlBase == "") {
        } else {
            //urlBase = '/accounting';
            urlBase = "";
        }
        var l = '/AccountingWebServices/Testers/TreeMapGLAccountBanlanceByCOA.aspx'.toLowerCase();
        var logitude_url = location.href.toLowerCase().replace(l, '');
        var basebase='<% =  Page.ResolveUrl("~/AccountingWebServices/Testers/TreeMapGLAccountBanlanceByCOA.aspx") %>';
        

        //alert(logitude_url);
        urlBase = logitude_url;

        var authUrl = urlBase + '/api/authentication?&tenant=';
        


        function DrawTable(divId, data) {
            
            var row = $("<tr />")
            $(divId).append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it

            if (data == null) return;
            if (data.length < 1) return;
            row.append($("<th> action1 </th>"));
            row.append($("<th> action2 </th>"));
            //row.append($("<th> open Balance</th>"));
            var arryCol = data[0];
            for (x in arryCol) {

                row.append($("<th> " + x + " </th>"));
            }
            
            for (var i = 0; i < data.length; i++) {
                var myChartOfAccountBalanceM = data[i];
                
                var row = $("<tr />")
                $(divId).append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it
                //row.append($("<th> <button click='alert(" + myChartOfAccountBalanceM.JournalId + ")' value='test' /> </th>"));
                //var arry=[ "JournalId","JournalLineNumber"];
                row.append($("<td> " + "<button onclick=\"ByType(this,); return false\" >ByType</button>" + "</td>"));
                row.append($("<td> " + "<button onclick=\"ById(this,'" +myChartOfAccountBalanceM.Parentid + "','" + myChartOfAccountBalanceM.ChildId+ "');  return false\" >ById</button>" + "</td>"));
                //row.append($("<td> <input type=number  value='" + myChartOfAccountBalanceM.OpenAmount + "' /> </td>"));
                for (x in arryCol) {
                    row.append($("<td> " + myChartOfAccountBalanceM[x] + "</td>"));
                }
            }
        }
        var _Tenant='<%=_Tenant%>';
        var _MyCollector='<%=_MyCollector%>';
        var _CallBackCOATypeCode='<%=_CallBackCOATypeCode%>';
        var _CallBackParentCOAId = '<%=_CallBackParentCOAId%>';
        var _ByBalance = '<%=_ByBalance%>';
        
        function ById(objButton, Parentid, ChildId) {
            var url=basebase ;
            url = url + "?Tenant=" + _Tenant;
            url=url+ "&MyCollector=" + _MyCollector ;
            url = url + "&ByBalance=" + _ByBalance;
            url = url + "&CallBackCOATypeCode=" + Parentid;
            if (ChildId=="null") {
                ChildId = "";
            }
            url = url + "&CallBackParentCOAId=" + ChildId;
            
            location.href = url;
        }
        function escapeJSON(str) {
            return str.replace(/\\/g, '\\');
        }
        var myJson = new String();
        ///myJson = escapeJSON('<% =_MyJson %>');
        myJson = '<% =_MyJson %>';
        //myJson = myJson.replace('\\"', '');

        

        
        $(document).ready(function () {
            var listChartOfAccountBalanceM = JSON.parse(myJson);
            DrawTable("#myTreeMap", listChartOfAccountBalanceM);///Must $(document).ready(function () {
        });




    </script>
</head>
<body>
    
    <form id="form2" runat="server">
        
        
    <div>
    
        <div id="myTreeMap"></div>
        
    </div>
    </form>
        
</body>
</html>
