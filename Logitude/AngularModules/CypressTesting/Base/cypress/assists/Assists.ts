export class Assists{

  static CreateInstance<T>(dataTable: any, isVerticalTable?: boolean): T{
    if(!isVerticalTable){
      return dataTable.hashes()[0] as T;
    }else{
      let dataTableObject = {};
      dataTable.raw().forEach((raw: any[]) => {
        dataTableObject[raw[0]] = raw[1];
      });
      return dataTableObject as T;
    }
  }
  
  static CreateSet<T>(dataTable: any): T[]{
    return dataTable.hashes() as T[];
  }

}