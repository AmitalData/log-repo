import { ApiQueryFilters } from '../Infrastructure/DataContracts/ApiQueryFilters';

export class CommonTool {

    public static FilterChargeTypesByDirection(ChargeTypesQueryFilters: ApiQueryFilters, direction: string) {
        ChargeTypesQueryFilters.addAdditionalFilter("ChargeTypesByDirectionFilter", direction, null, null, "Equals", true, true, false, "Boolean", false, true);
    }
}
