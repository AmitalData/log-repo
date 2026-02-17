import {TenantManagementPM} from '../EntityPMs/TenantManagementPM';
import {AppTool} from '../Tools';
import {Validator} from './Validator';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';

export class TenantManagementValidator {

    public Validate(entityPM: TenantManagementPM) {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        var objectTableName: "TenantManagement";
        var isInlandDomestic: boolean = false;

        if (entityPM != null) {
            if (entityPM.IsMultiPackage) {
                if (entityPM.TenantManagementLicenses.length == 0 && entityPM.AddOns.length == 0) {
                    errors.push("Multi Package tenant must have 1 package added at least");
                }
            }

            if (entityPM.IsTrial) {
                if (entityPM.TrialStartDate == null) {
                    errors.push("Trial Start Date is Required");
                }

                if (entityPM.TrialEndDate == null) {
                    errors.push("Trial End Date is Required");
                }
            }

            if (entityPM.IsRecurring) {
                if (entityPM.RecurringPeriodCode == null) {
                    errors.push("Recurring Period is Required");
                }
            }

            if (entityPM.TemporalPackageCode != null) {
                if (entityPM.TemporalStartDate == null) {
                    errors.push("Temporal Start Date is Required");
                }

                if (entityPM.TemporalEndDate == null) {
                    errors.push("Temporal End Date is Required");
                }
            }

            if (entityPM.AWBMessagesCCSTypeCode == "CHAMP") {
                //if (AppTool.IsNullOrEmpty(tenantMngmnt.TTY))
                //{
                //    errors.push("TTY field is Required");
                //}
            }

            else if (entityPM.AWBMessagesCCSTypeCode == "GLSHK") {
                if (AppTool.IsNullOrEmpty(entityPM.PIMA)) {
                    errors.push("PIMA field is Required");
                }
            }

            if (entityPM.TenantTypeCode == "AIR") {

            }
        }

        return errors;
    }
}