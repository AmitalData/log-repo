<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrailReport.aspx.cs" Inherits="WebFreight.Web.AccountingWebServices.Testers.TrailReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

    
        <select>
        <option>Trail</option>
        <option>RevenueExpense</option>
    </select>
        
&nbsp;<br />
     <asp:TextBox ID="_TextBoxParam" runat="server" MaxLength="60000" Height="100px" Width="90%"  TextMode="multiline"   
         CssClass="classTextBoxParam" ></asp:TextBox>    </div>
    <br />
    
        
         <asp:Button ID="_TrailReport" runat="server" Text="TrailReport" OnClick="_TrailReport_Click" /><asp:Button ID="_RevenueExpenseReport" runat="server" Text="_RevenueExpenseReport" OnClick="_RevenueExpenseReport_Click" />
        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        <asp:HiddenField ID="_HiddenFieldTrail" runat="server" />
        <asp:HiddenField ID="_HiddenFieldExpense" runat="server" />
            <script>
                $('select').on('change', function () {
                    if (this.value == "Trail") {
                        $(".classTextBoxParam").val($("#_HiddenFieldTrail").val());
                    } else {
                        $(".classTextBoxParam").val($("#_HiddenFieldExpense").val());
                    }
                })
                
    </script>
        <asp:TextBox ID="_Log" runat="server" MaxLength="60000" Height="100px" Width="90%"  TextMode="multiline"   ></asp:TextBox>  
        <asp:TextBox ID="_ResultXML" runat="server" MaxLength="60000" Height="100px" Width="90%"  TextMode="multiline"   ></asp:TextBox>  
          </div>
    </form>
</body>
</html>
