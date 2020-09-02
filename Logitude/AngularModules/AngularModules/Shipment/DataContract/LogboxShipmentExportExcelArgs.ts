
import { ApiQueryFilters, FilterItem } from '../../Infrastructure/DataContracts/ApiQueryFilters';
import { QueryColumnPM } from '../../Infrastructure/EntityPMs/QueryColumnPM';


export class LogboxShipmentExportExcelArgs {

    QueryColumns: QueryColumnPM[];
    Tenant: number;
    UserId: string;
    ObjectTableName: string;
    QueryName: string;
    AdditionalFilters: FilterItem[] = [];
    PageSize: number;
    PageIndex: number;
    QuerySection: string;
    SortBy: string;
    SortDirection: string;
    Filters: ApiQueryFilters;

}
