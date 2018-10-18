import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {GovernmentProcedureTypeList} from '../../EntityLists/GovernmentProcedureTypeList';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

export class GovernmentProcedureTypeDataChangeService {
   
    constructor() {
        
    }

    ApplyDataChange(data: GovernmentProcedureTypeList[]) {
        //.sort((a, b) => { return a.IndexOrder - b.IndexOrder });
        var result: GovernmentProcedureTypeList[] = [];
        var list1 = data.filter(d => d.IndexOrder != null).sort((a, b) => {
            if (a.IndexOrder > b.IndexOrder) { return 1; }
            else if (a.IndexOrder < b.IndexOrder) { return -1; }
            else {
                if (a.Code > b.Code) return 1;
                else if (a.Code < b.Code) return -1;
                else return 0;
            }
        });
        list1.forEach((item) => {
            result.push(item);
        });
        var list2 = data.filter(d => d.IndexOrder == null).sort((a, b) => {
            if (a.IndexOrder > b.IndexOrder) { return 1; }
            else if (a.IndexOrder < b.IndexOrder) { return -1; }
            else {
                if (a.Code > b.Code) return 1;
                else if (a.Code < b.Code) return -1;
                else return 0;
            }
        });
        list2.forEach((item) => {
            result.push(item);
        });
        return result;
    }



}