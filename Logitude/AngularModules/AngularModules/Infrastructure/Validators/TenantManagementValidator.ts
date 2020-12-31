import {TenantManagementPM} from '../EntityPMs/TenantManagementPM';
import {AppTool} from '../Tools';
import {Validator} from './Validator';
import {TextCodeTranslator} from '../Utilities/TextCodeTranslator';

export class TenantManagementValidator {

    public Validate(entityPM: TenantManagementPM) {
        var errors = [];

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

            if (AppTool.IsNullOrZero(entityPM.NumberOfUsers) && AppTool.IsNullOrZero(entityPM.FreeUsers)) {
                errors.push("You should enter Number of Users or Free Users");
            }

            entityPM.TenantManagementLicenses.forEach(item => {
                if (AppTool.IsNullOrZero(item.NumberOfUsers) && AppTool.IsNullOrZero(item.FreeUsers)) {
                    errors.push("You should enter Number of Users or Free Users for " + item.PackageCode);
                }
            });

            // if (entityPM.MainColor && this.IsValidHexCode(entityPM.MainColor)) {
            //     errors.push("Please enter valid color code");
            // }

            // if (entityPM.SecondaryColor && this.IsValidHexCode(entityPM.SecondaryColor)) {
            //     errors.push("Please enter valid color code");
            // }
        }

        return errors;
    }

    IsValidHexCode(colorHexCode: string)
    {
        const regex = new RegExp('^#([a-fA-F0-9]{6})$');
        var valid = regex.test(colorHexCode);
        if (colorHexCode && !valid)
            return false;
        return true;
    }
}
