import {UserList} from '../EntityLists/UserList';
import {AppTool} from '../../Infrastructure/Tools';

export class UserCustomFilter {

    public static  GetFilteredQuery(addtionalFiltersValues: any, data: UserList[]) {

        //addtionalFiltersValues = addtionalFiltersValues.replace('[', '').replace(']','');
        var mykeys = JSON.parse(addtionalFiltersValues);
        for (var i in mykeys) {
            var propName = mykeys[i];
            if (propName.FieldName == "EmployeeGroupCustomFilter") {
                var groupId = propName.FieldValue;
                if (!AppTool.IsNullOrEmpty(groupId)) {
                    data = data.filter(d => d.GroupId != null && (d.GroupId.indexOf(groupId) > -1));//.indexOf(groupId) > -1);
                }
            }
        }

        return data;
        //for (var i = 0; i < addtionalFiltersValues.length; i++) {
        //    var pair = addtionalFiltersValues[i].split('=');
        //    if (decodeURIComponent(pair[0]) == "AdditionalFilters") {
        //        var obj = JSON.parse(pair[1]);
        //        for (var key in obj) {
        //            if (key == 'EmployeeGroupCustomFilter') {
        //                var groupId = obj[key];
        //                if (!AppTool.IsNullOrEmpty(groupId)) {

        //                    data = data.filter(d => d.GroupId != null && d.GroupId.indexOf(groupId) > -1);
        //                }

        //                return data;
        //            }
        //        }
        //    }
        //}
    }
}