import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {InfrastructureDomainService} from '../Services/InfrastructureDomainService';

export class LastFilterClass {
    public static AllFilters: CRMFilterSettingList[] = [];
    public static MapJSON(json: any) {

        if (json != null) {
            var allLists = json;

            for (var key in allLists) {
                var entity: CRMFilterSettingList = this.MapJsonToEntityList(allLists[key]);
                this.AllFilters.push(entity);
            }
        }
    }
    private static MapJsonToEntityList(jsonItem: any) {
        var entity: CRMFilterSettingList = new CRMFilterSettingList();
        var jsonItemKeys = Object.keys(jsonItem);

        for (var key in jsonItemKeys) {
            var property = jsonItemKeys[key];
            entity[property] = jsonItem[property];
        }

        return entity;
    }


    public static GetFilterValue(myControlName: string, myFilterName: string) {
        var myResult = null;

        var filter = this.AllFilters.filter(f => f.ControlNameSpace == myControlName && f.FilterName == myFilterName)[0];

        if (filter != null) {
            myResult = filter.FilterValue;
        }

        return myResult;
    }
    public static UpdateFilter(myControlName: string, myFilterName: string, myFilterValue: string) {
        var myService = new InfrastructureDomainService();
        myService.UpdateLastFilter(myControlName, myFilterName, myFilterValue).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.AllFilters = [];
                    this.MapJSON(myResponse.Result);
                }
            }
        });
    }
}

export class CRMFilterSettingList {
    public Id: string;
    public Tenant: number;
    public UserId: string;
    public ControlNameSpace: string;
    public FilterName: string;
    public FilterValue: string;
}