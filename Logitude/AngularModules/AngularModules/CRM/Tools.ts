import {TicketPM} from './EntityPMs/TicketPM';
import {FeatureLocator} from '../Infrastructure/Utilities/FeatureLocator';

export class CRMTool {
    public static IsTicketEditEnabled(entityPM: TicketPM) {
        var myResult = true;
        var closedButton = !FeatureLocator.HasFeaturePermession("Ticket", "SaveAsClosed") && !FeatureLocator.HasFeaturePermession("Ticket", "SaveAsResolved") && !FeatureLocator.HasFeaturePermession("Ticket", "SaveAsOpen");
        if (entityPM != null) {
            if (entityPM.IsClosed || entityPM.IsCancelled || closedButton) {
                myResult = false;
            }
        }
        return myResult;
    }
    public static GetDurationsList(): ActivtyDuration[] {
        var durations: ActivtyDuration[] = [];
        durations.push(new ActivtyDuration(1, "1 minute", false));
        durations.push(new ActivtyDuration(5, "5 minutes", false));
        durations.push(new ActivtyDuration(15, "15 minutes", false));
        durations.push(new ActivtyDuration(30, "30 minutes", false));
        durations.push(new ActivtyDuration(45, "45 minutes", false));
        durations.push(new ActivtyDuration(60, "1 hour", false));
        durations.push(new ActivtyDuration(90, "1.5 hours", false));
        durations.push(new ActivtyDuration(120, "2 hours", false));
        durations.push(new ActivtyDuration(150, "2.5 hours", false));
        durations.push(new ActivtyDuration(180, "3 hours", false));
        durations.push(new ActivtyDuration(210, "3.5 hours", false));
        durations.push(new ActivtyDuration(240, "4 hours", false));
        durations.push(new ActivtyDuration(270, "4.5 hours", false));
        durations.push(new ActivtyDuration(300, "5 hours", false));
        durations.push(new ActivtyDuration(330, "5.5 hours", false));
        durations.push(new ActivtyDuration(360, "6 hours", false));
        durations.push(new ActivtyDuration(390, "6.5 hours", false));
        durations.push(new ActivtyDuration(420, "7 hours", false));
        durations.push(new ActivtyDuration(450, "7.5 hours", false));
        durations.push(new ActivtyDuration(480, "8 hours", false));
        durations.push(new ActivtyDuration(1440, "1 day", true));
        durations.push(new ActivtyDuration(2880, "2 days", true));
        durations.push(new ActivtyDuration(4320, "3 days", true));
        return durations;
    }
    public static RoundTimeForwardByMinutes(dateTime: Date, minutes: number) {
        if (dateTime.getUTCMinutes() < 30) {
            dateTime.setUTCMinutes((minutes));
        }
        else if (dateTime.getUTCMinutes() > 30) {
            dateTime.setUTCMinutes(60);
        }
        return dateTime;
    }
    public static GetActivityImageSrc(code: string) {
        var ImageSrc = "";
        switch (code) {
            case "TS": {
                ImageSrc = "./Images/Buttons/TS.png";
                break;
            }
            case "VM": {
                ImageSrc = "./Images/Buttons/VM.png";
                break;
            }
            case "AP": {
                ImageSrc = "./Images/Buttons/AP.png";
                break;
            }
            case "CL": {
                ImageSrc = "./Images/Buttons/CL.png";
                break;
            }
            case "EI":
                {
                    ImageSrc = "./Images/Buttons/EI.png";
                    break;
                }
            case "EO":
                {
                    ImageSrc = "./Images/Buttons/EO.png";
                    break;
                }
            case "TX": {
                ImageSrc = "./Images/Buttons/TX.png";
                break;
            }
        }
        return ImageSrc;
    }
}

export class ActivtyDuration {
    constructor(minuts: number, name: string, isday: boolean) {
        this.Minuts = minuts;
        this.Name = name;
        this.IsDay = isday;
    }

    public Minuts: number;
    public Name: string;
    public IsDay: boolean;
}
