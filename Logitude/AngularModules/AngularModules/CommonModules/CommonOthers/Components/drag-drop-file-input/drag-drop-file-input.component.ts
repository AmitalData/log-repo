import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
  selector: 'app-drag-drop-file-input',
  templateUrl: './drag-drop-file-input.component.html',
  styleUrls: ['./drag-drop-file-input.component.scss']
})
export class DragDropFileInputComponent implements OnChanges {
  @Output() base64FileSelected = new EventEmitter<string>();
  @Output() filesSelected = new EventEmitter<FileList | File>();
  @Output() fileCleared = new EventEmitter<void>();
  @Output() fileRotate = new EventEmitter<number>();
  @Input() width: number = 300;
  @Input() height: number = 410;
  @Input() input: Blob | File | string | null | FileList = null;
  @Input() displayOnly: boolean = false;
  @Input() allowedExtensions: string[] = [];
  file: File | null = null;
  fileType: string = '';
  _dataUrl: string = '';
  _dataUrlSafe: SafeUrl = '';
  error: string = '';
  set dataUrl(dataUrl: string) {
    this._dataUrl = dataUrl;
    this._dataUrlSafe = this.sanitizer.bypassSecurityTrustUrl(dataUrl);
  }
  base64File: string = '';
  fileName: string = '';
  zoom: number = 1.0;
  rotation: number = 0;
  currentPage: number = 1;
  totalPages: number = 1;

  constructor(private sanitizer: DomSanitizer) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.input && changes.input.currentValue) {
      this.handleInput(this.input);
    }
  }

  onFileDropped(event: DragEvent): void {
    event.preventDefault();
    const files: FileList | null = event.dataTransfer?.files || null;
    if (files) {
      this.handleFiles(files);
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  onFileSelected(event: Event): void {
    const input: HTMLInputElement = event.target as HTMLInputElement;
    if (input.files) {
      this.handleFiles(input.files);
    }
  }

  onContainerClick(fileInput: HTMLInputElement): void {
    fileInput.click();
  }

  handleInput(input: Blob | File | FileList | string | null): void {
    if (input instanceof FileList && input.length > 0)
      input = input[0];
    if (input instanceof File || input instanceof Blob) {
      this.handleBlob(input);
    } else if (typeof input === 'string') {
      if (input.startsWith('data:') || input.startsWith('blob:')) {
        this.handleDataOrBlobUrl(input);
      } else {
        this.handleBase64(input);
      }
    }
  }

  handleBlob(blob: Blob | File, selected: boolean = false): void {
    this.file = blob instanceof File ? blob : new File([blob], 'file', { type: blob.type });
    if (!this.isFileExtensionAllowed(this.file)) return;
    
    this.determineFileType(this.file);

    const reader: FileReader = new FileReader();
    reader.onload = (e: ProgressEvent<FileReader>) => {
      const arrayBuffer: ArrayBuffer = e.target?.result as ArrayBuffer;
      this.base64File = this.convertArrayBufferToBase64(arrayBuffer);
      this.dataUrl = URL.createObjectURL(blob);

      if (selected)
        this.base64FileSelected.emit(this.base64File);
    }
    reader.readAsArrayBuffer(blob);
  }

  handleFiles(file: FileList | File | null): void {    
    if (file instanceof FileList && file.length > 0)
      file = file[0];
    
    if (!(file instanceof File) || !this.isFileExtensionAllowed(file)) return;
    
    this.filesSelected.emit(file as FileList | File);
    this.file = file;
    this.handleBlob(this.file, true);
  }

  private isFileExtensionAllowed(file: File): boolean {
    if (this.allowedExtensions.length === 0) 
      return true;
    
    const fileExtension: string | undefined = file.name.split('.').pop()?.toLowerCase();
    const isAllowed = this.allowedExtensions.some(ext => ext.toLowerCase() === fileExtension);
    
    if (!isAllowed) {
      this.error = `${TextCodeTranslator.Translate('Customs.General.O.FileNotAllow')}: ${fileExtension}`;
      console.error(this.error);
    } else
      this.error = '';
    
    return isAllowed
  }

  private determineFileType(file: File): void {
    if (file.type === 'application/pdf') {
      this.fileType = 'pdf';
    } else if (file.type.startsWith('image/')) {
      this.fileType = 'image';
    } else {
      this.fileType = 'other';
    }
  }

  private convertArrayBufferToBase64(arrayBuffer: ArrayBuffer): string {
    let decodedString: string = '';
    const bytes: Uint8Array = new Uint8Array(arrayBuffer);
    for (let i: number = 0; i < bytes.byteLength; i++)
      decodedString += String.fromCharCode(bytes[i]);
    return btoa(decodedString);
  }

  handleBase64(base64: string | null): void {
    if (!base64) return;

    this.base64File = base64;
    const blob: Blob = this.createBlobFromBase64(base64);
    this.dataUrl = URL.createObjectURL(blob);
    this.file = new File([blob], 'file', { type: blob.type });
    this.determineFileType(this.file);
  }

  private createBlobFromBase64(base64: string): Blob {
    const binaryString: string = atob(base64);
    const bytes: Uint8Array = new Uint8Array(binaryString.length);
    for (let i: number = 0; i < binaryString.length; i++)
      bytes[i] = binaryString.charCodeAt(i);

    const isPdf: boolean = base64.startsWith('JVBERi0');
    const isImage: boolean = base64.startsWith('data:image/');
    const blob: Blob = new Blob([bytes], { type: isPdf ? 'application/pdf' : isImage ? 'image/jpeg' : undefined });
    return blob;
  }

  async handleDataOrBlobUrl(dataUrl: string | null): Promise<void> {
    if (!dataUrl) return;

    this.dataUrl = dataUrl;
    this.file = await this.convertDataUrlToFile(dataUrl);
    this.determineFileType(this.file);
  }

  private async convertDataUrlToFile(dataUrl: string): Promise<File> {
    const res: Response = await fetch(dataUrl);
    const blob: Blob = await res.blob();
    return new File([blob], 'file', { type: blob.type });
  }

  zoomIn(): void {
    this.zoom += 0.1;
  }

  zoomOut(): void {
    if (this.zoom > 0.1) {
      this.zoom -= 0.1;
    }
  }

  rotateClockwise(): void {
    this.rotation += 90;
    this.fileRotate.emit(this.rotation);
  }
  
  rotateCounterClockwise(): void {
    this.rotation -= 90;
    this.fileRotate.emit(this.rotation);
  }

  clearFile(): void {
    this.file = null;
    this.fileType = '';
    this.dataUrl = '';
    this.base64File = '';
    this.fileName = '';
    this.zoom = 1.0;
    this.rotation = 0;
    this.fileCleared.emit();
  }
  
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  onPdfLoaded(pdf: any): void {
    this.currentPage = 1;
    this.totalPages = pdf.numPages;
  }
}
