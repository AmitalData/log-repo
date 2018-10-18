import { PackageTypePM } from '../EntityPMs/PackageTypePM';

export class PackageTypePMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: PackageTypePM) {

        if (propertyName == "IsContainer") {
            if (entityPM.IsContainer) {
                entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", true);
            }

            else {
                entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", false);

                if (entityPM.IsRefrigerated) {
                    entityPM.IsRefrigerated = false;
                }
            }            
        }
    }
}