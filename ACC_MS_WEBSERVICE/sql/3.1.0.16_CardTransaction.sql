  INSERT INTO [tbl_DCS_TransactionType] (TransactionTypeID,Name,IsActive)
  VALUES
  ('04','REPLACE STOCK-IN',1),
  ('05','REPLACE STOCK-OUT',1),
  ('06','REPLACE SPOILED',1);


CREATE TABLE tbl_DCS_Card_Spoiled (
    ID INT IDENTITY (1, 1),
	TransactionNo  VARCHAR (22) NOT NULL,
	TransactionDate DateTime NOT NULL,
	BranchCode VARCHAR (4) NOT NULL,
	CardNumber VARCHAR (20) NOT NULL,
	Username VARCHAR (20) NOT NULL,
	Remarks VARCHAR(100) NULL,
	EntryDate DateTime NOT NULL,
	LastUpdate DateTime NOT NULL,
	PRIMARY KEY (ID)
);


--------------------------------------------------------------------------------------------------------------

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[prcAddCardSpoiled]
	(
	@TransactionNo VARCHAR(22),
	@TransactionDate DateTime,
	@BranchCode varchar(4),
	@CardNumber varchar(20),
	@Username varchar(20),
	@Remarks varchar(100)
	)
AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO tbl_DCS_Card_Spoiled (TransactionNo,TransactionDate,BranchCode,CardNumber,UserName,Remarks,EntryDate,LastUpdate)
VALUES (@TransactionNo,@TransactionDate,@BranchCode,@CardNumber,@Username,@Remarks,GETDATE(),GETDATE())

END
--------------------------------------------------------------------------------------------------------------
GO
ALTER PROCEDURE [dbo].[prcAddCardTransaction]
	(
	@Username varchar(20),
	@BranchCode varchar(4),
	@KioskID varchar(4),
	@TransactionTypeID varchar(2),
	@Quantity Decimal,
	@Remarks varchar(100)
	)
AS
BEGIN

DECLARE @transactionNo varchar(22);
DECLARE @transactionDate DateTime;
DECLARE @latestTransactionNo int;

SELECT TOP 1 @latestTransactionNo = CONVERT(INT, RIGHT(ISNULL(TransactionNo,0),10)) +1 from tbl_DCS_Card_Transaction WHERE BranchCode = @BranchCode ORDER BY TransactionNo DESC

SET @transactionDate = GETDATE()
-- 0000yyyyMMdd0000000000
SET @transactionNo = CONCAT(RIGHT('0000'+ISNULL(@BranchCode,''),4),FORMAT(@transactionDate, N'yyyyMMdd'),RIGHT('0000000000'+ISNULL(CONVERT(varchar(10),@latestTransactionNo),'1'),10))


INSERT INTO tbl_DCS_Card_Transaction (TransactionNo,TransactionTypeID,TransactionDate,BranchCode,KioskID,Quantity,Username,EntryDate,LastUpdate,Remarks)
VALUES (@transactionNo,@TransactionTypeID,@transactionDate,@BranchCode,@KioskID,@Quantity,@Username,@transactionDate,@transactionDate,@Remarks)

SELECT TransactionNo,TransactionTypeID,TransactionDate,BranchCode,KioskID,Quantity,Username,EntryDate,LastUpdate,Remarks FROm tbl_DCS_Card_Transaction WHERE TransactionNo = @transactionNo

END
