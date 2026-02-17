import {PortPM} from '../EntityPMs/PortPM';
import { CountryList } from '../EntityLists/CountryList';
import { CountryListService } from '../Services/StandardLists/CountryListService';

export class PortValidator {
    public Validate(entityPM: PortPM) {
        var errors = [];
        var countryService = new CountryListService();
        countryService.getSingleFromCache(entityPM.CountryId).subscribe(response => {
            if (response.Result) {
                var country = response.Result;
                if (country != null) {
                    if (entityPM.StateId == null) {
                        if (country.IsStateRequired && country.HasStates) {
                            errors.push("State Field is Required");
                        }
                    }
                }
            }
        });
        return errors;
    }
}