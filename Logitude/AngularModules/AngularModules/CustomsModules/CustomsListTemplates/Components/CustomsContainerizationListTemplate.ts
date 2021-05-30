import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralList } from '../../../Customs/EntityLists/CustomsCollateralList';
import { CustomsCollateralAnswerSharedDataService } from '../../../Customs/Services/DataChange/CustomsCollateralAnswerSharedDataService'
import { Declaration } from 'typescript';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { AppTool } from '../../../Infrastructure/Tools';


@Component({   
    templateUrl: 'CustomsContainerizationListTemplate.html',
})

export class CustomsContainerizationListTemplate {

    public CustomsContainerizationRecord: DeclarationList;
    public fieldName: any;
    public isAnswer: boolean;
    public isDisable: boolean;
     TableUpdateButtonIsEnabled: boolean = false;
    UpdateButtonVisibility: boolean = false;
    TableUpdateButtonOpacity: string = "1";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    //, private _customsCollateralAnswerSharedDataService: CustomsCollateralAnswerSharedDataService
    constructor(private CD: ChangeDetectorRef) {
     }

    setVariables(customsContainerizationRecord: DeclarationList, fieldName: string) {
        debugger;
        this.CustomsContainerizationRecord = customsContainerizationRecord;

        this.fieldName = fieldName;
         this.CD.detectChanges();
    }


    OnCheckedWithSystemEvent(eventM, id) {
        //eventM.stopPropagation();
        //if (!this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.includes(id)) {
        //    this._customsCollateralAnswerSharedDataService._SelectedItems.Insert(id);
        //}
        //else {
        //    var removedIndex = null;
        //    for (var i = 0; i < this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.length; i++) {
        //        if (id == this._customsCollateralAnswerSharedDataService._SelectedItems.Collection[i]) {
        //            removedIndex = i;
        //            break;
        //        }
        //    }
        //    if (removedIndex != null) {
        //        this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.splice(removedIndex, 1);
        //    }

        //}


      //  this._customsCollateralAnswerSharedDataService.IsDisplayButtonSend = (this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.length > 1);

    }



}
