import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimImporterDeclarsP3LoiPM } from '../../../../../Customs/EntityPMs/ClaimImporterDeclarsP3LoiPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';


@Component({
    
    templateUrl: './ClaimImporterDeclAP3LoisComponent.html',
})

export class ClaimImporterDeclAP3LoisComponent extends BaseComponent {
  public IsDisplayOnly: boolean = false;

    public DataContext: ClaimImporterDeclAP3LoisComponent = this;
    public EntityPM: ClaimImporterDeclarsP3LoiPM = new ClaimImporterDeclarsP3LoiPM(null);
    public ObjectTableName: string = "Customs.ClaimImporterDeclarsP3Loi";

    public ClaimImporterDeclarsP3Loilist: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;

    ValidationErrors: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.ValidationErrors = [];
        this.ClaimImporterDeclarsP3Loilist = new ObservableCollection([]);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

        this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsP3Loi").subscribe((response:any) => { });
    }

    SetWindowArgs(args: any) {
        //this.ClaimImporterDeclarsP3Loilist = args.ClaimImporterDeclarsP3Loilist;
        if (args.ClaimImporterDeclarsP3Loilist != null && args.ClaimImporterDeclarsP3Loilist.Collection.length > 0) {
            for (let item of args.ClaimImporterDeclarsP3Loilist.Collection) {
                this.ClaimImporterDeclarsP3Loilist.Insert(new DeclarationNumberComponent(item));
            }
        }
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }
    FooterMethods: any;

    AddNewImporterDeclarsP3LoiCommand() {
        if (!this.IsControlEnabled) return;

        this.ClaimImporterDeclarsP3Loilist.Insert(new DeclarationNumberComponent(""));
    }

    DeleteImporterDeclarsP3LoiCommand(item: DeclarationNumberComponent) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClaimImporterDeclarsP3Loilist.Remove(item);
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    OkButtonClicked() {
        let listOfDeclarations: string = null;
        for (let item of this.ClaimImporterDeclarsP3Loilist.Collection) {
            if (!AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
                listOfDeclarations = listOfDeclarations + "," + item.DeclarationNumber;
            }
        }
        this.CurrentSession.CloseCurrentWindowEmit(listOfDeclarations);
    }

    //#endregion
}

export class DeclarationNumberComponent extends BaseComponent {
    public DataContext: DeclarationNumberComponent = this;
    private _DeclarationNumber: string;

    constructor(declarationNumber: string) {
        super();
        this._DeclarationNumber = declarationNumber; 
    }

    public get DeclarationNumber() { return this._DeclarationNumber; }
    public set DeclarationNumber(newValue: string) { this._DeclarationNumber = newValue; }

}
