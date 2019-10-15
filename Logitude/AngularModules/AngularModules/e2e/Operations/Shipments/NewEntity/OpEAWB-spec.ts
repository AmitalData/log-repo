import { browser, by, element } from 'protractor';
import{OpEAWB} from './OpEAWB'
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';

describe('EAWB Module', () => {
    let EAWBTypes: OpEAWB = new OpEAWB();
    let GeneralFun: GeneralFunctions=new GeneralFunctions();

    afterEach(() => {
    })
    var shipperRef1 = GeneralFun.RandomNum();
    if (browser.params.ShipParams.ShipmentLevelCode == "D") {
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
        it('Search for House AWB # ' + shipperRef1, function () {
            browser.ignoreSynchronization = true;
            GeneralFun.UseSearchBox('Shipment_Search', shipperRef1, 'ListBoxItem');
        });
    }
    else if (browser.params.ShipParams.ShipmentLevelCode == "M") {
        it('Create Master AWB .. ', function () {
            browser.ignoreSynchronization = true;
            EAWBTypes.CreateAWB('M',shipperRef1);
        });
        it('Search for Master AWB # ' + shipperRef1, function () {
            browser.ignoreSynchronization = true;
            GeneralFun.UseSearchBox('Shipment_Search', shipperRef1, 'ListBoxItem');
        });
    }
});

