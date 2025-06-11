import { AdditionalCurrencyRatePM } from '../EntityPMs/AdditionalCurrencyRatePM';
import { TextCodeTranslator } from '../Utilities/TextCodeTranslator';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { AdditionalCurrencyRateListService } from 'Infrastructure/Services/StandardLists/AdditionalCurrencyRateListService';

export class AdditionalCurrencyRateValidator {

    public static CheckIdenticalRateValue(entityPM: AdditionalCurrencyRatePM) {
        return new Promise<boolean>((resolve) => {

            let filters = new ApiQueryFilters();
            filters.GetCount = true;
            filters.addAdditionalFilter("RateCoefficient", entityPM.RateCoefficient, null, null, "Equals", false, false, false, "number");

            const additionalCurrencyRateListService = new AdditionalCurrencyRateListService();

            return additionalCurrencyRateListService.getByFilters(filters).subscribe(response => {
                if (response.Count > 0) {
                    const msg = TextCodeTranslator.Translate("AdditionalCurrencyRate.O.AlreadyExistRate").replace("{name}", entityPM.Name);
                    const confirmWindow = new ConfirmWindow();
                    const widthOfWindow = 300;
                    const heightOfWindow = 150;

                    confirmWindow.Width = widthOfWindow;
                    confirmWindow.Height = heightOfWindow;
                    confirmWindow.YesButtonText = TextCodeTranslator.Translate("Accounting.General.B.OK");
                    confirmWindow.NoButtonText = TextCodeTranslator.Translate("Accounting.General.B.Cancel");
                    confirmWindow.Show(msg);

                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            resolve(true);
                        }
                        else {
                            resolve(false);
                        }
                    });
                }
                else {
                    resolve(true);
                }
            });
        });
    }
}

