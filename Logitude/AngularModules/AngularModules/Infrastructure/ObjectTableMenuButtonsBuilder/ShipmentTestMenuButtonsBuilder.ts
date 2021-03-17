import { IObjectTableMenuButtonsBuilder } from '../Interface/IObjectTableMenuButtonsBuilder';
import { MenuButtonPM } from '../EntityPMs/MenuButtonPM';

declare var window: any;
export class ShipmentTestMenuButtonsBuilder implements IObjectTableMenuButtonsBuilder {

    BuildMenuButtons(args: any): MenuButtonPM[] {
        let menuButtons: MenuButtonPM[] = [];

        return args.MenuButtons.filter(d => d.EventCode == "CopyShipment" || d.EventCode == "ExceptionResolved" || d.EventCode == "Actions"  );
    }
}
