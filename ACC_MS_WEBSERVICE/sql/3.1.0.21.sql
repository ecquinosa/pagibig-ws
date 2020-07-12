ALTER TABLE tbl_DCS_Card_Transaction 
ADD Status INT DEFAULT 0 NOT NULL;
UPDATE tbl_DCS_Card_Transaction SET Status = 1;

GO
CREATE TABLE tbl_DCS_CardTransactionApporver (
    ID INT IDENTITY (1, 1),
	GroupID INT,
	ApproverID INT NULL,
	TransactionTypeID VARCHAR (2) NOT NULL,
	Status INT NOT NULL,
	PRIMARY KEY (ID),
	CONSTRAINT FK_tbl_TransactionType_tbl_DCS_CardtransactionApporver FOREIGN KEY (TransactionTypeID)
	REFERENCES tbl_DCS_TransactionType (TransactionTypeID)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

GO
CREATE TABLE tbl_DCS_CardTransactionRouting (
    ID INT IDENTITY (1, 1),
	CardTransactionID INT NOT NULL,
	ApproverID INT NOT NULL,
	Remarks VARCHAR (300) NOT NULL,
	Status INT NOT NULL,
	DateTime DateTime NOT NULL,
	PRIMARY KEY (ID)
);

GO
ALTER PROCEDURE [dbo].[prcAddCardTransaction]
	(
	@Username varchar(20),
	@BranchCode varchar(4),
	@KioskID varchar(4),
	@TransactionTypeID varchar(2),
	@Quantity Decimal,
	@Remarks varchar(100),
	@TransDate DateTime = null
	)
AS
BEGIN

DECLARE @transactionNo varchar(22);
DECLARE @transactionDate DateTime;
DECLARE @latestTransactionNo int;

SELECT TOP 1 @latestTransactionNo = CONVERT(INT, RIGHT(ISNULL(TransactionNo,0),10)) +1 from tbl_DCS_Card_Transaction WHERE BranchCode = @BranchCode ORDER BY TransactionNo DESC

SET @transactionDate = ISNULL(@TransactionDate,GETDATE())
-- 0000yyyyMMdd0000000000
SET @transactionNo = CONCAT(RIGHT('0000'+ISNULL(@BranchCode,''),4),FORMAT(@transactionDate, N'yyyyMMdd'),RIGHT('0000000000'+ISNULL(CONVERT(varchar(10),@latestTransactionNo),'1'),10))


INSERT INTO tbl_DCS_Card_Transaction (TransactionNo,TransactionTypeID,TransactionDate,BranchCode,KioskID,Quantity,Username,EntryDate,LastUpdate,Remarks,Status)
VALUES (@transactionNo,@TransactionTypeID,@transactionDate,@BranchCode,@KioskID,@Quantity,@Username,@transactionDate,@transactionDate,@Remarks,1)

SELECT TransactionNo,TransactionTypeID,TransactionDate,BranchCode,KioskID,Quantity,Username,EntryDate,LastUpdate,Remarks FROm tbl_DCS_Card_Transaction WHERE TransactionNo = @transactionNo

END
