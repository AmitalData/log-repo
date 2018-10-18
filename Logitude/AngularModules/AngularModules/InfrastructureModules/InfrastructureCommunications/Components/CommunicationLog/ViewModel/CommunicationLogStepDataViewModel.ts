import {CommunicationLogStepList} from '../../../../../Common/EntityLists/CommunicationLogStepList';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';

export class CommunicationLogStepDataViewModel {
    CommunicationLogId: string;
    StepNumber: number;
    Tenant: number;
    Retries: number;
    Status: string;
    Log: string;
    StartDate: Date;
    EndDate: Date;
    DocumentId: string;
    StatusName: string;
    Duration: string;
    Name: string;
    public DivSelectBackgroud: string;

    StepList: CommunicationLogStepList

    public constructor(stepList: CommunicationLogStepList) {

        this.StepList = stepList;
        this.StepNumber = stepList.StepNumber;
        this.Name = stepList.Name;

        this.CommunicationLogId = stepList.CommunicationLogId;

        this.Tenant = stepList.Tenant;
        this.Retries = stepList.Retries;
 


        if (AppTool.IsNullOrEmpty( stepList.StatusName)) {
            this.Status =  this.GetCommunicationStatusTypesStatusName(stepList);
        }


        this.Log = stepList.Log;
        this.StartDate = stepList.StartDate;
        this.EndDate = stepList.EndDate;


     
        
        var t = new Date(stepList.EndDate).getTime() - new Date(stepList.StartDate).getTime();
        t = t / 1000;

        let n: number = parseFloat(t.toString());
        n = Math.round(n * 100) / 100;
        this.Duration = //t.toPrecision(2);
            parseFloat(n.toString()).toFixed(2);

        this.DocumentId = stepList.DocumentId;
        this.StatusName = stepList.StatusName;
  
    }



    GetCommunicationStatusTypesStatusName(step: CommunicationLogStepList) {

        switch (step.Status) {
            case "W":
                return "Waiting";
            case "D":
                return "Done";
            case "F":
                return "Fail";
            case "P":
                return "I.Progress";
            default:
                break;
        }
        return "";
    }




}