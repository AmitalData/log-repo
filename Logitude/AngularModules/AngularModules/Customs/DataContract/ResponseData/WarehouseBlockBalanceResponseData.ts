import { INF_MSG_GenericResponseData } from './INF_MSG_GenericResponseData';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';

export class WarehouseBlockBalanceResponseData extends INF_MSG_GenericResponseData {

    public SiteNumber: string;
    public SiteText: string;
    public WarehouseBlockNumber: string;
    public DeclarationNumber: string;
    public ImporterNumber: string;
    public ImporterTitle: string;
    public OpeningDate: string;
    public OriginalOpeningDate: string;
    public MaxStorageDate: string;
    public BlockClosureDate: string;
    public LogicalPackagesQuantityBalance: string;
    public PhysicalPackagesQuantityBalance: string;
    public Value: string;
    public BlockSpecialActivitiesList: Array<BlockSpecialActivities>;
    public ActionList: Array<ActionActivities>;
    public StorageActionList: Array<StorageAction>;
    public GoodsItemByInvoiceList: Array<GoodsItemByInvoice>;
}

export class BlockSpecialActivities {
    public SpecialActivityTypeName: string;
}

export class ActionActivities {

    public ActionDate: string;
    public ActionPackagesQuantity: string;
    public PackagesQuantityAfterAction: string;
    public ActionValue: string;
    public ValueAfterAction: string;
    public GovernmentProcedureType: string;
    public GovernmentProcedureTypeText: string;
    public DeclarationNumber: string;
}

export class StorageAction {

    public StorageActionDate: string;
    public StorageActionType: string;
    public StorageActionPackagesQuantity: string;
    public PackagesQuantityAfterStorageAction: string;
    public StorageReferenceType: string;
    public PackingDetailsList: Array<PackingDetails>;
    public PackingDetailsListObs: ObservableCollection;
    public StorageUnloadingExceptiontype: string;
    public StorageUnloadingExceptionTypeText: string;

}

export class PackingDetails {

    public PackingType: string;
    public PackingTypeText: string;
    public StorageActionPackagesQuantity: string;
    public PackagesQuantityAfterStorageAction: string;
    public StorageActionPackagesWeight: string;
    public PackagesWeightAfterStorageAction: string;

}

export class GoodsItemByInvoice {

    public InvoiceSequenceNumber: string;
    public GoodsItemSequenceNumber: string;
    public RemainingQuantity: string;
    public GoodsPriceBase: string;
    public GoodsPriceMAD: string;
    public GoodsPriceMADAEF: string;
    public CurrencyType: string;
    public ExchangeRate: string;
}