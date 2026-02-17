import {Component, EventEmitter, Output} from '@angular/core';
import {ClaimPM} from "../../../../Customs/EntityPMs/ClaimPM";
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ClaimsRelatedEntityPM} from "../../../../Customs/EntityPMs/ClaimsRelatedEntityPM";
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CustomsDocumentsTicketPM} from "../../../../Customs/EntityPMs/CustomsDocumentsTicketPM";
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './PointersFromClaimRelatedEntitiesSelectionComponent.html',
})

export class PointersFromClaimRelatedEntitiesSelectionComponent {
    public ClaimPM: ClaimPM;
    ClaimRelatedEntitiesList: ObservableCollection;
    public SelectedClaimRelatedEntities: ObservableCollection;
    entityResourceService: EntityResourceService = new EntityResourceService();
    CustomsDocumentsTicket: CustomsDocumentsTicketPM;
    public ConnectedClaimRelatedEntities: string;
    public IsTicketChanged: boolean;
    IsDisplayOnly: boolean;
    DisplayOnlyMessage: string;
    filterAgrs: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ClaimRelatedEntitiesList = new ObservableCollection([ClaimsRelatedEntityPM]);
        this.SelectedClaimRelatedEntities = new ObservableCollection([]);
    }

    IsVisibile: boolean = false;
    SetWindowArgs(args: any) {
        this.ClaimPM = args.ClaimPM;
        this.CustomsDocumentsTicket = args.CustomsDocumentsTicket;
        this.BuildClaimsRelatedEntitiesList();
        if (this.CustomsDocumentsTicket.RequestedCustomsDocId) {
            this.IsDisplayOnly = true;
            this.DisplayOnlyMessage = "מסמך לתצוגה בלבד";
        }
        else {
            this.IsDisplayOnly = false;
            this.DisplayOnlyMessage = "";
        }
    }

    BuildClaimsRelatedEntitiesList() {

        this.ClaimRelatedEntitiesList.Clear();
        for (var item of this.ClaimPM.ClaimsRelatedEntities) {
            this.ClaimRelatedEntitiesList.Insert(item);
        }

        this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((pointer) => {
            var CRE: ClaimsRelatedEntityPM = this.ClaimRelatedEntitiesList.Collection.filter(d => d.EntityCounterKey + "" == pointer.Child1EntityId)[0];
            if (CRE) {
                if (!this.SelectedClaimRelatedEntities.Collection.includes(CRE)) {
                    this.SelectedClaimRelatedEntities.Collection.push(CRE);
                }
            }
        });

    }


    SelectedRow: ClaimsRelatedEntityPM;
    SelectedRows: ClaimsRelatedEntityPM[];
    preventSelect: boolean;
    OnRowSelected(items: ClaimsRelatedEntityPM[]) {
        this.SelectedRows = items;
        this.SelectedClaimRelatedEntities.Collection.forEach((CRELine) => {
            var item = items.filter(d => d.EntityCounterKey == CRELine.EntityCounterKey)[0];
            if (!item) {
                this.SelectedClaimRelatedEntities.Remove(CRELine);
            }
        });

    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
      
        this.ConnectedClaimRelatedEntities = "";
        this.SelectedClaimRelatedEntities.Collection.forEach((item) => {
            this.ConnectedClaimRelatedEntities = this.ConnectedClaimRelatedEntities + "," + item.EntityCounterKey;
        });
        this.ConnectedClaimRelatedEntities = this.ConnectedClaimRelatedEntities.substr(1, this.ConnectedClaimRelatedEntities.length - 1);


        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

  


   
   
}


