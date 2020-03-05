import {GenericRequestParams} from './GenericRequestParams';

export class SendUnCorrectDocumentsRequestParams extends GenericRequestParams {

    public CourierMasterId: string;
    public HAWB: string;
    public Declarations: string[];

    public SelectedBOLValue: string;
    public SelectedStatusValue: string;
    public SelectedAvailableValue: string;
    public SelectedTotalInvoiceValue: string;
    public SelectedFastIndividualProcessValue: string;
    public SelectedCustomStatusValue: string;
    public IsCreateNewDocumentVersion: boolean;
}
