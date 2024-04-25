

export class PrivateLabelsBrandingDataService {

    public static DefaultBackground: string = "url('../../../Images/PrivateLabel/Background.png')";
    public static DefaultLoginProgress: string = "url('../../../Images/PrivateLabel/MainImage.png')";
    public static DefaultMainLogo: string = "../../../Images/PrivateLabel/LogBoxLogo.png";

    public static BackgroundImageURL: string = "";
    public static MainLogoURL: string = "";
    public static LoginProgressURL: string = "";
    public static LoginImageURL: string = "";

    constructor() {

    }

    private static GetImageFromBytes(ImageByte: any) {
        return "data:image/png;base64," + ImageByte;
    }

    private static GetImageFromStorage(ImgStorageKey: string) {
        return JSON.parse(localStorage.getItem(ImgStorageKey));
    }



    public static GetBackgroundImageFromStorage() {

        let background = PrivateLabelsBrandingDataService.GetImageFromStorage("BackgroundImage");
        console.log("BackgroundImage: " + background);

        if (background && background.Id != null) {
            console.log("BackgroundImage: Exist in local storage => " + background);
            this.BackgroundImageURL = "url(" + PrivateLabelsBrandingDataService.GetImageFromBytes(background.Data) + ")";
        }
        else {
            console.log("BackgroundImage: Not Exist in local storage ");
            //this.BackgroundImageURL = this.DefaultBackground;

        }
        return this.BackgroundImageURL;
    }

    public static GetMainLogoFromStorage() {

        let StorageMainImage = PrivateLabelsBrandingDataService.GetImageFromStorage("MainLogo");
        console.log("MainLogo: " + StorageMainImage);

        if (StorageMainImage && StorageMainImage.Id != null) {
            console.log("MainLogo: Exist in local storage => " + StorageMainImage);
            this.MainLogoURL = PrivateLabelsBrandingDataService.GetImageFromBytes(StorageMainImage.Data);
        }
        else {
            console.log("MainLogo: Not Exist in local storage ");
            // this.MainLogoURL = this.DefaultMainLogo;
        }
        return this.MainLogoURL;
    }


    public static GetLoginProgressFromStorage() {

        let loginProcess = PrivateLabelsBrandingDataService.GetImageFromStorage("LoginProgressImage");
        console.log("loginProcess: " + loginProcess);

        if (loginProcess && loginProcess.Id != null) {
            console.log("loginProcess: Exist in local storage => " + loginProcess);
            this.LoginProgressURL = "url(" + PrivateLabelsBrandingDataService.GetImageFromBytes(loginProcess.Data) + ")";
        }
        else {
            console.log("loginProcess: Not Exist in local storage ");
            //this.LoginProgressURL = this.DefaultLoginProgress;

        }
        return this.LoginProgressURL;
    }

    public static GetLoginImageFromStorage() {
        let loginImage = PrivateLabelsBrandingDataService.GetImageFromStorage("LoginImage");
        console.log("loginImage: " + loginImage);

        if (loginImage && loginImage.Id != null) {
            console.log("loginImage: Exist in local storage => " + loginImage);
            this.LoginImageURL = "url(" + PrivateLabelsBrandingDataService.GetImageFromBytes(loginImage.Data) + ")";
        }
        else {
            console.log("loginImage: Not Exist in local storage ");
            //this.LoginProgressURL = this.DefaultLoginProgress;

        }
        return this.LoginImageURL;
    }
}
