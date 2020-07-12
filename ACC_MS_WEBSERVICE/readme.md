3.0.0.1
- Add GetConfig
- Add IsMemberHasAccount
3.0.0.2
- Add ignore validation on certificate PHASE3
  ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
 3.0.0.3
 - Fix issue on not success UBP but creating file.
 3.1.0.4
 - Replace  ñÑ with nN
 3.1.0.5
 - Add card transaction tbl
 - add transaction type table
 - add prcAddCardTransaction
 - add service for adding card transaction type.
 - remove new line on ubp file creation.
 - Add 2  service GetTotalSpoiledCardByBranchAndDate, GetTotalBranchCards
 - Fix issue on -1 on Spoiled cards when null response.

 3.1.0.6
 - Fix issue on no data on create UBP  dump files remove arrdata.remove('\n').
 - Add new service create dump manual.
 - add new service IsBranchHasSpoiledCardsTransaction
 3.1.0.7
 - add GetActiveCardInfo
 - modify UpdateCardInfoLocal to get card number from pagibig before sending.
 3.1.0.8
 - Change GetConfig to response bank code.
3.1.0.9
- Fix issue on getkiosk recard and captured.
3.1.0.10
- Fix issue on null cardpfrdate
3.1.0.11
- Change to +63 all the receive country code.
3.1.0.12
- Reversal handling on sync add check if isRefnumExist.
3.1.0.13
- Reversal handling for saving OR.
3.1.0.14
- PushCardInfoLocal change application date from Card.EntryDate.Date to Member.ApplicationDate
- Remove active card info on UpdateCardInfoLocal.
3.1.0.15
- Adjustment of PackUBPData
 add -wsq and -ansi.
3.1.0.16
- Create ManualPushCard for the data that has been uploaded but not push to pagibig posible use for server side PushCardInfo.
- Adjustment on SaveMember validation on end if exist on database.
- Add SaveCardSpoiled
- Adjustment on manual create dump.
- Implement active card info on update.
- add prcSelectDCS_UploadForUpload
- add prcSelectDCS_Upload and saving
- add SaveDCSUpload
- update AUBRequestProcess add for getCardNo  If dynObj.ContainsKey("acctNo") Then aubResponse.accountNo = dynObj.Item("acctNo")
- On save member add saving of DCS_Upload.
- SaveMemberWithOptions pushcard and packupdata is optional.
- Bug fixing for already completed of transaction scope.
- Add response type on manualpackupdata
3.1.0.17
- Remove saving to DCS_Upload table on  SaveMemberWithOptions and SaveMember.
- Remove active card info on  SaveMemberWithOptions and SaveMember.
- Remove special characters on addresses.
- Update ManualPackUpData.
- Implement new file content of PackUpData.
Note: requires only 3.1.0.16_CardTranscation.sql
3.1.0.18
- Adjustment on IsRefNumExist.
3.1.0.19
- Add PackUBPData2

3.1.0.20
- Update SP prcValidateUserAndAccess add idNumber
- Add ID number on loginResponse
- Add time keeping request and settings.
3.1.0.21
- Implement recarding directory and file naming convention.
- Additional of new table for approval type of card transaction.
- adjustment on LogEmployee add datetime.
3.1.0.22
- Add Packupdata.
- Adjustment on LogResponse to support v24.
- Add new service to get the GetUserIDNumber.
- Additional handling of RemoveInvalidCharacters on Savemember,SaveMemberWithOptions, ManualPackupData
3.1.0.23
- Additional feature Save depostin transaction.
- Add Settings CentralizedDB and shared folder.
- Add validation for already exist reference no.
3.1.0.24
- Add reversal handling for SaveMember & SaveMemberWithOptions
- Add GetDepositByBranchAndDate to get daily total deposit amount.
3.1.0.25
- bug fixing on SaveMember & SaveMemberWithOptions handling on reversal of pushcard.
- transaction scope is not rollback after failed on push card info.
- general property for Dim ph3API As New PHASE3.PHASE3
- update manual packup data , save member & savemember with options with new handling of address.
- add handling for invalid card bin on savemember,savememberwithoptions,manualpackupdata,manualpushcardinfo.
- adjustment validation of card bin create dynamic via settings.
3.1.0.26
- Additional handler for invalid account number.
- add new feature to get all the available bank account.
- update packupdata2 to save image as an jpeg.
3.1.0.27
- Fix issue on manual packupdata and manual pushcard info
3.1.0.28
- Add handling on UBP packupdata on null SSS and GSIS add PAGIBIGID to GSIS
- Add old card number.
3.1.0.29
- Fix issue on signature manual packupdata2.
3.1.0.30
- Add saving of version on tbl_kiosk
3.1.0.31
- Add validation change validation on account number from = "0000000000000000" to startswith("000000000000")
3.1.0.32
- Fix issue on null data on MemberContactinfo , MembershipCategoryInfo,Member
- Fix error logs and add some DBNull handling.
3.1.0.33 - Optimization
- Add web services on pagibig.
- Remove web services.
- Remove decryption on 
	CentralizedServer
	ServerPassword
	PagIBIGPassword
- Restore description.
- Additional feature to add cash on hand.
- GetTop10 CashOnHand
- GetTop10 Deposit
- Fix issue on logging.
- Adjustment on recard no oldCardNumber.
3.1.0.34
- Fix issue on manual packup data on mask card numbers.
3.1.0.35
- Remove special characters on Employee ID
- Remove special characters on Employer Name/Company Name.
3.1.0.36
- Enhancement on query of IsMemberHasAccount remove order by and get refnum only to validate if user has exisiting account.
- Enhancement on query of IsMemberExistByMIDAndApplicationDate get only refnum and remove order b
- On exception rollback transaction if is not null.