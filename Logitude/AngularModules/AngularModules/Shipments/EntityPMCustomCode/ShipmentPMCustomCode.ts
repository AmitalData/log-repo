import { ShipmentPM } from '../EntityPMs/ShipmentPM';
import { IncotermListService } from '../../Common/Services/StandardLists/IncotermListService';
import { IncotermList } from '../../Common/EntityLists/IncotermList';
export class ShipmentPMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: ShipmentPM) {
        //if (propertyName == "IncotermId" && entityPM.IncotermId) {
        //    var incotermService = new IncotermListService();
        //    incotermService.getSingleFromCache(entityPM.IncotermId).subscribe((response:any) => {
        //        if (response.Result)
        //        {
        //            var incotermList: IncotermList = response.Result;
        //            entityPM.FreightPrepaidCollectId = incotermList.Freight;
        //            entityPM.OtherPrepaidCollectId = incotermList.OtherCharges;
        //        }
        //    });
            
        //}

    }

}
