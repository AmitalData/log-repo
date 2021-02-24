export function CreateInstance<T>(dataTable: any, isVerticalTable?: boolean): T {
  let instance: T;
  if (isVerticalTable) {
    instance = CreateInstanceFromVerticalTable<T>(dataTable);
  } else {
    instance = CreateInstanceFromHorizontalTable<T>(dataTable);
  }
  return instance;
}

export function CreateSet<T>(dataTable: any): T[] {
  let set: T[] = CreateSetFromHorizontalTable<T>(dataTable);
  return set;
}

function CreateInstanceFromVerticalTable<T>(dataTable: any): T {
  let dataTableObject = {};
  dataTable.raw().forEach((raw: any[]) => {
    dataTableObject[raw[0]] = raw[1];
  });
  return dataTableObject as T;
}

function CreateInstanceFromHorizontalTable<T>(dataTable: any): T {
  return dataTable.hashes()[0] as T;
}

function CreateSetFromHorizontalTable<T>(dataTable: any): T[] {
  return dataTable.hashes() as T[];
}