declare var System: any;
import {AppTool} from '../../../Infrastructure/Tools';
import {ImageLibraryService} from '../../../Common/Services/Others/ImageLibraryService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Component, Input, AfterViewInit, OnInit, ChangeDetectorRef, EventEmitter, Output} from '@angular/core';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ContactPMService} from '../../../Common/Services/StandardPMs/ContactPMService';
import {ImageParameter} from '../../../Infrastructure/DataContracts/ImageParameter';
declare var UploadLogoFile, HideImage, SetImage, ShowHideProgressDownload, ArrayBufferToBase64: any;

@Component({
    moduleId: module.id,

    selector: 'ImageComponent',
    templateUrl: './ImageComponent.html',
    inputs: ['EntityId', 'ImageId', "EntityName", 'ImageId', 'WidthImage', 'HeightImage', 'ImageResizeWidth', 'ImageResizeHeight', 'HideBorder', 'DisplayOnly', 'ConversationHeaderId'],
    providers: [ImageLibraryService],
})


export class ImageComponent implements AfterViewInit, OnInit {
    ImageId: string;
    EntityId: string;
    EntityName: string;
    HideBorder: boolean = false;
    IsLoadingImage: boolean = false;
    ImageResizeWidth: number;
    ImageResizeHeight: number;
    DefultImageHeight: string = "auto";
    ImageKey: string = Guid.newGuid();
    ImageFileHtmlId: string = Guid.NewRandomString();
    private contactPMService: ContactPMService;
    ProgressDownloadId: string = Guid.newGuid();
    DataImage: any;
    DisplayOnly: boolean = false;
    Tooltip: string = "Click to add the photo";// 
    CursorImage: string = "pointer";
    BorderStyle: string = "1px solid #d3d3d3";
    WidthImage: string = "100%";
    HeightImage: string = "100%";
    ConversationHeaderId: string;
    HeightSocialImage: string = "";
    WidthSocialImage: string = "";
    ColSpanArea3: string = "";
    IsShowSocialMessageAreaImage1: boolean = true;
    IsShowSocialMessageAreaImage2: boolean = true;
    IsShowSocialMessageAreaImage3: boolean = true;
    IsShowSocialMessageAreaImage4: boolean = true;


    @Output() UploadCompleted: EventEmitter<any> = new EventEmitter();
    constructor(public _imageLibraryService: ImageLibraryService, private cd: ChangeDetectorRef) {

        this.contactPMService = new ContactPMService();



    }
    ngOnInit() {

        if (this.HideBorder) {
            this.BorderStyle = "";
        }

        if (this.DisplayOnly) {
            this.CursorImage = "";
            this.Tooltip = "";
        }
    }

    ngAfterViewInit() {


        if (this.EntityName == "SocialMessage" && !AppTool.IsNullOrEmpty(this.ConversationHeaderId)) {
            this.LoadParticipantsImageId();
        }


        else {
            if (!AppTool.IsNullOrEmpty(this.ImageId)) {
                this.ShowLogosIfExist(this.ImageId);
            }
            else {

                if (this.EntityName == "Contact") {
                    ShowHideProgressDownload(true, this.ProgressDownloadId);
                    this.contactPMService.get(this.EntityId).subscribe((myResponse: ServiceResponse) => {
                        ShowHideProgressDownload(false, this.ProgressDownloadId);
                        if (!myResponse.HasError) {
                            var contact = myResponse.Result;
                            if (contact) {

                                this.ImageId = contact.ImageDetailId;
                                this.cd.detectChanges();
                                if (!AppTool.IsNullOrEmpty(this.ImageId)) {
                                    this.ShowLogosIfExist(this.ImageId);
                                }
                            }
                        }
                    });
                }

                if (this.EntityName == "Quotation") {
                    this.DefultImageHeight = "250px";
                }


            }
        }
    }



    ShowLogosIfExist(imageId: string, isUseCach = true) {
        if (!imageId) imageId = this.ImageId;

        if (isUseCach) {
            var imageByte = SessionLocator.UserIcons[imageId];
            if (!imageByte) {
                this.GetImageFile(imageId);
            }
            else {
                ShowHideProgressDownload(false, this.ProgressDownloadId);
                SetImage(this.ImageKey, imageByte, true);
            }
        }
        else {
            this.GetImageFile(imageId, !isUseCach);
        }


        if (!this.DisplayOnly) {
            this.Tooltip = "Click to change the photo";
        }

        this.cd.detectChanges();
        // this.GetImageFile(imageId);
    }

