import {Component, EventEmitter, Output, ComponentRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityPMServiceResponse} from '../../../../Infrastructure/DataContracts/EntityPMServiceResponse';
import {EntityPMService} from '../../../../Infrastructure/Services/EntityPMService';

@Component({
    moduleId: module.id,
    templateUrl: './ActionStepsTemplate.html',
})

export class ActionStepsTemplate extends BaseComponent {

    public EntityPM: any;
    public ObjectTableName: string;
    public DataContext: ActionStepsTemplate = this;
    public IsNotesStackPanelVisible: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ValidationWarningsList: string[] = [];
    public NotesHeader: string = "Notes";
    public EventNotes: string = "";
    public IsReasonStackPanel: boolean = false; 

    public ComponentRef: ComponentRef<ActionStepsTemplate>;

    @Output() SaveClicked: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() SaveCompleted: EventEmitter<boolean> = new EventEmitter < boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor( private entityPMService: EntityPMService) {
        super();
    }

    SetWindowArgs(args: ActionStepsTemplateArgs) {
        this.EntityPM = args.EntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.IsNotesStackPanelVisible = args.IsNotesStackPanelVisible;
        this.NotesHeader = args.NotesHeader;
        this.EventNotes = args.EventNote;
        this.IsReasonStackPanel = args.IsReasonStackPanel;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            //this.SaveClicked.emit(true);
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
            this.entityPMService.update(this.ObjectTableName, this.EntityPM).then((res:any)  => {
                res.subscribe(response => {

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

export class ActionStepsTemplateArgs {
    public EntityPM: any;
    public ObjectTableName: string;
    public IsNotesStackPanelVisible: boolean = false;
    public IsReasonStackPanel: boolean = false;
    public NotesHeader: string = "Notes";
    public EventNote: string = "";
}
