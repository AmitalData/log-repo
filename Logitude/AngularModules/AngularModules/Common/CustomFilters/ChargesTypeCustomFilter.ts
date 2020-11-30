import { ChargesTypeList } from '../EntityLists/ChargesTypeList';

export class ChargesTypeCustomFilter {

    public static GetFilteredQuery(addtionalFiltersValues: any, data: ChargesTypeList[]) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        for (var i in mykeys) {
            var propName = mykeys[i];
            if (propName.FieldName == "ChargeTypesByDirectionFilter") {
                data = this.FilterByDirection(propName, data);
            }
        }
        return data
    }

    private static FilterByDirection(propName, data: ChargesTypeList[]) {
        var directionCode = propName.FieldValue;
        switch (directionCode) {
            case "E":
                {
                    data = data.filter(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInExport));
                    break;
                }

            case "I":
                {
                    data = data.filter(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInImport));
                    break;
                }

            case "D":
                {
                    data = data.filter(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInDomestic));
                    break;
                }

            case "R":
                {
                    data = data.filter(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInDrop));
                    break;
                }
        }
        return data;
    }
}
