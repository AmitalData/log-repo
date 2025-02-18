import { TabItem } from '../../../Infrastructure/Components/EditComponent/EditComponent';
import { IObjectTableTabsBuilder } from '../../../Infrastructure/Interface/IObjectTableTabsBuilder';
declare var window: any;
export class ShipmentTabsBuilder implements IObjectTableTabsBuilder {
    BuildTabs(args: any): TabItem[] {
        let tabItems: TabItem[] = [];
        window.ObjectTableTabs.filter(d => d.ObjectTableId === args.ObjectTableId && d.Code == "SHAU").forEach(item => {
            tabItems.push(new TabItem(item));
        });

        return tabItems;
    }


}
