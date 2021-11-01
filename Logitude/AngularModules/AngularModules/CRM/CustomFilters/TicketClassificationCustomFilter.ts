import { TicketClassificationList} from '../EntityLists/TicketClassificationList';
import {AppTool} from '../../Infrastructure/Tools';

export class  TicketClassificationCustomFilter {

    public static GetFilteredQuery(addtionalFiltersValues: any, data:  TicketClassificationList[]) {

        var mykeys = JSON.parse(addtionalFiltersValues);
        for (var i in mykeys) {
            var propName = mykeys[i];
            if (propName.FieldName == "ParentId") {
                var value = propName.FieldValue;
                if (!AppTool.IsNullOrEmpty(value)) {
                    var myCheck = value.split('!')[1];
                    var myId = value.split('!')[0];

                    if (myCheck == "S") {
                        data = data.filter(d => d.ParentId != null && (d.ParentId == myId || d.ParentId.startsWith(myId+'-') )&& d.Inactive == false);
                    }

                    else if (myCheck == "F") {
                        data = data.filter(d => d.ParentId == myId && d.Inactive == false);
                    }
                }
            }
        }

        return data;
    }
}
