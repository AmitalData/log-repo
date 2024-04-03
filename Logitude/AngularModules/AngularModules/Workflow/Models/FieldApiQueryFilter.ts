import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";

export class FieldApiQueryFilter {
    public fieldCode: string;
    public apiQueryFilters: ApiQueryFilters;

    constructor() {
        this.fieldCode = null;
        this.apiQueryFilters = null;
    }
}