import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class PackagesTabComponent {
  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();
  }


  public PackagesTab(LogitudeShipType: string, shipmentType: string) {

    this.Helper.WaitByIdAndClick('Shipment.TH.Packages');
    if (LogitudeShipType == 'D' || LogitudeShipType == 'H') {
      this.Helper.WaitByIdAndClick('GenerateBTN');
    }
    
    if (shipmentType == '') {
      this.AddPackages('1', '100', '100', '100', '150', '8978965', 'Testing from protractor');
      this.AddPackages('2', '50', '50', '50', '70', '22257', 'Testing from protractor 2 ');
    }
    else if (shipmentType == 'FCL' || shipmentType == 'FTL') {
      this.AddContainer('PC1', 'ABCD1111117', '100', '111', 'Shipper Seal 1', 'Container 1 is Added', 'commod 1', 'Carrier seal 1');
      this.AddContainer('PC2', 'FXYZ8985558', '50', '222', 'Shipper Seal 2', 'Container 2 is added', 'commod 2', 'Carrier seal 2');
    }
    else if (shipmentType == 'LCL' || shipmentType == 'LTL') {
      this.AddLCL_LTLPackages('PP1', 'MKLE9998886', '1', '50', '50', '50', '100', 'Shipper Seal 1', 'First Package', 'Commod 1', 'Carrier Seal 1');
      this.AddLCL_LTLPackages('PP2', 'DEWR9998876', '2', '70', '70', '70', '140', 'Shipper Seal 2', 'Sec Package', 'Commod 2', 'Carrier Seal 2');
    }
     else if (LogitudeShipType == 'M') {

  
     if (shipmentType == '') {
      this.AddPackages('1', '100', '100', '100', '150', '8978965', 'Testing from protractor');
      this.AddPackages('2', '50', '50', '50', '70', '22257', 'Testing from protractor 2 ');
   }
     else if (shipmentType == 'FCL' || shipmentType == 'FTL' || shipmentType == 'OG' || shipmentType == 'IG') {
     this.AddContainer('PC1', 'ABCD1111117', '100', '111', 'Shipper Seal 1', 'Container 1 is Added', 'commod 1', 'Carrier seal 1');
     this.AddContainer('PC2', 'FXYZ8985558', '50', '222', 'Shipper Seal 2', 'Container 2 is added', 'commod 2', 'Carrier seal 2');
     }
     else if (shipmentType == 'LCL' || shipmentType == 'LTL' ) {
      this.AddLCL_LTLPackages('PP1', 'MKLE9998886', '1', '50', '50', '50', '100', 'Shipper Seal 1', 'First Package', 'Commod 1', 'Carrier Seal 1');
     this.AddLCL_LTLPackages('PP2', 'DEWR9998876', '2', '70', '70', '70', '140', 'Shipper Seal 2', 'Sec Package', 'Commod 2', 'Carrier Seal 2');
     }

     }

  }

  private AddPackages(quantity: string, length: any, Width: any, height: any, grossWeight: any, commodityNumber: any, note: any) {
    this.Helper.WaitByIdAndClick('AddPackage');

    this.Helper.WaitByIdAndFill('ShipmentPackage_Quantity', quantity);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Length', length);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Width', Width);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Height', height);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Weight', grossWeight);
    this.Helper.WaitByIdAndFill('ShipmentPackage_CommodityNumber', commodityNumber);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Notes', note);

    this.Helper.WaitByIdAndClick('OkAirPackage');

  }
  private AddContainer(containerType: string, containerNo: string, grossWeight: string, tare: any, shipperSeal: any, notes: any, commodityNumber: any, carrierSeal: any) {
    this.Helper.WaitByIdAndClick('AddPackage');

    var containerTypeValue = this.Helper.WaitByIdAndFill('ShipmentPackage_PackageTypeId', containerType);
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('ShipmentPackage_ContainerNumber', containerNo);
     this.Helper.WaitByIdAndFill('ShipmentPackage_Weight', grossWeight);

    this.Helper.WaitByIdAndFill('ShipmentPackage_Tare', tare);
    this.Helper.WaitByIdAndFill('ShipmentPackage_ShipperSeal', shipperSeal);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Notes', notes);

    this.Helper.WaitByIdAndFill('ShipmentPackage_CommodityNumber', commodityNumber);
    this.Helper.WaitByIdAndFill('ShipmentPackage_CarrierSeal', carrierSeal);

    this.Helper.WaitByIdAndClick('OkOceanPackage');
  }

  private AddLCL_LTLPackages(containerType: string, containerNo: string, quantity: string, length: any, Width: any, height: any, grossWeight: string, shipperSeal: any, notes: any, commodityNumber: any, carrierSeal: any) {

    this.Helper.WaitByIdAndClick('AddPackage');

    var containerTypeValue = this.Helper.WaitByIdAndFill('ShipmentPackage_PackageTypeId', containerType);
    this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);

    this.Helper.WaitByIdAndFill('ShipmentPackage_ContainerNumber', containerNo);

    this.Helper.WaitByIdAndFill('ShipmentPackage_Quantity', quantity);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Length', length);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Width', Width);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Height', height);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Weight', grossWeight);

    this.Helper.WaitByIdAndFill('ShipmentPackage_ShipperSeal', shipperSeal);
    this.Helper.WaitByIdAndFill('ShipmentPackage_Notes', notes);

    this.Helper.WaitByIdAndFill('ShipmentPackage_CommodityNumber', commodityNumber);

    this.Helper.WaitByIdAndClick('OkOceanPackage');
  }
}