    GetImageFile(imageId: string, isFirEvent: boolean = false) {

        var type = "Base64";
        if (this.EntityName == "Quotation") {
            type += ("^ImageDetail");
        }
        ShowHideProgressDownload(true, this.ProgressDownloadId);
        this._imageLibraryService.DownloadFile(this.ImageId, "jpg", "images", SessionInfo.LoggedUserTenant, type).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;

            ShowHideProgressDownload(false, this.ProgressDownloadId);
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SessionLocator.UserIcons[imageId] = result;
                    SetImage(this.ImageKey, result, true);
                    if (isFirEvent) this.UploadCompleted.emit(this.ImageId);


                }

            }

        });

    }


    OpenUpLoadLogo() {
        if (!this.DisplayOnly) {
            document.getElementById(this.ImageFileHtmlId).click();
        }

    }

    UploadogoFile(event: any) {
        if (this.EntityName == "Quotation") {
            this.DefultImageHeight = "auto";
        }

        var height: number = this.ImageResizeHeight ? this.ImageResizeHeight : 150;
        var width: number = this.ImageResizeWidth ? this.ImageResizeWidth : 150;
        var file: any = UploadLogoFile(this.ImageFileHtmlId);

        if (file) {

            var imageType: string = file.type ? file.type.toLowerCase() : "";
            if (imageType == "image/jpeg" || imageType == "image/jpg") {
                this.ArrayBufferToBase64(file, "images", width, height, this);
            } else if (this.EntityName == "Quotation" && file.type && imageType == "image/png") {
                this.ArrayBufferToBase64(file, "images", width, height, this);
            }
        }
    }



    ArrayBufferToBase64(file: any, filename: any, widht: number, height: number, viewmode: any) {

        if (file) {
            var reader: FileReader = new FileReader();
            var extension: string = "";
            var fileInfo = file.name.split('.');
  
            if (fileInfo.length > 1) {
                extension = fileInfo[fileInfo.length - 1];
            }
            else extension = fileInfo[1];

            if (extension) {
                extension = extension.toLowerCase();
            }
            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                ShowHideProgressDownload(true, viewmode.ProgressDownloadId);
                viewmode.SendBlockToServer(window.btoa(binary), filename, widht, height, extension);

            };

            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);



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

        filter.UploadMode = "ImageComponent";

        if (this.EntityName == "Customer") {
            filter.EntityId = this.EntityId;
            filter.ContactId = null;
            filter.Key = null;
        }
        else if (this.EntityName == "Contact") {

            filter.EntityId = null;
            filter.ContactId = this.EntityId;
            filter.Key = null;
        }
        else {
            filter.EntityId = null;
            filter.ContactId = null;
            filter.Key = this.ImageId;

        }


        this._imageLibraryService.UploadFile(filter).subscribe(res => {

            var pmResponse: ServiceResponse = res;
            var result: any;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (this.ImageId) {
                        var empty: any = "";
                        SessionLocator.UserIcons[this.ImageId] = empty
                    }

                    this.ImageId = result;

                    this.ShowLogosIfExist(this.ImageId, false);
                }
                else ShowHideProgressDownload(false, this.ProgressDownloadId);
            } else ShowHideProgressDownload(false, this.ProgressDownloadId);



        });

    }





    //SocialMessage
    ParticipantsImageId: string = "";
    ParticipantsImageIdList: string[] = [];
    IsShowOtherTextArea: boolean = false;
    OtherAreaText: string = "";
    LoadParticipantsImageId() {

        this._imageLibraryService.GetAllParticipantsConversationHeaderMessageId(this.ConversationHeaderId).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.ParticipantsImageId = pmResponse.Result;
                if (!AppTool.IsNullOrEmpty(this.ParticipantsImageId)) {
                    this.ParticipantsImageIdList = this.ParticipantsImageId.split(',');
                    this.BluidImage();
                }
            }

        });
    }
    IsDefultFontSize: boolean = false;
    ParticipantsImageList: string[][] = [];
    BluidImage() {
        if (this.ParticipantsImageIdList.length > 0) {

            if (this.ParticipantsImageIdList.length == 2) {

                this.HeightSocialImage = "100%";
                this.WidthSocialImage = "100%";

                var ParticipantsList1: string[] = this.ParticipantsImageIdList[0].split('_');
                var ParticipantsList2: string[] = this.ParticipantsImageIdList[1].split('_');
                if (ParticipantsList1[1] == SessionLocator.LoggedUserId) {
                    this.ShowSocialMessageLogosIfExist(ParticipantsList2[0], 1, ParticipantsList2[2], ParticipantsList2[3]);
                }
                else if (ParticipantsList2[1] == SessionLocator.LoggedUserId) {
                    this.ShowSocialMessageLogosIfExist(ParticipantsList1[0], 1, ParticipantsList1[2], ParticipantsList1[3]);
                }

                this.IsShowSocialMessageAreaImage2 = false;
                this.IsShowSocialMessageAreaImage3 = false;
                this.IsShowSocialMessageAreaImage4 = false;


            }

            else {

                this.HeightSocialImage = "20px";
                this.ColSpanArea3 = "1";

                if (!AppTool.IsNullOrEmpty(this.HeightImage)) {
                    var height = Number(this.HeightImage.replace("px", ""));
                    this.HeightSocialImage = (height / 2).toString() + "px";
                }
                if (!AppTool.IsNullOrEmpty(this.WidthImage)) {
                    var width = Number(this.WidthImage.replace("px", ""));
                    this.WidthSocialImage = (width / 2).toString() + "px";
                }



                this.IsDefultFontSize = true;
                this.ParticipantsImageIdList.forEach((item) => {
                    this.ParticipantsImageList.push(item.split('_'));
                });

                var count = 1;

                if (this.ParticipantsImageList.length > 4) {
                    this.IsShowOtherTextArea = true;
                    this.OtherAreaText = "+" + (this.ParticipantsImageList.length - 3).toString();
                }

                else {
                    this.IsShowSocialMessageAreaImage4 = false;
                }
                if (this.ParticipantsImageList.length < 2) {
                    this.IsShowSocialMessageAreaImage3 = false;
                }


                if (this.ParticipantsImageList.length == 3) {
                    this.ColSpanArea3 = "2";

                }


                this.ParticipantsImageList.forEach((item) => {
                    if (count < 5) {
                        this.ShowSocialMessageLogosIfExist(item[0], count, item[1], item[2]);
                    }
                    count += 1;
                });
            }

        }

    }



    ShowSocialMessageLogosIfExist(imageId: string, imageNumber: number, name: string, color: string) {
        if (!AppTool.IsNullOrEmpty(imageId)) {
            var imageByte = SessionLocator.UserIcons[imageId];
            if (!imageByte) {
                this.GetSocialMessageImageFile(imageId, imageNumber);
            }
            else {
                //ShowHideProgressDownload(false, this.ProgressDownloadId);
                this.SetSocialMessageImage(imageNumber, imageByte);
            }
        }
        else {
            this.SetSocialMessageAreaText(imageNumber, color, name);

        }

    }

    GetSocialMessageImageFile(imageId: string, imageNumber: number) {

        ShowHideProgressDownload(true, this.ProgressDownloadId);
        this._imageLibraryService.DownloadFile(imageId, "jpg", "images", SessionInfo.LoggedUserTenant, "Base64").subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;

            ShowHideProgressDownload(false, this.ProgressDownloadId);
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    SessionLocator.UserIcons[imageId] = result;
                    this.SetSocialMessageImage(imageNumber, result);
                }

            }

        });

    }



    IsShowSocialMessageImage1: boolean = true;
    IsShowSocialMessageImage2: boolean = true;
    IsShowSocialMessageImage3: boolean = true;
    IsShowSocialMessageImage4: boolean = true;


    SocialMessageImage1Key: string = Guid.newGuid();
    SocialMessageImage2Key: string = Guid.newGuid();
    SocialMessageImage3Key: string = Guid.newGuid();
    SocialMessageImage4Key: string = Guid.newGuid();





    SocialMessageImage1Color: string;
    SocialMessageImage2Color: string;
    SocialMessageImage3Color: string;
    SocialMessageImage4Color: string;

    SocialMessageImage1Text: string;
    SocialMessageImage2Text: string;
    SocialMessageImage3Text: string;
    SocialMessageImage4Text: string;

    SetSocialMessageImage(imageNumber: number, imageByte: any) {

        switch (imageNumber) {

            case 1:
                this.IsShowSocialMessageAreaImage1 = true;
                this.IsShowSocialMessageImage1 = true;
                SetImage(this.SocialMessageImage1Key, imageByte, true);
                break;

            case 2:
                this.IsShowSocialMessageAreaImage2 = true;
                this.IsShowSocialMessageImage2 = true;
                SetImage(this.SocialMessageImage2Key, imageByte, true);
                break;

            case 3:
                this.IsShowSocialMessageAreaImage3 = true;
                this.IsShowSocialMessageImage3 = true;
                SetImage(this.SocialMessageImage3Key, imageByte, true);
                break;

            case 4:
                this.IsShowSocialMessageAreaImage4 = true;
                this.IsShowSocialMessageImage4 = true;
                SetImage(this.SocialMessageImage4Key, imageByte, true);
                break;

            default:

                break;

        }
    }

    SetSocialMessageAreaText(imageNumber: number, color: string, nameCode: string) {

        switch (imageNumber) {

            case 1:
                this.IsShowSocialMessageAreaImage1 = true;
                this.IsShowSocialMessageImage1 = false;
                this.SocialMessageImage1Color = color;
                this.SocialMessageImage1Text = !AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";

                break;

            case 2:
                this.IsShowSocialMessageAreaImage2 = true;
                this.IsShowSocialMessageImage2 = false;
                this.SocialMessageImage2Color = color;
                this.SocialMessageImage2Text = !AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";
                break;

            case 3:
                this.IsShowSocialMessageAreaImage3 = true;
                this.IsShowSocialMessageImage3 = false;
                this.SocialMessageImage3Color = color;
                this.SocialMessageImage3Text = !AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";
                break;

            case 4:
                this.IsShowSocialMessageAreaImage4 = true;
                this.IsShowSocialMessageImage4 = false;
                this.SocialMessageImage4Color = color;
                this.SocialMessageImage4Text = !AppTool.IsNullOrEmpty(nameCode) ? nameCode.toUpperCase() : "";
                break;

            default:

                break;

        }
    }
}