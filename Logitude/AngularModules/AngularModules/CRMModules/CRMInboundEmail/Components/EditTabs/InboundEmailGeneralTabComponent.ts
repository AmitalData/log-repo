import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {InboundEmailPM} from '../../../../Infrastructure/EntityPMs/InboundEmailPM'; 
import {InboundEmailLinePM} from '../../../../Infrastructure/EntityPMs/InboundEmailLinePM'; 
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'TicketDocsOutTabComponent',
    moduleId: module.id,
    templateUrl: './InboundEmailGeneralTabComponent.html',
})

export class InboundEmailGeneralTabComponent {

    public entityPM: InboundEmailPM;
    //public ObjectTableName = "InboundEmail";
    public DataContext = this;
    public EmailLinesList: ObservableCollection; 

    constructor(private entityArgs: EntityArgs) {
        this.entityPM = entityArgs.EntityPM;
        this.EmailLinesList = new ObservableCollection([]);
        this.BuildData();
    }
    public BuildData() {
        this.EmailLinesList.Clear();
        var list = [];
        this.entityPM.InboundEmailLines.forEach(item => {
            list.push(new InboundEmailLineData(item));
        });

        this.EmailLinesList.InsertCollection(list);
    }

    public EmailLineSelectedItem: any = null;
    EmailLineSelected(item) {
        this.EmailLineSelectedItem = item;
    }

    get ObjectTableName() { return this.entityPM.ObjectTableName; }
    set ObjectTableName(value: string) { this.entityPM.ObjectTableName = value; }

    get ObjectTableId() { return this.entityPM.ObjectTableId; }
    set ObjectTableId(value: string) { this.entityPM.ObjectTableId = value; }
}

export class InboundEmailLineData {
    public ObjectTableName = "InboundEmailLine";
    public DataContext: this;
    public entityPM: InboundEmailLinePM;
    constructor(item: InboundEmailLinePM) {
        this.entityPM = item;
    }

    get Sender() { return this.entityPM.Sender; }
    set Sender(value: string) { this.entityPM.Sender = value; }

    get Recepient() { return this.entityPM.Recepient; }
    set Recepient(value: string) { this.entityPM.Recepient = value; }

    get Subject() { return this.entityPM.Subject; }
    set Subject(value: string) { this.entityPM.Subject = value; }

    get CCs() { return this.entityPM.CCs; }
    set CCs(value: string) { this.entityPM.CCs = value; }

    get InternalUsers() { return this.entityPM.InternalUsers; }
    set InternalUsers(value: string) { this.entityPM.InternalUsers = value; }

    get Body() { return this.entityPM.Body; }
    set Body(value: string) { this.entityPM.Body = value; }

    get FullBody() { return this.entityPM.FullBody; }
    set FullBody(value: string) { this.entityPM.FullBody = value; }

    get CreateDate() { return this.entityPM.CreateDate; }
    set CreateDate(value: Date) { this.entityPM.CreateDate = value; }


    public ViewBody(arg: string) {
        var title = "";
        var description = null;
        if (arg == 'B') {
            title = "Body";
            description = this.entityPM.Body;
        }
        else {
            title = "Full Body";
            description = this.entityPM.FullBody;
        }

        var logWindow = new LogitudeWindow();
        logWindow.Title = title;
        logWindow.WindowArgs = description;
        logWindow.Show("./CRMModules/CRMInboundEmail/Components/EditTabs/ViewInboundLineBodyComponent");
    }

}