import { Component, OnInit } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { DeclarationMamanSpecialActionPM } from '../../../../Customs/EntityPMs/DeclarationMamanSpecialActionPM';
import { CourierPendingReasonExtendedListService } from '../../../../Customs/Services/ExtendedLists/CourierPendingReasonExtendedListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditMamanStickerComponent.html',
})

export class AddEditMamanStickerComponent
    extends BaseComponent
    implements OnInit{

    public DataContext: any = this;
    public ObjectTableName: string = "Customs.DeclarationMamanSpecialAction";
    public EntityPM: DeclarationMamanSpecialActionPM;
    isWindowMode: boolean = true;
    isFromUnifreight: boolean = false;
    ValidationErrorsList: any[] = [];

    private _EntityResourceService: EntityResourceService = new EntityResourceService();
    private _CourierPendingReasonPMService: CourierPendingReasonPMService = new CourierPendingReasonPMService();

    constructor(public entityArgs: EntityArgs) {
        super();

        SessionLocator.CurrentSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
                this.EntityPM = new DeclarationMamanSpecialActionPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.isWindowMode = true;
            } 
        });
    }

    Loaded: boolean = false;
    ngOnInit() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            SessionLocator.CurrentSession.StopBusyIndicator();
            this.Loaded = true;
        });
    }


    //#region Properties
    public get MamanLabelText1() { return this.EntityPM.MamanLabelText1; }
    public set MamanLabelText1(newValue: string) {
        this.EntityPM.MamanLabelText1 = newValue;
    }

    public get MamanLabelText2() { return this.EntityPM.MamanLabelText2; }
    public set MamanLabelText2(newValue: string) {
        this.EntityPM.MamanLabelText2 = newValue;
    }

    public get MamanLabelText3() { return this.EntityPM.MamanLabelText3; }
    public set MamanLabelText3(newValue: string) {
        this.EntityPM.MamanLabelText3 = newValue;
    }

    public get MamanLabelText4() { return this.EntityPM.MamanLabelText4; }
    public set MamanLabelText4(newValue: string) {
        this.EntityPM.MamanLabelText4 = newValue;
    }

    public get MamanLabelText5() { return this.EntityPM.MamanLabelText5; }
    public set MamanLabelText5(newValue: string) {
        this.EntityPM.MamanLabelText5 = newValue;
    }

    //#endregion\

    OkButtonClicked() {

        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
            return;
        } 
            
        this._CourierPendingReasonPMService.insert(this.EntityPM).subscribe(myResult => {
            if (myResult.HasError) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                return;
            }
            this.CancelButtonClicked();
        });
        
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }


}
