import { InvoiceApiStatusList } from "Accounting/EntityLists/InvoiceApiStatusList";
import { InvoiceApiStepList } from "Accounting/EntityLists/InvoiceApiStepList";

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
            this.Status = statusList?.find((s: any) => s.StatusCode === InvoiceApiStatusEnum.Done)?.StatusName;
        } 
        else{
            if(stepList.Code === entity.Step)
                this.Status = statusList?.find((s: any) => s.StatusCode === entity.StatusCode)?.StatusName;
            else
                this.Status = statusList?.find((s: any) => s.StatusCode === InvoiceApiStatusEnum.Pending)?.StatusName;
        }      
        this.AllowedResend = stepList.IsAllowResend && stepList.Code === entity.Step && entity.StatusCode === InvoiceApiStatusEnum.Failed;
        this.AllowedViewXml =  Number(entity.Step) >= Number(InvoiceApiStepEnum.GetInvoiceApiInvoicesList)  && Number(stepList.Code) >= Number(InvoiceApiStepEnum.GetInvoiceApiInvoicesList);
    }
  


   




}


export enum InvoiceApiStatusEnum {
    Created = "1",
    Pending = "2",
    InProgress = "3",
    Done = "4",
    Failed = "5"
}
export enum InvoiceApiStepEnum {
    OpenInvoiceApiSession = "1",
    CloseInvoiceApiSession = "2",
    GetInvoiceApiInvoicesList = "3",
    GetInvoiceApiInvoice = "4",
    GenerateInvoice = "5",
    GetConfirmationNumber = "6",
    ApproveInvoice = "7",
    PrintOrSendInvoice = "8"
}