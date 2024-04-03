

export interface IScreenLayoutService {
    GenerateScreen(screenItem: any);
    GetScreenRows(sectionNumber: number);
    ChangeScreenFieldPosition(screenFieldPositionargs: any);
    BuildScreenUpdateArgs();

}
