import { RequestParamsBase } from './RequestParamsBase';


export class SystemTableRequestParams extends RequestParamsBase {
    UpdateAllTables: boolean;
    TableId: string;
    AsTableData: boolean;
    Pseudo: boolean;
}