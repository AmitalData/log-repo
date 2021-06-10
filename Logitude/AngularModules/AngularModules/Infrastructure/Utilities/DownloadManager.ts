
import {AppTool} from '../Tools';
import {SessionLocator} from './SessionLocator';
import {CommunicationLogList} from '../../Common/EntityLists/CommunicationLogList';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

export class DownloadManager {
    public static DownloadCommunicationLogXML(item: CommunicationLogList) {
        if (item) {
     
            if (item.SecurityId) {
                this.DownloadPage("",item.SecurityId);
            }

            else {
                this.DownloadPage(item.DocumentId);
            }
        }
    }

    public static DownloadTransferHeaderFile(fileName: string) {
        if (fileName) {
            var token = ServiceHelper.GetLDocumentDownloadToken();
            var link = AppTool.GetLogitudeURL() + "WebPages/DownloadFileName.aspx?id=" + fileName + "&tempId=" + token;
            var win = window.open(link, '_blank');

            if (win) {
                win.focus();
            }
        }
    }


    public static DownloadPage(id: string, securityId: string=null ) {

        var url: string = !AppTool.IsNullOrEmpty(securityId) ? "securityId=" + securityId: "id=" + id;
        if (!AppTool.IsNullOrEmpty(id) && !AppTool.IsNullOrEmpty(securityId)  ) {
            url += ("~" + id );
        }
        var token = ServiceHelper.GetLDocumentDownloadToken();
        var link = AppTool.GetLogitudeURL() + "WebPages/DownloadPage.aspx?" + url + "&tempId=" + token;
        var win = window.open(link, '_blank');
        
        if (win) {
            win.focus();
        }       
    }

    public static DownloadExternalPage(id: string,tenant : number, securityId: string = null) {

        var url: string = !AppTool.IsNullOrEmpty(securityId) ? "securityId=" + securityId : "id=" + id;
        if (!AppTool.IsNullOrEmpty(id) && !AppTool.IsNullOrEmpty(securityId)) {
            url += ("~" + id);
        }
        //var token = ServiceHelper.GetLDocumentDownloadToken();
        var link = AppTool.GetLogitudeURL() + "WebPages/DownloadPage.aspx?" + url + "&tempId=" + securityId + "&tenant=" + tenant;
        var win = window.open(link, '_blank');
        
        if (win) {
            win.focus();
        }
    }


    public static DownloadTermsOfUse(privateLabalId: string) {
        const token = ServiceHelper.GetLDocumentDownloadToken();
        let link = AppTool.GetLogitudeURL() + "WebPages/TermsOfUseDownloadPage.aspx?PrivateLableId=" + privateLabalId + "&tempId=" + token;
        let win = window.open(link, '_blank');
        if (win) {
            win.focus();
        }
    }



}

