import {AppTool} from '../../Infrastructure/Tools';

export class AWBDescriptionOfGoodsCustomFilter {
    public static GetFilteredQuery(addtionalFiltersValues: any, data: any) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        if (mykeys.filter(d => d.IsCustom == true)[0]) {
            for (var i in mykeys.filter(d => d.IsCustom)) {
                var propName = mykeys[i];
                if (propName.FieldName == "AirlineCode") {
                    var myAirlineCode = propName.FieldValue;
                    if (!AppTool.IsNullOrEmpty(myAirlineCode)) {
                        data = data.filter(d => d.AirlineCode == null || d.AirlineCode == myAirlineCode);
                    }
                    else {

                        data = data.filter(d => d.AirlineCode == null);
                    }
                }
            }
        }
        else {

            data = data.filter(d => d.AirlineCode == null);
        }

        return data
    }
}