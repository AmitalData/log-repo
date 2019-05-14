import { Component, OnInit } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { DeclarationMamanSpecialActionPM } from '../../../../Customs/EntityPMs/DeclarationMamanSpecialActionPM';
import { DeclarationMamanSpecialActionPMService } from '../../../../Customs/Services/StandardPMs/DeclarationMamanSpecialActionPMService';
import { DeclarationWebService } from '../../../../Customs/Services/WebServices/DeclarationWebService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditMamanStickerComponent.html',
})

export class AddEditMamanStickerComponent
    extends BaseComponent{

    public DataContext: any = this;
    public ObjectTableName: string = "Customs.DeclarationMamanSpecialAction";
    public EntityPM: DeclarationMamanSpecialActionPM;
    isWindowMode: boolean = true;
    ValidationErrorsList: any[] = [];
    IsLoaded: boolean = false;

    private _EntityResourceService: EntityResourceService = new EntityResourceService();
    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
    private _DeclarationMamanSpecialActionPMService: DeclarationMamanSpecialActionPMService = new DeclarationMamanSpecialActionPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.CurrentSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this.CurrentSession.StopBusyIndicator();
            this.IsLoaded = true;
        });
    }

    SetWindowArgs(entityArgs: any) {
        if (!AppTool.IsNullOrEmpty(entityArgs)) {
            this.isWindowMode = true;

            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
                this.EntityPM = new DeclarationMamanSpecialActionPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.EntityPM.DeclarationId = entityArgs.DeclarationId;
                this.EntityPM.MamanSpecialActionCode = "4";
            }
            else {
                this.EntityPM = entityArgs.EntityPM;
            }
        }
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
        this.CurrentSession.StartBusyIndicatorCreating();
        this._DeclarationMamanSpecialActionPMService.insert(this.EntityPM).subscribe(res => {
            this._DeclarationWebService.GetDeclarationMamanSpecialAction(this.EntityPM.DeclarationId, this.EntityPM.Tenant, "U", "4").subscribe(myResult => {
                if (myResult.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                    return;
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    var myMessageWindow = new MessageWindow();
                    myMessageWindow.Show(myResult.Result);
                }
                this.CancelButtonClicked();
            });
        });
        
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}
