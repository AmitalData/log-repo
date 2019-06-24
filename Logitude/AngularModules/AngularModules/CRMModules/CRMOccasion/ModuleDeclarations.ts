import { NewOccasionComponent } from './Components/NewEntity/NewOccasionComponent';


export const Components =
    [
        NewOccasionComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewOccasionComponent": { myResult = NewOccasionComponent; break; } 

        }

        return myResult;
    }
}
