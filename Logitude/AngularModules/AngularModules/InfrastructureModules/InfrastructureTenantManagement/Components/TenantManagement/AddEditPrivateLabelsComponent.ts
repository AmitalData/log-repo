import {Component,ViewChildren,OnInit} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagmentPrivateLabelsPM} from '../../../../Infrastructure/EntityPMs/TenantManagmentPrivateLabelsPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TenantManagmentPrivateLabelsPMService} from '../../../../Infrastructure/Services/StandardPMs/TenantManagmentPrivateLabelsPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';

declare var UploadLogoFile, HideImage, SetImage, ArrayBufferToBase64: any;

@Component({
    
    templateUrl: './AddEditPrivateLabelsComponent.html',
})

export class AddEditPrivateLabelsComponent extends BaseComponent implements OnInit {
    public EntityPM: TenantManagmentPrivateLabelsPM;
    public DataContext: AddEditPrivateLabelsComponent = this;
    public ObjectTableName: string = "TenantManagmentPrivateLabels";
    public ValidationErrorsList: string[] = [];
    public IsNewEntity: boolean;
    private EntityId: string = null;
    public DataImageMain: any;
    public DataImageSmall: any;
    private IsEditMode: boolean = false;
    LogoMainFileHtmlId: string = Guid.NewRandomString();
    LogoSmallFileHtmlId: string = Guid.NewRandomString();

