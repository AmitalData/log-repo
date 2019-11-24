import {Component} from '@angular/core'
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
declare var startLinking;
@Component({
    selector: 'DropBoxLogin',
    moduleId: module.id,
    template: `

<iframe [src]="URI | SafePipe" style="width:100%;height:100%"></iframe>


 `    
})

export class DropBoxLogin {

    public MyPage: string="";
    public theFrame: string = "theFrame";
    public URI: string = "";
    constructor() {

        this.onLoadFunc();
        //var win = window.open(link, '_blank');

    }
    SetWindowArgs(args: any) {
        this.URI = args.URI + "&output=embed"; 
    }
    source: string = 'https://appcenter.intuit.com/Connect/Begin?oauth_token=qyprdjn7a5B5J4YiS26DmCe2WjlGQ1kjgujdDoCX7bAft6K9&oauth_callback=http%3A%2F%2Flocalhost%3A65281%2FOauthManager.aspx%3F';
    public link = "";
    public onLoadFunc() {
        this.link = AppTool.GetLogitudeURL() + "QuickbooksOnlineAuth2.aspx?connect=true&tenant=" + SessionLocator.Tenant;
  // var win = window.open(this.link , 'theFrame', "location = 1, status = 1, scrollbars = 1, width = 400, height = 400");
       
     //   win.focus();

    }

    


    

}
