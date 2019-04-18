<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoogleMap.aspx.cs" Inherits="WebFreight.Web.WebPages.GoogleMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Google Map</title>
   
      <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    </style>
      <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false&key=AIzaSyD-P3L-FWMOzaeB-xT_u_WjTa8DmJDdeyw"></script>
     <script language="javascript" type="text/javascript">

         var map;
         var geocoder;
         function InitializeMap() {

//             var latlng = new google.maps.LatLng(32, 35);
             var myOptions =
        {
            zoom: 17,
//            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            disableDefaultUI: false
              
        };
             map = new google.maps.Map(document.getElementById("map"), myOptions);
         }

         function FindLocaiton() {
             geocoder = new google.maps.Geocoder();
             InitializeMap();

             var address = document.getElementById("addressinput").value;
             geocoder.geocode({ 'address': address }, function (results, status) {
                 if (status == google.maps.GeocoderStatus.OK) {
                     map.setCenter(results[0].geometry.location);
                     var marker = new google.maps.Marker({
                         map: map,
                         position: results[0].geometry.location
                     });

                 }
                 else {
                     alert("Could not find the location: " + address + "\n test");
                 }
             });

         }




         function Button1_onclick() {
             FindLocaiton();
         }

//         window.onload = InitializeMap;

</script>
</head>
<body>
    <form id="form1" runat="server" style="height:100%">
   
    <div id="silverlightControlHost">
  
<div id ="map" style="height:100%" >
  
        
</div>
 
      
       </div>
       <input ID="addressinput" runat="server"/>
     
    </form>
</body>
</html>

