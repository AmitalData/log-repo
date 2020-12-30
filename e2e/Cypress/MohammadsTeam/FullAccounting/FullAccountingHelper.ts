
export class FullAccountingHelper {
      constructor() {


 
  }
  public static  CheckIsElementExist(Item:string){
    var IsExist:boolean=false;
    const GetItem = Cypress.$(Item);
    if (GetItem && GetItem.length) {
      IsExist = true;
    }
    return IsExist;
  }


}



