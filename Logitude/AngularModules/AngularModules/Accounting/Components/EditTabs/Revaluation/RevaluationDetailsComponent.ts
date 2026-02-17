import {Component}  from '@angular/core';

import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {RevaluationPM} from '../../../EntityPMs/RevaluationPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {JournalExtendedListService} from '../../../Services/ExtendedLists/JournalExtendedListService';

import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';


@Component({
    moduleId: module.id,
    templateUrl: './RevaluationDetailsComponent.html',
})

export class RevaluationDetailsComponent extends BaseComponent {

    DataContext: any = this;
    ObjectTableName: string = "Revaluation";
    public EntityPM: RevaluationPM = null;
    Journals: ObservableCollection;
    JournalLines: ObservableCollection;
    journalExtendedListService: JournalExtendedListService = new JournalExtendedListService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    visible: boolean;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityResourceService.getEntityResourceByTableName("Journal").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("JournalLine").subscribe(response => {
                this.entityResourceService.getEntityResourceByTableName("Revaluation").subscribe(response => {
                    this.visible = true;
                    this.EntityPM = entityArgs.EntityPM;
                    this.Journals = new ObservableCollection([]);
                    this.JournalLines = new ObservableCollection([]);
                    this.GetJournals();

                    this.UIProperties.SetEnabled("GLAccountId", "Revaluation", false);
                    this.UIProperties.SetEnabled("Message", "Revaluation", false);
                    this.UIProperties.SetEnabled("RevaluationNumber", "Revaluation", false);
                    this.UIProperties.SetEnabled("ChartOfAccountsId", "Revaluation", false);
                    this.UIProperties.SetEnabled("Status", "Revaluation", false);
                    this.UIProperties.SetEnabled("RevaluationDate", "Revaluation", false);
                    this.UIProperties.SetEnabled("RevaluationEnabled", "Revaluation", false);

                });
            });
        });
    }

    get GLAccountId() { if (this.EntityPM) return this.EntityPM.GLAccountId; else return null }
    get Message() { if (this.EntityPM) return this.EntityPM.Message; else return null }
    get RevaluationNumber() { if (this.EntityPM) return this.EntityPM.RevaluationNumber; else return null }
    get ChartOfAccountsId() { if (this.EntityPM) return this.EntityPM.ChartOfAccountsId; else return null }
    get Status() { if (this.EntityPM) return this.EntityPM.Status; else return null }
    get RevaluationDate() { if (this.EntityPM) return this.EntityPM.RevaluationDate; else return null }
    get RevaluationEnabled() { if (this.EntityPM) return this.EntityPM.RevaluationEnabled; else return null }


    GetJournals() {

        this.journalExtendedListService.GetJournalsByAccountingEntityId(this.EntityPM.Id,"8").subscribe((myResponse: ServiceResponse) => {
            if (myResponse) {
                if (myResponse.Result) {
                    this.Journals.InsertCollection(myResponse.Result);
                    if(this.Journals.Length >0)
                    this.OnRowSelected(this.Journals.Collection[0]);
                }
            }

        });
    }

    JournalHyperlinkClicked(item: any)
    {
        this.EditEntity("Journal", item.Id, null, "JNDT");
    }

    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {
   

        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
       
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(res => {

        

        });

    }
    public SelectedRow: any = null;
    OnRowSelected(item: any) {
     //   GetJournalLinesByJournalId
        this.SelectedRow = item;
        this.journalExtendedListService.GetJournalLinesByJournalId(item.Id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse) {
                if (myResponse.Result) {
                    this.JournalLines.InsertCollection(myResponse.Result);
                }
            }

        });

    }

}