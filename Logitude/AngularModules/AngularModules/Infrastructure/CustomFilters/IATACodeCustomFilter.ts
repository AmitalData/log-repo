import {AppTool} from '../Tools';

export class IATACodeCustomFilter {

    public static GetFilteredQuery(addtionalFiltersValues: any, data: any) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        if (mykeys.filter(d => d.IsCustom == true)[0]) {
            for (var i in mykeys.filter(d => d.IsCustom)) {
                var propName = mykeys[i];
                if (propName.FieldName == "AirlineId") {
                    var myAirlineId = propName.FieldValue;
                    if (!AppTool.IsNullOrEmpty(myAirlineId)) {
                        data = data.filter(d => d.AirlineId == null || d.AirlineId == myAirlineId);
                    }
                    else {

                        data = data.filter(d => d.AirlineId == null);
                    }
                }
            }
        }
        else {

            data = data.filter(d => d.AirlineId == null);
        }

        return data
    }
}