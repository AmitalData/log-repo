declare var System: any;
declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';

@Component({
    selector: 'TicketDocsInTabComponent',
    
    templateUrl: './DeclarationDocsInTabComponent.html',
})

export class DeclarationDocsInTabComponent implements OnInit {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;
    public ObjectTableId: string;
    public EntityId: string;
  public EntityNumber: string;
  public ExternalEntityReference: string;
  public ExternalEntityName: string;
    constructor(private entityArgs: EntityArgs) {

    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
            if (table) this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            if (this.EntityPM.Direction == 'E') {

                this.ExternalEntityReference = this.EntityPM.ExportFile;
                if (this.EntityPM.TransferImporterId == 'O')
                    this.ExternalEntityName = 'MFIFILEM';
                else
                    this.ExternalEntityName = 'EFIFILEM';

                this.EntityNumber = this.EntityPM.CustomFileNo;
            }
          
        }
    }
}
