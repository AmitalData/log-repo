import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import {WizardTabComponent} from '../EditEntity/WizardTabs';
export class DirectAWB {
    private Helper: FieldsHelper;
    private WizardTabs: WizardTabComponent;
    constructor() {
        this.Helper = new FieldsHelper();
        this.WizardTabs = new WizardTabComponent();
    }
    public CreateDirectAWB(shipperRef1: string, LogitudeWizardType: string) {
        this.FillDirectAWBFields(shipperRef1, LogitudeWizardType);

        this.Helper.WaitByIdAndClick('SaveWizard');
        this.Helper.WaitBusyIndicator();

        this.Helper.WaitByIdAndClick('CloseWizard');
    }

    FillDirectAWBFields(shipperRef1: string, LogitudeWizardType: string) {
        this.WizardTabs.FillPartnersTab(shipperRef1, LogitudeWizardType);
        this.WizardTabs.FillRoutingTab(shipperRef1, LogitudeWizardType);
        this.WizardTabs.FillPackagesTab(LogitudeWizardType);
        this.WizardTabs.FillFreightChargesTab(LogitudeWizardType);
        this.WizardTabs.FillOtherChargesTab(LogitudeWizardType);
        this.WizardTabs.FillRADetails(LogitudeWizardType);
        this.WizardTabs.FillGeneralDetailsTab(LogitudeWizardType);
        this.WizardTabs.FillOCITab(LogitudeWizardType);
        this.WizardTabs.FillOtherPartnersTab(LogitudeWizardType);
    }
}



