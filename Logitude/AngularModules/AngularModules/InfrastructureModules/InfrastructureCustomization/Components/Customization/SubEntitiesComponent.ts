import {Component} from '@angular/core';
import { TextCodeTranslationPipe } from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { FieldsTranslations, GeneralDomainService } from '../../../../Infrastructure/Services/GeneralDomainService';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectTables } from '../../../../Workflow/Models/ObjectTables';
import { CustomizationObjectTableService } from '../../ExternalService/CustomizationObjectTableService';
import { CustomizationEditComponent } from './CustomizationEditComponent';

declare var window: any;

@Component({
    
    templateUrl: './SubEntitiesComponent.html',
})

export class SubEntitiesComponent {

    private CurrentSession = SessionLocator.SelectedSession;
    public customizationEditComponent: CustomizationEditComponent;
    public ObjectTableId: string;
    public IsObjectTableFilterEnabled: boolean;
    private customizationObjectTableService: CustomizationObjectTableService;
    public SubEntitiesList: Array<ObjectTablePM>;
    private _entityResourceService: EntityResourceService;
    private textCodeTranslationPipe: TextCodeTranslationPipe;
    public enableAddCustomChildEntity: boolean = false;
    constructor() {
        this.customizationObjectTableService = new CustomizationObjectTableService();
        this._entityResourceService = new EntityResourceService();
        this.textCodeTranslationPipe = new TextCodeTranslationPipe();
    }

    SetWindowArgs(args: any) {
        this.ObjectTableId = args['ObjectTableId'];
        this.IsObjectTableFilterEnabled = args['IsObjectTableFilterEnabled'];
        this.enableAddCustomChildEntity = FeatureLocator.HasFeaturePermession("General", "AddCustomChildEntity");
        this.BuildSubEntitiesList();
    }
    private BuildSubEntitiesList() {
        this.SubEntitiesList = this.customizationObjectTableService.GetChildsById(this.ObjectTableId);
    }

    public ShowCustomizationEditComponentForSubEntity(objectTableId: string, objectTableName: string) {
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this._entityResourceService.getEntityResourceByTableName(objectTableName).subscribe((response: any) => {
            if (response.HasError) return;
            this.CurrentSession.StopBusyIndicator();
            this.ShowSubEntityComponent(objectTableId, objectTableName);
        });
    }
    private ShowSubEntityComponent(objectTableId: string, objectTableName:string) {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.IsShowCloseButton = false;
        logWindow.Title = "";
        logWindow.WindowArgs = {
            Title: this.textCodeTranslationPipe.transform(objectTableName),//depend on chosen entity to edit
            IsCustomFieldsMenue: false,
            IsObjectTableFilterEnabled: this.IsObjectTableFilterEnabled,
            ObjectTableId: objectTableId,
            IsSubEntity: true
        };
        logWindow.Show('./InfrastructureCustomization/Components/Customization/CustomizationEditComponent');
    }

    AddNewSubEntity() {

    }

    Save() {

    }
    Cancel() {

    }
}


