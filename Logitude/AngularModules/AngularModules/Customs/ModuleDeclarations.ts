 

// Edit Tabs


import { FieldTemplateComponent } from './Components/Templates/FieldTemplateComponent';
import { DocumentsFilingTemplateComponent } from './Components/Templates/DocumentsFilingTemplateComponent';
import {EndDateComponent} from './Components/ListTemplates/EndDateComponent';



//Short Titles
import {DeclarationShortTitleComponent} from './Components/ShortTitles/DeclarationShortTitleComponent';










//PhysicalCheck

import {CustomsSpotlightComponent} from './Components/Spotlight/CustomsSpotlightComponent';


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

                
        }

        return myResult;
    }
}
