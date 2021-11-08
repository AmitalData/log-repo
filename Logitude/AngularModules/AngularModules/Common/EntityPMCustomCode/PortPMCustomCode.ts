import {PortPM} from '../EntityPMs/PortPM';
import { IncotermListService } from '../../Common/Services/StandardLists/IncotermListService';
import { StateList } from '../../Common/EntityLists/StateList';
import { StateListService } from '../../Common/Services/StandardLists/StateListService';
import { CountryList } from '../../Common/EntityLists/CountryList';
import { CountryListService } from '../../Common/Services/StandardLists/CountryListService';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class PortPMCustomCode {
    public static ObjectTableName = "Port";
    public static entityPM: PortPM;
    public static Country: CountryList; 
    public static State: StateList; 

    public static ApplyEntityChanged(propertyName: string, entityPM: PortPM) {
        this.entityPM = entityPM;      

        if (propertyName == "CountryId" && entityPM.CountryId) {
            var countryService = new CountryListService();
            countryService.getSingleFromCache(entityPM.CountryId).subscribe((response:any) => {
                if (response.Result) {
                    this.Country = response.Result;
                    this.OnCountryChanged(this.Country);
                }
            });
        }
        if (propertyName == "StateId") {
            if (entityPM.StateId) {
                var stateService = new StateListService();
                stateService.getSingleFromCache(entityPM.StateId).subscribe((response:any) => {
                    if (response.Result) {
                        this.State = response.Result;
                        this.OnStateChanged(this.State);
                    }
                });
            }
            else {
                if (this.Country != null) {
                    if (this.Country.IsStateRequired) {
                        this.entityPM.UIProperties.SetRequired("StateId", this.ObjectTableName, true);
                    }
                }
            }
        }
    }

    private static SetUIProperties_State() {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    }
    private static SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }

        this.entityPM.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    }
    private static SetUIProperties_StateRequired() {
        var isRequired = false;

        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }

        this.entityPM.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    }

    private static OnCountryChanged(list: CountryList) {
        if (list == null) {
            this.entityPM.CountryCode = null;
            this.entityPM.CountryName = null;
        }

        else {
            this.entityPM.CountryCode = list.Code;
            this.entityPM.CountryName = list.EnglishName;
        }

        this.SetUIProperties_State();
    }
    private static OnStateChanged(list: StateList) {
        if (list == null) {
            this.entityPM.StateCode = null;
        }

        else {
            this.entityPM.StateCode = list.Code;
        }

        this.SetUIProperties_StateRequired();
    }
}
