<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewPage.aspx.cs" Inherits="WebFreight.Web.NewPage" %>

<!DOCTYPE html>
<html lang="en">

<head runat="server">
    <title>test</title>

    <link type="text/css" href="htmlhelpers/css/LogitudeMainTheme.css" rel="stylesheet" />

    <!-- Le styles -->
    <link href="htmlhelpers/css/bootstrap.min.css" rel="stylesheet" />
    <link href="htmlhelpers/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="htmlhelpers/css/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="htmlhelpers/css/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <link href="htmlhelpers/css/kendo.dataviz.min.css" rel="stylesheet" type="text/css" />
    <link href="htmlhelpers/css/sunburst.css" rel="stylesheet" />
    <link href="htmlhelpers/css/app.css" rel="stylesheet" />

    <script type="text/javascript" src="htmlhelpers/js/jquery.min.js"></script>
    <script type="text/javascript" src="htmlhelpers/js/kendo.all.min.js"></script>

        <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
    <script src="//html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->

  <%--  <script type="text/javascript" src="Entities/ShipmentEntity.js" ></script>--%>

    <style type="text/css">
        
.k-grid td {
 
    white-space: nowrap;
 
    overflow: hidden;
 
}
 
 
 
.k-grid table {
 
    table-layout: fixed;
 
}


        body 
        {
            margin:10px;
            padding:0px;            
        }
        
        td
        {
            vertical-align:top;
        }
        
        .box
        {
            border: 1px solid #DADAB9;
             border-radius:  5px 5px 3px 3px;
            -webkit-border-radius: 5px 5px 3px 3px;
            -moz-border-radius:  5px 5px 3px 3px;            
        }
        
        .box .headerDiv
        {
            height:25px;
            width:100%;
            background: #DADAB9; 
            text-indent: 10px;                                               
            display:table;
             border-radius:  5px 5px 0 0;
            -webkit-border-radius: 5px 5px 0 0;
            -moz-border-radius:  5px 5px 0 0;  
        }
        
        .box .headerDiv span
        {
            font-size: 14px;
            font-family: "Lucida Sans Unicode";
            color: #1B90CB;
            display:table-cell;
            vertical-align: middle;            
        }
        
                
        #mainDiv
        {
            padding:5px;
            border: 1px solid #D1D1D1;
            border-radius: 8px; 
            -webkit-border-radius: 8px; 
            -moz-border-radius: 8px;            	
        }
       
        #headerDiv
        {
            height:80px;
            vertical-align:top;
        }
        
        
        #tabsBar li
        {
            width:100px;
            font-size: 13px;
            font-family: "Lucida Sans Unicode";
            color: #1B90CB;                    
        }
               
        #tabsBar li:hover
        {
            text-decoration:none;
        }
               
        .tabItem, .k-state-active
        {            
            border-color: #D1D1D1;
        }
        
        .tabPage
        {
            height:570px;
            
            border-color: #D1D1D1;
        }
         
        .tabPage:hover, .tabItem:hover, .k-state-active:hover
        {
            border-color: #D1D1D1;
        }      
      
      .headerTitle
      {
            margin:0px;
            padding:0px;
            font-size: 12px;
            font-family: "Lucida Sans Unicode";
            color: #1B90CB;          
      }
                        
    </style>

</head>

