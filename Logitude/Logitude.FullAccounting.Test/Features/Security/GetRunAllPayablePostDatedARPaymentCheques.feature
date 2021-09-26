Feature: Get Run All Payable Post Dated ARPayment Cheques
	we want to check this api security.

Scenario: Get run all payable post dated arpayment cheques by not authentication user.
	When get run all payable post dated arpayment cheques by not login user
	Then get run all payable post dated arpayment cheques api should return you have no permissions

Scenario: Get run all payable post dated arpayment cheques by not authorize user.
	When get run all payable post dated arpayment cheques by not authorize user
	Then get run all payable post dated arpayment cheques api should return you have no permissions

Scenario: Get run all payable post dated arpayment cheques from unauthorizes tenant.
	When get run all payable post dated arpayment cheques from unauthorizes tenant
	Then get run all payable post dated arpayment cheques api should return you have no permissions