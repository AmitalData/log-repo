import { InvoiceApiStepList } from "Accounting/EntityLists/InvoiceApiStepList";
import { set } from "cypress/types/lodash";

export class InvoiceApiCommunicationLogViewModel {
    Step: string;
    Status: string;
    Code: string;
    LocalName: string;   
    Name: string;
    AllowedResend: boolean;
    AllowedViewXml: boolean;
    StepList: InvoiceApiStepList

    public constructor(stepList:InvoiceApiStepList, entity: any)  {

        this.StepList = stepList;
        this.Step = stepList.Code;
        this.Name = stepList.EnglishName;
        this.LocalName = stepList.LocalName;
        if(stepList.Code < entity.StepNumber){ 
            this.Status = "4";
        } 
        else{
            if(stepList.Code === entity.Step)
                this.Status = entity.StatusCode;
            else
                this.Status = "2";
        }      
        this.AllowedResend = stepList.IsAllowResend && stepList.Code === entity.Step && stepList.Code === "5";
        this.AllowedViewXml =  stepList.Code === "4" &&  (entity.Step === "4" && entity.Status === "4"|| entity.Step > "4");
    }



   




}