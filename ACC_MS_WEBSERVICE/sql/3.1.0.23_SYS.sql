GO
ALTER PROCEDURE [dbo].[spSaveDepositTransaction]
(
  @BranchCode varchar(50),
  @KioskID varchar(4),
  @AcctNo varchar(12),
  @ReferenceNo varchar(50),
  @Amount decimal(10,4),
  @DepositBy int,
  @DepositDate datetime,
  @DepositBank varchar(300),
  @TransactionType varchar(2),
  @TransactionDate  datetime,
  @BankID int,
  @RequestDate datetime,
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
    ,[AcctNo]
	,[ReferenceNo]
    ,[Amount]
    ,[DepositBy]
    ,[DepositDate]
    ,[DepositBank]
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
	@AcctNo,
	@ReferenceNo,
	@Amount,
	@DepositBy,
	@DepositDate,
	@DepositBank,
	0,
	@TransactionType,
	@TransactionDate,
	@BankID,
	@RequestDate,
	@RequestedBy,
	@DepositImage,
	@Remarks,
	@RequestedBy,
	GETDATE(),
	@RequestedBy,
	GETDATE()
	)
END
-----------------------------------------------------------------------------
GO
INSERT INTO [dbo].[tbl_DCS_DepositTransactionType]
           ([Id]
           ,[Name]
           ,[IsActive])
     VALUES
           ('01','DEPOSIT',1),
		   ('02','ADJUSTMENT DEPOSIT IN',1),
		   ('03','ADJUSTMENT DEPOSIT OUT',1)
GO
----------------------------------------------------------------------------
GO
CREATE PROCEDURE [dbo].[spGet_isDepositReferenceNoExist]
(
	@ReferenceNo varchar(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN
		IF EXISTS(SELECT ID FROM tbl_DCS_DepositTransaction WHERE ReferenceNo = @ReferenceNo)
			BEGIN
				SELECT 1;
			END
		ELSE
			BEGIN
				SELECT 0;
			END
	END

END
------------------------------------------------------------------------------------
GO
CREATE PROCEDURE [dbo].[spGetDepositTotalByBranchAndTransactionDate]
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
SELECT @Debit = SUM(Amount) From tbl_DCS_DepositTransaction 
	WHERE BranchCode = @BranchCode 
	AND TransactionDate BETWEEN CONCAT(@TransactionDate,' 00:00:00') AND CONCAT(@TransactionDate,' 23:59:59') 
	AND TransactionType IN ('03')

SELECT ISNULL(@Credit,0.00) - ISNULL(@Debit,0.00) 
END


