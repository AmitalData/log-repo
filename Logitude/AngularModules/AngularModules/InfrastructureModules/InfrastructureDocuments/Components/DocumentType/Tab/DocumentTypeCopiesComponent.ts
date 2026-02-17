declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypeCopyPM} from '../../../../../Common/EntityPMs/DocumentTypeCopyPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentCopiesViewModel} from '../../DocumentComponent/DocsOut/ViewModel/DocumentCopiesViewModel';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    selector: 'DocumentTypeCopiesTab',
    templateUrl: './DocumentTypeCopiesComponent.html',
})

export class DocumentTypeCopiesComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;

    SelectedDocumentCopiesViewModel: DocumentCopiesViewModel;
    public DocumentTypeCopiesLists: DocumentCopiesViewModel[];
    public IsVisibile: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(public entityArgs: EntityArgs) {
        super();


    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeCopy", 0).subscribe(response => {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {
                this.Run();

            }
        });



    }



    Run() {

        if (this.EntityPM.DocumentTypeCopies) {
            this.DocumentTypeCopiesLists = [];
            this.EntityPM.DocumentTypeCopies.forEach((copy) => {

                this.DocumentTypeCopiesLists.push(new DocumentCopiesViewModel(copy, null, null, null, null, null, null, null));
            });

       
        }




    }


    CheckboxIsSelectedByDefaultClick(selectedItem: DocumentCopiesViewModel, value:any) {

        if (selectedItem != null) {
            selectedItem.IsSelectedByDefault = value;
            if (selectedItem.CurrentDocumentTypeCopy) {
                selectedItem.CurrentDocumentTypeCopy.IsSelectedByDefault = value;
            }
        }
        

        //selectedItem.IsSelectedByDefault = this.EntityPM.DocumentTypeCopies.filter(d=> d.Id == selectedItem.Id)[0].IsSelectedByDefault = !selectedItem.IsSelectedByDefault;
    }









}






