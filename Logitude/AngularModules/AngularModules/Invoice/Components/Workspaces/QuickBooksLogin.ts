import {Component} from '@angular/core'
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
declare var startLinking;
@Component({
    selector: 'QuickBooksLogin',
    moduleId: './Invoice/Components/Workspaces/',
    template: `

<iframe src="link" style="width:100%;height:100%"></iframe>


 `    
})

export class QuickBooksLogin {

    public MyPage: string="";
    public theFrame: string = "theFrame";
    constructor() {

        this.onLoadFunc();
        //var win = window.open(link, '_blank');

    }

    source: string = 'https://appcenter.intuit.com/Connect/Begin?oauth_token=qyprdjn7a5B5J4YiS26DmCe2WjlGQ1kjgujdDoCX7bAft6K9&oauth_callback=http%3A%2F%2Flocalhost%3A65281%2FOauthManager.aspx%3F';
    public link = "";
    public onLoadFunc() {
   this.link = AppTool.GetLogitudeURL() + "Quickbooksonline.aspx?connect=true&tenant=" + SessionLocator.Tenant;
  // var win = window.open(this.link , 'theFrame', "location = 1, status = 1, scrollbars = 1, width = 400, height = 400");
       
     //   win.focus();

    }

    


    

}