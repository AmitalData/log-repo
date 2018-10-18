import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class ClientSearchByIDResponseData extends INF_MSG_GenericResponseData {
    public ExternalID: string;
    public DunsNumber: boolean;
    public MerkavahNumber: string;
    public IsActive: boolean;
    public LocalFirstName: string;
    public LocalLastName: string;
    public LocalCorporationName: string;
    public EnglishFirstName: string;
    public EnglishLastName: string;
    public EnglishCorporationName: string;

    public AddressContactPhoneList: Array<AddressContactPhone>;
    public AuthorizedList: Array<Authorized>;
    public AuthorizerList: Array<Authorized>;
    public CustomerActivityList: Array<CustomerActivity>;
    public ExportRequestList: Array<ExportRequest>;
    public IndicationPerClassificationList: Array<IndicationPerClassification>;
}

export class AddressContactPhone {

    public Address: string;
    public AddressPurposeID: string;
    public AddressPurposeName: string;
    public AddressTypeID: string;
    public AddressTypeName: string;

    public ContactPhoneList: Array<ContactPhone>;
}

export class ContactPhone {

    public CommunicationAddress: string;
    public CommunicationTypeID: string;
    public CommunicationTypeName: string;
}

export class Authorized {

    public AuthorizedName: string;
    public CustomerActivityTypeID: string;
    public CustomerActivityTypeName: string;
    public EndDate: string;
    public PoaAuthorizationTypeID: string;
    public PoaAuthorizationTypeName: string;
    public PoaID: string;
    public PoaStatus: string;
    public PoaStatusName: string;
    public StartDate: string;
}

export class CustomerActivity {

    public CustomerActivityTypeID: string;
    public CustomerActivityTypeName: string;
    public IsActive: boolean;
    public LogisticIdentification: string;
    public StartDate: string;

    public CustomerIndicationList: Array<CustomerIndication>;
}

export class CustomerIndication {

    public CustomerIndicationTypeID: string;
    public CustomerIndicationTypeName: string;
    public EndDate: string;
    public IsActive: boolean;
    public StartDate: string;
}

export class ExportRequest {

    public ApprovementEndDate: string;
    public ApprovementStartDate: string;
    public CreateDate: string;
    public CustomerRequestStatusID: string;
    public CustomerRequestStatusName: string;
    public OrganizationialUnitID: string;
    public RequestID: string;
    public RequestTypeID: string;
    public RequestTypeName: string;
    public StationName: string;
}

export class IndicationPerClassification {

    public ClassificationID: string;
    public EndDate: string;
    public GoodsItemDescription: string;
    public IndicationPerClassificationTypeID: string;
    public IndicationPerClassificationTypeName: string;
    public StartDate: string;
}