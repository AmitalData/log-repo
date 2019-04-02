import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CustomerPM} from '../../EntityPMs/CustomerPM';
import {CustomerSalesNotePM} from '../../EntityPMs/CustomerSalesNotePM';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {Validator} from '../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: "./AddEditCustomerSalesNoteComponent.html",
})

export class AddEditCustomerSalesNoteComponent extends BaseComponent {
    public CustomerPM: CustomerPM;
    public EntityPM: CustomerSalesNotePM;
    public ObjectTableName: string = "CustomerSalesNote";
    public DataContext = this;
    public IsNewEntity: boolean = false;    
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    private oldNotesField: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.CustomerPM = args['CustomerPM'];
        this.IsNewEntity = args['IsNewEntity'];
        this.oldNotesField = this.EntityPM.Notes;
        this.SetInfoData();
        this.Clone();

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
        });
    }


    public CreateByUser: string;
    public CreateDateString: string;
    public CreateByUserWidth: number;
    public CreateDateStringWidth: number;
    public UpdateByUser: string;
    public UpdateDateString: string;
    public UpdateByUserWidth: number;
    public UpdateDateStringWidth: number;
    SetInfoData() {
        this.CreateByUser = this.EntityPM.CreatedByUserName;
        this.UpdateByUser = this.EntityPM.UpdatedByUserName;
        this.CreateDateString = "(" + DateTool.GetDateFormats(this.EntityPM.CreateDate).DateString + ")";
        this.UpdateDateString = "(" + DateTool.GetDateFormats(this.EntityPM.UpdateDate).DateString + ")";

        var width: number = this.CurrentSession.CurrentWindow.Width - 10;
        this.CreateDateStringWidth = AppTool.GetTextWidth(this.CreateDateString, 11) + 5;
        this.UpdateDateStringWidth = AppTool.GetTextWidth(this.UpdateDateString, 11) + 5;

        var createByUserWidth: number = AppTool.GetTextWidth(this.CreateByUser, 12) + 5;
        var updateByUserWidth: number = AppTool.GetTextWidth(this.UpdateByUser, 12) + 5;

        var emptySpaceWidth_Create: number = width - (70 + this.CreateDateStringWidth);
        var emptySpaceWidth_Update: number = width - (70 + this.UpdateDateStringWidth);

        this.CreateByUserWidth = emptySpaceWidth_Create < createByUserWidth ? emptySpaceWidth_Create : createByUserWidth;
        this.UpdateByUserWidth = emptySpaceWidth_Update < updateByUserWidth ? emptySpaceWidth_Update : updateByUserWidth;
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get PostToFollowers() { return this.EntityPM.PostToFollowers; }
    set PostToFollowers(value: boolean) {
        if (this.EntityPM.PostToFollowers != value) {
            this.EntityPM.PostToFollowers = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length == 0) {
            if (AppTool.IsNullOrEmpty(this.Notes)) {
                errors.push("Notes is required");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.IsNewEntity) {
                this.CustomerPM.AddCustomerSalesNotePM(this.EntityPM);
            }

            else {
                if (this.oldNotesField != this.EntityPM.Notes) {
                    this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    this.EntityPM.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;

                    var maxDate: Date = null;
                    var maxDateTicks: number = 0;

                    if (this.CustomerPM.SalesNotes.length > 0) {
                        this.CustomerPM.SalesNotes.forEach(item => {
                            var itemDateTicks: number = DateTool.GetDateParts(item.UpdateDate).DateTicks;

                            if (itemDateTicks > maxDateTicks) {
                                maxDateTicks = itemDateTicks;
                                maxDate = item.UpdateDate;
                            }
                        });
                    }

                    if (maxDate == null) {
                        maxDate = DateTool.GetCurrentDateTimeAsUtc();
                    }

                    else {
                        maxDate = DateTool.AddHours(maxDate, 1);
                    }

                    this.EntityPM.UpdateDate = maxDate;
                }
            }

            this.CurrentSession.CloseCurrentWindowEmit("Ok");
            this.CurrentSession.FireEvent("CustomerSalesNotesChanged");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.CustomerPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
