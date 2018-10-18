import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

export class MasavPaymentsToAgentResponseData extends INF_MSG_GenericResponseData {

    public AgentMasavPaymentResultList: Array<AgentMasavPaymentResult>;
}

export class AgentMasavPaymentResult {
    public PaymentProcess: string;
    public PaymentProcessName: string;
    public PaymentID: string;
    public Amount: string;
    public PaymentType: string;
    public PaymentTypeName: string;
    public ExternalID: string;
    public ExternalName: string;
    public CustomsUnit: string;
    public PaymentMethodAmount: string;
    public Bank: string;
    public Branch: string;
    public AccountNumber: string;
    public AgentAccountPosessionX: string;
    public AgentAccountPosessionV: string;
    public EntityID: number;
    public EntityIdExternalReferenceID: string;
    public BankCode: string;
    public AgentMasavPaymentResultHeader: string;
    public RelatedEntityList: Array<RelatedEntityResult>;
    public RelatedEntityListObs: ObservableCollection;
}

export class RelatedEntityResult {

    public EntityIdExternalReferenceID: string;
    public EntityIdKey1: string;
    public EntityIdKey2: string;
    public EntityIdKey3: string;
    public EntityType: string;
    public EntityTypeName: string;
    public EntityPath: string;
}
