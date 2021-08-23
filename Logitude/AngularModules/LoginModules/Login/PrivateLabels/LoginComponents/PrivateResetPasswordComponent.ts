import { Component } from '@angular/core'; 
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';   
import { BrandingDataService } from '../Services/BrandingDataService'; 

@Component({
    selector: 'PrivateResetPasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'PrivateResetPasswordComponent.html',
    styleUrls: ['PrivateResetPasswordComponent.css']
})
export class PrivateResetPasswordComponent extends ResetPasswordComponent {
      
    public authHeader;  
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = "";
    public Id = "";
    public MainLogo: string = "";
    public ContactUsEmail: string = "mailto:" + sessionStorage.getItem('ContactEmail'); 
    public SecondaryColor: string = null;

    constructor(
        private ss: LoginService) {
        super(ss);
    }

    ngOnInit() { 
        this.GetPrivateLabelsData();
         
    }
     
    GetPrivateLabelsData() {   
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage"); 
        this.MainLogo = BrandingDataService.GetImage("MainLogo");   
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage"); 
        this.SecondaryColor = BrandingDataService.GetColor("SecondaryColor");  
     } 
  }
 