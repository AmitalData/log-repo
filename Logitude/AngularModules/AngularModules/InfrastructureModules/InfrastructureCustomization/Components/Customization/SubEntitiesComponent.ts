import { Component } from '@angular/core';
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
const defaultWindowWidth = 600;
const defaultWindowHeight = 400;
const newTabWindowTitle = "New Custom Object";

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
    public SupportSubEntity: boolean = false;
    constructor() {
        this.customizationObjectTableService = new CustomizationObjectTableService();
        this._entityResourceService = new EntityResourceService();
        this.textCodeTranslationPipe = new TextCodeTranslationPipe();
    }

    SetWindowArgs(args: any) {
        this.ObjectTableId = args['ObjectTableId'];
        this.IsObjectTableFilterEnabled = args['IsObjectTableFilterEnabled'];
        this.enableAddCustomChildEntity = FeatureLocator.HasFeaturePermession("General", "AddCustomChildEntity");
        this.SupportSubEntity = window.ObjectTables.filter(o => o.Id == this.ObjectTableId)[0].SupportSubEntity;
        this.BuildSubEntitiesList();
    }
    private BuildSubEntitiesList() {
        this.SubEntitiesList = this.customizationObjectTableService.GetChildsById(this.ObjectTableId);
    }

    public ShowCustomizationEditComponentForSubEntity(objectTablePM: ObjectTablePM) {
        this.CurrentSession.StartBusyIndicator("Loading ...");

        if (objectTablePM.IsCustom) {
            this.ShowSubEntityComponent(objectTablePM);
            this.CurrentSession.StopBusyIndicator();
            return;
        }
        this._entityResourceService.getEntityResourceByTableName(objectTablePM.Name).subscribe((response: any) => {
            if (response.HasError) return;
            this.CurrentSession.StopBusyIndicator();
            this.ShowSubEntityComponent(objectTablePM);
        });
    }
    private ShowSubEntityComponent(objectTablePM: ObjectTablePM) {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.IsShowCloseButton = false;
        logWindow.Title = "";
        logWindow.WindowArgs = {
            Title: this.GetItemNameAfterTranslation(objectTablePM),
            IsCustomFieldsMenue: false,
            IsObjectTableFilterEnabled: this.IsObjectTableFilterEnabled,
            ObjectTableId: objectTablePM.Id,
            IsSubEntity: true
        };
        logWindow.Show('./InfrastructureCustomization/Components/Customization/CustomizationEditComponent');
    }
    GetItemNameAfterTranslation(objectTable: ObjectTablePM) {
        if (objectTable.IsNew) return objectTable.DefaultText;
        let objectTableNameAfterTranslation = this.textCodeTranslationPipe.transform(objectTable.Name);
        if (objectTableNameAfterTranslation) return objectTableNameAfterTranslation;
        return objectTable.Name;
    }

    AddNewSubEntity() {
        var window = new LogitudeWindow();
        window.Width = defaultWindowWidth;
        window.Height = defaultWindowHeight;
        window.Title = newTabWindowTitle;
        window.WindowArgs = {
            CustomizationSubEntitiesComponent: this
        };
        window.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddSubEntityComponent');
    }

    ApplyChanges(objectTablePM: ObjectTablePM) {
        this.SubEntitiesList.push(objectTablePM);
    }

    Save() {

    }
    Cancel() {

    }
}


