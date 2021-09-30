@Pre-Prepare-DocumentType
Feature: UploadFilewithChunks
	we want to upload File with 3 chunks
@Smoke
@Release 
Scenario: Upload File with chunks
Given Document Filing
Given file size 267607 bytes
When get file size format
Then file size format should be not null
When Upload File
Then uploaded file size should equal file size
When update document with file
Then the document should be updated