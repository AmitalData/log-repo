import {Pipe} from '@angular/core';
import {AppTool, FormatTool, FontTool, DateTool} from '../Tools';

@Pipe({ name: 'StringToColorPipe' })

export class StringToColorPipe {

    transform(input: any, Parameter: string = null, Direction:string=null): string {

        var myResult: string = "#282E30";

        if (!AppTool.IsNullOrEmpty(input)) {

            var value = input + "";
             if (!AppTool.IsNullOrEmpty(Parameter)) {
                myResult = this.ApplyParameterPipe(value, Parameter,Direction);                
            }

            else {
                myResult = this.ApplyPipe(value);
            }
        }

        return myResult;
    }

    private ApplyPipe(value: string) {
        var color: string = "#282E30";

        switch (value) {
            case "Draft":
                {
                    color = "Orange";
                    break;
                }

            case "Created":
            case "Waiting For Approval":
            case "Approval Canceled":
            case "Not Sent":
            case "Partially Sent":
            case "Not Transferred":
                {
                    // Black
                    color = "#282E30";
                    break;
                }

            case "Void":
                {
                    color = "#6E7172";
                    break;
                }

            case "Booking Request Rejected":
            case "Cancellation Request Rejected":
            case "Declined":
            case "Error":
                {
                    // Red
                    color = "#E53030";
                    break;
                }

            case "Air waybill":
            case "Received by Airline":
            case "Waiting for Confirmation":
            case "Accepted":
            case "Hybriding":
            case "Booking Request":
            case "Used":
            case "Entered":
                {
                    // Green
                    color = "#009161";
                    break;
                }

            case "Confirmed":
            case "Booking Confirmed":
            case "Departed":
            case "Arrived":
            case "Sent":
            case "Transferred":
            case "In Progress":
            case "Arrival_referant":

                {
                    color = "#27AAE1";
                    break;
                }
            case "Arrival_referant":

                {
                    color = "#27AAE1";
                    break;
                }
            case "Printed":
            case "Pick Up":
            case "On Hand":
                {
                    color = "#F37021";
                    break;
                }

            case "Paid":
            case "Cleared":
            case "Delivery":
            case "Delivered":
                {
                    color = "#2BB673";
                    break;
                }

            case "Approved":
            case "Unpaid":
            case "Approved By Customer":
                {
                    color = "#8DC63F";
                    break;
                }

            case "Auto Credit":
            case "Auto Credited":
            case "Sent To Customer":
                {
                    color = "Orange";
                    break;
                }

            case "Viewed":
                {
                    color = "#00D377";
                    break;
                }

            case "Not Connected":
                {
                    color = "Gray";
                    break;
                }

            case "Connected":
                {
                    color = "Blue";
                    break;
                }

            case "InActive":
            case "Customs Delay":
            case "Transferred With Errors":
                {
                    color = "Red";
                    break;
                }

            case "Waiting":
                {
                    color = "Orange";
                    break;
                }
            case "Past": {
                color = "red";
                break;
            }
            case "Present": {
                color = "Green";
                break;
            }
            case "Future": {
                color = "Magenta";
                break;

            }


            case "Potential":{
                color = "orange";
                break;
            }

            case "Active":{
                color = "green";
                break;
            }

            case "Waiting for Activation": {
                color = "red";
                break;

            }

            case "Inactive": {
                color = "pink";
                break;
            }

        }

        return color;
    }
    private ApplyParameterPipe(value: string, Parameter: string ,Direction: string) {
        var myResult: string = "#282E30";
         if (Parameter == "CustomerStatusCode") {
            myResult = this.ApplyCustomerStatusCodePipe(value);
        }

        if (Parameter == "ArrivalDate") {
            myResult = this.ApplyArrivalDatePipe(value);
        }

        else if (Parameter == "TicketSeverityCode") {
            myResult = this.ApplyTicketSeverityCodePipe(value);
         }

        else if (Parameter == "QuoteRatingCode") {
            myResult = this.ApplyQuoteRatingCodePipe(value);
        }

        else if (Parameter == "ActivityPriorityCode") {
            myResult = this.ActivityPriorityCodePipe(value);
        }

        else if (Parameter == "OpportunityRatingCode") {
            myResult = this.OpportunityRatingCodePipe(value);
        }

        else if (Parameter == "OpportunityStage") {
            myResult = this.OpportunityStagePipe(value);
        }

        else if (Parameter == "RatingCodeToImage") {
            myResult = this.RatingCodeToImage(value);
        }

        else if (Parameter == "UpdateDateOpportunity") {
            myResult = this.UpdateDateOpportunity(value);
        }

        else if (Parameter == "Number" || Parameter == "number") {
            myResult = this.ApplyNumberPipe(value);
        }  
        else if (Parameter == "ActivityDueDate"){
            myResult = this.ActivityDueDatePipe(value);
        }

        else if (Parameter == "DeclarationStatusTypeCode") {
            myResult = this.ApplyDeclarationStatusCodePipe(value);
        }
            
        else if (Parameter == "DeclarationAmendmentStatus")
        {
            myResult = this.ApplyDeclarationAmendmentStatusPipe(value,Direction);
            
        }

        else if (Parameter == "CustomsTransmissionsStatus") {
            myResult = this.ApplyCustomsTransmissionsStatusPipe(value);
        }

        else if (Parameter == "CourierCustomStatusCode") {
            myResult = this.ApplyCourierCustomStatusPipe(value);
        }

        else {
            
            switch (Parameter + ":" + value) {
                case "MessagingStock:New":
                case "MessagingStock:Active":
                    {
                        myResult = "#009161";
                        break;
                    }
            }
        }

        

        return myResult;
    }


