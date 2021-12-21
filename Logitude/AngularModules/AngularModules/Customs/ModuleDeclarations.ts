 

// Edit Tabs


import { FieldTemplateComponent } from './Components/Templates/FieldTemplateComponent';
import { DocumentsFilingTemplateComponent } from './Components/Templates/DocumentsFilingTemplateComponent';
import {EndDateComponent} from './Components/ListTemplates/EndDateComponent';



//Short Titles
import {DeclarationShortTitleComponent} from './Components/ShortTitles/DeclarationShortTitleComponent';
import { ExportStorgeShortTitleComponent } from './Components/ShortTitles/ExportStorgeShortTitleComponent';










//PhysicalCheck

import {CustomsSpotlightComponent} from './Components/Spotlight/CustomsSpotlightComponent';
import { ReferantSpotlightDataTemplate } from './Components/Spotlight/ReferantSpotlightDataTemplate';
import { DeclarationReferantDataListActionBarComponent } from './Components/ListActionBar/DeclarationReferantDataListActionBarComponent';
import { PhysicalCheckListActionBarComponent } from './Components/ListActionBar/PhysicalCheckListActionBarComponent';

export const CustomsControlsComponents =
    [
       
    ];

     
export const Components =
    [

  
        FieldTemplateComponent,
        
        DocumentsFilingTemplateComponent,
        
        EndDateComponent,
        
        //Controls
        
       

        //short titles
        DeclarationShortTitleComponent,
     
        ExportStorgeShortTitleComponent,
     
  
      

      
        
        

        //PhysicalCheck
       
      

        CustomsSpotlightComponent,
        ReferantSpotlightDataTemplate,

        ReferantSpotlightDataTemplate,
        DeclarationReferantDataListActionBarComponent,
        PhysicalCheckListActionBarComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
         
          
        
          
            // New Entity
         
           
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }
            case "DocumentsFilingTemplateComponent": { myResult = DocumentsFilingTemplateComponent; break; }
            case "EndDateComponent": { myResult = EndDateComponent; break; }
                        //short titles
            case "DeclarationShortTitleComponent": {myResult = DeclarationShortTitleComponent; break;}
            case "ExportStorgeShortTitleComponent": { myResult = ExportStorgeShortTitleComponent; break; }

           

          
            case "CustomsSpotlightComponent": { myResult = CustomsSpotlightComponent; break; }
            case "ReferantSpotlightDataTemplate": { myResult = ReferantSpotlightDataTemplate; break; }

            case "DeclarationReferantDataListActionBarComponent": { myResult = DeclarationReferantDataListActionBarComponent; break; }
            case "PhysicalCheckListActionBarComponent": { myResult = PhysicalCheckListActionBarComponent; break; }
        }

        return myResult;
    }
}
