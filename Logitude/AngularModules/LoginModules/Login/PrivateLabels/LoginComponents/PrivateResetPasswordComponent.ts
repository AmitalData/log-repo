import { Component } from '@angular/core'; 
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';  
import { SessionInfo } from '../../SessionInfo';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { PrivateLabelsBrandingDataService } from '../Services/PrivateLabelsBrandingDataService';

@Component({
    selector: 'PrivateResetPasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'PrivateResetPasswordComponent.html',
    styleUrls: ['PrivateResetPasswordComponent.css']
})
export class PrivateResetPasswordComponent extends ResetPasswordComponent {
      
    public authHeader;
    private privateUrl;
    public MainColor: string = null;
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = "";
    public Id = "";
    public MainLogo: string = "";
    public ContactUsEmail: string = sessionStorage.getItem('ContactEmail'); 
    constructor(
        private ss: LoginService) {
        super(ss);
    }

    ngOnInit() {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsData();
         
    }
     
    GetPrivateLabelsData() {   
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage"); 
        this.MainLogo = BrandingDataService.GetImage("MainLogo");   
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");       
     } 
  }
 