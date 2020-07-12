GO
CREATE PROCEDURE [dbo].[spGet_DepositBankAccount]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	select
		ID,
		AccountName,
		AccountNumber
	from tbl_DCS_DepositBankAccount
	where Status = 1
	order by ID asc;
	
	
END


---------------------------------------------------------------------------
GO
ALTER PROCEDURE [dbo].[spSaveDepositTransaction]
(
  @BranchCode varchar(50),
  @KioskID varchar(4),
  @DepositBankID int,
  @ReferenceNo varchar(50),
  @Amount decimal(10,4),
  @DepositBy int,
  @DepositDate datetime,
  @TransactionType varchar(2),
  @TransactionDate  datetime,
  @BankID int,
  @RequestedDate datetime,
  @RequestedBy int,
  @DepositImage varchar(2000),
  @Remarks varchar(300) = null
)
AS
BEGIN
   -- SET NOCOUNT ON added to prevent extra result sets from
   -- interfering with SELECT statements.
   SET NOCOUNT ON;
	INSERT INTO tbl_DCS_DepositTransaction
	([BranchCode]
    ,[KioskID]
	,[ReferenceNo]
	,[DepositBankID]
    ,[Amount]
    ,[DepositBy]
    ,[DepositDate]
    ,[Status]
    ,[TransactionType]
    ,[TransactionDate]
    ,[BankID]
    ,[RequestDate]
    ,[RequestedBy]
    ,[DepositImage]
    ,[Remarks]
    ,[UpdatedBy]
    ,[UpdatedDate]
    ,[InsertedBy]
    ,[InsertedDate])
	VALUES (
	@BranchCode,
	@KioskID,
	@ReferenceNo,
	@DepositBankID,
	@Amount,
	@DepositBy,
	@DepositDate,
	0,
	@TransactionType,
	@TransactionDate,
	@BankID,
	@RequestedDate,
	@RequestedBy,
	@DepositImage,
	@Remarks,
	@RequestedBy,
	GETDATE(),
	@RequestedBy,
	GETDATE()
	)
END


----------------------------------------------------------------------------
GO
ALTER PROCEDURE [dbo].[spGetDepositTotalByBranchAndTransactionDate]
(
	@BranchCode varchar(4),
	@TransactionDate Date
)
AS
BEGIN
	SET NOCOUNT ON;

DECLARE @Credit as decimal(10,4)
DECLARE @Debit as decimal(10,4)

SELECT @Credit = SUM(Amount) From tbl_DCS_DepositTransaction 
	WHERE BranchCode = @BranchCode 
	AND TransactionDate BETWEEN CONCAT(@TransactionDate,' 00:00:00') AND CONCAT(@TransactionDate,' 23:59:59') 
	AND TransactionType IN ('01','02')
	AND Status = 1
SELECT @Debit = SUM(Amount) From tbl_DCS_DepositTransaction 
	WHERE BranchCode = @BranchCode 
	AND TransactionDate BETWEEN CONCAT(@TransactionDate,' 00:00:00') AND CONCAT(@TransactionDate,' 23:59:59') 
	AND TransactionType IN ('03')
	AND Status = 1
SELECT ISNULL(@Credit,0.00) - ISNULL(@Debit,0.00) 
END
