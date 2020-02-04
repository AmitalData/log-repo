import {Component, OnInit, AfterViewInit}  from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ImageLibraryService} from '../../../../Common/Services/Others/ImageLibraryService';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ImageParameter} from '../../../../Infrastructure/DataContracts/ImageParameter';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
declare var UploadLogoFile, HideImage , SetImage, ArrayBufferToBase64: any;
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    moduleId: module.id,
    selector: 'UploadLogo',
    templateUrl: './UploadLogoComponent.html',
    providers: [ImageLibraryService]
})

export class UploadLogoComponent implements AfterViewInit {
    IsShowMessageComplate: boolean = false;
    IsShowProgressLoading: boolean = false;
    ShowUploadVerySmallLogo: boolean = false;

    SharedLogisticsLogoHtmlId: string = Guid.newGuid();
    MobilelogoHtmlId: string = Guid.newGuid();
    logoHtmlId: string = Guid.newGuid();
    SmalllogoHtmlId: string = Guid.newGuid();
    MobileLogoFileHtmlId: string = Guid.NewRandomString();
    SharedLogisticsLogoFileHtmlId: string = Guid.NewRandomString();

    LogoFileHtmlId: string = Guid.NewRandomString();
    LogoHelpText: string = TextCodeTranslator.Translate("Tenant.O.LogoHelpText");
    DemoMessageVisibility: boolean;
    ShowUploadSharedLogisLogo: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    IsHideAreaCloseButton: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _imageLibraryService: ImageLibraryService) {

        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(response=> {
        
        });

        if (FeatureLocator.HasFeaturePermession("General", "MOBILELOGO")) {
            this.ShowUploadVerySmallLogo = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICSLOGO")) {
            this.ShowUploadSharedLogisLogo = true;
        }



        if (SessionLocator.Tenant == 65) {
            this.DemoMessageVisibility = true;
       
            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                this.DemoMessageVisibility = false;
             
            }
        }

        this.CurrentSession.StartBusyIndicator("loading..."); 
    }

    ngAfterViewInit() {
        if (this.ShowUploadVerySmallLogo) this.LoadMobileLogo(false);
        if (this.ShowUploadSharedLogisLogo) this.LoadSharedLogtsitcsLogo(false);
        this.LoadLogo(false);
    
      

      }
  


    SetDataContext(dataContext: any) {

    }



    LoadLogo(isload: boolean) {

        if (isload) {
            this.CurrentSession.StartBusyIndicator("loading...");

        }
        this._imageLibraryService.DownloadFile("logo" + SessionInfo.LoggedUserTenant, "jpg", "logos", SessionInfo.LoggedUserTenant).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(this.logoHtmlId, result, false);
               
                } else {
                    this.CurrentSession.StopBusyIndicator();
                    HideImage(this.logoHtmlId);
                }
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                HideImage(this.logoHtmlId);
              
            }


            this._imageLibraryService.DownloadFile("smalllogo" + SessionInfo.LoggedUserTenant, "jpg", "logos", SessionInfo.LoggedUserTenant).subscribe((res: any) => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.StopBusyIndicator();

                if (!pmResponse.HasError) {
                    var result = pmResponse.Result;
                    if (result) {
                        SetImage(this.SmalllogoHtmlId, result, false);
                    } else HideImage(this.SmalllogoHtmlId);

                } else HideImage(this.SmalllogoHtmlId);

            });


        });

    }
    OpenUpLoadLogo() {

        document.getElementById(this.LogoFileHtmlId).click();

    }


    UploadogoFile(event: any) {

        var file: any = UploadLogoFile(this.LogoFileHtmlId);
        if (file && (file.type == "image/jpeg" || file.type == "image/jpg")) {
        this.IsShowMessageComplate = false;
        this.IsShowProgressLoading = true;
        this.ArrayBufferToBase64(file, "logo", 300, 300, "jpg", this);
        }
    }

    LoadMobileLogo(isload: boolean) {

        if (isload) {
            this.CurrentSession.StartBusyIndicator("Loading...");
        }
        this._imageLibraryService.DownloadFile("verysmalllogo" + SessionInfo.LoggedUserTenant, "png", "logos", SessionInfo.LoggedUserTenant).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(this.MobilelogoHtmlId, result, false);
                } else HideImage(this.MobilelogoHtmlId);
            } else HideImage(this.MobilelogoHtmlId);

            if (isload) {
                this.CurrentSession.StopBusyIndicator();
            }

        });

    }



    LoadSharedLogtsitcsLogo(isload: boolean) {

        if (isload) {
            this.CurrentSession.StartBusyIndicator("Loading...");
           
        }
        this._imageLibraryService.DownloadFile("sharedLogtsitcslogo" + SessionInfo.LoggedUserTenant, "png", "logos", SessionInfo.LoggedUserTenant).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SetImage(this.SharedLogisticsLogoHtmlId, result, false);
                } else HideImage(this.SharedLogisticsLogoHtmlId);
            } else HideImage(this.SharedLogisticsLogoHtmlId);

            if (isload) {
                this.CurrentSession.StopBusyIndicator();
            }

        });

    }




    











    OpenUpLoadMobileLogo() {
        document.getElementById(this.MobileLogoFileHtmlId).click();
    }
    UploadMobileLogoFile(event: any) {

        var file: any = UploadLogoFile(this.MobileLogoFileHtmlId);

        if (file && (file.type == "image/png" || file.type == "image/Png")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "verysmalllogo", 65, 65, "png", this);
        }

    }


    OpenUpLoadSharedLogisticsLogo() {
        document.getElementById(this.SharedLogisticsLogoFileHtmlId).click();
    }
    UploadSharedLogisticsLogoFile(event: any) {

        var file: any = UploadLogoFile(this.SharedLogisticsLogoFileHtmlId);

        if (file && (file.type == "image/png" || file.type == "image/Png")) {
            this.IsShowMessageComplate = false;
            this.IsShowProgressLoading = true;
            this.ArrayBufferToBase64(file, "sharedLogtsitcslogo", 65, 65, "png", this);
        }

    }









    SendBlockToServer(data: any, filename, widht: number, height: number, extension: string) {
        var filter = new ImageParameter();
        filter.Base64String = data;
        filter.FileName = filename;

        filter.BufferNumber = 0;
        filter.Tenant = SessionInfo.LoggedUserTenant;
        filter.Width = widht;
        filter.Height = height;
        filter.Extension = extension;
        filter.UploadMode = "CompanyLogos";
        this._imageLibraryService.UploadFile(filter).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            var result: any;
            if (!pmResponse.HasError) {
                 var result = pmResponse.Result;
                 if (result) {

                    if (filename == "logo") {
                        this.SendBlockToServer(filter.Base64String, "smalllogo", 150, 150, "jpg");
                    }
                    else {

                        this.IsShowMessageComplate = true;
                        this.IsShowProgressLoading = false;
                        if (filename == "logo" || filename == "smalllogo") {
                            this.LoadLogo(true);
                        }
                        else if (filename == "verysmalllogo" ) {
                            this.LoadMobileLogo(true);
                        } else if (filename == "sharedLogtsitcslogo") {
                            this.LoadSharedLogtsitcsLogo(true);
                        }

                    }


                }
            }
            this.CurrentSession.StopBusyIndicator();

        });

    }

    ArrayBufferToBase64(file: any, filename: any, widht: number, height: number, extension: string, viewmode: any) {

        if (file) {
            var reader: FileReader = new FileReader();

            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result=  ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewmode.SendBlockToServer(window.btoa(binary), filename, widht, height, extension);

            };

            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);



        }
    }





    SaveButtonClicked() {
        this.CloseButtonClicked();
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }




}
