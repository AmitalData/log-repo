import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AccountingTransferHeaderPM} from '../../../EntityPMs/AccountingTransferHeaderPM';
import {AccountingTransferLinePM} from '../../../EntityPMs/AccountingTransferLinePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DownloadManager} from '../../../../Infrastructure/Utilities/DownloadManager';
import {InvoiceDomainService} from '../../../Services/InvoiceDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    templateUrl: './MasavInterfaceDetailsTabComponent.html',
})

export class MasavInterfaceDetailsTabComponent extends BaseComponent {
   
}
