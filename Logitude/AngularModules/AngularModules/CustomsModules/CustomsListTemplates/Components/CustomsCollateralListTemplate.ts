import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralList } from '../../../Customs/EntityLists/CustomsCollateralList';
import { CustomsCollateralAnswerSharedDataService } from '../../../Customs/Services/DataChange/CustomsCollateralAnswerSharedDataService'


@Component({   
    templateUrl: 'CustomsCollateralListTemplate.html',
})

export class CustomsCollateralListTemplate {

    public _CustomsCollateralRecord: CustomsCollateralList;
    public fieldName: any;
    public isAnswer: boolean;
    public isDisable: boolean;
    //@Output() selectItem: EventEmitter<any> = new EventEmitter();
    TableUpdateButtonIsEnabled: boolean = false;
    UpdateButtonVisibility: boolean = false;
    TableUpdateButtonOpacity: string = "1";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, private _customsCollateralAnswerSharedDataService: CustomsCollateralAnswerSharedDataService) {
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;

    }

    setVariables(CustomsCollateralRecord: CustomsCollateralList, fieldName: string) {
        ///console.log(rowData);
        this._CustomsCollateralRecord = CustomsCollateralRecord;



        this.fieldName = fieldName;
        this.isDisable = (this._CustomsCollateralRecord.IsAnswer)|| this._CustomsCollateralRecord.IsClosed;//|| (this._CustomsCollateralRecord.CollateralRequestStatusCode != '1' && this._CustomsCollateralRecord.CollateralRequestStatusCode != null);
        //|| (this._CustomsCollateralRecord.CollateralRequestStatusCode != '1' && this._CustomsCollateralRecord.CollateralRequestStatusCode != '' && this._CustomsCollateralRecord.CollateralRequestStatusCode != null
        this.CD.detectChanges();
    }


    OnCheckedWithSystemEvent(eventM, id) {
        eventM.stopPropagation();
        if (!this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.includes(id)) {
            this._customsCollateralAnswerSharedDataService._SelectedItems.Insert(id);
        }
        else {
            var removedIndex = null;
            for (var i = 0; i < this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.length; i++) {
                if (id == this._customsCollateralAnswerSharedDataService._SelectedItems.Collection[i]) {
                    removedIndex = i;
                    break;
                }
            }
            if (removedIndex != null) {
                this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.splice(removedIndex, 1);
            }

        }


        this._customsCollateralAnswerSharedDataService.IsDisplayButtonSend = (this._customsCollateralAnswerSharedDataService._SelectedItems.Collection.length > 1);

    }



}