    private ApplyArrivalDatePipe(value: string) {
        var myResult: string = "Blue";
        if (value != 'ETA')
            myResult = "Black";
        return myResult;
    }
 
    private ApplyCustomerStatusCodePipe(value: string) {
        var myResult: string = "#282E30";

        switch (value) {
            case "POT":
                {
                    myResult = "#E36C0A";
                    break;
                }

            case "ACT":
                {
                    myResult = "#00B076";
                    break;
                }

            case "WAC":
                {
                    myResult = "#FF0000";
                    break;
                }

            case "INA":
                {
                    myResult = "#F40CB2";
                    break;
                }
        }

        return myResult;
    }
    private ApplyDeclarationAmendmentStatusPipe(value: string,direction:string) {
        var myResult: string = "";

        if(direction != "E")
        {

            switch (value) {
                case "3":
                case "6":

                    {
                        myResult = "#009161"; // green

                        break;
                    }
                case "1":
                case "2":
                    {
                        myResult = "#F37021"; // orange
                        break;
                    }
                case "4":
                case "5":

                    {
                        myResult = "#E53030"; // red
                        break;
                    }

            }
        }
        else{
            switch (value) {
                case "1":
                case "2":
                case "11":    

                    {
                        myResult = "#009161"; // green

                        break;
                    }
                case "6":
                case "7":
                case "8":
                case "10":
                    {
                        myResult = "#F37021"; // orange
                        break;
                    }
                case "4":
                case "5":

                    {
                        myResult = "#E53030"; // red
                        break;
                    }

            }
        }

        return myResult;
    }

    private ApplyDeclarationStatusCodePipe(value: string) {
        var myResult: string = "";

        switch (value) {
            case "3":
            case "7":
            case "8":
            case "13":
            case "43":
            case "44":
            {
                myResult = "#009161"; // green
                break;
            }
            case "4":
            case "5":
            case "6":
            case "11":
            case "15":
            case "21":
            case "22":
            case "23":
            case "24":
            case "25":
            case "26":
            case "27":
            case "28":
            case "29":
            case "30":
            case "31":
            case "32":
            case "33":
            case "34":
            case "35":
            case "40":
            case "41":
            case "42":
            case "45":
            case "47":
            case "48":
            case "49":
            case "50":    
            {
                myResult = "#F37021"; // orange
                break;
            }
            case "0":
            case "1":
            case "2":
            case "9":
            case "12":
            case "14":
            case "16":
            case "17":
            case "18":
            case "19":
            case "20":
            case "37":
            {
                myResult = "#E53030"; // red
                break;
            }

        }

        return myResult;
    }



