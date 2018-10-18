import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

export class PaymentOrderReplyResponseData extends INF_MSG_GenericResponseData {

    public PaymentNumber: string;
    public PaymentOrderTotalSumToPay: number;
    public PaymentStatus: number;
    public PaymentStatusName: string;
    public PaymentOrderType: number;
    public PaymentOrderTypeName: string;
    public PaymentOrderPayDate: Date;
    public PaymentProcess: number;
    public PaymentProcessName: string;
    public CustomsHouse: number;
    public CustomsHouseName: string;
    public PaymentOrderReason: string;

    public PaymentDetailData: Array<PaymentDetailData>;
    public ConnectedEntityData: Array<ConnectedEntityData>;
    public TaxParagraphList: Array<TaxParagraphData>;
    public PaymentMethodsList: Array<PaymentMethodData>;
}

export class PaymentDetailData {

    public CustomerActivityType: number;
    public CustomerActivityTypeName: string;
    public ExternalID: number;
}

export class PaymentMethodData {
    public PaymentMethodType: number;
    public PaymentMethodTypeName: string;
    public Amount: number;
    public PaymentMethodStatus: number;
    public PaymentMethodStatusName: string;
}

export class ConnectedEntityData {
    public EntityType: number;
    public EntityTypeName: string;
    public EntityIdKey1: string;
    public EntityIdKey2: string;
    public EntityIdKey3: string;
}

export class TaxParagraphData {
    public ParagraphType: number;
    public ParagraphTypeName: string;
    public Amount: number;

}
