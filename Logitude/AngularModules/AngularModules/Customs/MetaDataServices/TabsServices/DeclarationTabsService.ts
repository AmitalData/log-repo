import { IObjectTableTabsService } from '../../../Infrastructure/Interface/IObjectTableTabsService';
import { ObjectTableTabPM } from '../../../Infrastructure/EntityPMs/ObjectTableTabPM';
declare var window: any;
export class DeclarationTabsService implements IObjectTableTabsService {

    GetTabs(args: any): ObjectTableTabPM[] {

      return  window.ObjectTableTabs.filter(d => d.ObjectTableId === args.ObjectTableId);
    }

}
