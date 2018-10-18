import {TicketPM} from '../EntityPMs/TicketPM';
import {TicketStageList} from '../EntityLists/TicketStageList';
import {TicketStageListService} from '../Services/StandardLists/TicketStageListService';
import {TicketSourceList} from '../EntityLists/TicketSourceList';
import {TicketSourceListService} from '../Services/StandardLists/TicketSourceListService';
import {TicketCreatedByTypeList} from '../EntityLists/TicketCreatedByTypeList';
import {TicketCreatedByTypeListService} from '../Services/StandardLists/TicketCreatedByTypeListService';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

export class TicketPMInitService {

    public static InitValues(entityPM: TicketPM, isNew: boolean) {

        if (isNew) {
            this.GetTicketStageMethod(entityPM);
            this.GetTicketSourceMethod(entityPM);
            this.GetTicketCreatedByTypeMethod(entityPM);
        }
    }

    public static ApplyUIPoperties(entityPM: TicketPM, isNew: boolean) {
       
    }


    // Ticket Stages
    private static GetTicketStageMethod(entityPM: TicketPM) {
        var myService: TicketStageListService = new TicketStageListService();
        myService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var list = resp.Result;
                var stage = list.filter(d => d.Code == "OP" && d.Tenant == SessionLocator.TenantPM.Id)[0];
                if (stage != null) {
                    entityPM.StageId = stage.Id;
                }
            }
        });
    }

    // Ticket Source
    private static GetTicketSourceMethod(entityPM: TicketPM) {
        var myService: TicketSourceListService = new TicketSourceListService();
        myService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var list = resp.Result;
                var source = list.filter(d => d.Code == "LOG")[0];
                if (source != null) {
                    entityPM.Source = source.Code;
                }
            }
          
        });
    }

    //Ticket CreatedByType List
    private static GetTicketCreatedByTypeMethod(entityPM: TicketPM) {
        var myService: TicketCreatedByTypeListService = new TicketCreatedByTypeListService();
        myService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var list = resp.Result;
                var type = list.filter(d => d.Code == "INU")[0];
                if (type != null) {
                    entityPM.CreatedbyType = type.Code;
                }
            }
        });
    }
}