import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';

export class ExportDeclarationDataResponseData extends INF_MSG_GenericResponseData {

    //public ResponseStatusXML: string;
    public DeclarationID: string;
    public ReshimonNumber: string;
    public Title: string;
    public LoadingDate: string;
    public CalculationDate: string;
    public AgentCustomerExternalID: string;
    public DeclarationNumber: string;
    public FOBNetoNISAmount: string;
    public FOBNISAmount: string;
    public InvoiceList: Array<Invoice>;
    public RequestList: Array<Request>;
}

export class Invoice {

    public SequenceNumber: string;
    public ExternalID: string;
    public InvoiceAmount: string;
    public InvoiceCurrency: string;
    public InvoiceAmountCurrency: string;
}

export class Request {

    public SequenceNumber: string;
    public CustomsItem: string;
    public ValueQuantity: string;
    public ForeignAmount: string;
    public ForeignCurrency: string;
    public ForeignCurrencyAmount: string;
    public OriginCountry: string;
    public OriginCountryName: string;
    public GovernmentProcedureList: Array<GovernmentProcedure>;
    public VehicleList: Array<Vehicle>;
}

export class GovernmentProcedure {

    public ItemGovernmentProcedureType: string;
    public ItemGovernmentProcedureName: string;
}

export class Vehicle {

    public CargoIdentityQualifierID: string;
    public RichbitNumber: string;
    public VehicleExternalIDNum: string;
}