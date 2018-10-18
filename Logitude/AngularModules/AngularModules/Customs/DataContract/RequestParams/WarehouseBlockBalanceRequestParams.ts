import {GenericRequestParams} from './GenericRequestParams';

export class WarehouseBlockBalanceRequestParams extends GenericRequestParams {

    public CustomFileNo: string;
    public DeclarationNumber: string;
    public StorageSiteNumber: string;
    public WarehouseBlockNumber: string;
    public DisplayGoodsItemByInvoice: string;

    public DeclarationRadio: boolean;
    public StorageSiteRadio: boolean;
}
