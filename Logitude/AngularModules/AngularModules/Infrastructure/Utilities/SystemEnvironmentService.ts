import { ObjectsLocator } from '../Locators/ObjectsLocator';
import {AppTool} from '../Tools';  

export class SystemEnvironmentService {
  
    public static GetLogitudeURL() {
        return AppTool.GetLogitudeURL();
    }  
  
    public  static IsLogBox(): boolean {

        let url: string = this.GetLogitudeURL().toLowerCase();

        if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") {
            if (url.indexOf("logbox") > -1 || url.indexOf("test.logitudeworld.com") > -1) {
                return true;
            }
        }
        return false;
    }

}

 

