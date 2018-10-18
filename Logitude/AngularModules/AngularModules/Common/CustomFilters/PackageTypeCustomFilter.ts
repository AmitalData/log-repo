export class PackageTypeCustomFilter {

    public static GetFilteredQuery(addtionalFiltersValues: any, data: any) {
        var mykeys = JSON.parse(addtionalFiltersValues);
        for (var i in mykeys) {
            var propName = mykeys[i];
            if (propName.FieldName == "TransportModeId") {
                var value: string = propName.FieldValue as string;

                if (value == "A") {
                    data = data.filter(d => d.IsAir == true);
                }

                if (value == "O") {
                    data = data.filter(d => d.IsOcean == true);
                }

                if (value == "I") {
                    data = data.filter(d => d.IsInland == true);
                }

            }
        }

        return data
    }
}