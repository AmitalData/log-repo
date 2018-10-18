import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AnalyzeQueuePM} from '../../../../Infrastructure/EntityPMs/AnalyzeQueuePM';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';

@Component({
    moduleId: module.id,
    templateUrl: './MessageBodyTabComponent.html',
})

export class MessageBodyTabComponent extends BaseComponent {
    public EntityPM: AnalyzeQueuePM;
    public DataContext: MessageBodyTabComponent = this;
    public ObjectTableName: string = "AnalyzeQueue";

    constructor(public args: EntityArgs) {
        super();
        this.EntityPM = args.EntityPM;
    }


    get MessageBodyString() { if (this.EntityPM != null) { return this.EntityPM.MessageBodyString; } return null; }
    set MessageBodyString(value: string) { this.EntityPM.MessageBodyString = value; }

    ViewXMLClicked() {
        var entityId =  this.EntityPM.Id + "_" + "analyzeQueue";
        DownloadManager.DownloadPage(entityId);

    }

}