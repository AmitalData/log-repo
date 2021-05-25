import {Component,ViewChildren,OnInit, ViewChild, ElementRef} from '@angular/core';
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
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TermsofUseService } from '../../../../Infrastructure/Services/WebServices/TermsofUseService';
import { TermsofUsePM } from '../../../../Common/EntityPMs/TermsofUsePM';
import { HybridPartnerListService } from '../../../../Common/Services/StandardLists/HybridPartnerListService';
import { HybridPartnerPM } from '../../../../Common/EntityPMs/HybridPartnerPM';
import { getLocaleDateTimeFormat } from '@angular/common';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';

declare var UploadLogoFile, HideImage, SetImage, ArrayBufferToBase64: any;
declare var querySelection, StringToBase64, resultToUnitArray: any;
@Component({
    
    templateUrl: './AddEditPrivateLabelsComponent.html',
})

export class AddEditPrivateLabelsComponent extends BaseComponent implements OnInit {
    public EntityPM: TenantManagmentPrivateLabelsPM;
    public DataContext: AddEditPrivateLabelsComponent = this;
    public ObjectTableName: string = "TenantManagmentPrivateLabels";
    public ValidationErrorsList: string[] = [];
    public IsNewEntity: boolean;
    //private EntityId: string = null;
    public DataImageMain: any;
    public DataImageSmall: any;
    private IsEditMode: boolean = false;
    LogoMainFileHtmlId: string = Guid.NewRandomString();
    LogoSmallFileHtmlId: string = Guid.NewRandomString();

    mainColorOpacity: number = 100;
    private mainColorCode: string;
    wrongMainColor: boolean = false;
    secondaryColorOpacity: number = 100;
    private secondaryColorCode: string;
    wrongSecondaryColor: boolean = false;
    public BackgroundImageId: string;
    public LoginImageId: string;
    public LoginProgressImageId: string;
    public ForgetPasswordImageId: string;
    public SelectedTabCode: string;

    TermsofUsePMLists: TermsofUsePMViewModel[]; 
    TermsofUseSelectedViewModel: TermsofUsePMViewModel;
    private termsofUseService: TermsofUseService = new TermsofUseService();
    private hybridPartnerListService: HybridPartnerListService = new HybridPartnerListService();
    private entityResourceService: EntityResourceService = new EntityResourceService();
    IsVisibile: boolean;
    public EntityId: number;
    public hybridPartner: HybridPartnerPM;
    public ParentTenant: number;

    public TermsOfUsePM: TermsofUsePM;
     
    VersionDocumentId: string = Guid.NewRandomString();


