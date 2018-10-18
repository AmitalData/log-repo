import { CustomsCollateralComponent } from './Components/CustomsCollateralComponent';
import { CustomsCollateralAnswerComponent } from './Components/CustomsCollateralAnswerComponent';


export const Components =
    [
    
    CustomsCollateralComponent,
    CustomsCollateralAnswerComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
          
          case "CustomsCollateralComponent": { myResult = CustomsCollateralComponent; break; }
          case "CustomsCollateralAnswerComponent": { myResult = CustomsCollateralAnswerComponent; break; }
        }

        return myResult;
    }
}
