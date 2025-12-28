<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Designer.aspx.cs" Inherits="WebFreight.Web.Stimulsoft.Designer" %>

<%@ Register Assembly="Stimulsoft.Report.WebDesign, Version=2026.1.1" Namespace="Stimulsoft.Report.Web" TagPrefix="cc3" %>

<%--<%@ Register Assembly="Stimulsoft.Report.MobileDesign, Version=2017.1.11.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.MobileDesign" TagPrefix="cc2" %>--%>

<%--<%@ Register Assembly="Stimulsoft.Report.MobileDesign" Namespace="Stimulsoft.Report.MobileDesign"
    TagPrefix="cc1" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style  type="text/css">
        .RedButton {
    background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(237, 192, 147, 1) 100%);
    background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);
    background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(237, 192, 147, 1) ));
    background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);
    background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);
}

.Button {
    background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(186, 206, 227, 1) 100%);
    background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
    background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(186, 206, 227, 1) ));
    background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
    background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
}
    </style>

    <script type="text/javascript" >
        var parent = window.parent;
        function onBodyLoad() {
             
            console.log(parent.document.getElementById("stimuldesignerframeId"));
            //parent.document.getElementById("stimuldesignerframeId").src = "";
           
           // window.designerClosed = false;
            //alert("loaded!!!");
            var designer = document.getElementById('StiMobileDesigner1');
            
            designer.attributes["width"] = window.innerWidth;
            designer.attributes["height"] = window.innerHeight - 30;

           // var m = document.getElementById('stimuldesignerframe');
            // console.log(m);
            //console.log(window.name);
            //window.parent.stimuldesignerFinished("1");
            //console.log(window.parent.frames["stimuldesignerframeId"]);

            //var frame = document.getElementById('stimuldesignerframeId'); 

           
            
            //console.log(window);
            //window.stimuldesignerFinished('true');
            //alert(window.name);

            //var f = window.frames['stimuldesignerframe'].document.getElementById('stimuldesignerframe');
           // console.log(window.frames);
            //alert(window.frames);
            //window.parent.postMessage('true', '*');
        }

     
        function close() {
            console.log("olaaaaaaaaaaaaaaaaa");
            alert("Called!");
           // window.parent.postMessage("true", '*');

        }

        

    </script>
</head>

<body>

    <form id="form1" runat="server">
            <table  style="width:100%;height:100%">
        
        <tr style="height:80%;">
            <td>
              <%-- <cc2:StiMobileDesigner ID="StiMobileDesigner1" runat="server"
                    onsavereport="StiMobileDesigner1_SaveReport"   Visible="true"
            />--%>
                <cc3:StiWebDesigner ID="LogitudeStiWebDesigner"  runat="server" OnSaveReport="LogitudeStiWebDesigner_SaveReport"></cc3:StiWebDesigner>


            </td>
         <%--   <td>
                <asp:Label runat="server" Visible="false" ID="MyLabel">abc</asp:Label>
            </td>--%>
        </tr>
   
        <tr>
            <td>
             <table style="height: 22px;">
                    <tr>
                        <td style="width:90%">
                            <div></div>
                        </td>


                     <%--   <td style="width: 57px;">
                            <button class="Button" >Cancel</button>
                        </td>

                        <td style="width: 5px;">
                            <div></div>
                        </td>


                        <td style="width: 57px;">
                            <button class="RedButton" >Save</button>
                        </td>--%>
                    </tr>
                </table>
            </td>
        </tr>
               </table>
    </form>
</body>
</html>


<%-- oncreatereport="StiMobileDesigner1_CreateReport" 
            onsavereport="StiMobileDesigner1_SaveReport"
            onloadreport="StiMobileDesigner1_GetDataSetOnLoad"--%>