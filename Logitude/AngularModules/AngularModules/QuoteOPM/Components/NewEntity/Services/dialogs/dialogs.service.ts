import { Injectable } from '@angular/core';
import { AddressPM } from 'Common/EntityPMs/AddressPM';
import { ContactInputTemplateArgs } from 'CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { NewEntityArgs } from 'Infrastructure/Args';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

export type AddressCode = 'P' | 'D';

@Injectable()
export class DialogsService {

  constructor() { }

  async addCustomer(customerType: string): Promise<any> {
    const myComponentPath: string = "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent";
    const title: string = "New " + customerType;
    const args = new NewEntityArgs();
    args.Perspective = "ShippersAndConsignees";

    const logWindow = new LogitudeWindow();
    logWindow.Title = title;
    logWindow.Width = 960;
    logWindow.Height = 600;
    logWindow.WindowArgs = args;
    logWindow.Show(myComponentPath);

    await new Promise<void>(resolve => logWindow.ComponentLoaded.subscribe(comp => resolve()))
    return new Promise<any>(resolve => logWindow.WindowClosed.subscribe(s => resolve(s)))
  }

  async addPotentialCustomer(customerType: string): Promise<any> {
    var _entityResourceService: EntityResourceService = new EntityResourceService();
    await new Promise<void>(resolve => _entityResourceService.getEntityResourceByTableName("Customer").subscribe(() => resolve()))
    await new Promise<void>(resolve => _entityResourceService.getEntityResourceByTableName("Contact").subscribe(() => resolve()))

    const args = new NewEntityArgs();
    args.Perspective = "ShippersAndConsignees";

    const logWindow = new LogitudeWindow();
    logWindow.Title = "New Potential " + customerType;
    logWindow.Width = 990;
    logWindow.Height = 600;
    logWindow.WindowArgs = args;
    logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");

    await new Promise<void>(resolve => logWindow.ComponentLoaded.subscribe(comp => resolve()))
    return new Promise<any>(resolve => logWindow.WindowClosed.subscribe(s => resolve(s)))
  }

  addContact(partnerId: string, type: string) {
    var logWindow = new LogitudeWindow();
    logWindow.Width = 960;
    logWindow.Height = 570;
    logWindow.Title = "New Contact";
    var args = new ContactInputTemplateArgs();
    args.CustomerId = partnerId;
    // args.CardDependencyProperty1 = this.CardDependencyProperty1;
    args.CustomerLable = type == "SH" ? "Shipper" : "Consignee";
    args.ComponentName = "Partners";
    logWindow.WindowArgs = args;
    logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
    // logWindow.WindowClosed.subscribe(($event: any) => this.OnNewContactWindowClosed($event, type));
  }

  addAddress(myAddressCode: AddressCode, cardId: string): Promise<any> {
    const entityPM: AddressPM = new AddressPM();
    entityPM.Tenant = SessionLocator.Tenant;
    entityPM.AddressTypeId = myAddressCode;
    entityPM.CardId = cardId;

    const logeWindow = new LogitudeWindow();
    logeWindow.Width = 630;
    logeWindow.Height = 430;
    logeWindow.Title = "Add Address";
    logeWindow.WindowArgs = { EntityPM: entityPM };
    logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");

    return new Promise<any>((resolve) =>
      logeWindow.WindowClosed.subscribe(s => resolve(s)));
  }

  editAddress(addressId: string): Promise<any> {
    const logeWindow = new LogitudeWindow();
    logeWindow.Width = 630;
    logeWindow.Height = 430;
    logeWindow.Title = "Edit Address";
    logeWindow.WindowArgs = { EntityId: addressId };
    logeWindow.Show("./CommonPartners/Components/AddEdit/AddEditPartnerAddressComponent");

    return new Promise<any>((resolve) =>
      logeWindow.WindowClosed.subscribe(s => resolve(s)));
  }
}
