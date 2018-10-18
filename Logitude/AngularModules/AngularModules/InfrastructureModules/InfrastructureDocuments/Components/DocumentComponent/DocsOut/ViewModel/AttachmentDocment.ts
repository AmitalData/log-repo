import {EntityArgs} from '../../../../../../Infrastructure/DataContracts/EntityArgs';




export class AttachmentDocment {
    public Order: number;
    public FileName: string;
    public FileSize: number;
    public DocumentId: string;


    // public AttachmentDocment(string fileName, double? fileSize, string documentId, int order)
    constructor(fileName: string, fileSize: number, documentId: string, order: number) {
        this.DocumentId = documentId;
        this.FileName = fileName;
        this.FileSize = fileSize;
        this.Order = order;


    }
}