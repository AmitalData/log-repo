export class CachedData {
    private static cachedDataList: { [key: string]: any[] };

    public static GetCachedData(objectTableName: string) {
        if (!this.cachedDataList) {
            this.cachedDataList = {};
        }

        return this.cachedDataList[objectTableName];
    }

    public static StoreCachedData(objectTableName: string, data: any[]) {
        if (!this.cachedDataList) {
            this.cachedDataList = {};
        }
        this.cachedDataList[objectTableName] = data;
    }
}