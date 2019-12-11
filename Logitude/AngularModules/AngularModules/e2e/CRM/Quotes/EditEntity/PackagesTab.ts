import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';

export class PackagesTabComponent {

  private Helper: FieldsHelper;

  constructor() {
    this.Helper = new FieldsHelper();

  } 
  PackagesTab(ShipmentType: string,QuoteType :String) {
    this.Helper.WaitBusyIndicator();

    this.Helper.WaitByIdAndClick('Quote.TH.Packages');
    if(QuoteType=='SpotRate'){
 if(ShipmentType=='FCL'){
      this.Helper.WaitByIdAndFill('Quote_PackageType2Quantity','2');
      this.Helper.WaitByIdAndFill('Quote_PackageType2Id','20fr');
      this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'Quote_PackageType2Id','20fr');
    }
    else if(ShipmentType=='' || ShipmentType=='LTL'){
      this.Helper.WaitByIdAndClick('AddPackage');
      this.Helper.WaitByIdAndFill('QuotePackage_Quantity','2');
        if(ShipmentType=='LTL'){
      this.Helper.WaitByIdAndFill('QuotePackage_PackageTypeId','A0');
      this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'QuotePackage_PackageTypeId','A0');
    }
      this.Helper.WaitByIdAndFill('QuotePackage_Length','100');
      this.Helper.WaitByIdAndFill('QuotePackage_Width','100');
      this.Helper.WaitByIdAndFill('QuotePackage_Height','100');
      this.Helper.WaitByIdAndFill('QuotePackage_GrossWeight','1000');
      this.Helper.WaitByIdAndClick('OkAddPackage');
      
    }
  }
  else{
  /*  if(ShipmentType=='FTL' || ShipmentType=='FCL'){
      this.Helper.WaitByIdAndFill('Quote_PackageType2Id','20fr');
      this.Helper.WaitByCssAndClick_FromTagInsideListWithCheck('.DropDownListItem', 0,'Quote_PackageType2Id','20fr');
    }else{
    this.Helper.WaitByIdAndFill('Quote_GrossWeight','1000');
    this.Helper.WaitByIdAndFill('Quote_Volume','100');
    }*/
    
  }
    

  //  this.Helper.WaitByIdAndClick('Quote-Save');
  }


}
