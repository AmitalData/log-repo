import { IObjectTableTabsBuilder } from '../Interface/IObjectTableTabsBuilder';
import { TabItem } from '../Components/EditComponent/EditComponent';

declare var window: any;
export class ShipmentTestTabsBuilder implements IObjectTableTabsBuilder {

    BuildTabs(args: any): any[] {
        let tabsItemsSource: TabItem[] = [];
        let allTabs: [] = window.ObjectTableTabs.filter(d => d.ObjectTableId === args.ObjectTableId && d.Code == "SHAU");

        allTabs.forEach((item) => {
            var itemTab: TabItem = new TabItem(item);
            tabsItemsSource.push(itemTab);
        });
        return tabsItemsSource;
    }

}
