import { IObjectTableMenuButtonsService } from '../../../Infrastructure/Interface/IObjectTableMenuButtonsService';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM';
declare var window: any;
export class ShipmentMenuButtonService implements IObjectTableMenuButtonsService {
    GetMenuButtons(args: any): MenuButtonPM[] {
        return args.MenuButtons.filter(d => d.EventCode == "Actions" || d.EventCode == "ExceptionResolved"  );
    }

}