    @ViewChildren(LocationDirective) public AllLocations: LocationDirective; 
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
        this.SelectedTabCode = "TMM";
        this.EntityPM = new TenantManagmentPrivateLabelsPM();
        this.TermsOfUsePM = new TermsofUsePM();
        this.hybridPartner = new HybridPartnerPM();
         
    }
     

    ngOnInit() {
        this.SelectedTabCode = "TMM";
        this.UIProperties.SetRequired("HybridPartnerId", this.ObjectTableName, true); 
        this.GetHybridPartnerTermsOfUse();
 
    }


    // Upload Terms Of Use
    OpenUpLoadTemplateFile() {
        document.getElementById(this.VersionDocumentId).click();

    }

    FileName: string;
    UpLoadTemplateFileMethod(event: any) {

        var file = querySelection(this.VersionDocumentId);

        if (file) {
            var fileExtension = file.name.split('.')[1];
            this.FileName = file.name.split('.')[0];
            if (fileExtension) {
                if (fileExtension != "Pdf") {
                    this.ShowMessage("File extension must be pdf");
                } else {
                    this.ConvertArrayBufferToBase64(file, this); 
                }
            } 
        }

    }



    ViewFile(item: TermsofUsePMViewModel) {

            var documentName = item.DocumentId
            DownloadManager.DownloadPage(documentName);

    }


    ConvertArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }  
            viewmodel.createTermsOfUse(window.btoa(binary));
        };

        reader.onerror = function (e) {

        };
        reader.readAsArrayBuffer(file);

 
    }

    createTermsOfUse(file: any) {

        var termsofUsePM = new TermsofUsePM();
        termsofUsePM.FileData = file;
        termsofUsePM.Date = new Date();
        termsofUsePM.Tenant = this.ParentTenant;
        termsofUsePM.VersionDocumentName = this.FileName;

        this.InsertTermsOfUse(termsofUsePM);
         
    }

    private InsertTermsOfUse(termsofUsePM: TermsofUsePM) {
        this.termsofUseService.insert(termsofUsePM).subscribe((res: any) => {

            var response: ServiceResponse = res;
            var response: ServiceResponse = res;
            if (!response.HasError) {
                var myResult = response.Result;
                if (myResult) {
                    this.TermsofUsePMLists.push(new TermsofUsePMViewModel(termsofUsePM));
                }
            }
        });
    }

    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }


    GetHybridPartnerTermsOfUse() {
        this.TermsofUsePMLists = [];
        this.hybridPartnerListService.getSingle(this.EntityPM.HybridPartnerId).subscribe((res: any) => {

            var serviceResponse: ServiceResponse = res;
            if (!serviceResponse.HasError) {
                var result = serviceResponse.Result;
                if (result) {
                    this.hybridPartner = result
                    this.ParentTenant = this.hybridPartner.PartnerTenant;
                    this.GetTermsOfUse(this.ParentTenant);
                }
            }

        });

    }

    GetTermsOfUse(PartnerTenant) { 
        this.termsofUseService.GetTermOfUseByTenant(PartnerTenant).subscribe((res: any) => {

                var serviceResponse: ServiceResponse = res;
                if (!serviceResponse.HasError) {
                    var result = serviceResponse.Result;
                    if (result && result != null) {
                        result.forEach((item) => {
                            this.TermsofUsePMLists.push(new TermsofUsePMViewModel(item));
                        });
                    } 
                } 
            });
    }

    LoadData() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");

        this.TermsofUsePMLists = [];

        this.hybridPartnerListService.getSingle(this.EntityPM.HybridPartnerId).subscribe((res: any) => {

            var serviceResponse: ServiceResponse = res;
            if (!serviceResponse.HasError) {
                var result = serviceResponse.Result;
                if (result) {
                    this.hybridPartner = result
                }
            }
            else {
                if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show(serviceResponse.ErrorsArray[0]); 
                }  
            }

        });

        this.termsofUseService.GetTermOfUseByTenant(this.hybridPartner.PartnerTenant).subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {

                    myResult.forEach((item) => {
                        this.TermsofUsePMLists.push(new TermsofUsePMViewModel(item));
                    });

                }
                this.CurrentSession.CurrentWindow.StopBusyIndicator();

            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
               
        });
    }


    private InitializeImageIds() {
        this.BackgroundImageId = this.EntityPM.BackgroundImageId;
        this.LoginImageId = this.EntityPM.LoginImageId;
        this.LoginProgressImageId = this.EntityPM.LoginProgressImageId;
        this.ForgetPasswordImageId = this.EntityPM.ForgetPasswordImageId;
    }

    RemoveImage(name) {

        switch (name) {
            case "BackgroundImage": {
                this.EntityPM.BackgroundImageId = null;
                this.BackgroundImageId = null;
                break;
            }
            case "LoginImage": {
                this.EntityPM.LoginImageId = null;
                this.LoginImageId = null;
                break;
            }
            case "LoginProgressImage": {
                this.EntityPM.LoginProgressImageId = null;
                this.LoginProgressImageId = null;
                break;
            }
            case "ForgetPasswordImage": {
                this.EntityPM.ForgetPasswordImageId = null;
                this.ForgetPasswordImageId = null;
                break;
            }
            default: {
                //statements; 
                break;
            }
        }
    }
    private SetColorsFromEntity() {
        if (this.EntityPM.MainColor) {
            this.mainColorOpacity = this.GetOpacityFromRGBA(this.EntityPM.MainColor);
            this.mainColorCode = this.ConvertRGBAToHexColor(this.EntityPM.MainColor);
        }
        if (this.EntityPM.SecondaryColor) {
            this.secondaryColorOpacity = this.GetOpacityFromRGBA(this.EntityPM.SecondaryColor);
            this.secondaryColorCode = this.ConvertRGBAToHexColor(this.EntityPM.SecondaryColor);
        } 
    }

    GetOpacityFromRGBA(rgba: string) {
        var numbers = rgba.replace('rgba(', '').replace(')', '').replace(' ', '');
        var splittedNumbers = numbers.split(',');
        var opacity = parseFloat(splittedNumbers[3].trim());
        return opacity * 100;
    }

    ConvertRGBAToHexColor(rgba: string) {
        var numbers = rgba.replace('rgba(', '').replace(')', '').replace(' ', '');
        var splittedNumbers = numbers.split(',');
        var red = parseInt(splittedNumbers[0].trim());
        var green = parseInt(splittedNumbers[1].trim());
        var blue = parseInt(splittedNumbers[2].trim());
        var opacity = parseInt(splittedNumbers[3].trim());

        var r = red.toString(16);
        var g = green.toString(16);
        var b = blue.toString(16);

        if (r.length == 1)
            r = "0" + r;
        if (g.length == 1)
            g = "0" + g;
        if (b.length == 1)
            b = "0" + b;

        return "#" + r + g + b;
    }

    ConvertHexToRGBColor(hex: string, alpha: number) {
        if (hex && hex.length >= 7) {
            const r = parseInt(hex.slice(1, 3), 16);
            const g = parseInt(hex.slice(3, 5), 16);
            const b = parseInt(hex.slice(5, 7), 16);

            if (alpha) {
                return `rgba(${r}, ${g}, ${b}, ${alpha / 100})`;
            } else {
                return `rgb(${r}, ${g}, ${b})`;
            }
        } else {
            return 'rgba(0,0,0,1)';
        }
    }

    get MainColorOpacity() {
        return this.mainColorOpacity;
    }
    set MainColorOpacity(value: number) {
        this.mainColorOpacity = value;
        this.UpdateEntityMainColor();
    }
     
    private UpdateEntityMainColor() {
        this.EntityMainColor = this.ConvertHexToRGBColor(this.MainColorCode, this.MainColorOpacity);
    }

    public get MainColorCode(): string {
        return this.mainColorCode;
    }
    public set MainColorCode(hexColor: string) {
        this.mainColorCode = hexColor;
        this.ValidateMainColorCode(hexColor);
        this.UpdateEntityMainColor();
    }
     
    get SecondaryColorOpacity() {
        return this.secondaryColorOpacity;
    }
    set SecondaryColorOpacity(value: number) {
        this.secondaryColorOpacity = value;
        this.UpdateEntitySecondaryColor();
    }

    private UpdateEntitySecondaryColor() {
        this.EntitySecondaryColor = this.ConvertHexToRGBColor(this.SecondaryColorCode, this.SecondaryColorOpacity);
    }

    public get SecondaryColorCode(): string {
        return this.secondaryColorCode;
    }
    public set SecondaryColorCode(hexColor: string) {
        this.secondaryColorCode = hexColor;
        this.ValidateSecondaryColorCode(hexColor);
        this.UpdateEntitySecondaryColor();
    }

    ValidateHexCode(value: string, fieldName: string) {

        const regex = new RegExp('^#([a-fA-F0-9]{6})$');
        var valid: boolean = regex.test(value);
        if ((!valid || value.length > 9 || value.length < 7) && value != null) {
            this.UIProperties.SetValidity(fieldName, "TenantManagement", false, "this is not a valid hex code");
            return false;
        }
        else {
            this.UIProperties.SetValidity(fieldName, "TenantManagement", true, null);
            return true;
        }
    }


    private ValidateMainColorCode(hexColor: string) {
        if (!this.ValidateHexCode(hexColor, "MainColorCode"))
            this.wrongMainColor = true;
        else
            this.wrongMainColor = false;

      //  this.UpdateEditComponentValidationErrors();
    }

    private UpdateEditComponentValidationErrors() {
        if (this.wrongMainColor ) {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = false;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = ['Please enter valid color hex code'];
        } else {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = true;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
        }
    }

    get EntityMainColor() {
        return this.EntityPM.MainColor;
    }
    set EntityMainColor(value: string) {
        this.EntityPM.MainColor = value;

    }

    private ValidateSecondaryColorCode(hexColor: string) {
        if (!this.ValidateHexCode(hexColor, "SecondaryColorCode"))
            this.wrongSecondaryColor = true;
        else
            this.wrongSecondaryColor = false;

        //  this.UpdateEditComponentValidationErrors();
    }

    get EntitySecondaryColor() {
        return this.EntityPM.SecondaryColor;
    }
    set EntitySecondaryColor(value: string) {
        this.EntityPM.SecondaryColor = value;

    }

    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs.Entity;
        this.IsEditMode = true;
        this.EntityId = +this.EntityPM.Id;

        this.InitializeImageIds();
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response: any) => {
            this.IsVisibile = true;
            this.EntityPM = windowArgs.Entity; 
        });



        this.SetColorsFromEntity();
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


    BackgroundImageUploadedCompleted(imageId) {
        this.BackgroundImageId = imageId;
        this.EntityPM.BackgroundImageId = imageId;

    } 
 
   LoginImageImageUploadedCompleted(imageId) {
       this.LoginImageId = imageId;
       this.EntityPM.LoginImageId = imageId;

    }

    LoginProgressImageUploadedCompleted(imageId) {
        this.LoginProgressImageId = imageId;
        this.EntityPM.LoginProgressImageId = imageId;

    }

    ForgetPasswordImageUploadedCompleted(imageId) {
        this.ForgetPasswordImageId = imageId;
        this.EntityPM.ForgetPasswordImageId = imageId;

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

    get HasLogboxAccess() {
        return this.EntityPM.HasLogboxAccess;
    }
    set HasLogboxAccess(value: boolean) {
        if (value != this.EntityPM.HasLogboxAccess)
            this.EntityPM.HasLogboxAccess = value;
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
   
    get MainColor() {
        return this.EntityPM.MainColor;
    }

    set MainColor(value: string) {
        if (value != this.EntityPM.MainColor) {
            this.EntityPM.MainColor = value;
        }
    }
 
    get SecondaryColor() {
        return this.EntityPM.SecondaryColor;
    }

    set SecondaryColor(value: string) {
        if (value != this.EntityPM.SecondaryColor) {
            this.EntityPM.SecondaryColor = value;
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
        if (this.wrongMainColor) {
            errors.push("Please Enter Valid Main Color");
        }

        if (this.wrongSecondaryColor) {
            errors.push("Please Enter Valid Secondary Color");
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
class TermsofUsePMViewModel {


    Date: Date;
    VersionNumber: number;
    DocumentId: string;
    constructor(item: TermsofUsePM) {
        this.Date = item.Date;
        this.VersionNumber = item.VersionNumber;
        this.DocumentId = item.VersionDocumentId;
    }

}
