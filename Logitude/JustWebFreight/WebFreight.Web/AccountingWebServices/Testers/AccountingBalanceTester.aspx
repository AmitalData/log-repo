<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountingBalanceTester.aspx.cs" Inherits="WebFreight.Web.AccountingWebServices.Testers.AccountingBalanceTester" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Acc Balance</title>
    <style>
        table, th, td {
   border: 1px solid black;
}
    </style>
    <script src="<% =  Page.ResolveUrl("~/js/jquery-3.5.1.min.js") %>"></script>

    <script lang="javascript" >

        


        var  arryColumns=[ "JournalId","JournalLineNumber"];
        function DrawTableReconciliation(data) {
            $("#jsonDataTable").html("");
            var row = $("<tr />")
            $("#jsonDataTable").append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it
            //row.append($("<th> action </th>"));
            //row.append($("<th> JournalId </th>"));
            //row.append($("<th> JournalLineNumber </th>"));
            //row.append($("<th> LocalAmountDebit </th>"));
            //row.append($("<th> rowData.CumulativeLocalAmount </th>"));
            if (data==null) return;
            if (data.length < 1) return;
            for (x in data[0]) {
                
                row.append($("<th> " + x + " </th>"));
            }
            for (var i = 0; i < data.length; i++) {
                var rowData = data[i];
                var row = $("<tr />")
                $("#jsonDataTable").append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it
                //row.append($("<th> <button click='alert(" + rowData.JournalId + ")' value='test' /> </th>"));
                //var arry=[ "JournalId","JournalLineNumber"];
                
                for (x in rowData) {
                    row.append($("<td> " + rowData[x] + "</td>"));
                }
            }

            
            
        }



        var arrayGLAccountColumns = ["GLAccountId", "DisplayNumber"];
        function DrawTableRevaluation(data) {
            $("#jsonDataTable").html("");
            var row = $("<tr />")
            $("#jsonDataTable").append(row); //this will append an element to the table... keep its reference for a while since we will add cells to it
            //row.append($("<th> action </th>"));
            //row.append($("<th> GLAccountId </th>"));
            //row.append($("<th> DisplayNumber </th>"));
            //           //row.append($("<th> LocalAmountDebit </th>"));
            //           //row.append($("<th> rowData.CumulativeLocalAmount </th>"));
            if (data == null) return;
            if (data.length < 1) return;
            for (x in data[0]) {

                row.append($("<th> " + x + " </th>"));
            }
            for (var i = 0; i < data.length; i++) {
                var rowData = data[i];
                var row = $("<tr />")
                $("#jsonDataTable").append(row); //this will append an element to the table... keep its reference for a while since we will add cells to it
                //row.append($("<th> <button click='alert(" + rowData.GLAccountId + ")' value='test' /> </th>"));
                //var arry=[ "GLAccountId","DisplayNumber"];

                for (x in rowData) {
                    row.append($("<td> " + rowData[x] + "</td>"));
                }
            }

        }


        var arrayARPaymentChequeColumns = ["Id", "ChequeNumber"];
        function DrawTableARPaymentCheque(data) {
            $("#jsonDataTable").html("");
            var row = $("<tr />")
            $("#jsonDataTable").append(row); //this will append an element to the table... keep its reference for a while since we will add cells to it
            //row.append($("<th> action </th>"));
            //row.append($("<th> Id </th>"));
            //row.append($("<th> ChequeNumber </th>"));
            if (data == null) return;
            if (data.length < 1) return;
            for (x in data[0]) {

                row.append($("<th> " + x + " </th>"));
            }
            for (var i = 0; i < data.length; i++) {
                var rowData = data[i];
                var row = $("<tr />")
                $("#jsonDataTable").append(row); //this will append an element to the table... keep its reference for a while since we will add cells to it
                //row.append($("<th> <button click='alert(" + rowData.Id + ")' value='test' /> </th>"));
                //var arry=[ "Id","ChequeNumber"];

                for (x in rowData) {
                    row.append($("<td> " + rowData[x] + "</td>"));
                }
            }

        }



            function LedgerTransactionCardIndexFilter(){
                var today = new Date();
                var lastMonth = new Date();
                lastMonth.setDate(today.getDate()-30);

                this.IsLedgerTransactionCardIndexFilter=true;
                this.Tenant =1;

                this.GLAccountId ="1-30";
                this.CurrencyId ="1-7";
                this.Category1Id ="1-12";
                this.Category2Id ="";
                this.Category3Id ="";
                this.Category4Id ="";
                this.Category5Id = "";
                this.AccountTypeCode = "2";
                this.From =  lastMonth;
                this.To = today;
                this.IncludeChildAccounts = true;
                this.DateTypeCode = "1";
                this.PageSize =100;
                this.CurrZeroPage =0;

                this.IsReconciled = false;
                this.CallBack = null;

            }
        

            function OnClickButtonLedgerTransactionCardIndex() {
                var defaultParam = new LedgerTransactionCardIndexFilter();

                if (!_ResponseToken) {
                    getToken();
                }
                try {
                    var myJson = $(".classTextBoxParam").val();
                    var objToCheck;

                    if (myJson) { objToCheck = JSON.parse(myJson); }
                    if (objToCheck && objToCheck.IsLedgerTransactionCardIndexFilter) {
                    } else {
                        var str = JSON.stringify(defaultParam);
                        $(".classTextBoxParam").val(str);
                        return false;
                    }
                } catch (e) {

                    var str = JSON.stringify(defaultParam);
                    $(".classTextBoxParam").val(str);
                    return false;
                }
            
            
                if (myJson) {
                
                
                    $(".class_LabelLog").val("OnClickButtonLedgerTransactionCardIndex ..." + _ResponseToken);
                    $.ajax({
                        ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                        url: _ReconciliationUrl,// url + '/api/Journals', //this is the path to web api controller method
                        type: 'POST',
                        dataType: 'json',
                        headers: { 'Token': _ResponseToken },
                        contentType: 'application/json; charset=UTF-8', // This is the money shot
                        data: myJson,
                        success: function (response, textStatus, xhr) {
                            //handle success 
                            //alert("success ");;
                            $(".class_LabelLog").val(JSON.stringify(response));
                            DrawTableReconciliation(response.MyLedgerTransactionList,arryColumns);
                            response.MyLedgerTransactionList = null;
                            var obj = JSON.parse(myJson);
                            obj.CallBack = response;
                            var str = JSON.stringify(obj);
                            $(".classTextBoxParam").val(str);
                            //response.Token = response.Token;
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            if (textStatus != 'abort') {
                                //handle error

                                alert("error" + textStatus + errorThrown);

                                $(".class_LabelLog").val(xhr.responseText);
                            }
                        }
                    });
                }
            



                return false;
            }



            function DrawTableCardIndex(data) {
                $("#jsonDataTable").html("");
                var row = $("<tr />")
                $("#jsonDataTable").append(row); //this will append an element to the table... keep its reference for a while since we will add cells to it
                //row.append($("<th> action </th>"));
                //row.append($("<th> GLAccountId </th>"));
                //row.append($("<th> DisplayNumber </th>"));
                //           //row.append($("<th> LocalAmountDebit </th>"));
                //           //row.append($("<th> rowData.CumulativeLocalAmount </th>"));
                if (data == null) return;
                if (data.length < 1) return;
                for (x in data[0]) {

                    row.append($("<th> " + x + " </th>"));
                }
                for (var i = 0; i < data.length; i++) {
                    var rowData = data[i];
                    var row = $("<tr />")
                    $("#jsonDataTable").append(row); //this will append an element to the table... keep its reference for a while since we will add cells to it
                    //row.append($("<th> <button click='alert(" + rowData.GLAccountId + ")' value='test' /> </th>"));
                    //var arry=[ "GLAccountId","DisplayNumber"];

                    for (x in rowData) {
                        row.append($("<td> " + rowData[x] + "</td>"));
                    }
                }
        }


        var _DefaultJornalPM = '<% =GetDefaultJornalPM()%>';
        var _DefaultGLaccountPM = '{"AutomaticReconcile": null,"Category1": null,"Category2": null,"Category3": null,"Category4": null,"Category5": null,"ChartOfAccount": null,"ChartOfAccountsType": null,"ControlAccount": null,"Currency": null,"GLAccountType": 1,"PreviousChartOfAccount": null,"ReconcileMethod": null,"RevenueExpense": null,"Id": "","Tenant": 989,"InternalNumber": "1000","AccountTypeCode": "1","DisplayNumber": "Customers","LocalName": "יהי טוב","EnglishName": "Customer xx","SearchFields": "לקוחות","IsMultiCurrency": true,"CurrencyId": null,"RevenueExpenseType": "1","IsControlAccount": false,"ChartOfAccountsId": "1-105","Inactive": false,"ChartOfAccountsTypeCode": "3","ReconcileMethodCode": "0","ControlAccountId": null,"AutomaticReconcileId": null,"PreviousEnglishName": null,"PreviousEnglishNameChangeDate": "2016-11-23T07:00:35.407","PreviousLocalName": null,"PreviousLocalNameChangeDate": "2016-11-23T07:00:35.407","PreviousNumber": null,"PreviousNumberChangeDate": "2016-11-23T07:00:35.407","PreviousChartOfAccountsId": null,"PreviousChartOfAccountsChangeDate": "2016-11-23T07:00:35.407","CustomerGLAccountId": null,"BalanceInLocalCurrency": null,"RevaluationEnabled": null,"ParentAccountId": null,"Category1Id": null,"Category2Id": null,"Category3Id": null,"Category4Id": null,"Category5Id": null,"IsVATExempt": null}';
        var _DefaultChartOfAccountsPM = '{"Id": null,  "Tenant": 62,  "Code": "2521",  "EncodeBase64NVARCHARFieldsBy": "windows-1255",  "LocalName": "5OX24OX6IOvs7Onl+g==",  "EnglishName": null,  "ParentId": null,  "TypeCode": "2",  "Inactive": null,  "TypeName": null,  "ParentName": null,  "SearchFields": null}';
        

        var urlBase = '<% =GetHost() %>';
        //http://accountingtest/accounting/api/authentication
        if (urlBase == "") {
        } else {
            //urlBase = '/accounting';
            urlBase = "";
        }
        var l = '/AccountingWebServices/Testers/AccountingBalanceTester.aspx'.toLowerCase();
        var logitude_url = location.href.toLowerCase().replace(l, '');


        //alert(logitude_url);
        urlBase = logitude_url;
        
        
        var authUrl = urlBase + '/api/authentication?&tenant='; //api/authentication?&tenant=1';
        var journalUrl = urlBase + '/api/Journals';
        var _GLAccountsUrl = urlBase + '/api/GLAccounts';
        var _ChartOfAccountsUrl = urlBase + '/api/ChartOfAccounts';
        var JournalOpUrl = urlBase + '/api/JournalOp';
        
        var _ReconciliationUrl = urlBase + '/api/ReconciliationOp';
        var _ReconciliationAfterConversionUrl = urlBase + '/api/ReconciliationAfterConversion';
        var _ReconciliationStageBUrl = urlBase + '/api/ReconciliationStageB';
        var _ReconciliationStageCUrl = urlBase + '/api/ReconciliationStageC';
        var _CardGLAccountConnectUrl = urlBase + '/api/CardGLAccountConnect';
        var _RevaluationUrl = urlBase + '/api/RevaluationOp';

        var _ARPaymentChequeUrl = urlBase + '/api/ARPaymentChequeOp';


        //var loginParameterJson = '{ "Email": "admin@fnarsoft.com", "Password": "!J123456.0", "IsUser": true, "CardId": null, "CardType": null, "ByToken": false, "IsMobileLogin": false, "GetToken": true }';
        var _ResponseToken = "";
        function getToken() {
            //var loginParameterJson = JSON.stringify(loginParameter)
            var loginParameterJson = $(".class_loginParameter").val();//JSON.stringify(loginParameter)
            var loginParameter = JSON.parse(loginParameterJson);
            var tenant = loginParameter.Tenant;
            var authUrlWithtenant = authUrl + tenant;
            
            //alert(loginParameterJson);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: authUrlWithtenant, //url + '/api/authentication?&tenant=1', //this is the path to web api controller method
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: loginParameterJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    // alert("success " + JSON.stringify(response));
                    
                    
                    _ResponseToken = response.Token;
                    $("#_myToken").val(_ResponseToken);
                    //$(".class_LabelLog").val(JSON.stringify(_ResponseToken));
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert("error" + textStatus + errorThrown);
                    $(".class_LabelLog").val(JSON.stringify(xhr));
                    if (textStatus != 'abort') {
                        //handle error
                        //    alert("error" + textStatus + errorThrown);
                    }
                }
            });
        }
        //if (!_ResponseToken ) {
        //    _ResponseToken = $("#_myToken").val();
        //    if (!_ResponseToken ) {
        //        getToken();
        //    }
        //}






        function LedgerTransactionBalanceFilter(){
            var today = new Date();
            var lastMonth = new Date();
            lastMonth.setDate(today.getDate()-30);

            this.IsLedgerTransactionBalanceFilter=true;
            this.Tenant =1;

            this.GLAccountId ="1-30";
            this.CurrencyId ="1-7";
            this.From =  lastMonth;
            this.To = today;


            this.PageSize =100;
            this.CurrZeroPage =0;

            this.IncludeRelatedCurrenciesAccount = false;
            this.CallBack = null;

        }
        

        function OnClickButtonLedgerTransactionBalance() {
            var defaultParam = new LedgerTransactionBalanceFilter();

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
                if (objToCheck.IsLedgerTransactionBalanceFilter) {
                } else {
                    var str = JSON.stringify(defaultParam);
                    $(".classTextBoxParam").val(str);
                    return false;
                }
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            
            
            if (myJson) {
                
                
                $(".class_LabelLog").val("OnClickButtonLedgerTransactionBalance ..." + _ResponseToken);
                $.ajax({
                    ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                    url: _ReconciliationUrl,// url + '/api/Journals', //this is the path to web api controller method
                    type: 'POST',
                    dataType: 'json',
                    headers: { 'Token': _ResponseToken },
                    contentType: 'application/json; charset=UTF-8', // This is the money shot
                    data: myJson,
                    success: function (response, textStatus, xhr) {
                        //handle success 
                        //alert("success ");;
                        $(".class_LabelLog").val(JSON.stringify(response));
                        DrawTableReconciliation(response.MyLedgerTransactionList,arryColumns);
                        response.MyLedgerTransactionList = null;
                        var obj = JSON.parse(myJson);
                        obj.CallBack = response;
                        var str = JSON.stringify(obj);
                        $(".classTextBoxParam").val(str);
                        //response.Token = response.Token;
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        if (textStatus != 'abort') {
                            //handle error

                            alert("error" + textStatus + errorThrown);

                            $(".class_LabelLog").val(xhr.responseText);
                        }
                    }
                });
            }
            



            return false;
        }


        function OnClickButtonAutomaticReconcile() {

            var defaultParam = new Object();
            defaultParam.IsAutomaticReconcile = true;
            defaultParam.AccountId = "1-30";
            defaultParam.Tenant = 1;

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
                if (objToCheck.IsAutomaticReconcile) {
                } else {
                    var str = JSON.stringify(defaultParam);
                    $(".classTextBoxParam").val(str);
                    return false;
                }
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl = _ReconciliationUrl + "?gLAccountId=" + objToCheck1.AccountId + "&tenant=" + objToCheck1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonLedgerTransactionBalance ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response,arryColumns);
                    
                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });





            return false;
        }
  




        function OnClickButtonReconcileAfterConversion() {

            var defaultParam = new Object();
            defaultParam.Tenant = 1;
            defaultParam.FromExtNum = "1";
            defaultParam.ToExtNum = "99";

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl;
            if (objToCheck1.FromExtNum == "" && objToCheck1.ToExtNum == "") {
                myUrl = _ReconciliationAfterConversionUrl + "?tenant=" + objToCheck1.Tenant;
            }
            else {
                myUrl = _ReconciliationAfterConversionUrl + "?tenant=" + objToCheck1.Tenant + "&fromExtNum=" + objToCheck1.FromExtNum + "&toExtNum=" + objToCheck1.ToExtNum;
            }

            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonReconcileAfterConversion ..." + _ResponseToken);
            $.ajax({
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response, arryColumns);

                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });


            return false;
        }



        function OnClickButtonReconcileAfterConversionNoBatch() {

            var defaultParam = new Object();
            defaultParam.Tenant = 1;
            defaultParam.FromExtNum = "1";
            defaultParam.ToExtNum = "99";

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl;
            if (objToCheck1.FromExtNum == "" && objToCheck1.ToExtNum == "") {
                myUrl = _ReconciliationAfterConversionUrl + "?tenant=" + objToCheck1.Tenant + "&noBatch=1";
            }
            else {
                myUrl = _ReconciliationAfterConversionUrl + "?tenant=" + objToCheck1.Tenant + "&fromExtNum=" + objToCheck1.FromExtNum + "&toExtNum=" + objToCheck1.ToExtNum + "&noBatch=1";
            }

            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonReconcileAfterConversionNoBatch ..." + _ResponseToken);
            $.ajax({
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response, arryColumns);

                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });


            return false;
        }


        function OnClickButtonReconcileStageB() {

            var defaultParam = new Object();
            defaultParam.Tenant = 1;

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl;
            myUrl = _ReconciliationStageBUrl + "?tenant=" + objToCheck1.Tenant;
            
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonReconcileStageB ..." + _ResponseToken);
            $.ajax({
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response,arryColumns);
                    
                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });


            return false;
        }




        function OnClickButtonReconcileStageBNoBatch() {

            var defaultParam = new Object();
            defaultParam.Tenant = 1;
            defaultParam.GLAccountId = "1-clear2get_all";

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl;
            if (objToCheck1.GLAccountId == "")
            {
                myUrl = _ReconciliationStageBUrl + "?tenant=" + objToCheck1.Tenant + "&noBatch=1";
            }
            else
            {
                 myUrl = _ReconciliationStageBUrl + "?tenant=" + objToCheck1.Tenant + "&gLAccountId=" + objToCheck1.GLAccountId + "&noBatch=1";
           }
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonReconcileStageB ..." + _ResponseToken);
            $.ajax({
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response, arryColumns);

                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });


            return false;
        }




        function OnClickButtonReconcileStageC() {

            var defaultParam = new Object();
            defaultParam.Tenant = 1;
            defaultParam.GLAccountId = "Id, or empty value to get all";
            defaultParam.AccountTypeCode = "2=Client, 3=Vendor";
            defaultParam.UpToDueDate = "01.01.2020";
            defaultParam.LT_LinesMaximum = 50;
            defaultParam.MaxPageSize = 500;

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl;
            myUrl = _ReconciliationStageCUrl + "?tenant=" + objToCheck1.Tenant;
            myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;

            myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;

            myUrl = myUrl + "&upToDueDate=" + objToCheck1.UpToDueDate;

            myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;

            myUrl = myUrl + "&maximalPageSize=" + objToCheck1.MaxPageSize;

            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonReconcileStageC ..." + _ResponseToken);
            $.ajax({
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response, arryColumns);

                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });


            return false;
        }




        function OnClickButtonReconcileStageCNoBatch() {

            var defaultParam = new Object();
            defaultParam.Tenant = 1;
            defaultParam.GLAccountId = "Id, or empty value to get all";
            defaultParam.AccountTypeCode = "2=Client, 3=Vendor";
            defaultParam.UpToDueDate = "01.01.2020";
            defaultParam.LT_LinesMaximum = 50;
            defaultParam.MaxPageSize = 500;
            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl;
            myUrl = _ReconciliationStageCUrl + "?tenant=" + objToCheck1.Tenant;
            myUrl = myUrl + "&gLAccountId=" + objToCheck1.GLAccountId;

            myUrl = myUrl + "&accountTypeCode=" + objToCheck1.AccountTypeCode;

            myUrl = myUrl + "&upToDueDate=" + objToCheck1.UpToDueDate;

            myUrl = myUrl + "&lT_LinesMaximum=" + objToCheck1.LT_LinesMaximum;

            myUrl = myUrl + "&maxPageSize=" + objToCheck1.MaxPageSize;

            myUrl = myUrl + "&noBatch=1";

            

            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonReconcileStageC ..." + _ResponseToken);
            $.ajax({
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response, arryColumns);

                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });


            return false;
        }


     
        function OnClickButtonCardGLAccountConnect() {

            var defaultParam = new Object();
            defaultParam.Tenant = 1;

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl;
            myUrl = _CardGLAccountConnectUrl + "?tenant=" + objToCheck1.Tenant;
            
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonCardGLAccountConnect ..." + _ResponseToken);
            $.ajax({
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8',  
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableReconciliation(response,arryColumns);
                    
                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });


            return false;
        }



        function OnClickButtonRevaluationsBatch() {

            var defaultParam = new Object();
            //defaultParam.RevaluationEnabled = true;
            //defaultParam.ChartOfAccountsTypeCode = "1-1";
            //defaultParam.ChartOfAccountsId = "1-1";
            //defaultParam.AccountTypeCode = "1-1";
            //defaultParam.GLAccountId = "1-30";
            //defaultParam.AccountingCurrencyId = "1-1";
            //defaultParam.RevaluationDate = new DateTime(2016, 12, 31);
            defaultParam.Tenant = 1;

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
                //if (objToCheck.RevaluationEnabled) {
                //} else {
                //    var str = JSON.stringify(defaultParam);
                //    $(".classTextBoxParam").val(str);
                //    return false;
                //}
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl = _RevaluationUrl + "?gLAccountId=" + objToCheck1.AccountId + "&tenant=" + objToCheck1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonRevaluationsSearch ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableRevaluation(response, arrayGLAccountColumns);

                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });





            return false;
        }


        function OnClickButtonPostDatedChequeRedemptionBatch() {

            var defaultParam = new Object();
            //defaultParam.RevaluationEnabled = true;
            //defaultParam.ChartOfAccountsTypeCode = "1-1";
            //defaultParam.ChartOfAccountsId = "1-1";
            //defaultParam.AccountTypeCode = "1-1";
            //defaultParam.GLAccountId = "1-30";
            //defaultParam.AccountingCurrencyId = "1-1";
            //defaultParam.RevaluationDate = new DateTime(2016, 12, 31);
            defaultParam.Tenant = 1;

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classTextBoxParam").val();

                var objToCheck = JSON.parse(myJson);
                //if (objToCheck.RevaluationEnabled) {
                //} else {
                //    var str = JSON.stringify(defaultParam);
                //    $(".classTextBoxParam").val(str);
                //    return false;
                //}
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var objToCheck1 = JSON.parse(myJson);

            var myUrl = _ARPaymentChequeUrl + "?Id=" + objToCheck1.Id + "&tenant=" + objToCheck1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonRedemptionSearch ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'GET',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: myJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    DrawTableARPaymentCheque(response, arrayARPaymentChequeColumns);

                    var obj = JSON.parse(myJson);
                    obj.CallBack = response;
                    var str = JSON.stringify(obj);
                    $(".classTextBoxParam").val(str);
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });





            return false;
        }



        function SendGLaccount() {
            if (!_ResponseToken) {
                getToken();
            }
            var journalJson = $(".classTextBoxParam").val();

            $(".class_LabelLog").val("GLaccount ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: _GLAccountsUrl,// url + '/api/Journals', //this is the path to web api controller method
                type: 'POST',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: journalJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });
            return false;
        }

        
        function SendChartOfAccounts() {
            if (!_ResponseToken) {
                getToken();
            }
            var chartJson = $(".classTextBoxParam").val();

            $(".class_LabelLog").val("ChartOfAccounts ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: _ChartOfAccountsUrl,// url + '/api/ChartOfAccounts', //this is the path to web api controller method
                type: 'POST',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: chartJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });
            return false;
        }
        function Clear_ResponseToken(){
            _ResponseToken = null;
            return false;
        }

        function SendJornal() {
            if (!_ResponseToken) {
                getToken();
            }
            var journalJson = $(".classTextBoxParam").val();
            
            $(".class_LabelLog").val("Journals ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: journalUrl ,// url + '/api/Journals', //this is the path to web api controller method
                type: 'POST',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: journalJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error
                        
                        alert("error" + textStatus + errorThrown  );
                        
                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });
            return false;
        }


        //_JournalId2Void" /><button id="btnVoidJournal" onclick="javascript:return ;
        function VoidJournal() {
            if (!_ResponseToken) {
                getToken();
            }
            
            var strOverrideStorno = $(".class_OverrideStorno").val();
            var objOverrideStorno = JSON.parse(strOverrideStorno);
            var journalId = $(".class_JournalId2Void").val();
            var loginParameterJson = $(".class_loginParameter").val();//JSON.stringify(loginParameter)
            var loginParameter = JSON.parse(loginParameterJson);
            var voidurl = JournalOpUrl + "?JournalOp=void&JournalId=" + journalId + "&tenant=" + loginParameter.Tenant
            + "&AccountingEntityCode=" + objOverrideStorno.AccountingEntityCode
            + "&AccountingEntityId=" + objOverrideStorno.AccountingEntityId
            + "&AccountingEntityReference=" + objOverrideStorno.AccountingEntityReference
            ;
            
            $(".class_LabelLog").val("voidurl ..." + voidurl);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: voidurl,// url + '/api/Journals', //this is the path to web api controller method
                type: 'DELETE',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                //data: journalJson,
                success: function (response, textStatus, xhr) {
                    //handle success 
                    //alert("success ");;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    //response.Token = response.Token;
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus != 'abort') {
                        //handle error

                        alert("error" + textStatus + errorThrown);

                        $(".class_LabelLog").val(xhr.responseText);
                    }
                }
            });
            return false;
        }
        $(document).ready(function () {
            if ($(".class_loginParameter").val() == "") {
                var loginParameter =
                        {
                            Email: "yaronc@amital.co.il",
                            Password: "!Y123456",
                            IsUser: true,
                            CardId: null,
                            CardType: null,
                            ByToken: false,
                            IsMobileLogin: false,
                            GetToken: true,
                            Tenant: 62

                        };
                $(".class_loginParameter").val(JSON.stringify(loginParameter));


                var overrideStorno =
                        {
                            AccountingEntityCode : "7" ,//	הפקדת מזומן	Cash Deposit
                            AccountingEntityReference : "Cash Deposit 7",
                            AccountingEntityId : "Deposit1212",
                        };

                $(".class_OverrideStorno").val(JSON.stringify(overrideStorno));
                
                //throw "Please Init authentication User ";
            }
            if (!_ResponseToken) {
                getToken();
            }
           
        });
        
    </script>
  <%--  <script> alert(Date(-62135596800000));
        alert(Date(1461704400000));
    </script>--%>
    <style>
        section {
    width: 90%;
    height: 300px;
    background: aqua;
    margin: auto;
    padding: 10px;
}
div#one {
    width: 60%;
    height: 300px;
    background: red;
    float: left;
}
div#two {
    margin-left: 60%;
    height: 300px;
    background: pink;
}
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <span >LoginParameter:</br><textarea rows="2" cols="200" class="class_loginParameter" ></textarea></span>    
        <div style="background: yellow">
            
            
            <ul>
                <li>Reverse 
                <ul>
                    <li>
                        <asp:Button ID="_ButtonReverseTotal" runat="server" Text="Reverse from Total to Ledger" OnClick="_ButtonReverseTotal_Click" />
                        <asp:Button ID="_ButtonReverseTrans" runat="server" Text="Reverse from Ledger to journal line" OnClick="_ButtonReverseTrans_Click" />
                        <asp:Button ID="_ButtonReverseGLBalance" runat="server" Text="ReverseGLBalance" OnClick="_ButtonReverseGLBalance_Click" />
                        <asp:Button ID="_ButtonReverseTotal0" runat="server" Text="Reverse from Total to Ledger" OnClick="_ButtonReverseTotal_Click" />
                        <asp:Button ID="_ButtonReverseTotalControl" runat="server" Text="***Control***Reverse from Total to Ledger" OnClick="_ButtonReverseTotalControl_Click" />
                    <asp:Button ID="_ButtonReverseEngineerControlAccountAccumulateChild" runat="server" Text="*Control*TOT Sum Child" OnClick="_ButtonReverseEngineerControlAccountAccumulateChild_Click" />
                    </li>
                    <li>
                        SysCheck

                            <asp:Button ID="_AccountingIntegrityService" runat="server" Text="AccountingIntegrityService" OnClick="_AccountingIntegrityService_Click" />

                            <asp:Button ID="_ButtonSysCheckTotalSumIsZero" runat="server" Text="TotalSumIsZero" OnClick="_ButtonSysCheckTotalSumIsZero_Click" />
                        <asp:Button ID="_ButtonSysCheckLdegerTransSumIsZero" runat="server" Text="LdegerTransSumSumIsZero" OnClick="_ButtonSysCheckLdegerTransSumIsZero_Click" />
                        <asp:Button ID="_ButtonSysCheckGLAccJL2Total" runat="server" Text="GLAccJL2Total" OnClick="_ButtonSysCheckGLAccJL2Total_Click" />
                        <asp:Button ID="_ButtonIsApprovedJournalTOTZero" runat="server" Text="IsApprovedJournalTOTZero" OnClick="_ButtonIsApprovedJournalTOTZero_Click" />
                    </li>
                    </li>
                    <li>
                        <asp:Button ID="_ButtonReverseDueDate" runat="server" Text="ReverseDueDate" OnClick="_ButtonReverseDueDate_Click" />
                        <asp:Button ID="_ButtonDueLocalBalance" runat="server" Text="FixDueLocalBalance" OnClick="_ButtonDueLocalBalance_Click"/>
                    </li>
                
                <li>
                    FIX PRA PRA 
                    <asp:Button ID="_ButtonReverseTotalFIX" runat="server" Text="FIX TOTAL from  Ledger " OnClick="_ButtonReverseTotalFIX_Click" />
                    <asp:Button ID="_ButtonReverseGLBalanceFIX" runat="server" Text="FIX ReverseGLBalance" OnClick="_ButtonReverseGLBalanceFIX_Click" />
                    <asp:Button ID="_ButtonReverseTotalFIXControl" runat="server" Text="FIX TOTAL from  Ledger ***control**" OnClick="_ButtonReverseTotalFIXControl_Click" />
                </li>
                    
                </ul>
                </li>
                <li>JournalApproveService
                    <ul>
                        <li>
                        <button id="btnClear_ResponseToken" onclick="javascript:return Clear_ResponseToken();" >Clear _ResponseToken</button>
                            </li>
                        <li>
                            <asp:Button ID="_ButtonJournalApproveQueue" runat="server" Text=".Queue" OnClick="_ButtonJournalApproveQueue_Click" /></li>
                        <li>
                            <asp:Button ID="_ButtonJournalApprove" runat="server" Text=".WorkWithoutQueue" OnClick="_ButtonJournalApprove_Click" /></li>
                                                

                    </ul>
                </li>
                <li>
                    <%--<asp:Button ID="_ButtonCreateNewJournal1" runat="server" Text="example Journal" OnClick="_ButtonCreateNewJournal_Click" />--%>
                    <button id="_ButtonCreateNewJournal1" onclick="javascript: $('.classTextBoxParam').val(_DefaultJornalPM);  return false;" >example Journal</button>
                    
                    <a href="http://www.jsoneditoronline.org/" > format json</a>
                    
                    <button id="btnSendJournal" onclick="javascript:return SendJornal();" >btnSendJournal</button>
                    <asp:Button ID="_ButtonCreateRandomJournal" runat="server" Text="Create RandomJournal" OnClick="_ButtonCreateRandomJournal_Click" />
                    <br />
                    JournalId2Void<input type="text" value="1-407" class="class_JournalId2Void"  id="_JournalId2Void" /><input type="text" value="1-407" class="class_OverrideStorno"  /><button id="btnVoidJournal" onclick="javascript:return VoidJournal();"  >Void</button>

                </li>
                <li>
                    <%--<asp:Button ID="_ButtonCreateNewJournal1" runat="server" Text="example Journal" OnClick="_ButtonCreateNewJournal_Click" />--%>
                    <button id="Button1" onclick="javascript: $('.classTextBoxParam').val(_DefaultGLaccountPM);  return false;" >DefaultGLaccountPM</button>
                    <button id="Button2" onclick="javascript:return SendGLaccount();" >btnSendGLaccountPM</button>
                </li>
                <li>
                    <button id="Button3" onclick="javascript: $('.classTextBoxParam').val(_DefaultChartOfAccountsPM);  return false;" >DefaultChartOfAccountsPM</button>
                    <button id="Button4" onclick="javascript:return SendChartOfAccounts();" >btnSendChartOfAccountsPM</button>
                </li>
                <li><asp:Button ID="_ButtonAging" runat="server" Text="Aging" OnClick="_ButtonAging_Click" />
                    <asp:Button ID="_ButtonCurrBalanceByType" runat="server" Text="GetGLAccountsLocalBalanceGByChartOfAccountsTypeCode" OnClick="_ButtonCurrBalanceByType_Click" />
                    <asp:Button ID="_ButtonTreeMapCOA" runat="server" Text="TreeMapGLAccountsLocalBalanceGByChartOfAccountsTypeCode" OnClick="_ButtonTreeMapCOA_Click" />
                    
                    <asp:Button ID="ButtoBalanceByCollector" runat="server" Text="BalanceByCollector" OnClick="_ButtonBalanceByCollector_Click" />
                    <asp:Button ID="ButtonLoadBankPages" runat="server" Text="LoadBankPages" OnClick="_ButtonLoadBankPages_Click" />
                    
                    
                </li>
                <li><asp:Button ID="_ButtonCheckBalance" runat="server" Text="Check Balance" OnClick="_ButtonCheckBalance_Click" />
                <asp:Button ID="_ButtonLedgerTransactionBalance" runat="server" Text="Check Balance +  Trans" OnClick="_ButtonLedgerTransactionBalance_Click" /></li>
                <button id="javaButtonLedgerTransactionBalance" onclick="javascript:return OnClickButtonLedgerTransactionBalance();" >javaButtonLedgerTransactionBalance</button>
                <li> 
                    <button id="ButtonAutomaticReconcile" onclick="javascript:return OnClickButtonAutomaticReconcile();" >AutomaticReconcile</button>
                    <a href="Reconcile.aspx">Reconcile.aspx</a>
                    <a href="TrailReport.aspx">TrailReport.aspx</a>
                    <button id="ButtonReconcileAfterConversion"  onclick="javascript:return OnClickButtonReconcileAfterConversion();">Reconcile After Conversion</button>        
                    <button id="ButtonReconcileStageB"  onclick="javascript:return OnClickButtonReconcileStageB();">Reconcile Stage B</button>        
                    <asp:Button id="_ButtonExternalReconcile" runat="server" onclick="_ButtonExternalReconcile_click"   Text="ExternalReconcile" />
                    <button id="ButtonReconcileAfterConversionNoBatch"  onclick="javascript:return OnClickButtonReconcileAfterConversionNoBatch();">Reconcile After Conversion - No Batch</button>        
                    <button id="ButtonReconcileStageBNoBatch"  onclick="javascript:return OnClickButtonReconcileStageBNoBatch();">Reco Stage B - No Batch</button>        
                </li>
                <li>
                    <button id="ButtonRevaluationsBatch" onclick="javascript:return OnClickButtonRevaluationsBatch();" >RevaluationsBatch</button>
                    <%--<button id="ButtonCardIndex" onclick="javascript:return OnClickButtonLedgerTransactionCardIndex();" >CardIndex</button>--%>
                    <asp:Button id="ButtonCardIndex" runat="server" Text="Card Index" OnClick="_ButtonLedgerTransactionCardIndex_Click" />
                    <%--<asp:Button ID="ButtonCardGLAccountConnect" runat="server" Text="Card GLAccount Connect (Tenant)" OnClick="ButtonCardGLAccountConnect_Click" />--%>        
                    <button id="ButtonCardGLAccountConnect"  onclick="javascript:return OnClickButtonCardGLAccountConnect();">Card GLAccount Connect (Tenant)</button>        
                    <asp:Button id="_ButtonCardIndexNew" runat="server" Text="Card Index New" OnClick="_ButtonCardIndexNew_Click" />
                    <button id="ButtonReconcileStageC"  onclick="javascript:return OnClickButtonReconcileStageC();">Reconcile Stage C</button>        
                    <button id="ButtonReconcileStageCNoBatch"  onclick="javascript:return OnClickButtonReconcileStageCNoBatch();">Reco Stage C - No Batch</button>        
                </li>
                <li>
                    <button id="ButtonPostDatedChequeRedemptionBatch" onclick="javascript:return OnClickButtonPostDatedChequeRedemptionBatch();" >PostDatedChequeRedemptionBatch</button>
                    <%--<button id="ButtonCardIndex" onclick="javascript:return OnClickButtonLedgerTransactionCardIndex();" >CardIndex</button>--%>
                    <%--<asp:Button id="ButtonPostDatedChequeRedemption" runat="server" Text="Card Index" OnClick="_ButtonLedgerTransactionCardIndex_Click" />--%>
                </li>
            
            <li>
                                    
                    <asp:Button ID="ButtonBuildTenant" runat="server" Text="BuildTenant" OnClick="ButtonBuildTenant_Click" />        
                    
                </li>
                <li>
            <asp:Button ID="ButtonYearTransfer" runat="server" Text="YearTransfer(LastY)" OnClick="ButtonYearTransfer_Click" />        
            <asp:Button ID="ButtonYearTransferCancel" runat="server" Text="CancelYearTransfer(LastY)" OnClick="ButtonYearTransferCancel_Click" />        
                </li>
                <li>
            <asp:Button ID="ButtonGetSystem1000" runat="server" Text="Get System 1000(Tenant)" OnClick="ButtonGetSystem1000_Click" />        
            <asp:Button ID="ButtonLoadSystem1000" runat="server" Text="Load System 1000(Tenant)" OnClick="ButtonLoadSystem1000_Click" />        
            <asp:Button ID="ButtonLoadConsolTaxRep" runat="server" Text="Load Consol. Tax Rep.(Tenant)" OnClick="ButtonLoadConsolTaxRep_Click" />        
                </li>
            </ul>
            
        </div>
        
        <section>
    
            <div id="one">
                <asp:HiddenField  ID="_MyLastAction" runat="server"    ></asp:HiddenField>    
                <asp:HiddenField  ID="_myToken" runat="server"   ></asp:HiddenField>    
            <asp:TextBox ID="_TextBoxParam" runat="server" MaxLength="60000" Height="300px" Width="90%"  TextMode="multiline"   CssClass="classTextBoxParam" ></asp:TextBox>    </div>
    
            <div id="two"><asp:TextBox ID="_LabelResult" Enabled="false" Width="80%" Height="80%" runat="server"  MaxLength="60000"  TextMode="multiline" /><br /> 
                <asp:TextBox ID="_LabelLog" Enabled="false" Width="80%" Height="80%" runat="server"  MaxLength="60000"  TextMode="multiline" CssClass="class_LabelLog"  />
                          </div>

</section>
        <div style="background: green">
            
            <asp:GridView ID="GridView1" runat="server"></asp:GridView>
            <div id="jsonDataTable"></div>
        </div>
    </form>
</body>
</html>
