import { ObjectsLocator } from "../../../Infrastructure/Locators/ObjectsLocator";
import { FeatureLocator } from "../../../Infrastructure/Utilities/FeatureLocator";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";

export class CustomizationPermissionService {

    public static HasFeaturePermession(objectTableName: string, featureCode: string) {
        if (SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            return true;
        }
        return FeatureLocator.HasFeaturePermession(objectTableName, featureCode);
    }

    public static HasToggleFeaturePermession(toggleCode: string) {
        if (SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            return true;
        }
        return SessionLocator.FeatureToggles.some(d => d.ToggleCode == toggleCode);
    }

    public static HasEntityPermessions(objectTableName: string, featureCode: string, showWindow: boolean) {
        if (SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            return true;
        }
        return FeatureLocator.HasEntityPermessions(objectTableName, featureCode, showWindow);
    }

    public static IsFeatureGrantedByUniqeCode(featureUniqeCode: string) {
        if (SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            return true;
        }
        return FeatureLocator.IsFeatureGrantedByUniqeCode(featureUniqeCode);
    }
}
