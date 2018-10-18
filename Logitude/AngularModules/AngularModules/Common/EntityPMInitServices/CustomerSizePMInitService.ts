/// <reference path="../entitypms/airlinepm.ts" />
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {CustomerSizePM} from '../EntityPMs/CustomerSizePM';

export class CustomerSizePMInitService {

    public static InitValues(entityPM: CustomerSizePM, isNew: boolean) {
        if (isNew) {
            entityPM.Order = 0;
        }
    }

    public static ApplyUIPoperties(entityPM: CustomerSizePM, isNew: boolean) {
        
    }

}