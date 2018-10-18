 if not exists (select * from ARPaymentTransferStatus where Code = 'IP')
                begin
	                Insert	into  ARPaymentTransferStatus(Code, Name, SearchFields) values ('IP', 'In progress', 'ip,in progress')
                end