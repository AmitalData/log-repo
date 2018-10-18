
import { ResponseDataBase } from './ResponseDataBase';

export class ReleaseGoodsResponseData extends ResponseDataBase {

    public DeclarationNumber: string;
    public FileNumber: string;
    public governmentProcedureType: string;
    public releaseDate: string;
    public dealValueNIS: string;
    public CifValueNis: string;
    public CurrencyTypeCode: string;
    public ExchangeRate: string;
    public TaxationDate: string;
    public importerExpoterExternalID: string;
    public cargoIdentifierType: string;
    public cargoIdentifierKey1: string;
    public cargoIdentifierKey2: string;
    public loadingPort: string;
    public unloadingSiteNumber: string;
    public storageSiteNumber: string;
    public cargoDescription: string;
    public packageType: string;
    public packageQuantity: string;
    public packagesWeight: string;

    public GoodsItemsList: Array<GoodsItems>;
}

export class GoodsItems {
    public SupplierInvoice: string;
        public GoodsItemPath: string;
        public CustomItemID: string;
    }




