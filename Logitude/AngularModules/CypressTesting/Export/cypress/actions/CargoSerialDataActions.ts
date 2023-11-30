import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { CargoSerialDataDetails } from "cypress/models/CargoSerialDataDetails";
import { CargoSerialDataSelectors } from '../selectors/CargoSerialDataSelectors';


export function NavigatesExportWizerd() {
    
    cy.Click(BaseExportSelectors.ExportDeclaration, null);
}

export function SearchFields(cargoSerialDatasDetails: CargoSerialDataDetails) 
{
   cy.FillLogTextBox(CargoSerialDataSelectors.SearchField, cargoSerialDatasDetails.SearchField, true);
   cy.wait(1000)
   cy.Click(CargoSerialDataSelectors.FirestDeclaration, null);
   cy.Click(CargoSerialDataSelectors.AddButton, null);
  
}

export function FillCargoSerialData(cargoSerialDataDetails: CargoSerialDataDetails) {
    debugger
    FillDropdownInRowTable('סוג כמות', cargoSerialDataDetails.TypeOfQuantity);
    FillDropdownInRowTable('סוג אריזה', cargoSerialDataDetails.PackagingType);
    FillInRowTable('כמות', cargoSerialDataDetails.Quantity);
    FillInRowTable('משקל', cargoSerialDataDetails.Weight);
    FillDropdownInRowTable('קוד סוג משקל אריזה', cargoSerialDataDetails.PackingWeightCode);
    cy.Click(CargoSerialDataSelectors.SignsAndNumbersClick,null);
    cy.FillLogTextBox(CargoSerialDataSelectors.SignsAndNumbers, cargoSerialDataDetails.SignsAndNumbers); 
    
}


function FillDropdownInRowTable(headerText: string, value: string) {
    cy.get(`.ag-header-cell div:contains("${headerText}")`).invoke('attr', 'id').then(id => {
        if (id?.indexOf('HeaderTemplateDiv') > -1) {
            let i = id.replace('HeaderTemplateDiv', '');
            FillGLAccountDDL(`.LogCellTemplate:eq(${i})`, value);
            cy.Click('.EditableGridBody',null);
        }
       
    }); 
   
}

function FillInRowTable(headerText: string, value: string) {
    cy.get(`.ag-header-cell div:contains("${headerText}")`).each(ele => {
        if(ele.text() != headerText)return;
        const id = ele.attr('id')  
            if (id?.indexOf('HeaderTemplateDiv') > -1) {
                let i = id.replace('HeaderTemplateDiv', '');
                FillGLAccount('[index="' + i + '"]', value,);
            }
    })
}

// function FillInRowTable(headerText: string, value: string) {
//     cy.get(`.ag-header-cell div:contains("${headerText}")`).invoke('attr', 'id').then(id => {        
//         if (id?.indexOf('HeaderTemplateDiv') > -1) {
//             let i = id.replace('HeaderTemplateDiv', '');
//             FillGLAccount('[index="' + i + '"]', value,);
//         }
//     });
// }


export function FillGLAccountDDL(selector, value) {
    cy.get(selector).type(value);
    cy.get(BaseSelectors.DropDownList).contains(value).then(a => {
        a[0].click();
    });
}


export function FillGLAccount(selector, value) {
    cy.get(selector).type(value);
    
}


export function AssertSaveCargoSerialData() {
    BaseAssertion.AssertElementExist(CargoSerialDataSelectors.CargoSerialDataFirstRow);
    
}



export function DeleteRow(){
     cy.Click('.EditableGridBody',null);
     cy.Click(CargoSerialDataSelectors.CargoSerialDataDeletRow,null);
     cy.Click(CargoSerialDataSelectors.Yes,null);
     
    }

    export function AssertDeleteRow() {
        BaseAssertion.AssertElementNotExist(CargoSerialDataSelectors.CargoSerialDataFirstRow);
        
    }
    //#endregion