<body>

    <script type="text/javascript" src="htmlhelpers/js/knockout-2.2.0.js"></script>
    <script type="text/javascript" src="htmlhelpers/js/knockout-kendo.min.js"></script>
    <script type="text/javascript" src="htmlhelpers/js/highlight.pack.js"></script>
    <script type="text/javascript" src="htmlhelpers/js/app.js"></script>


      <div id="mainDiv">


        <table style="width:100%; height:700px">
           <tr>

        <td style="vertical-align: top;">
            <div id="headerDiv">
                 <table cellpadding="0" cellspacing="0" style="width:100%; margin:0px; padding:0px;">
        <tr>
            <td style="width:33%;">
                <p class="headerTitle">General</p>
                <div style="margin:0 0 0 5px;">
                <div>
                <span class="LabelTextStyle" >Shipment Number: </span>
                <span class="ValueTextStyle" data-bind="text: ShipmentNumber"></span>
                </div>
                <div>
                <span class="LabelTextStyle">House: </span>
                <span class="ValueTextStyle" data-bind="text: House"></span>
                </div>
                </div>
            </td>

            <td style="width:33%;">
                <p class="headerTitle">Routing</p>
                <div style="margin:0 0 0 5px;">
                <div>
                <span class="LabelTextStyle">Shipper: </span>
                <span class="ValueTextStyle" data-bind="text: Shipper"></span>
                </div>
                <div>
                <span class="LabelTextStyle">Status: </span>
                <span class="ValueTextStyle" data-bind="text: StatusName"></span>
                </div>
                </div>
            </td>

            <td style="width:33%;">
                <p class="headerTitle">Consignee</p>
                <div style="margin:0 0 0 5px;">
                <div class="ValueTextStyle">Ayman Khalaf</div>
                <div class="ValueTextStyle">12345</div>
                <div class="ValueTextStyle">Ramalla, Palestinian Territory</div>
                </div>
            </td>
        </tr>
    </table>
            </div>

             <div id="busyIndicator" style="display:none"><img src="http://www.frontiers.it/m/images/indicator.gif" alt='loading' /></div>


            <div id='tabControlDiv'>
                <div data-bind='kendoTabStrip: {}'>
            <ul id="tabsBar">
                <li class="k-state-active">Legs</li>
                <li class="tabItem">Partners</li>
                <li class="tabItem">Events</li>
            </ul>

            <div class="tabPage">
                <div id="one">
                    <div data-bind="kendoGrid: 
                        {
                            data: items,
                            scrollable: true,
                            selectable: true
                        }
                        "> 
                        </div>

                </div>
            </div>

            <div class="tabPage">tab two content</div>    
                     
            <div class="tabPage">              
                <div id="events"> </div>
            </div>
        </div>

                <script type='text/javascript'>

            $(function () {

                var events =
                [
                    { Code: "RTL", Name: "Created", CreatedBy: "Ayman khalaf", Picture: "images/ayman.jpg" },
                    { Code: "RTL", Name: "Created", CreatedBy: "Ayman khalaf", Picture: "images/ayman.jpg" },
                    { Code: "RTL", Name: "Created", CreatedBy: "Ayman khalaf", Picture: "images/ayman.jpg" },
                    { Code: "RTL", Name: "Created", CreatedBy: "Ayman khalaf", Picture: "images/ayman.jpg" },
                    { Code: "RTL", Name: "Created", CreatedBy: "Ayman khalaf", Picture: "images/ayman.jpg" },
                    { Code: "RTL", Name: "Created", CreatedBy: "Ayman khalaf", Picture: "images/ayman.jpg" },
                    { Code: "RTL", Name: "Created", CreatedBy: "Ayman khalaf", Picture: "images/ayman.jpg" }

                ];

                $("#events").kendoGrid({                    
                    
                    columns:
                        [
                            { title: "Code", field: "Code", width:"100px" },
                            { title: "Name", field: "Name" },
                            { title: "Created By", field: "CreatedBy" },
                            { title: "Picture", field: "Picture", width:100, template: "<img src='#=Picture#' />" },
                        ],

                    dataSource: {
                         data: events,
                        pageSize:4
                    },

                    height: 500,
                    scrollable: true,

                    pageable: true,

//                    pageable: {
//                        refresh: true,
//                        pageSizes: true
//                    },

//                    selectable: true
                });


                var ViewModel = function (result) {
                    
                    this.items = ko.observableArray();
                 
                    
                    for (ShipmentDataView in result)
                    {
                        this.items.push(ShipmentDataView);
                        }
                    
                    
                    
                };


                var ViewModel = function () {

                    this.items = ko.observableArray([
                                        { id: "1", name: "apple" },
                                        { id: "2", name: "orange" },
                                        { id: "3", name: "banana" },
                                        { id: "4", name: "banana" },
                                        { id: "5", name: "banana" },
                                        { id: "6", name: "banana" },
                                        { id: "7", name: "banana" },
                                        { id: "8", name: "banana" },
                                        { id: "9", name: "banana" },
                                        { id: "10", name: "banana" },
                                        { id: "11", name: "banana" },
                                        { id: "12", name: "banana" },
                                        { id: "13", name: "banana" },
                                        { id: "14", name: "banana" },
                                        { id: "15", name: "banana" },
                                        { id: "13", name: "banana" },
                                        { id: "14", name: "banana" },
                                        { id: "15", name: "banana" },
                                        { id: "16", name: "banana" }
                    ]);
                };

                ko.applyBindings(new ViewModel(), document.getElementById("tabControlDiv"));

                //$("#busyIndicator").show();

                //var url = "/api/shipments?tenant=1";

                
                //$.ajax({
                //    url: url,
                //    type: 'GET',

                //    contentType: 'application/json',
                //    success: function (result) {

                //        $("#busyIndicator").hide();
                        
                //        ko.applyBindings(new ViewModel(result), document.getElementById("tabControlDiv"));
                        
                //    },
                //    error: function (jqXHR, textStatus, errorThrown) {


                //        var errorMessage = '';
                //        $('#message').html(jqXHR.responseText);
                //    }
                //});

       
               
            });
            </script>
            </div>
        </td>

        <td style="width:5px"></td>

        <td style="width:280px">
            <table cellpadding="0" cellspacing="0" style="width:100%; height:700px; margin:0px; padding:0px;">
                <tr>
                    <td class="box">
                        <div class="headerDiv"><span>Cargo Information</span></div>  
                        <div id="cargoDiv">
                            <span data-bind="text: No"></span>
                        </div>                    
                    </td>
                </tr>

                <tr style="height:5px;"><td></td></tr>

                <tr>
                    <td class="box">
                        <div class="headerDiv"><span>Documents</span></div>
                    </td>
                </tr>

                <tr style="height:5px;"><td></td></tr>

                <tr>
                    <td class="box" style="height:150px;">
                        <div class="headerDiv"><span>Chat</span></div>
                    </td>
                </tr>
            </table>
        </td>

            </tr>   
        </table>


         
    </div>


    

      <script type="text/javascript">


           

          // Initialized the namespace
          var KnockoutDemoNamespace = {};

          // View model declaration
          KnockoutDemoNamespace.initViewModel = function (shipment) {
              var shipmentViewModel = {
                  
                  ShipmentNumber: ko.observable(shipment.ShipmentNumber),
                  House: ko.observable(shipment.House),
                  StatusName: ko.observable(shipment.StatusName),
                  Shipper: ko.observable(shipment.Shipper),
              };
               
              return shipmentViewModel;
          }

          // Bind the customer
          KnockoutDemoNamespace.bindData = function (shipment) {
              // Create the view model
              var viewModel = KnockoutDemoNamespace.initViewModel(shipment);

              ko.applyBindings(viewModel,document.getElementById("headerDiv"));
          }

          KnockoutDemoNamespace.getShipment = function (id, tenant) {

             
              $("#busyIndicator").show();
              var url1 = "/api/shipments/" + id + "/" + tenant;
              var url = "/api/shipments?id="+id+"&tenant="+tenant;
              var url2 = "/api/shipments/getsinglepmwithoutcomposition/" + id + "/" + tenant;
              $.ajax({
                  url: url,
                  type: 'GET',

                  contentType: 'application/json',
                  success: function (result) {
                      
                      $("#busyIndicator").hide();
                      KnockoutDemoNamespace.bindData(result);
                  },
                  error: function (jqXHR, textStatus, errorThrown) {
                      
                     
                      var errorMessage = '';
                      $('#message').html(jqXHR.responseText);
                  }
              });
          }




          $(document).ready(function () {
              KnockoutDemoNamespace.getShipment("1-3",1);

          });
</script>

   <%-- <script type='text/javascript'>
        $(function () {    
        
            //var shipmentPM = new ShipmentPM();
            //shipmentPM.Id = "1";
            //shipmentPM.ShipmentNumber = "SHI-RRRRR";
           
            //var CargoAreaViewModel = function (shipmentPM) {

            //    this.No = ko.observable(shipmentPM.ShipmentNumber);
            //};

            var CargoAreaViewModel = function () {

                this.No = ko.observable("ssss");
            };

            var HeaderViewModel = function (myRef, House, Routing, Status) {

                this.myRef = ko.observable(myRef);
                this.House = ko.observable(House);
                this.Routing = ko.observable(Routing);
                this.Status = ko.observable(Status);

                this.EnglishName = ko.observable("");
            };

            ko.applyBindings(new HeaderViewModel("88888888", "1119999", "TLV > JFK", "Order"), document.getElementById("headerDiv"));
            ko.applyBindings(new CargoAreaViewModel(), document.getElementById("cargoDiv"));
        });
    </script>--%>

  
    
</body>

</html>
