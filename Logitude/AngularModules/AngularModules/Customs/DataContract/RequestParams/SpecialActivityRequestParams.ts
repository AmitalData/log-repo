import {GenericRequestParams} from './GenericRequestParams';

export class SpecialActivityRequestParams extends GenericRequestParams {

    public GeneralDetailsData: GeneralDetails;
    public GoodsDetailsData: GoodsDetails;
    public RePackingApprovalDetailsData: RePackingApprovalDetails;
    public SampleRequestDetailsDataList: Array<SampleRequestDetails>;
    public CurrentPackingDetailsDataList: Array<CurrentPackingDetails>;
    public DesiredPackingDetailsDataList: Array<DesiredPackingDetails>;
    public OtherActivityDetailsData: OtherActivityDetails;
}

export class GeneralDetails {
    public ActivityRequestStartDate: Date;
    public ActivityRequestStartTime: Date;
    public ActivityRequestEndDate: Date;
    public ActivityRequestEndTime: Date;
    public ApplicantAgentNumber: number;
    public AuthorityCode: string;
    public AuthorityCodeSpecified: boolean;
    public CargoRowNumber: string;
    public CargoRowNumberSpecified: boolean;
    public CheckSite: string;
    public ClientFullName: string;
    public ContainerNumber: string;
    public ImporterName: string;
    public ImporterNumber: string;
    public ImporterNumberSpecified: boolean;
    public IsContainer: boolean;
    public IsContainerSpecified: boolean;
    public SealNumber: string;
    public SiteNumber: string;
    public SpecialActivityRequestNumber: string;
    public SpecialActivityType: number;
    public WarehouseBlockNumber: number;
    public CargoIdentifier: CargoIdentifier;
    public SpecialActivityTypeEssence: string;
    public CustomFileNo: string;
    public DeclarationId: string;
}

export class CargoIdentifier {

    public CargoIdentifierKey1: string;
    public CargoIdentifierKey2: string;
    public CargoIdentifierKey3: string;
    public CargoIdentifierType: number;
}

export class GoodsDetails {

    public GoodsDescription: string;
    public IdemanderType: number;
    public IdemanderTypeSpecified: boolean;
    public OtherDescription: string;
    public SpecialActionsCode: number;
    public SpecialActionsCodeSpecified: boolean;
    public RepresentativeList: Array<RepresentativeDetails>;;
}

export class RepresentativeDetails {

    public RepresentativeID: string;
    public RepresentativeIDSpecified: boolean;
    public RepresentativeName: string;
    public RepresentativeNumber: number;
    public RepresentativeNumberSpecified: boolean;
}

export class SampleRequestDetails {

    public CurrencyTypeCode: string;
    public CurrencyTypeName: string;
    public CustomsItem: string;
    public CustomsItemQuantity: number;
    public CustomsItemQuantitySpecified: boolean;
    public SampleDescription: string;
    public SampleReturnDate: Date;
    public SampleRowNumber: number;
    public SampleRowNumberSpecified: boolean;
    public SampleValue: number;
    public SamplePackingDetails: PackingDetails;
}

export class RePackingApprovalDetails {

    public ApprovalDate: Date;
    public ApprovalDateSpecified: boolean;
    public ApprovalName: string;
    public SiteNumber: string;
}

export class CurrentPackingDetails {

    public PresentPackingStateContent: string;
    public RePackingOldLineNumber: number;
    public RePackingOldLineNumberSpecified: boolean;
    public PackingDetails: PackingDetails;
}

export class DesiredPackingDetails {

    public RePackingNewLineNumber: number;
    public RePackingNewLineNumberSpecified: boolean;
    public RePackingOldLineNumber: number;
    public RePackingOldLineNumberSpecified: boolean;
    public PackingDetails: PackingDetails;
}

export class PackingDetails {

    public PackageId: string;
    public PackageType: string;
    public PackageTypeName: string;
    public Quantity: string;
    public Weight: number;
    public WeightSpecified: boolean;
}

export class OtherActivityDetails {

    public OtherActivityComment: string;
}
