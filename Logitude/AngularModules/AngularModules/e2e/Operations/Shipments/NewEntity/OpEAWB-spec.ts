import { browser, by, element } from 'protractor';
import{OpEAWB} from './OpEAWB'
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

describe('EAWB Module', () => {
    let EAWBTypes: OpEAWB = new OpEAWB();
    let GeneralFun: GeneralFunctions=new GeneralFunctions();

    var shipperRef1 ;
    afterEach(() => {
    })

    if (browser.params.ShipParams.ShipmentLevelCode == "D") {
        shipperRef1 = GeneralFun.RandomNum();
        it('Create Direct AWB .. ', function () {
            browser.ignoreSynchronization = true;
            EAWBTypes.CreateAWB('D',shipperRef1);
        });
        it('Search for Direct AWB # '+ shipperRef1,function(){
            browser.ignoreSynchronization = true;
            GeneralFun.UseSearchBox('Shipment_Search', shipperRef1, 'ListBoxItem');
            
        });
    }
    else if (browser.params.ShipParams.ShipmentLevelCode == "H") {
        it('Create House AWB .. ', function () {
            browser.ignoreSynchronization = true;
            EAWBTypes.CreateAWB('H',shipperRef1);
        });
    }
    else if (browser.params.ShipParams.ShipmentLevelCode == "M") {
        it('Create Master AWB .. ', function () {
            browser.ignoreSynchronization = true;
            EAWBTypes.CreateAWB('M',shipperRef1);
        });
    }
});

