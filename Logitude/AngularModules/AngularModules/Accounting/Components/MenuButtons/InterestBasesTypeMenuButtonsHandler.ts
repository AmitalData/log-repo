import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { InterestBasesTypePM } from '../../EntityPMs/InterestBasesTypePM';

export class InterestBasesTypeMenuButtonsHandler {
    public EntityPM: InterestBasesTypePM;
    public entityArgs: EntityArgs;
    public TenantPM: TenantPM;
    public ObjectTableName: string = "InterestBasesType";


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
                        case "InterestBasesTypeInactive":
                            {
                                    if (this.EntityPM.InActive == false) {
                                        button.IsDisabled = false;
                                    }
                                    else {
                                        button.IsDisabled = true;
                                    }
      
                                break;
                            }

                        case "InterestBasesTypeInactives":
                            {
                                     button.IsHidden = false;
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
                case "InterestBasesTypeInactive":
                    {
                        this.EntityPM.InActive = true;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
            }
        }
        else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    }
}
