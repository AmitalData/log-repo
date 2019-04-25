/// <reference path="../entitypms/airlinepm.ts" />
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {PackageTypePM} from '../EntityPMs/PackageTypePM';

export class PackageTypePMInitService {

    public static InitValues(entityPM: PackageTypePM, isNew: boolean) {
        if (isNew) {
            entityPM.AddedManually = true;
        }
    }

    public static ApplyUIPoperties(entityPM: PackageTypePM, isNew: boolean) { 
        entityPM.UIProperties.SetEnabled("IsRefrigerated", "PackageType", entityPM.IsContainer);
        entityPM.UIProperties.SetEnabled("IsVehicle", "PackageType", !entityPM.IsContainer);
    }
}
