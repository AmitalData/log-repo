import {GenericRequestParams} from './GenericRequestParams';

export class SendALLCorrectRequestParams extends GenericRequestParams {

    public CourierMasterId: string;
    public HAWB: string;
    public CourierDeclarationStatusCode: string;
    public Declarations: string[];

    public SelectedBOLValue: string;
    public SelectedStatusValue: string;
    public SelectedAvailableValue: string;
    public SelectedTotalInvoiceValue: string;

}
