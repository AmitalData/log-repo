import { ShipmentTestTabsBuilder } from '../ObjectTableTabsBuilder/ShipmentTestTabsBuilder';

export class ObjectTableTabBuilderService {

    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ShipmentTest": { myResult = new ShipmentTestTabsBuilder(); break; }
                
        }
        return myResult;

    }

}
