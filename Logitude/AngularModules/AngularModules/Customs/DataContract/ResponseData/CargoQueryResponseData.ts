import { ResponseDataBase } from './ResponseDataBase';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';


export class CargoQueryResponseData extends ResponseDataBase {

    public ApplicationID: string;
    public ResponseStatusXML: string;
    public IsShowUserMessage: boolean //Yuval Chalup 07.03.2016 TASK-20176

    public ManifestType: string;
    public ManifestTypeName: string;
    public ManifestStatus: string;
    public ManifestStatusName: string;
    public Manifestnumber: string;
    public IsSendingMorethenOneFlights: boolean;
    public CargoResultList: CargoResult;
    public DeliveryOrderResultList: DeliveryOrderResult[];
    public CargosVersionResultList: CargosVersionResult[];
    public GatepassResultList: GatepassResult;
    public CargoItemResultList: CargoItemResult[];
    public CargoDocumentResultList: CargoDocumentResult[];
    public CargoErrorResultList: CargoErrorResult[];

}
export class CargoResult {
    public CargoIdentifierKey1: string;
    public CargoIdentifierKey2: string;
    public ParentCargoID: string;
    public MasterBolNumber: string;
    public BillOfLadingNumber: string;
    public GovernmentProcedureType: string;
    public GovernmentProcedureTypeName: string;
    public TreatmentWayCode: string;
    public TreatmentWayName: string;
    public TotalNumberOfPackeges: string;
    public TotalWeight: string;
    public CargoAdditionalDataList: CargoAdditionalData;
}

export class CargoAdditionalData {
    public Version: string;
    public ManifestNumber: string;
    public UnloadingLocationID: string;
    public UnloadingLocationName: string;
    public GoodsReceiptPlaceSiteID: string;
    public GoodsReceiptPlaceSiteName: string;
    public BoardedQuantity: string;
    public TotalRecordNumberOfPackeges: string;
    public TotalRecordWeight: string;
    public TransitDestinationLocationID: string;
    public TransitDestinationLocationName: string;
    public AcceptedArrivalSiteID: string;
    public StorageSiteID: string;
    public StorageSiteName: string;
    public StorageDate: string;
}

export class DeliveryOrderResult {
    public DeliveryOrderNumber: string;
    public ProducerName: string;
    public ReceiverCustomerActivityType: string;
    public ReceiverCustomerActivityTypeName: string;
    public ReceiverName: string;
    public DeliveryOrderDate: string;
    public DeliveryOrderStatus: string;
    public DeliveryOrderStatusName: string;
    public DeliverySiteId: string;
    public DeliverySiteName: string;
    public ResponseStatus: string;
}

export class CargosVersionResult {
    public Version: string;
    public SubmiterName: string;
    public CreateDate: string;
    public ActionDate: string;
    public CargoStatus: string;
    public CargoStausName: string;
}

export class GatepassResult {
    public GatepassNumber: string;
    public UpdateCode: string;
    public UpdateName: string;
    public SubmiterName: string;
    public SourceSiteCode: string;
    public SourceSiteName: string;
    public DestinationSiteCode: string;
    public DestinationSiteName: string;
    public RequestDate: string;
    public ConfirmDate: string;
    public GatepassStatus: string;
    public GatepassStatusName: string;
    public IsGatepassImplemented: boolean;
}

export class CargoItemResult {
    public RowNumber: string;
    public ParentCargoRowDetailsID: string;
    public ContainerNumber: string;
    public CharacteristicCode: string;
    public ContainerType: string;
    public Length: string;
    public PackingType: string;
    public Quantity: string;
    public GrossMassMeasureWeight: string;
    public RecordNumberOfPackeges: string;
    public TotalRecordWeight: string;
    public DangerousGoodsIndication: string;
    public SealDetailsList: SealDetails[];
    public SealDetailsObservableCollection: ObservableCollection;
    public CargoMovmentList: CargoMovment[];
    public CargoMovmentObservableCollection: ObservableCollection;
}


export class SealDetails {
    public RowNumber: string;
    public SealType: string;
    public SealTypeName: string;
    public SealNumber: string;
}

export class CargoMovment {
    public RowNumber: string;
    public ExitReasonID: string;
    public ExitReasonName: string;
    public StatusID: string;
    public StatusName: string;
    public DocumentNumber: string;
    public ReferenceTypeID: string;
    public ReferenceTypeName: string;
    public ReferenceNum: string;
    public ExitSiteId: string;
    public ExitSiteName: string;
    public ExitDateTime: string;
    public EntrySiteId: string;
    public EntrySiteName: string;
    public EntryDateTime: string;
}

export class CargoDocumentResult {
    public ExternalIDNum: string;
    public TypeName: string;
    public RequiredDate: string;
    public AcceptedDate: string;
    public DocumentID: string;
}


export class CargoErrorResult {
    public errorDate: string;
    public errorCode: string;
    public errorSource: string;
    public errorSourceName: string;
    public errorText: string;
}