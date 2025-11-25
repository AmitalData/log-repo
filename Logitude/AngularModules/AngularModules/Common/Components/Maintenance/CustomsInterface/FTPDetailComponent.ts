import {Component} from '@angular/core';
import {FTPDetailPM} from '../../../EntityPMs/FTPDetailPM';
import {FTPDetailPMService} from '../../../Services/StandardPMs/FTPDetailPMService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './FTPDetailComponent.html',
})

export class FTPDetailComponent extends BaseComponent {
    public EntityPM: FTPDetailPM;
    public ObjectTableName: string = "FTPDetail";
    public DataContext = this;
    public Code: string;
    public IsNew: boolean;
    public ShowInactive: boolean = false;
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    private myService: FTPDetailPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityResourceService: EntityResourceService) {
        super();
    }

    private isINTTRA: boolean = false;
    SetWindowArgs(args: any) {
        this.Code = args['Code'];
        this.IsNew = args['IsNew'];
        this.isINTTRA = args["IsINTTRA"];

        this.ShowInactive = !this.IsNew;
        this.myService = new FTPDetailPMService();

        if (this.Code) {
            this.Code = this.Code.toUpperCase();
        }

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            if (this.IsNew) {
                this.EntityPM = new FTPDetailPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
                this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
                this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;


                if (this.isINTTRA == true) {

                    var isTestMode = args["IsTestMode"];

                    if (this.Code == "OUT") {
                        this.EntityPM.Folder = "inbound";
                    }

                    else {
                        this.EntityPM.Folder = "outbound";
                    }

                    if (isTestMode == true) {
                        this.EntityPM.Host = "ftp.cvt.inttra.com";
                    }

                    else {
                        this.EntityPM.Host = "ftp.inttraworks.inttra.com";
                    }
                }

                this.IsResourcesReady = true;
                this.Clone();
            }

            else {
                var entityId = args['EntityId'];

                this.myService.get(entityId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }

                    else {
                        this.EntityPM = myResponse.Result;
                    }

                    this.IsResourcesReady = true;
                    this.Clone();
                });
            }
        });
    }
    
    get UserName() { return this.EntityPM.UserName; }
    set UserName(value: string) {
        if (this.EntityPM.UserName != value) {
            this.EntityPM.UserName = value;
        }
    }

    get Password() { return this.EntityPM.Password; }
    set Password(value: string) {
        if (this.EntityPM.Password != value) {
            this.EntityPM.Password = value;
        }
    }

    get Host() { return this.EntityPM.Host; }
    set Host(value: string) {
        if (this.EntityPM.Host != value) {
            this.EntityPM.Host = value;
        }
    }

    get Folder() { return this.EntityPM.Folder; }
    set Folder(value: string) {
        if (this.EntityPM.Folder != value) {
            this.EntityPM.Folder = value;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    get PrivateKey() { return this.EntityPM.PrivateKey; }
    set PrivateKey(value: string) {
        if (this.EntityPM.PrivateKey != value) {
            this.EntityPM.PrivateKey = value;
        }
    }

    get Port() { return this.EntityPM.Port; }
    set Port(value: string) {
        if (this.EntityPM.Port != value) {
            this.EntityPM.Port = value;
        } 
    }
    

  get UseSFTP() { return this.EntityPM.UseSFTP; }
  set UseSFTP(value: boolean) {
    if (this.EntityPM.UseSFTP != value) {
      this.EntityPM.UseSFTP = value;
    }
  }


    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService: FTPDetailPMService = new FTPDetailPMService();

            if (this.IsNew) {
                myService.insert(this.EntityPM).subscribe((myResult:any) => {
                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }

            else {
                myService.update(this.EntityPM).subscribe((myResult:any) => {
                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('UserName');
        this.myCloner.AddField('Password');
        this.myCloner.AddField('Host');
        this.myCloner.AddField('Folder');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
