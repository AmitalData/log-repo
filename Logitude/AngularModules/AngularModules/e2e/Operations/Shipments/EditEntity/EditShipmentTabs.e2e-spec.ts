
import { EditTabsComponent } from './EditShipmentTabs.po';
import { browser, by, element } from 'protractor';

describe('Operations Module', () => {
  let page: EditTabsComponent;

  beforeEach(() => {
    page = new EditTabsComponent();
  });


  it('ShipmentTabs', function () {
    page.GoToShipment();
    page.EditTabs('M', '');

  });
});
