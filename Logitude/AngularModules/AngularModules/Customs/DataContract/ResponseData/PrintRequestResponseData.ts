import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class PrintRequestResponseData extends INF_MSG_GenericResponseData {

    public DeclarationPrintAnswer: Array<PrintRequestResultList> 
}

export class PrintRequestResultList {
    public SequenceNumber: number;
    public DeclarationNumber:string;
    public CustomFileNo:string;
    public ErrorText: string;
    public IsFiled: boolean;
}