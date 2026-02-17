
import { EditTabsComponent } from './EditShipmentTabs.po';
import { browser, by, element } from 'protractor';

describe('Operations Module', () => {
  let page: EditTabsComponent;

  beforeEach(() => {
    page = new EditTabsComponent();
  });


  it('ShipmentTabs', function () {
    page.GoToShipment();
    page.EditTabs('314971','D','');
       // this.QuickSearch.UseQuickSearch('4445364363');
        // this.EditShipmentTabs.EditTabs('4445364363',LogitudeShipType, ShipmentType);

  });
});
