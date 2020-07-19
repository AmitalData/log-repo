 

// Edit Tabs


import { FieldTemplateComponent } from './Components/Templates/FieldTemplateComponent';
import { DocumentsFilingTemplateComponent } from './Components/Templates/DocumentsFilingTemplateComponent';
import {EndDateComponent} from './Components/ListTemplates/EndDateComponent';



//Short Titles
import {DeclarationShortTitleComponent} from './Components/ShortTitles/DeclarationShortTitleComponent';










//PhysicalCheck

import {CustomsSpotlightComponent} from './Components/Spotlight/CustomsSpotlightComponent';
import { DeclarationReferantDataFiltersMenuComponent } from './Components/FiltersMenu/DeclarationReferantDataFiltersMenuComponent';
import { ReferantSpotlightDataTemplate } from './Components/Spotlight/ReferantSpotlightDataTemplate';
import { DeclarationReferantDataListActionBarComponent } from './Components/ListActionBar/DeclarationReferantDataListActionBarComponent';

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
     
       
     
  
      

      
        
        

        //PhysicalCheck
       
      

        CustomsSpotlightComponent,
        ReferantSpotlightDataTemplate,
        DeclarationReferantDataFiltersMenuComponent,

        ReferantSpotlightDataTemplate,
        DeclarationReferantDataFiltersMenuComponent,
        DeclarationReferantDataListActionBarComponent,
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
         
           

          
            case "CustomsSpotlightComponent": { myResult = CustomsSpotlightComponent; break; }
            case "ReferantSpotlightDataTemplate": { myResult = ReferantSpotlightDataTemplate; break; }
            case "DeclarationReferantDataFiltersMenuComponent": { myResult = DeclarationReferantDataFiltersMenuComponent; break; }

            case "DeclarationReferantDataListActionBarComponent": { myResult = DeclarationReferantDataListActionBarComponent; break; }
        }

        return myResult;
    }
}
