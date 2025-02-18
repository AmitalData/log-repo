import {QueryColumnPM} from '../../Infrastructure/EntityPMs/QueryColumnPM';
import { ApiQueryFilters, FilterItem} from '../../Infrastructure/DataContracts/ApiQueryFilters';

import {Injectable} from '@angular/core';
@Injectable()

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
