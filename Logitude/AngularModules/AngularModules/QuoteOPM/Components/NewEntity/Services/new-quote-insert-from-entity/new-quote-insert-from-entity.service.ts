import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CardPM } from 'Common/EntityPMs/CardPM';
import { ContactPM } from 'Common/EntityPMs/ContactPM';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { QuoteOPPropertiesPM } from 'QuoteOPM/EntityPMs/QuoteOPPropertiesPM';
import { NewQuoteExpectedOrderComponent } from '../../components/new-quote-expected-order/new-quote-expected-order.component';
import { NewQuoteGeneralComponent } from '../../components/new-quote-general/new-quote-general.component';
import { NewQuotePartnerComponent } from '../../components/new-quote-partner/new-quote-partner.component';
import { NewQuotePropertiesComponent } from '../../components/new-quote-properties/new-quote-properties.component';
import { NewQuoteDataService } from '../new-quote-data/new-quote-data.service';

@Injectable()
export class NewQuoteInsertFromEntityService {
  EntityPM: QuoteOPPM;

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  partnerInsert(partnerRef: NewQuotePartnerComponent) {
    this.setPartner(partnerRef);
    this.setContact(partnerRef);
    partnerRef.partnerform.controls.reference1.setValue(this.EntityPM[partnerRef.capitalizeType + 'Reference1']);
    // partnerRef.partnerform.controls[partnerRef.capitalizeType + 'Note'].setValue(this.EntityPM[partnerRef.capitalizeType + 'Note']);

  }


  private async setPartner(partnerRef: NewQuotePartnerComponent) {
    const prtnerId = this.EntityPM[partnerRef.capitalizeType + 'Id']
    if (prtnerId) {
      const card: CardPM = await this.newQuoteDataService.getCard(prtnerId)
      partnerRef.partnerform.controls.partner.setValue(card)
    }
  }


  private async setContact(partnerRef: NewQuotePartnerComponent) {
    const contactId = this.EntityPM[partnerRef.capitalizeType + 'ContactId']
    if (contactId) {
      const contact: ContactPM = await this.newQuoteDataService.getContact(contactId)
      partnerRef.partnerform.controls.contact.setValue(contact)
    }
  }


  propertiesInsert(propRef: NewQuotePropertiesComponent) {
    const dataExist: boolean = !!this.EntityPM && !!this.EntityPM.ToPortId;
    if (!dataExist) return;

    this.EntityPM.QuoteProperties.forEach((prop: QuoteOPPropertiesPM, i: number) => {
      if (propRef.formArray.length < i + 1)
        propRef.formArray.push(propRef.propForm)

      const propCtrl = (propRef.formArray.at(i) as FormGroup).controls;
      this.setFromPort(prop, propCtrl);
      this.setToPort(prop, propCtrl);
      this.setCarrier(prop, propCtrl);
      this.setIncoterm(prop, propCtrl);
      this.setSpecialService(prop, propCtrl);
    });
  }


  private async setSpecialService(prop: QuoteOPPropertiesPM, propCtrl: any) {
    if (!prop.SpecialServiceID) return;
    const specialService = await this.newQuoteDataService.getSpecialServiceById(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, prop.SpecialServiceID);
    propCtrl.specialService.setValue(specialService);
  }


  private async setIncoterm(prop: QuoteOPPropertiesPM, propCtrl: any) {
    if (!prop.IncotermId) return;
    const incoterm = await this.newQuoteDataService.getIncotermById(prop.IncotermId);
    propCtrl.incoterm.setValue(incoterm);
  }


  private async setCarrier(prop: QuoteOPPropertiesPM, propCtrl: any) {
    if (!prop.MainCarriageCarrierId) return;
    const carrier = await this.newQuoteDataService.getCarriersesById(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, prop.MainCarriageCarrierId);
    propCtrl.mainCarriageCarrier.setValue(carrier);
  }


  private async setToPort(prop: QuoteOPPropertiesPM, propCtrl: any) {
    if (!prop.ToPortId) return;
    const toPort = await this.newQuoteDataService.getPortsById(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, prop.ToPortId);
    propCtrl.toPort.setValue(toPort);
  }


