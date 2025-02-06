import {GenericRequestParams} from './GenericRequestParams';

export class SendRecoverDecRequestParams extends GenericRequestParams {

    public CourierMasterId: string;
    public HAWB: string;
    public CourierDeclarationStatusCode: string;
    public Declarations: string[];
    public IsWorkSheetFromExcel:boolean;
    
}