    @ViewChildren(LocationDirective) public AllLocations: LocationDirective;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new TenantManagmentPrivateLabelsPM();          
    }

    ngOnInit() {
        this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, true);
    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs.Entity;
        this.IsEditMode = true;
       
        this.RunComponent();
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }


    private isViewInited = false;

    RunComponent() {


        if (this.AllLocations) {

            if (this.AllLocations) {
                this.isViewInited = true;
                this.InitializeComponent();
            }

            else {
                this.RunComponentTimer();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    IsShowMessageComplate: boolean = false;
    IsShowProgressLoading: boolean = false;

    public DemoMessageVisibility: boolean = false;
            public imageParameter: ImageParameter = new ImageParameter();

    UploadogoFileSmall(event: any) {

        var file: any = UploadLogoFile(this.LogoSmallFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.imageParameter = new ImageParameter();
            this.imageParameter.Extension = file.type.split('/')[1];

            this.ArrayBufferToBase64(file,  this,2);
        }

    }
    UploadogoFileMain(event: any) {

        var file: any = UploadLogoFile(this.LogoMainFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
            this.imageParameter = new ImageParameter();
            this.imageParameter.Extension = file.type.split('/')[1];
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, this, 1);

            }
        
    }

    MainLogoData: any;
    SmallLogoData: any;

    OpenUpLoadMainLogo() {
        document.getElementById(this.LogoMainFileHtmlId).click();
    }

    OpenUpLoadSmallLogo() {
        document.getElementById(this.LogoSmallFileHtmlId).click();
    }

    ArrayBufferToBase64(file: any, viewmode: any , imageIndex) {

        if (file) {
            var reader: FileReader = new FileReader();

            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                if (imageIndex == 1) viewmode.MainLogoData = window.btoa(binary);
                else viewmode.SmallLogoData = window.btoa(binary);
                viewmode.SetImage(imageIndex);

            };

            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);



        }
    }


    MainLogoImageHtmlId: string = Guid.newGuid();
    SmallLogoImageHtmlId: string = Guid.newGuid();

    LogoMainImageId: string;
    SetImage(imageIndex) {
        var service: ImageLibraryService = new ImageLibraryService();
        this.imageParameter.Base64String = imageIndex == 1 ? this.MainLogoData : this.SmallLogoData;
        this.imageParameter.Width = 239;
        this.imageParameter.Height = 85 ;
        service.PostImageAfterResize(this.imageParameter).subscribe((Result: ServiceResponse) => {
            if (!Result.HasError) {
                var image = "data:image/" + "jpg" + ";base64," + Result.Result.Base64String;
                imageIndex == 1 ? this.EntityPM.MainLogo = Result.Result.Base64String : this.EntityPM.SmallLogo = Result.Result.Base64String;
                SetImage(imageIndex == 1 ? this.MainLogoImageHtmlId : this.SmallLogoImageHtmlId, image, false);
            }

        });

       // LogoMainFileHtmlId
       // "data:image/" + documentExtension + ";base64,"
    }



    private InitializeComponent() {
        if (this.isViewInited) {
            if (!this.EntityPM)
                this.EntityPM = new TenantManagmentPrivateLabelsPM();
            else {
                this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, false);
                if (this.MainLogo != null) {
                    SetImage(this.MainLogoImageHtmlId, "data:image/" + "jpg" + ";base64," + this.MainLogo, false);
                }

                if (this.SmallLogo != null) {
                    SetImage(this.SmallLogoImageHtmlId, "data:image/" + "jpg" + ";base64," + this.SmallLogo, false);
                }     
            }
        }
    }

     get PrivateLabelName() {
        return this.EntityPM.PrivateLabelName;
    }
     set PrivateLabelName(value: string) {
         if (value != this.EntityPM.PrivateLabelName)
             this.EntityPM.PrivateLabelName = value;
     }

     get PrivateLabelShortName() {
         return this.EntityPM.PrivateLabelShortName;
     }
     set PrivateLabelShortName(value: string) {
         if (value != this.EntityPM.PrivateLabelShortName)
             this.EntityPM.PrivateLabelShortName = value;
     }

     get PrivateLabelUrl() {
         return this.EntityPM.PrivateLabelUrl;
     }
     set PrivateLabelUrl(value: string) {
         if (value != this.EntityPM.PrivateLabelUrl) {
             this.EntityPM.PrivateLabelUrl = value;

         }
    }

    get PrivateLabelDomain() {
        return this.EntityPM.PrivateLabelDomain;
    }
    set PrivateLabelDomain(value: string) {
        if (value != this.EntityPM.PrivateLabelDomain) {
            this.EntityPM.PrivateLabelDomain = value;

        }
    }

   

     get ContactUsEmail() {
         return this.EntityPM.ContactUsEmail;
     }
     set ContactUsEmail(value: string) {
         if (value != this.EntityPM.ContactUsEmail)
             this.EntityPM.ContactUsEmail = value;
     }

     get HybridPartnerId() {
         return this.EntityPM.HybridPartnerId;
     }
     set HybridPartnerId(value: string) {
         if (value != this.EntityPM.HybridPartnerId) {
             this.EntityPM.HybridPartnerId = value;
             if (value != null) {
                 this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, false);
             }
             else {
                 this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, true);                 
             }
         }
     }

     get ReceiveAllStatuses() {
         return this.EntityPM.ReceiveAllStatuses;
     }
     set ReceiveAllStatuses(value: boolean) {
         if (value != this.EntityPM.ReceiveAllStatuses)
             this.EntityPM.ReceiveAllStatuses = value;
     }

     get InActive() {
         return this.EntityPM.InActive;
     }
     set InActive(value: boolean) {
         if (value != this.EntityPM.InActive)
             this.EntityPM.InActive = value;
     }

     get MainLogo() {
         return this.EntityPM.MainLogo;
     }

     set MainLogo(value: string) {
         if (value != this.EntityPM.MainLogo) {
             this.EntityPM.MainLogo = value;
         }
     }

     get SmallLogo() {
         return this.EntityPM.SmallLogo;
     }

     set SmallLogo(value: string) {
         if (value != this.EntityPM.SmallLogo) {
             this.EntityPM.SmallLogo = value;
         }
     }
   




    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.HybridPartnerId == null || this.HybridPartnerId == "") {
            errors.push("Hybrid Partner Field is Required");
        }
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {        
               
            var service: TenantManagmentPrivateLabelsPMService = new TenantManagmentPrivateLabelsPMService();
            if (!this.IsEditMode) {
                service.insert(this.EntityPM).subscribe((response: ServiceResponse) => {

                    if (response) {
                        if (!response.HasError) {
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });
            }
            else {
                service.update(this.EntityPM).subscribe((response: ServiceResponse) => {

                    if (response) {
                        if (!response.HasError) {
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            this.ValidationErrorsList = response.ErrorsArray;
                        }
                    }
                });

            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('PackageCode');
        this.myCloner.AddField('NumberOfUsers');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
