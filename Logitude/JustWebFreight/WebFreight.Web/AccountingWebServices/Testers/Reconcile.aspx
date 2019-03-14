<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reconcile.aspx.cs" Inherits="WebFreight.Web.AccountingWebServices.Testers.Reconcile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        var urlBase = '<% =WebFreight.Web.AccountingWebServices.Testers.AccountingBalanceTester.GetHost() %>';
        //http://accountingtest/accounting/api/authentication
        if (urlBase == "") {
        } else {
            //urlBase = '/accounting';
            urlBase = "";
        }
        var l = '/AccountingWebServices/Testers/Reconcile.aspx'.toLowerCase();
        var logitude_url = location.href.toLowerCase().replace(l, '');


        //alert(logitude_url);
        urlBase = logitude_url;

        var authUrl = urlBase + '/api/authentication?&tenant=';
        var journalUrl = urlBase + '/api/Journals';
        var _ReconciliationUrl = urlBase + '/api/ReconciliationOp';
        var _ReconciliationCancelUrl = urlBase + '/api/ReconciliationCancel'; //ReconciliationCancelController
        var _myORMatchLedgerTransactionList = new Array();
        var _ResponseToken = "";
        var arryCol = ["Id", "DocumentDate", "JournalId", "JournalNumber", "Source", "SourceType", "AccountingDate", "DueDate", "CurrencyId", "ExchangeRate", "OpenAmount", "OpenAmountCurrencyId", "AmountToReconcile", "Reference1", "Reference2", "Reference3", "SearchFields", "Mark", "IsReconciled", "GroupHash"];

        var _LastAutomaticReconcileResponse = new Object();

        //var loginParameter =
        //{
        //    Email: "admin@fnarsoft.com",
        //    Password: "!J123456.0",
        //    IsUser: true,
        //    CardId: null,
        //    CardType: null,
        //    ByToken: false,
        //    IsMobileLogin: false,
        //    GetToken: true
        //};
        //$(".class_loginParameter").val(JSON.stringify(loginParameter));
        //$(".class_loginParameter").val("ssss");


        function getToken() {
           
            var loginParameterJson = $(".class_loginParameter").val();//JSON.stringify(loginParameter)
            var loginParameter = JSON.parse(loginParameterJson);
            var tenant = loginParameter.Tenant;
            var authUrlWithtenant =authUrl + tenant;
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
                    if (!_ResponseToken) {
                        alert("error _ResponseToken " + _ResponseToken);
                        $(".class_LabelLog").val(JSON.stringify(xhr));
                        throw "ResponseToken  is null , try again"; 
                    }
                    
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert("error" + textStatus + errorThrown);
                    $(".class_LabelLog").val(JSON.stringify(xhr));
                    if (textStatus != 'abort') {
                        //handle error
                        //    alert("error" + textStatus + errorThrown);
                    }
                    throw "ResponseToken  is null , try again";
                }
            });
        }
       
   
        function DrawTableReconciliation(divId,data) {
            $(divId).html("");
            var row = $("<tr />")
            $(divId).append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it
            //row.append($("<th> action </th>"));
            //row.append($("<th> JournalId </th>"));
            //row.append($("<th> JournalLineNumber </th>"));
            //row.append($("<th> LocalAmountDebit </th>"));
            //row.append($("<th> myLedgerTransactionPM.CumulativeLocalAmount </th>"));
            if (data == null) return;
            if (data.length < 1) return;
            row.append($("<th> action1 </th>"));
            //row.append($("<th> open Balance</th>"));
            for (x in arryCol) {

                row.append($("<th> " + arryCol [x] + " </th>"));
            }
            for (var i = 0; i < data.length; i++) {
                var myLedgerTransactionPM = data[i];
                var row = $("<tr />")
                $(divId).append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it
                //row.append($("<th> <button click='alert(" + myLedgerTransactionPM.JournalId + ")' value='test' /> </th>"));
                //var arry=[ "JournalId","JournalLineNumber"];
                row.append($("<td> " + "<button onclick=\"MatchIt(this,'" + myLedgerTransactionPM.Id + "'); return false\" >Match It</button>" + "</td>"));
                //row.append($("<td> <input type=number  value='" + myLedgerTransactionPM.OpenAmount + "' /> </td>"));
                for (x in arryCol) {
                    row.append($("<td> " + myLedgerTransactionPM[arryCol[x]] + "</td>"));
                }
            }



        }
        
        function RevertMatch(id) {
            //alert(id);
            
            for (var i = 0; i < _myORMatchLedgerTransactionList.length; i++) {
                var cur = _myORMatchLedgerTransactionList[i];
                if (cur.Id == id) {
                    _myORMatchLedgerTransactionList =_myORMatchLedgerTransactionList.slice(i, 1);
                    break;
                }
            }
            
            //alert(JSON.stringify(_myORMatchLedgerTransactionList));
            var divname='#myMatchLedgerTransactionListOpenReconcile' + id ;
            $(divname).remove();
        }

        function OpenAmountonchange(objInputNumber, id) {
            for (var i = 0; i < _myORMatchLedgerTransactionList.length; i++) {
                var cur = _myORMatchLedgerTransactionList[i];
                if (cur.Id == id) {
                    cur.AmountToReconcile = objInputNumber.value;
                    break;
                }
            }
            //alert(JSON.stringify(_myORMatchLedgerTransactionList));
            
        }
        function MatchIt(objB,id) {
            //alert(id);
            objB.style.background = 'red';
            //return false;
            for (i in _LastResponse.OpenReconciliation) {
                var myLedgerTransactionPM = _LastResponse.OpenReconciliation[i];
                if (myLedgerTransactionPM.Id == id) {
                    myLedgerTransactionPM.AmountToReconcile = myLedgerTransactionPM.OpenAmount;
                    AddMatch(myLedgerTransactionPM,true);
                }
            }
            
        }
        function PutAutomaticReconcileFilterCallBack(response, canChange)
        {
            for (var i = 0; i < response.length; i++) {
                var myLedgerTransactionPM = response[i];
                if (canChange) {
                    myLedgerTransactionPM.AmountToReconcile = myLedgerTransactionPM.AmountToReconcile;
                } else {
                    myLedgerTransactionPM.AmountToReconcile = myLedgerTransactionPM.OpenAmount;
                }
                //
                AddMatch(myLedgerTransactionPM, canChange);
            }
        }
        function AddMatch(myLedgerTransactionPM,canChange) {
            _myORMatchLedgerTransactionList.push(myLedgerTransactionPM);
            var row = $("<tr id=myMatchLedgerTransactionListOpenReconcile" + myLedgerTransactionPM.Id + " />")
            if (canChange) {
                row.append($("<td> <input type=number onchange=\"OpenAmountonchange(this,'" + myLedgerTransactionPM.Id + "'); return false\"  value='" + myLedgerTransactionPM.AmountToReconcile + "' /> </td>"));
            } else {
                row.append($("<td> <input type=number  disabled  value='" + myLedgerTransactionPM.AmountToReconcile + "' /> </td>"));
            }
            
            $("#myMatchLedgerTransactionListOpenReconcile").append(row)
            
            row.append($("<td> " + "<button onclick=\"RevertMatch('" + myLedgerTransactionPM.Id + "'); return false\" >RevertMatch</button>" + "</td>"));
            
            for (x in myLedgerTransactionPM) {
                row.append($("<td> " + myLedgerTransactionPM[x] + "</td>"));
            }
        }
        var ReconcileParam = function () {
            var today = new Date();
            var lastMonth = new Date();
            lastMonth.setDate(today.getDate() - 365);

            this.OnlyDraft = null;
            this.Search = null;
            this.FromDate = lastMonth;
            this.ToDate = today;
            this.OpenAmount_GreaterThan = 0;
            this.OpenAmount_LessThan = 1000000;
        };
        
        function onclickBring() {
            alert("onclickBring");
        }
        function onclickAuto() {
            
            ClearScreenMatch()
           
            PutAutomaticReconcileFilter();
        }
        function ClearScreenMatch() {
            for (var i = 0; i < _myORMatchLedgerTransactionList.length; i++) {
                RevertMatch(_myORMatchLedgerTransactionList[i].Id);
            }
            _myORMatchLedgerTransactionList = new Array();
            $("#myMatchLedgerTransactionListOpenReconcile").html("");
        }
        

        function PutAutomaticReconcileFilter() {
            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classReconcileParam").val();;

                var paramFilteredReconciliation = JSON.parse(myJson);
                
                
                var QueryOperationsJson = JSON.stringify(paramFilteredReconciliation.QueryOperations);

                //defaultParam.AccountId = "1-1";
                //defaultParam.Tenant = 1;
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var paramFilteredReconciliation1 = JSON.parse(myJson);
            ;
            paramFilteredReconciliation.QueryOperations.method1 = $("#method1").val();
            paramFilteredReconciliation.QueryOperations.method2 = $("#method2").val();
            paramFilteredReconciliation.QueryOperations.method3 = $("#method3").val();
            var QueryOperationsJson = JSON.stringify(paramFilteredReconciliation.QueryOperations);
            var myUrl = _ReconciliationUrl + "?gLAccountId=" + paramFilteredReconciliation1.AccountId + "&tenant=" + paramFilteredReconciliation1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonLedgerTransactionBalance ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'PUT',//PutAutomaticReconcileFilter
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: QueryOperationsJson,
                success: function (response, textStatus, xhr) {

                    _LastAutomaticReconcileResponse = response;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    PutAutomaticReconcileFilterCallBack(response,false)

                    //var myJson = $(".classReconcileParam").val();;

                    //var paramFilteredReconciliation = JSON.parse(myJson);
                    //paramFilteredReconciliation.QueryOperations.CallBack = response.CallBack;

                    //$(".classReconcileParam").val(JSON.stringify(paramFilteredReconciliation));
                    //;
                    //DrawTableReconciliation("#myLedgerTransactionListOpenReconcile", response.OpenReconciliation);


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
        function QueryFilterItem(name, value, isCustom, Operator, value2, displayInList) {

            this.FieldName = name;
            this.FieldValue = value;
            this.FieldValue2 = value2;
            this.IsCustom = isCustom;
            this.Operator = Operator;

            this.DisplayInList = displayInList;
            //public bool IsCustomField { get; set; }
            //public string FieldDataType { get; set; }

        }

        function GetParams(withMore) {

            


            var today = new Date();
            var lastMonth = new Date();
            lastMonth.setDate(today.getDate() - 365);
            var defaultParam = new Object();
            //defaultParam.IsAutomaticReconcile = true;

            defaultParam.AccountId = "1-216872"; // 59-Diaz
            defaultParam.Tenant = 1071;
            var QueryOperations = new Object();
            QueryOperations.IsFilteredReconciliation = true;
            QueryOperations.PageSize = 10;
            QueryOperations.PageIndex = 1;
            QueryOperations.SortByColumnName = "SourceType";
            QueryOperations.SortDirectin = "ascending";//Descending
            QueryOperations.DataCount = null;
            QueryOperations.QueryFilterItems = new Array();
            if (withMore) {
                

                //var p = new QueryFilterItem("FromDate", today, false, lastMonth, true);
                QueryOperations.QueryFilterItems.push(new QueryFilterItem("AccountingDate", lastMonth, false, "Between", today, false));
                QueryOperations.QueryFilterItems.push(new QueryFilterItem("DecimalOpenAmount", "0",true, "GreaterThanOrEqual", null, false)); 
                QueryOperations.QueryFilterItems.push(new QueryFilterItem("DecimalOpenAmount", "100000", true, "LessThanOrEqual", null, false));
                //QueryOperations.QueryFilterItems.push(new QueryFilterItem("AccountId", "1-1", false, "Equals", null, true));
                QueryOperations.QueryFilterItems.push(new QueryFilterItem("SearchFields", "", false, "Contains", null, false));


                QueryOperations.CallBack = null;
                
            }
            defaultParam.QueryOperations = QueryOperations;

            $(".classReconcileParam").val(JSON.stringify(defaultParam));
        }


        function DeleteDraft() {

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classReconcileParam").val();;

                var paramFilteredReconciliation = JSON.parse(myJson);
                if (!paramFilteredReconciliation.QueryOperations.CallBack) {
                    ClearScreenMatch();
                }
                var QueryOperationsJson = JSON.stringify(paramFilteredReconciliation.QueryOperations);

                //defaultParam.AccountId = "1-1";
                //defaultParam.Tenant = 1;
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var paramFilteredReconciliation1 = JSON.parse(myJson);

            var myUrl = _ReconciliationUrl + "?gLAccountId=" + paramFilteredReconciliation1.AccountId + "&tenant=" + paramFilteredReconciliation1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonLedgerTransactionBalance ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'DELETE',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                //data: QueryOperationsJson,
                success: function (response, textStatus, xhr) {

                    _LastResponse = response;
                    $(".class_LabelLog").val(JSON.stringify(response));

                    ClearScreenMatch();
                    location.reload();


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

        function GetOpenReconcile() {
            
            

            if (!_ResponseToken) {
                getToken();
            }
            try {
                var myJson = $(".classReconcileParam").val();;

                var paramFilteredReconciliation = JSON.parse(myJson);
                if (!paramFilteredReconciliation.QueryOperations.CallBack) {
                    ClearScreenMatch();
                }
                var   QueryOperationsJson =JSON.stringify(paramFilteredReconciliation.QueryOperations);

                //defaultParam.AccountId = "1-1";
                //defaultParam.Tenant = 1;
            } catch (e) {

                var str = JSON.stringify(defaultParam);
                $(".classTextBoxParam").val(str);
                return false;
            }
            var paramFilteredReconciliation1 = JSON.parse(myJson);

            var myUrl = _ReconciliationUrl + "?gLAccountId=" + paramFilteredReconciliation1.AccountId + "&tenant=" + paramFilteredReconciliation1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonLedgerTransactionBalance ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'POST',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: QueryOperationsJson,
                success: function (response, textStatus, xhr) {
                    
                    _LastResponse = response;
                    $(".class_LabelLog").val(JSON.stringify(response));
                    

                    var myJson = $(".classReconcileParam").val();;

                    var paramFilteredReconciliation = JSON.parse(myJson);
                    paramFilteredReconciliation.QueryOperations.CallBack = response.CallBack;

                    $(".classReconcileParam").val(JSON.stringify(paramFilteredReconciliation));
                    ;
                    DrawTableReconciliation("#myLedgerTransactionListOpenReconcile", response.OpenReconciliation);
                    if (response.OpenReconciliationDraft) {
                        PutAutomaticReconcileFilterCallBack(response.OpenReconciliationDraft,true);
                    }
                    
                    
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

        function SendMatchReconcile() {
            var firstMatch = _myORMatchLedgerTransactionList[0];
            var ReconciliationPM = new Object();
            ReconciliationPM.id = "new";
            ReconciliationPM.ChangeSetOp = 1;
            ReconciliationPM.Tenant = firstMatch.Tenant;
            ReconciliationPM.AccountId = firstMatch.AccountId;
            ReconciliationPM.Number = "get";
            ReconciliationPM.CreateDate = new Date();
            ReconciliationPM.CreatedByUserId = null;
            ReconciliationPM.CreatedByUserId = null;

            ReconciliationPM.ReconciliationLines = new Array();

            for (var i = 0; i < _myORMatchLedgerTransactionList.length; i++) {
                var currLedgerTrans = _myORMatchLedgerTransactionList[i];
                var ReconciliationLinePM = new Object();
                ReconciliationLinePM.ChangeSetOp = 1;
                ReconciliationLinePM.ReconciliationId = ReconciliationPM.id;
                ReconciliationLinePM.Tenant = ReconciliationPM.Tenant;
                ReconciliationLinePM.line = i;
                ReconciliationLinePM.CurrencyId = currLedgerTrans.OpenAmountCurrencyId;
                ReconciliationLinePM.TransactionId = currLedgerTrans.Id;
                ReconciliationLinePM.ReconciliationAmount = //currLedgerTrans.OpenAmount;
                            currLedgerTrans.AmountToReconcile;

                //ReconciliationLinePM.IsPartial = currLedgerTrans.OpenAmount;
                ReconciliationLinePM.GroupNumber = currLedgerTrans.GroupHash;

                ReconciliationPM.ReconciliationLines.push(ReconciliationLinePM);
            }
            var jsonReconciliationPM = JSON.stringify(ReconciliationPM)
            $(".classMatchReconcile").val(jsonReconciliationPM);



            var myUrl = _ReconciliationUrl;// + "?gLAccountId=" + paramFilteredReconciliation1.AccountId + "&tenant=" + paramFilteredReconciliation1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonLedgerTransactionBalance ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'POST',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: jsonReconciliationPM,
                success: function (response, textStatus, xhr) {

                    _LastResponse = response;
                    $(".class_LabelLog").val(JSON.stringify(response));



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

        function CancellReconcile() {
            var id = $(".classReconcile2Cancel").val();
            var loginParameterJson = $(".class_loginParameter").val();//JSON.stringify(loginParameter)
            var loginParameter = JSON.parse(loginParameterJson);
            var tenant = loginParameter.Tenant;
            var myUrl = _ReconciliationUrl + "?ReconciliationOperation=Cancell&reconciliationId=" + id + "&tenant=" + tenant;
            //alert(myUrl);
            
            $(".class_LabelLog").val("GetReconcile ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl ,
                type: 'delete',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                ///data: jsonLedgerTransactionPM,
                success: function (response, textStatus, xhr) {

                    _LastResponse = response;
                    //if (response.IsCancell) {
                    //    alert("Already Cancelled");
                    //    return;
                    //}
                    if (response.IsCancell == true) {
                        alert("done !!");
                    }
                    
                    



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

        


        function SaveDraft() {
            
            if (_myORMatchLedgerTransactionList.count < 1) {
                if (confirm("No Items On Match List,To Delete ?")) {
                    DeleteDraft();
                }
                return;
            }
            
            var jsonLedgerTransactionPM = JSON.stringify(_myORMatchLedgerTransactionList)
            $(".classMatchReconcile").val(jsonLedgerTransactionPM);



            var myUrl = _ReconciliationUrl;// + "?gLAccountId=" + paramFilteredReconciliation1.AccountId + "&tenant=" + paramFilteredReconciliation1.Tenant;
            //alert(myUrl);


            $(".class_LabelLog").val("OnClickButtonLedgerTransactionBalance ..." + _ResponseToken);
            $.ajax({
                ///url: 'http://localhost:9999/api/authentication?&tenant=1', //this is the path to web api controller method
                url: myUrl,
                type: 'PUT',
                dataType: 'json',
                headers: { 'Token': _ResponseToken },
                contentType: 'application/json; charset=UTF-8', // This is the money shot
                data: jsonLedgerTransactionPM,
                success: function (response, textStatus, xhr) {

                    _LastResponse = response;
                    $(".class_LabelLog").val(JSON.stringify(response));



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
            var loginParameter_val =$(".class_loginParameter").val();
            if ( !loginParameter_val) {
                var loginParameter =
                        {
                            Email: "basel@amital.co.il",
                            Password: "!B123456",
                            IsUser: true,
                            CardId: null,
                            CardType: null,
                            ByToken: false,
                            IsMobileLogin: false,
                            GetToken: true,
                            Tenant: 1
                        };
                $(".class_loginParameter").val(JSON.stringify(loginParameter));
                //throw "Please Init authentication User ";
            }
            if (!_ResponseToken) {
                getToken();
            }
            GetParams(false);
            //UseFilteronchange(false);
            //GetOpenReconcile(); return false;
        });
        
    </script>
</head>
<body>
    
    <form id="form1" runat="server">
        
        <div class="dropdown">
          <span><a href="#">Menu</a></span>
          <div class="dropdown-content">
                <button onclick="GetParams(false);return false;">Min Params</button><button onclick="GetParams(true);return false">Search Params</button>  
            <button onclick="GetOpenReconcile();return false;">Get Open Reconcile</button> 
              <button onclick="onclickAuto();return false">Auto</button>
              <select id="method1" >
                  <option ></option>
                  <option value="1" >OpenAmountABS</option>
                  <option value="2" >ReferenceDate</option>
                  <option value="3" >DueDate</option>
                  <option value="4" >AccountingDate</option>
                  <option value="5" >Reference1</option>
                  <option value="6" >Reference2</option>
                  <option value="7" >Reference3</option>
              </select>
                 <select id="method2" >
                     <option ></option>
                  <option value="1" >OpenAmountABS</option>
                  <option value="2" >ReferenceDate</option>
                  <option value="3" >DueDate</option>
                  <option value="4" >AccountingDate</option>
                  <option value="5" >Reference1</option>
                  <option value="6" >Reference2</option>
                  <option value="7" >Reference3</option>
              </select>
                 <select id="method3" >
                     <option ></option>
                  <option value="1" >OpenAmountABS</option>
                  <option value="2" >ReferenceDate</option>
                  <option value="3" >DueDate</option>
                  <option value="4" >AccountingDate</option>
                  <option value="5" >Reference1</option>
                  <option value="6" >Reference2</option>
                  <option value="7" >Reference3</option>
              </select>
              <input type="button" onclick="SendMatchReconcile(); return false" value="Send Match Reconcile" />
              <button onclick="DeleteDraft();return false;">Delete Draft</button> 
              <button onclick="SaveDraft();return false;">Save Draft</button> 
              <br />
              <button onclick="CancellReconcile();return false;">CancellReconcile :</button><input type="text" class="classReconcile2Cancel"  value="1-1" />
          </div>
        </div>
    <div>
    <span >LoginParameter:</br><textarea rows="2" cols="200" class="class_loginParameter" ></textarea></span>    
        <span >Log:</br><textarea rows="6" cols="200" class="class_LabelLog" ></textarea></span>
        <span>ReconcileParam:</br><textarea rows="6" cols="200" class="classReconcileParam" ></textarea></span>
<%--        <div>***<input type="checkbox" value="Use Filter" onchange="UseFilteronchange(false);" /> filter By***  
<div class="class_Filter"> Search<input type="text" value=""> From:<input type="date" id="_FromDate" value="" />To:<input type="date" id="_ToDate" value="" /> Min OpenAmount <input type="number" id="_MinOpenAmount " /> Max OpenAmount <input type="number" id="_MaxOpenAmount " /></div></div>  --%>
        LedgerTransactionList:</br><div id="myLedgerTransactionListOpenReconcile"></div>
        MatchList:</br><div id="myMatchLedgerTransactionListOpenReconcile"></div>
        
        <span>MatchReconcileJSon:</br><textarea rows="6" cols="200" class="classMatchReconcile" ></textarea></span>
    </div>
    </form>
        
</body>
</html>