    private ApplyTicketSeverityCodePipe(value: string) {
        var myResult: string = "#282E30";

        switch (value) {
            case "UR":
                {
                    myResult = "Red";
                    break;
                }

            case "HI":
                {
                    myResult = "Orange";
                    break;
                }

            case "MD":
                {
                    myResult = "Gray";
                    break;
                }

            case "LW":
                {
                    myResult = "Blue";
                    break;
                }
        }

        return myResult;
    }
    private ApplyQuoteRatingCodePipe(value: string) {
        var myResult: string = "#282E30";

        switch (value) {
            case "C":
                {
                    myResult = "#2772D0";
                    break;
                }

            case "H":
                {
                    myResult = "#CE1111";
                    break;
                }

            case "N":
                {
                    myResult = "Gray";
                    break;
                }

            case "W":
                {
                    myResult = "#FF893B";
                    break;
                }
        }

        return myResult;
    }
    private ApplyNumberPipe(value: string) {
        var myResult: string = "#282E30";

        if (!AppTool.IsNullOrEmpty(value)) {
            var myValue: number = 0;

            if (typeof (value) == "string") {
                value = value.replace(',', '');

                if (FormatTool.IsDecimal(value)) {
                    myValue = +value;
                }
            }

            if (myValue < 0) {
                myResult = FontTool.Red;
            }

            else if (myValue > 0) {
                myResult = FontTool.Green;
            }
        }

        return myResult;
    }
    private ActivityPriorityCodePipe(value: string) {
        var myResult: string = "#282E30";

        switch (value) {
            case "01":
                {
                    //Low
                    myResult = "#2772D0";
                    break;
                }

            case "02":
                {
                    //Normal
                    myResult = "#D1D1D1";
                    break;
                }

            case "03":
                {
                    //High
                    myResult = "#CE1111";
                    break;
                }
        }

        return myResult;
    }
    private OpportunityRatingCodePipe(value: string) {
        var myResult: string = "#282E30";

        switch (value) {
            case "H":
                {
                    myResult = "#CE1111";
                    break;
                }

            case "C":
                {
                    myResult = "#2772D0";
                    break;
                }

            case "W":
                {
                    myResult = "#FF893B";
                    break;
                }

            case "N":
                {
                    myResult = "#D1D1D1";
                    break;
                }
        }

        return myResult;
    }

    private OpportunityStagePipe(value: string) {
        var myResult: string = "#E483FB";

        if (value == "Qualification") {
            myResult = "#5F3255";
        }

        else if (value == "Approved") {
            myResult = "#574600";
        }

        else if (value == "Development") {
            myResult = "#505AD2";
        }

        else if (value == "Quote") {
            myResult = "#E400FF";
        }

        else if (value == "Closed Won") {
            myResult = "#49AE2C";
        }

        else if (value == "Closed Lost") {
            myResult = "#FF0000";
        }

        else if (value == "Draft") {
            myResult = "Orange";
        }

        return myResult;
    }
    private UpdateDateOpportunity(value: string) {
        var myResult: string = "Red";

        if (value == "Today") {
            myResult = "Green";
        }
      

        return myResult;


    }
    private RatingCodeToImage(value: string) {
        var myResult: string = "#E483FB";

        if (value == "Hot") {
            myResult = "red";
        }

        else if (value == "Cold") {
            myResult = "blue";
        }

        else if (value == "Warm") {
            myResult = "Orange";
        }

        else if (value == "Neutral") {
            myResult = "lightgray";
        }

        return myResult;


    } 

    private ActivityDueDatePipe(value: any) {
        var myresult = "rgb(110,113,114)";
        if (value != null) {
            if (DateTool.GetDateParts(value).DateObject < DateTool.GetCurrentDateAsUtc()) {
                myresult = "red";
            }
        }
        return myresult;
    }

    private ApplyCustomsTransmissionsStatusPipe(value: string) {
        var myResult: string = "black";

        switch (value) {
            case "SENT":
            case "ACPT":
                {
                    myResult = "Green";
                    break;
                }

            case "EROR":
                {
                    myResult = "Red";
                    break;
                }

            case "NSEN":
                {
                    myResult = "Orange";
                    break;
                }
        }

        return myResult;
    }

    private ApplyCourierCustomStatusPipe(value: string) {

        var myResult: string = "#282E30";

        switch (value) {
            case "1": //Hatara
                {
                    myResult = "#00B076";
                    break;
                }

            case "2": //Suspended
                {
                    myResult = "#FF0000";
                    break;
                }

        }

        return myResult;
    }

}
