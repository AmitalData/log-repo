import { RequestParamsBase } from './RequestParamsBase';

export class BlockListInWarehouseRequestParams extends RequestParamsBase {
                          
    public FromDate?: Date;
    public ToDate?: Date;
    public StorageSiteNumber: string;
    public ShowResetBlocks: ShowResetBlocksTypesEnum;

}
export enum ShowResetBlocksTypesEnum {
    No = 0,
    Yes = 1,
    All = 2
}