


import {ShipmentOrderListService} from './Services/StandardLists/ShipmentOrderListService';
import {ShipmentOrderPMService} from './Services/StandardPMs/ShipmentOrderPMService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ShipmentOrderPMService": { myResult = new ShipmentOrderPMService(); break; }
            case "ShipmentOrderListService": { myResult = new ShipmentOrderListService(); break; }
        }

        return myResult;
    }
}
