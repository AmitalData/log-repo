import { Component, OnInit } from '@angular/core';
import { LoginComponent } from '../../Components/LoginComponent'; 
import { LoginService } from '../../LoginService'; 
import { BrandingDataService } from '../Services/BrandingDataService'; 

@Component({
    selector: 'HybridLoginProcessComponent',
    moduleId: './Login/Components/',
    templateUrl: 'HybridLoginProcessComponent.html',
    styleUrls: ['HybridLoginProcessComponent.css']
})
export class HybridLoginProcessComponent extends LoginComponent implements OnInit {

    public authHeader; 
    public MainColor: string = null;
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = "";
    public LoginProgressImage: string = "";  
    public Id = "";
    public MainLogo: string = "";
    public ContactUsEmail: string = ""; 

    public show = true;
    constructor(
        private ss: LoginService ) {
        super(ss);
    }

    ngOnInit() { 
        this.GetHybridLabelsData();
    }
     
    GetHybridLabelsData() { 
        this.BackgroundImage = BrandingDataService.GetBackgroundImageFromStorage();
        this.MainLogo = BrandingDataService.GetMainLogoFromStorage();
        this.LoginProgressImage = BrandingDataService.GetLoginProgressFromStorage();  
    }
}
