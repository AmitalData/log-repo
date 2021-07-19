import { IObjectTableMenuButtonsService } from '../../../Infrastructure/Interface/IObjectTableMenuButtonsService';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM';
declare var window: any;
export class DeclarationMenuButtonService implements IObjectTableMenuButtonsService {
    GetMenuButtons(args: any): MenuButtonPM[] {
        return args.MenuButtons;
    }

}
