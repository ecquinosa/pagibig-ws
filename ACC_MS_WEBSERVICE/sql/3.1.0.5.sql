------------------------------------------------------------------------------
CREATE TABLE tbl_DCS_TransactionType (
    TransactionTypeID VARCHAR (2)  PRIMARY KEY NOT NULL,
    Name VARCHAR (50) NOT NULL,
    IsActive bit,
);

INSERT INTO tbl_DCS_TransactionType (TransactionTypeID,Name,IsActive)
VALUES 
('01','STOCK-IN',1)
,('02','STOCK-OUT',1)
,('03','SPOILED',1)

-------------------------------------------------------------------------------
CREATE TABLE tbl_DCS_Card_Transaction (
    ID INT IDENTITY (1, 1),
	TransactionNo VARCHAR (22) NOT  NULL,
	TransactionTypeID VARCHAR (2) NOT NULL,
	TransactionDate DateTime NOT NULL,
	BranchCode VARCHAR (4) NOT NULL,
	KioskID VARCHAR (4) NOT NULL,
	Quantity Decimal NOT NULL,
	Username VARCHAR (20) NOT NULL,
	Remarks VARCHAR(100) NULL,
	EntryDate DateTime NOT NULL,
	LastUpdate DateTime NOT NULL,
	PRIMARY KEY (ID, TransactionNo),
	CONSTRAINT FK_CardTransaction_tbl_DCS_TransactionType FOREIGN KEY (TransactionTypeID)
	REFERENCES tbl_DCS_TransactionType (TransactionTypeID)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

--------------------------------------------------------------------------------

GO
CREATE PROCEDURE [dbo].[prcAddCardTransaction]
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

SELECT TransactionNo,TransactionTypeID,TransactionDate,BranchCode,KioskID,Quantity,Username,EntryDate,LastUpdate FROm tbl_DCS_Card_Transaction WHERE TransactionNo = @transactionNo

END




GO
CREATE PROCEDURE [dbo].[prcSelectBranchCardBalance]
	(
	@BranchCode varchar(4)
	)
AS
BEGIN

DECLARE @successCount Decimal;
DECLARE @outCount Decimal;
DECLARE @spoiledCount Decimal;
DECLARE @inCount Decimal;

SELECT @successCount = COUNT(PagIBIGID) FROM tbl_Member WHERE BranchCode = @BranchCode
SELECT @outCount = SUM(Quantity) FROM tbl_DCS_Card_Transaction WHERE BranchCode = @BranchCode AND TransactionTypeID = '02'
SELECT @spoiledCount = SUM(Quantity) FROM tbl_DCS_Card_Transaction WHERE BranchCode = @BranchCode AND TransactionTypeID = '03'
SELECT @inCount = SUM(Quantity) FROM tbl_DCS_Card_Transaction WHERE BranchCode = @BranchCode AND TransactionTypeID ='01'


SELECT  ISNULL(@inCount,0.00) - ( ISNULL(@outCount,0) + ISNULL(@spoiledCount,0) + ISNULL(@successCount,0))  as Stock


END




/*
BEGIN 
DROP PROC  prcAddCardTransaction
DROP TABLE tbl_DCS_Card_Transaction
DROP TABLE tbl_DCS_TransactionType
DROP PROC prcSelectBranchCardBalance
COMMIT
*/