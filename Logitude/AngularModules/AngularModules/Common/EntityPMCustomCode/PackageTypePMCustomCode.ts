import { PackageTypePM } from '../EntityPMs/PackageTypePM';

export class PackageTypePMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: PackageTypePM) {

        if (propertyName == "IsContainer") {
            if (entityPM.IsContainer) {
                entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", true);
                entityPM.UIProperties.SetEnabled("IsVehicle", "PackageType", false);

                if (entityPM.IsVehicle) {
                    entityPM.IsVehicle = false;
                }
            }

            else {
                entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", false);
                entityPM.UIProperties.SetEnabled("IsVehicle", "PackageType", true);

                if (entityPM.IsRefrigerated) {
                    entityPM.IsRefrigerated = false;
                }                
            }            
        }

        else if (propertyName == "IsVehicle") {
            if (entityPM.IsVehicle) {
                entityPM.UIProperties.SetEnabled("IsContainer", "PackageType", false);

                if (entityPM.IsContainer) {
                    entityPM.IsContainer = false;
                }
            }

            else {
                entityPM.UIProperties.SetEnabled("IsContainer", "PackageType", true);
            }
        }
    }
}
