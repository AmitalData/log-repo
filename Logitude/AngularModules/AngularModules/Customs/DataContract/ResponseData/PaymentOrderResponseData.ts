import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

export class PaymentOrderResponseData extends INF_MSG_GenericResponseData {

    public PaymentsDetailsList: Array<PaymentsDetailsResult>;
}

export class PaymentsDetailsResult {

    public PaymentID: string;
    public PaymentType: string;
    public PaymentAmount: string;
    public Importer: string;
    public Agent: string;
    public PaymentMethodType: string;
    public PaymentStatus: string;
    public EntityExternalID: string;
    public EntityType: string;
}