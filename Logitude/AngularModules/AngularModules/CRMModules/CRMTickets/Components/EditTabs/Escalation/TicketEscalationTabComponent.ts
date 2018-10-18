import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {TicketEscalationList} from '../../../../../CRM/EntityLists/TicketEscalationList';
import {CRMDomainService} from '../../../../../CRM/Services/CRMDomainService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool} from '../../../../../Infrastructure/Tools'; 
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'TicketEscalationTabComponent',
    moduleId: module.id,
    templateUrl: './TicketEscalationTabComponent.html',
})

export class TicketEscalationTabComponent {
    public EntityPM: TicketPM = null;
    public ObjectTableName = "Ticket";
    public DataContext: this;
    //public TicketEscalationsList: TicketEscalationList[] = [];
    public TicketEscalationsList: ObservableCollection;

    constructor(private entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.TicketEscalationsList = new ObservableCollection([]);
        this.GetEscalationsList();
    }
    GetEscalationsList() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetTicketEscalationListsByTicketId(this.EntityPM.Id).subscribe((resp: ServiceResponse) => {
            if (resp != null && !resp.HasError) {
                //this.TicketEscalationsList = [];
                this.TicketEscalationsList.InsertCollection(resp.Result.sort((a, b) => { return (DateTool.GetDateParts(a.DueDate) === DateTool.GetDateParts(b.DueDate)) ? 0 : (DateTool.GetDateParts(a.DueDate) > DateTool.GetDateParts(b.DueDate)) ? 1 : -1 }));
            }
        });
    }
}