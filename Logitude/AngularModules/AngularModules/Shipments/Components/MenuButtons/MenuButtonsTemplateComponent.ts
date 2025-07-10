import {Component, EventEmitter, Output, ComponentRef} from '@angular/core';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityPMServiceResponse} from '../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './MenuButtonsTemplateComponent.html',
})

export class MenuButtonsTemplateComponent extends BaseComponent {

    public EntityPM: any;
    public ObjectTableName: string;
    public DataContext: MenuButtonsTemplateComponent = this;
    public IsNotesStackPanelVisible: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ValidationWarningsList: string[] = [];
    public NotesHeader: string = "Notes";
    public EventNotes: string = "";
    public IsReasonStackPanel: boolean = false;
    public ActionStepsStateList: any;
    public IsConvertShipmentType: boolean = false; 
    public ComponentRef: ComponentRef<MenuButtonsTemplateComponent>;
    @Output() ReopenDone: EventEmitter<string> = new EventEmitter<string>();
    @Output() SaveClicked: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() SaveCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityPMService: EntityPMService) {
        super();
    }

    SetWindowArgs(args: MenuButtonsTemplateArgs) {
        this.EntityPM = args.EntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.IsNotesStackPanelVisible = args.IsNotesStackPanelVisible;
        this.NotesHeader = args.NotesHeader;
        this.EventNotes = args.EventNote;
        this.IsReasonStackPanel = args.IsReasonStackPanel;
        this.ActionStepsStateList = args.ActionStepsStateList;
        this.IsConvertShipmentType = args.IsConvertShipmentType;

        if (args.EnabledOkButton != null) {
            this.EnabledOkButton = args.EnabledOkButton;
        }

        this.ValidationErrorsList = args.ValidationErrorsList;
        this.ValidationWarningsList = args.ValidationWarningsList;
    }
    public EnabledOkButton: boolean = true;
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    OkButtonClicked() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.ReopenDone.emit(this.EventNotes);
            this.CurrentSession.CloseCurrentWindowEmit("confirm");
        }
    }

    SaveChanges() {
        if (this.EntityPM.IsDirty) {
            this.SaveEntityChanges(false);
        }

        else {
            this.SaveCompleted.emit(true);
        }
    }

    SaveChangesAndClose() {
        this.SaveEntityChanges(true);
    }

    private SaveEntityChanges(isClosing: boolean) {
        if (this.EntityPM.IsDirty) {

            this.CurrentSession.StartBusyIndicatorSaving();
            this.entityPMService.update(this.ObjectTableName, this.EntityPM).then((res: any) => {
                res.subscribe((response:any) => {

                    this.CurrentSession.StopBusyIndicator();

                    var mm: EntityPMServiceResponse = response;
                    if (!mm.HasError) {
                        this.ValidationErrorsList = [];
                        if (isClosing) {
                            this.DestroyEditControl();
                        }

                        else {
                            this.SaveCompleted.emit(true);
                        }
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                    }

                }, error => {
                    console.log("Error===========>", error);
                    this.CurrentSession.StopBusyIndicator();
                });
            });
        }
    }

    DestroyEditControl() {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    }    
}

export class MenuButtonsTemplateArgs {
    public EntityPM: any;
    public ObjectTableName: string;
    public IsNotesStackPanelVisible: boolean = false;
    public IsReasonStackPanel: boolean = false;
    public NotesHeader: string = "Notes";
    public EventNote: string = "";
    public ActionStepsStateList: any;
    public EnabledOkButton: boolean = true;
    public ValidationWarningsList: Array<string> = [];
    public ValidationErrorsList: Array<string> = [];
    public IsConvertShipmentType: boolean = false;   
}
