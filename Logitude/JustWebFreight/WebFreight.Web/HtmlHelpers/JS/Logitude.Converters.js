(function (jQuery) {
    jQuery.PadLeft = (function (myString, myCount, myChar) {
        if ($.trim(myCount) == "") {
            myCount = 0;
        }

        if ($.trim(myChar) == "") {
            myChar = "";
        }

        if ($.trim(myString) == "") {
            myString = "";
        }

        while (myString.length < myCount) {
            myString = myChar + myString;
        }

        return myString;
    }());

    jQuery.Convert = (function () {
        return {
            
            ToShortDate: function (input, TenantDateTimeFormat) {

                var Result = "";
                
               if ($.trim(input) != "") {

                   var myDate = $.format.date(input, "dd/MM/yyyy");
                   var todayDate = $.format.date(new Date(), "dd/MM/yyyy");
                   var tomorDate = $.format.date(new Date().setDate(new Date().getDate() + 1), "dd/MM/yyyy");
                   var yestrDate = $.format.date(new Date().setDate(new Date().getDate() - 1), "dd/MM/yyyy");

                   if (myDate == todayDate) {
                       Result = "Today";
                   }

                   else if (myDate == tomorDate) {
                       Result = "Tomorrow";
                   }

                   else if (myDate == yestrDate) {
                       Result = "Yesterday";
                   }

                   else {
                       if ($.trim(TenantDateTimeFormat) != "") {
                           var myDateFormat = null;

                           var myDateTimeFormatPrefix = TenantDateTimeFormat.toLowerCase().substring(0, 2);

                           if (myDateTimeFormatPrefix == "mm") {
                               myDateFormat = "MM/dd/yyyy";
                           }

                           else {
                               myDateFormat = "dd/MM/yyyy";
                           }

                           Result = $.format.date(input, myDateFormat);
                       }

                       else {                          

                           var dateTimeString = (input + "").replace('Z', '');

                           var splitBy = " ";

                           if (dateTimeString.indexOf("CET") !== -1) {
                               splitBy = "CET";
                           }

                           else if (dateTimeString.indexOf("T") !== -1) {
                               splitBy = "T";
                           }

                           var dateString = dateTimeString.split(splitBy)[0];
                           var dateParts = dateString.split('-');
                           var Year = +dateParts[0];
                           var Month = +dateParts[1];
                           var Day = +dateParts[2];

                           var dateObject = new Date();
                           dateObject.setUTCFullYear(Year);
                           dateObject.setUTCMonth((Month - 1));
                           dateObject.setUTCDate(Day);
                           dateObject.setUTCHours(0);
                           dateObject.setUTCMinutes(0);
                           dateObject.setUTCSeconds(0);
                           dateObject.setUTCMilliseconds(0);

                           Result = dateObject.toLocaleDateString();
                       }
                   }
                }

                return Result;
            },

            ToShortTime: function (value) {

                var Result = "";

                if ($.trim(value) != "") {

                    //Result = $.format.date(value, "hh:mm");
                    Result = $.format.date(value, "HH:mm");                    
                }

                return Result;
            },

            ToShortTime24: function (value) {

                var Result = "";

                if ($.trim(value) != "") {

                    Result = $.format.date(value, "HH:mm");
                }

                return Result;
            },

            ToColor: function (value) {

                var Result = "#45494A";

                var Status = $.trim(value);

                if (Status != "") {

                    switch (Status) {
                        case "Order":
                        case "Draft":
                        case "Waiting For Approval":
                        case "Approval Canceled":
                        case "Not Sent":
                        case "Partially Sent":
                            {
                                Result = "#282E30";
                                break;
                            }

                        case "Void":
                            {
                                Result = "#FF6E7172";
                                break;
                            }

                        case "Error":
                            {
                                Result = "#E53030";
                                break;
                            }

                        case "Accepted":
                            {
                                Result = "#009161";
                                break;
                            }

                        case "Departed":
                        case "Arrived":
                        case "Sent":
                            {
                                Result = "#27AAE1";
                                break;
                            }

                        case "Printed":
                        case "Pick Up":
                        case "On Hand":
                            {
                                Result = "#F37021";
                                break;
                            }

                        case "Paid":
                        case "Cleared":
                        case "Delivery":
                        case "Delivered":
                            {
                                Result = "#2BB673";
                                break;
                            }

                        case "Approved":
                        case "Unpaid":
                            {
                                Result = "#8DC63F";
                                break;
                            }

                        case "Auto Credit":
                        case "Auto Credited":
                            {
                                color = "Orange";
                                break;
                            }

                    }
                }

                return Result;
            },

            ToDateShortAge: function (value) {

                var Result = "";               
               
                if (value != null) {                    

                    var date1 = new Date();
                    var date2 = $.format.date(value);                    

                    var year1 = $.format.date(date1, "yyyy")
                    var year2 = $.format.date(date2, "yyyy")
                    var years = Math.abs(year1 - year2);

                    if (years > 0) {
                        Result = years + "y";
                    }

                    else {

                        var month1 = $.format.date(date1, "MM")
                        var month2 = $.format.date(date2, "MM")
                        var months = Math.abs(month1 - month2);

                        if (months > 0) {
                            Result = months + "m";
                        }

                        else {

                            var day1 = $.format.date(date1, "dd")
                            var day2 = $.format.date(date2, "dd")
                            var days = Math.abs(day1 - day2);

                            if (days > 0) {
                                Result = days + "d";
                            }

                            else {

                                var hour1 = $.format.date(date1, "HH")
                                var hour2 = $.format.date(date2, "HH")
                                var hours = Math.abs(hour1 - hour2);

                                if (hours > 0) {
                                    Result = hours + "h";
                                }

                                else {

                                    var minut1 = $.format.date(date1, "mm")
                                    var minut2 = $.format.date(date2, "mm")
                                    var minuts = Math.abs(minut1 - minut2);

                                    if (minuts > 0) {
                                        Result = minuts + "m";
                                    }

                                    else {
                                        Result = "now";
                                    }
                                }
                            }
                        }
                    }
                }

                return Result;
            },

            ToDateLongAge: function (value) {

                var Result = "";

                if (value != null) {                   

                    var date1 = new Date();
                    var date2 = $.format.date(value);

                    var year1 = $.format.date(date1, "yyyy")
                    var year2 = $.format.date(date2, "yyyy")
                    var years = Math.abs(year1 - year2);

                    if (years > 0) {

                        if (years == 1) {
                            Result = years + " year";
                        }

                        else {
                            Result = years + " years";
                        }
                    }

                    else {

                        var month1 = $.format.date(date1, "MM")
                        var month2 = $.format.date(date2, "MM")
                        var months = Math.abs(month1 - month2);

                        if (months > 0) {

                            if (months == 1) {
                                Result = months + " month";
                            }

                            else {
                                Result = months + " months";
                            }
                        }

                        else {

                            var day1 = $.format.date(date1, "dd")
                            var day2 = $.format.date(date2, "dd")
                            var days = Math.abs(day1 - day2);

                            if (days > 0) {

                                if (days == 1) {
                                    Result = days + " day";
                                }

                                else {
                                    Result = days + " days";
                                }
                            }

                            else {

                                var hour1 = $.format.date(date1, "HH")
                                var hour2 = $.format.date(date2, "HH")
                                var hours = Math.abs(hour1 - hour2);

                                if (hours > 0) {

                                    if (hours == 1) {
                                        Result = hours + " hour";
                                    }

                                    else {
                                        Result = hours + " hours";
                                    }
                                }

                                else {

                                    var minut1 = $.format.date(date1, "mm")
                                    var minut2 = $.format.date(date2, "mm")
                                    var minuts = Math.abs(minut1 - minut2);

                                    if (minuts > 0) {

                                        if (minuts == 1) {
                                            Result = minuts + " minut";
                                        }

                                        else {
                                            Result = minuts + " minuts";
                                        }                                        
                                    }

                                    else {
                                        Result = "just now";
                                    }
                                }
                            }
                        }
                    }
                }

                return Result;
            },

            ToShortMonthYear: function (value) {

                var Result = "";

                if ($.trim(value) != "") {

                    var myDate = $.format.date(value, "dd/MM/yyyy")
                    var todayDate = $.format.date(new Date(), "dd/MM/yyyy")
                    var tomorDate = $.format.date(new Date().setDate(new Date().getDate() + 1), "dd/MM/yyyy")
                    var yestrDate = $.format.date(new Date().setDate(new Date().getDate() - 1), "dd/MM/yyyy")

                    if (myDate == todayDate) {
                        Result = "Today";
                    }

                    else if (myDate == tomorDate) {
                        Result = "Tomorrow";
                    }

                    else if (myDate == yestrDate) {
                        Result = "Yesterday";
                    }

                    else {
                        Result = $.format.date(value, "MMM yyyy");
                    }
                }

                return Result;
            },

            ToFileExtentionImage: function (value) {

                var Result = "attachment-icon.png";

                if ($.trim(value) != "") {
                    switch (value.toUpperCase()) {
                        case "PDF":
                            {
                                Result = "File-pdf-48.png";
                                break;
                            }

                        case "TXT":
                            {
                                Result = "txt_48.png";
                                break;
                            }

                        case "XLS":
                        case "XLSX":
                            {
                                Result = "Microsoft-Office-Excel-48.png";
                                break;
                            }

                        case "DOC":
                        case "DOCX":
                            {
                                Result = "Microsoft-Office-Word-48.png";
                                break;
                            }

                        case "PPT":
                        case "PPTX":
                            {
                                Result = "Microsoft-Office-PowerPoint-48.png";
                                break;
                            }

                        case "ZIP":
                            {
                                Result = "Zip-icon.png";
                                break;
                            }

                        case "RAR":
                            {
                                Result = "RAR.png";
                                break;
                            }

                        case "XML":
                            {
                                Result = "Document-xml-48.png";
                                break;
                            }

                        case "PNG": {
                            Result = "Photo.png";
                            break;

                        }

                        default:
                            {
                                Result = "attachment-icon.png";
                                break;
                            }
                    }
                }

                return Result;
            },
        }
    }());
}(jQuery));