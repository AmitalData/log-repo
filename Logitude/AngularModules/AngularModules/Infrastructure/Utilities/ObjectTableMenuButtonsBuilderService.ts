
import { ShipmentTestMenuButtonsBuilder } from '../ObjectTableMenuButtonsBuilder/ShipmentTestMenuButtonsBuilder';

export class ObjectTableMenuButtonsBuilderService {

    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ShipmentTest": { myResult = new ShipmentTestMenuButtonsBuilder(); break; }
        }
        return myResult;

    }

}
