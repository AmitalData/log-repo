

import {AppTool, DateTool} from '../../Infrastructure/Tools';

export class WarehouseReleaseValidator {
    public Validate(entityPM: any) {
        var error: any = [];
        var message = "Can't set Field to future date";
        var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();

        if (entityPM.ActualReleaseDate) {
            if (entityPM.ActualReleaseDate.valueOf() > todayDateTime.valueOf()) {
                error.push(message.replace("Field", "Actual Release Date"));
            }
        }

        if ((entityPM.WarehouseReleasePackages && entityPM.WarehouseReleasePackages.length == 0) || !entityPM.WarehouseReleasePackages) {
            error.push("You should at least choose one package");
        }



        return error;
    }
}