import {AppTool} from '../Tools';

export class EntityStatusCustomFilter {

    public static GetFilteredQuery(addtionalFiltersValues: any, data: any) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        if (mykeys.filter(d => d.IsCustom == true)[0]) {
            for (var i in mykeys.filter(d => d.IsCustom)) {
                var propName = mykeys[i];
                if (propName.FieldName == "AutomationStatusField") {
                    var result: any[] = [];  
                    data.forEach((item) => {
                        var fieldValue = propName.FieldValue;
                        var status: any = result.filter(d => d.Name == item.Name)[0];
                        if (!status) {
                            result.push(item);
                        }
                        else {
                            if (!AppTool.IsNullOrEmpty(fieldValue)) {
                                if (item.Id == fieldValue) {
                                    var index = result.indexOf(status);
                                    if (index > -1) {
                                        result.splice(index, 1);
                                        result.push(item);
                                    }
                                }
                            }

                        }
                    });
                    data = result;
                }
            }
        }
       
        return data
    }
}