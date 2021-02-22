 

export class HybridLabelsBrandingDataService {

    public static DefaultBackground: string = "url('../../../Images/LoginScreen/map.png')"; 
    public static DefaultLoginProgress: string = "url('../../../Images/LoginScreen/screen_kids.jpg')"; 
    public static DefaultMainLogo: string = "'../../../Images/LoginScreen/header.jpg'"; 

    public static BackgroundImageURL: string = "";
    public static MainLogoURL: string = "";
    public static LoginProgressURL: string = "";
     

    constructor() {

    }
  

    private static GetImageFromBytes(ImageByte: any) {
        return "data:image/png;base64," + ImageByte;
    }
     
    private static GetImageFromStorage(ImgStorageKey: string) {
        return JSON.parse(localStorage.getItem(ImgStorageKey));
    } 
 
     

    public static GetBackgroundImageFromStorage() {

        let background = HybridLabelsBrandingDataService.GetImageFromStorage("BackgroundImage");
        if (background && background.Id != null) {
            this.BackgroundImageURL = "url(" + HybridLabelsBrandingDataService.GetImageFromBytes(background.Data) + ")";
        }
        else {
            this.BackgroundImageURL = this.DefaultBackground;

        }
        return this.BackgroundImageURL;
    }


    public static GetMainLogoFromStorage() {

        let StorageMainImage = HybridLabelsBrandingDataService.GetImageFromStorage("MainLogo");
        if (StorageMainImage && StorageMainImage.Id != null) {
            this.MainLogoURL = HybridLabelsBrandingDataService.GetImageFromBytes(StorageMainImage.Data);
        }
        else {
            this.MainLogoURL = this.DefaultMainLogo;
        }
        return this.MainLogoURL;
    }


    public static GetLoginProgressFromStorage() {

        let loginProcess = HybridLabelsBrandingDataService.GetImageFromStorage("LoginProgressImage");
        if (loginProcess && loginProcess.Id != null) {
            this.LoginProgressURL = "url(" + HybridLabelsBrandingDataService.GetImageFromBytes(loginProcess.Data) + ")";
        }
        else {
            this.LoginProgressURL = this.DefaultLoginProgress;

        }
        return this.LoginProgressURL;
    }
 
      
}
