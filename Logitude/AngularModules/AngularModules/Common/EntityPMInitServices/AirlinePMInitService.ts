/// <reference path="../entitypms/airlinepm.ts" />
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {AirlinePM} from '../EntityPMs/AirlinePM';

export class AirlinePMInitService {

    public static InitValues(entityPM: AirlinePM, isNew: boolean) {
    }

    public static ApplyUIPoperties(entityPM: AirlinePM, isNew: boolean) {
        //entityPM.UIProperties = new UIProperties;
        entityPM.UIProperties.SetVisibility("EnableConsolidationInvoices", "Airline", FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent"));

        if (SessionLocator.TenantPM.Id != 0) {
            entityPM.UIProperties.SetEnabled("Prefix", "Airline", false);
        }
    }

}