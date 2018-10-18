<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadpageAsHtml.aspx.cs" Inherits="WebFreight.Web.WebPages.DownloadpageAsHtml" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<script>
    var qs = '<%= Request.QueryString %>';
    
    function loadXMLDoc(filename) {
        if (window.ActiveXObject) {
            xhttp = new ActiveXObject("Msxml2.XMLHTTP");
        }
        else {
            xhttp = new XMLHttpRequest();
        }
        
        xhttp.open("GET", filename, false);
        try
        {
            xhttp.responseType = "msxml-document";
        } catch (err)
        {
            alert("err");
        } // Helping IE11


        xhttp.send("");
        //alert(xhttp);
        //alert(xhttp.responseXML);
        //return xhttp.responseXML;
        return xhttp.response;
    }

    function displayResult() {
        //window.URL
        alert("Downloadpage.aspx?" + qs);
        
        var xml = loadXMLDoc("Downloadpage.aspx?" + qs);
        alert(xml);
        var xsl = loadXMLDoc("xml2HTML.xsl");
        alert(xsl);
        // code for IE
        if (window.ActiveXObject || xhttp.responseType == "msxml-document") {
            ex = xml.transformNode(xsl);
            document.getElementById("example").innerHTML = ex;

        }
            // code for Chrome, Firefox, Opera, etc.
        else if (document.implementation && document.implementation.createDocument) {
            xsltProcessor = new XSLTProcessor();
            xsltProcessor.importStylesheet(xsl);
            resultDocument = xsltProcessor.transformToFragment(xml, document);
            alert(resultDocument);
            document.getElementById("example").appendChild(resultDocument);
        }
    }
</script>
</head>
<body onload="displayResult()">
<div id="example" />
</body>
</html>
