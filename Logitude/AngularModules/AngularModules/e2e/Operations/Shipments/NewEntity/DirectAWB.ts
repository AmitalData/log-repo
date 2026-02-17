import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import {WizardTabComponent} from '../EditEntity/WizardTabs';
export class DirectAWB {
  private Helper: FieldsHelper;
  private WizardTabs:WizardTabComponent;

  constructor() {
    this.Helper = new FieldsHelper();
    this.WizardTabs=new WizardTabComponent();
  }


  public CreateDirectAWB(shipperRef1: string,LogitudeWizardType:string) {

    var AWBToggle = this.Helper.WaitByIdAndClick('NewAWB');
    this.Helper.WaitByIdAndClick('DirectAWB');
    this.FillDirectAWBFields(shipperRef1,LogitudeWizardType);

    this.Helper.WaitByCssButtonClick(".EntityChangesButton", "Save");
    this.Helper.WaitBusyIndicator();

    this.Helper.WaitByCssButtonClick(".Button", "Close");

  }



  FillDirectAWBFields(shipperRef1: string,LogitudeWizardType:string) {
    this.WizardTabs.FillPartnersTab(shipperRef1,LogitudeWizardType);
    this.WizardTabs.FillRoutingTab(shipperRef1,LogitudeWizardType);
    this.WizardTabs.FillPackagesTab(LogitudeWizardType);
    this.WizardTabs.FillFreightChargesTab(LogitudeWizardType);
    // this.WizardTabs.FillOtherChargesTab(LogitudeWizardType);
    this.WizardTabs.FillGeneralDetailsTab(LogitudeWizardType);
    this.WizardTabs.FillOCITab(LogitudeWizardType);
    this.WizardTabs.FillOtherPartnersTab(LogitudeWizardType);

  }

}



