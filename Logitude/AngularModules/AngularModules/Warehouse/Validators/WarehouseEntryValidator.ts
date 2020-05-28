import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {WarehouseEntryPM} from '../EntityPMs/WarehouseEntryPM';
import {WarehouseTools} from  '../Tools'; 

export class WarehouseEntryValidator {
    public Validate(entityPM: WarehouseEntryPM) {
        var error: any = [];
        var message = "Can't set Field to future date";
        var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();

        if (entityPM.ActualEntryDate) {
            if (entityPM.ActualEntryDate.valueOf() > todayDateTime.valueOf()) {
                error.push(message.replace("Field", "Actual Entry Date"));
            }
        }

        if ((entityPM.WarehouseEntryPackages && entityPM.WarehouseEntryPackages.length == 0) || !entityPM.WarehouseEntryPackages) {
            error.push("You should at least add one package");
        }

        if (!WarehouseTools.IsInlandDomestic(entityPM.TransportModeId, entityPM.DirectionId)) {
            //if (AppTool.IsNullOrEmpty(entityPM.FromPortId)) {
            //    error.push("Origin field is required");
            //}
            //if (AppTool.IsNullOrEmpty(entityPM.ToPortId)) {
            //    error.push("Destination field is required");
            //}
        }

        return error;
    }
}