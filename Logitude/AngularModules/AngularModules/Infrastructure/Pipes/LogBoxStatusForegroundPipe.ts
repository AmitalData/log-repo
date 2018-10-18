import {Pipe} from '@angular/core';
import {AppTool} from '../Tools';

@Pipe({ name: 'LogBoxStatusForegroundPipe' })

export class LogBoxStatusForegroundPipe {

    transform(value: string, Parameter: string = null): string {

        var myResult: string = "#282E30";
        if (Parameter == 'CA') {
            if (!AppTool.IsNullOrEmpty(value)) { 
                myResult = this.ApplyCAPipe(value);
            }
        }
        else {
            if (!AppTool.IsNullOrEmpty(value)) {

                myResult = this.ApplyPipe(value);
            }
        }

        return myResult;
    }

    private ApplyPipe(value) {
        var color: string = "#282E30";

        switch (value) {
            case "Draft":
                {
                    color = "Orange";
                    break;
                }

            case "Created":
            case "Approval Canceled":
            case "Not Sent":
            case "Partially Sent":
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
            case "In Progress":
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
                {
                    color = "Red";
                    break;
                }

            case "Waiting":
            case "Waiting For Approval":
                {
                    color = "Orange";
                    break;
                }

            //case "Accepted":
            //    {
            //        color = "Green";
            //        break;
            //    }
        }

        return color;
    }

    private ApplyCAPipe(value) {
        var color: string = "#282E30";

        switch (value) {
            case "Waiting":
            case "In Progress":
                {
                    color = "Orange";
                    break;
                }
            case "InActive": 
                {
                    // Red
                    color = "#E53030";
                    break;
                }
                 
            case "Approved":
                {
                    // Green
                    color = "#009161";
                    break;
                } 
        }

        return color;
    }
}