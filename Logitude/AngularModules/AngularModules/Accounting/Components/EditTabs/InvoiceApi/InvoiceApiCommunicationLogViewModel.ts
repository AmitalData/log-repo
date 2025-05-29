import { InvoiceApiStatusList } from "Accounting/EntityLists/InvoiceApiStatusList";
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

    public constructor(stepList:InvoiceApiStepList, entity: any,statusList: InvoiceApiStatusList[])  {

        this.StepList = stepList;
        this.Step = stepList.Code;
        this.Name = stepList.EnglishName;
        this.LocalName = stepList.LocalName;
        if(stepList.Code < entity.Step){ 
            this.Status = statusList?.find((s: any) => s.StatusCode === "4")?.StatusName;
        } 
        else{
            if(stepList.Code === entity.Step)
                this.Status = statusList?.find((s: any) => s.StatusCode === entity.StatusCode)?.StatusName;
            else
                this.Status = statusList?.find((s: any) => s.StatusCode === "2")?.StatusName;
        }      
        this.AllowedResend = stepList.IsAllowResend && stepList.Code === entity.Step && entity.StatusCode === "5" ;
        this.AllowedViewXml =  entity.Step >= "3"  && stepList.Code >="3" 
    }
  


   




}