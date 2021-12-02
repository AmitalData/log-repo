import { ObjectsLocator } from '../Locators/ObjectsLocator';
import { AppTool } from '../Tools';
import { SessionLocator } from './SessionLocator';

export class SystemEnvironmentService {

    public static GetLogitudeURL() {
        return AppTool.GetLogitudeURL();
    }

    public static IsLogBox(): boolean {
        let isPrivateLabel = SessionLocator.PrivateLableSettings ? true : false;
        let url: string = this.GetLogitudeURL().toLowerCase();

        if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" || ObjectsLocator.GlobalSetting.DeploymentStage == "Dev") {
            if ((url.indexOf("logbox") > -1 || url.indexOf("test.logitudeworld.com") > -1 || url.indexOf("localhost:9996") > -1) && !isPrivateLabel) {
                return true;
            }
        }
        return false;
    }

}



