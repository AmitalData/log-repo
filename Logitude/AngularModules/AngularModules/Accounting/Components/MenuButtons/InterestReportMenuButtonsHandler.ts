import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { InterestBasesTypePM } from '../../EntityPMs/InterestBasesTypePM';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { GeneralPrintHelper } from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';

export class InterestReportMenuButtonsHandler {
    public EntityPM: InterestReportPM;
    public entityArgs: EntityArgs;
    public TenantPM: TenantPM;
    public ObjectTableName: string = "InterestReport";


    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "InterestPrint":
                            {
                                button.IsDisabled = false;
                                break;
                            }
                    }
                }
            }
        }
        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        var errors = [];

        if (errors.length == 0) {
            switch (menuButton.EventCode) {
                case "InterestPrint":
                    {
                        this.PrintInterestReport();
                        break;
                    }
            }
        }
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    }

    private PrintInterestReport() {
        var myPrintHelper = new GeneralPrintHelper(this.ObjectTableName, "ITDT", this.EntityPM.Id, null, this.EntityPM.ReportNumber, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "InterestReport");
            myPrintHelper.ShowPrintControl();
        }
    }
}