  private async setFromPort(prop: QuoteOPPropertiesPM, propCtrl: any) {
    if (!prop.FromPortId) return;
    const fromPort = await this.newQuoteDataService.getPortsById(this.EntityPM.DirectionId, this.EntityPM.TransportModeId, prop.FromPortId);
    propCtrl.fromPort.setValue(fromPort);
  }
  

  async generalInsert(generalRef: NewQuoteGeneralComponent) {
    const dataExist: boolean = !!this.EntityPM?.ExpirationDate;
    if (!dataExist) return;

    generalRef.formGroup.controls.startDate.setValue(this.EntityPM.StartDate ? new Date(this.EntityPM.StartDate) : null);
    generalRef.formGroup.controls.expirationDays.setValue(this.EntityPM.ExpirationDays);
    generalRef.formGroup.controls.expirationDate.setValue(this.EntityPM.ExpirationDate ? new Date(this.EntityPM.ExpirationDate) : null);
    generalRef.formGroup.controls.isAutomaticallyClosed.setValue(this.EntityPM.IsAutomaticallyClosed);
    generalRef.formGroup.controls.automaticallyCloseDays.setValue(this.EntityPM.AutomaticallyCloseDays);
    generalRef.formGroup.controls.automaticallyCloseDate.setValue(this.EntityPM.AutomaticallyCloseDate ? new Date(this.EntityPM.AutomaticallyCloseDate) : null);
    generalRef.formGroup.controls.quoteType.setValue(this.EntityPM.QuoteTypeCode);
    generalRef.formGroup.controls.moveType.setValue(await this.newQuoteDataService.getMoveTypeById(this.EntityPM.MoveTypeId));
  }


  async expectedOrderInsert(expectedOrderRef: NewQuoteExpectedOrderComponent): Promise<void> {
    const dataExist: boolean = !!this.EntityPM?.ExpirationDate;
    if (!dataExist) return;

    this.insertQuantitys(expectedOrderRef);
    expectedOrderRef.formGroup.controls.isDangerous.setValue(this.EntityPM.IsDangerous);
    expectedOrderRef.formGroup.controls.descriptionOfGoods.setValue(this.EntityPM.DescriptionOfGoods);
    expectedOrderRef.formGroup.controls.notes.setValue(this.EntityPM.Notes);

    await this.insertPackages(expectedOrderRef);

    expectedOrderRef.calcChargeableWeight();
  }


  private insertQuantitys(expectedOrderRef: NewQuoteExpectedOrderComponent) {
    expectedOrderRef.formGroup.controls.quantity1.setValue(this.EntityPM.PackageType1Quantity);
    expectedOrderRef.formGroup.controls.quantityType1.setValue(this.EntityPM.PackageType1Id);
    expectedOrderRef.formGroup.controls.quantity1.setValue(this.EntityPM.PackageType2Quantity);
    expectedOrderRef.formGroup.controls.quantityType1.setValue(this.EntityPM.PackageType2Id);
    expectedOrderRef.formGroup.controls.quantity1.setValue(this.EntityPM.PackageType3Quantity);
    expectedOrderRef.formGroup.controls.quantityType1.setValue(this.EntityPM.PackageType3Id);
    expectedOrderRef.formGroup.controls.quantity1.setValue(this.EntityPM.PackageType4Quantity);
    expectedOrderRef.formGroup.controls.quantityType1.setValue(this.EntityPM.PackageType4Id);
  }


  private async insertPackages(expectedOrderRef: NewQuoteExpectedOrderComponent) {
    await this.EntityPM.QuotePackages.forEach(async (pack, i) => {
      if (expectedOrderRef.formArray.length < i + 1)
        expectedOrderRef.addPackage(false);

      const packCtrl = (expectedOrderRef.formArray.at(i) as FormGroup).controls;
      packCtrl.volume.setValue(pack.Volume);
      packCtrl.grossWeight.setValue(pack.GrossWeight);
      packCtrl.packageType.setValue(await this.newQuoteDataService.getPackageTypeById(pack.PackageTypeId));
      packCtrl.Ldimension.setValue(pack.Length);
      packCtrl.Wdimension.setValue(pack.Width);
      packCtrl.Hdimension.setValue(pack.Height);
      packCtrl.quantity.setValue(pack.Quantity);
    });
  }
}
