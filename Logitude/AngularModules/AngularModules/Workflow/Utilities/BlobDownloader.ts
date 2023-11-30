export class BlobDownloader {

    static downloadJson(jsonObject: any, fileName: string | null = null) {
        if (jsonObject) {
            let json = JSON.stringify(jsonObject, null, "\t");
            let blob = new Blob([json], { type: "application/json" });
            let url = URL.createObjectURL(blob);
            let linkElement = document.createElement("a");
            linkElement.href = url;
            linkElement.download = fileName || ((Date.now())?.toString());
            linkElement.click();
            linkElement.remove();
        }
    }

}