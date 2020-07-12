CREATE TABLE tbl_DCS_CashOnHand_Status (
    ID INT IDENTITY (1, 1),
	Name VARCHAR(150) NOT NULL,
	UpdatedBy INT NOT NULL,
	UpdatedDate DateTime NOT NULL,
	InsertedBy INT NOT NULL,
	InsertedDate DateTime NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE tbl_DCS_CashOnHand (
    ID INT IDENTITY (1, 1),
	Status INT NOT NULL,
	BankID INT NOT NULL,
	BranchCode VARCHAR(4) NOT NULL,
	KioskID VARCHAR(4) NOT NULL,
	Amount DECIMAL(14,4) NOT NULL DEFAULT 0,
	Remarks VARCHAR(300) NULL,
	UpdatedBy INT NOT NULL,
	UpdatedDate DateTime NOT NULL,
	InsertedBy INT NOT NULL,
	InsertedDate DateTime NOT NULL,
	PRIMARY KEY (ID)
);

----------------------------------------------------------------------
INSERT INTO tbl_DCS_CashOnHand_Status 
	(Name,UpdatedBy,UpdatedDate,InsertedBy,InsertedDate) 
VALUES 
	('NEW',1,GETDATE(),1,GETDATE()),
	('CANCELLED',1,GETDATE(),1,GETDATE());



----------------------------------------------------------------------
GO
CREATE PROCEDURE [dbo].[spSave_CashOnHand]
(
  @ID int,
  @BranchCode varchar(50),
  @KioskID varchar(4),
  @Amount decimal(10,4),
  @Remarks varchar(300) = null,
  @RequestedBy int,
  @Status int,
  @BankID int
)
AS
BEGIN
   -- SET NOCOUNT ON added to prevent extra result sets from
   -- interfering with SELECT statements.
   SET NOCOUNT ON;
	
	IF @ID = 0 
	BEGIN 
		INSERT INTO tbl_DCS_CashOnHand
		(
		 [Status]
		,[BankID]
		,[BranchCode]
		,[KioskID]
		,[Amount]
		,[Remarks]
		,[UpdatedBy]
		,[UpdatedDate]
		,[InsertedBy]
		,[InsertedDate])
		VALUES (
		1,
		@BankID,
		@BranchCode,
		@KioskID,
		@Amount,
		@Remarks,
		@RequestedBy,
		GETDATE(),
		@RequestedBy,
		GETDATE()
		)
		END 
	ELSE
	BEGIN
		UPDATE tbl_DCS_CashOnHand
		SET
		[Status] = @Status,
		[BranchCode] = @BranchCode,
		[KioskID] = @KioskID,
		[Amount] = @Amount,
		[Remarks] = @Remarks,
		[UpdatedBy] = @RequestedBy,
		[UpdatedDate] = GETDATE()
		WHERE [ID] = @ID AND [BankID] = @BankID
	END

END
----------------------------------------------------------------------
GO
CREATE PROCEDURE [dbo].[spGet_CashOnHandTop10ByUser]
(
  @KioskID varchar(4),
  @RequestedBy int,
  @BankID int
)
AS
BEGIN
   -- SET NOCOUNT ON added to prevent extra result sets from
   -- interfering with SELECT statements.
   SET NOCOUNT ON;
	
	SELECT TOP 10 [ID]
      ,[Status]
      ,[BankID]
      ,[BranchCode]
      ,[KioskID]
      ,[Amount]
      ,[Remarks]
      ,[UpdatedBy]
      ,[UpdatedDate]
      ,[InsertedBy]
      ,[InsertedDate]
  FROM [LCDB_01_SYS].[dbo].[tbl_DCS_CashOnHand]
  WHERE [KioskID] = @KioskID AND [InsertedBy] = @RequestedBy AND [BankID] = @BankID AND Status = 1
    ORDER BY [InsertedDate] DESC
END
----------------------------------------------
GO
CREATE PROCEDURE [dbo].[spGet_DepositRequestTop10ByUser]
(
  @KioskID varchar(4),
  @RequestedBy int,
  @BankID int,
  @Status int
)
AS
BEGIN
   -- SET NOCOUNT ON added to prevent extra result sets from
   -- interfering with SELECT statements.
   SET NOCOUNT ON;
	
	SELECT TOP 10 [Id]
      ,[BranchCode]
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
      ,[InsertedDate]
  FROM [LCDB_01_SYS].[dbo].[tbl_DCS_DepositTransaction]
  WHERE [KioskID] = @KioskID AND [RequestedBy] = @RequestedBy AND [BankID] = @BankID AND Status = @Status AND [TransactionType] = '01'
  ORDER BY [InsertedDate] DESC

END
---------------------------------------------------------------------
GO
CREATE PROCEDURE [dbo].[spGet_isCashOnHandExistByBranchAndDate]
(
	@pID int,
	@RequestDate date,
	@BranchCode varchar(20)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN
	IF @pID = 0
	BEGIN
		IF EXISTS(SELECT ID FROM [tbl_DCS_CashOnHand] 
			WHERE 
			[BranchCode] = @BranchCode 
			AND [InsertedDate] 
			BETWEEN CONCAT(FORMAT(@RequestDate, N'yyyy-MM-dd'),' 00:00:00') 
			AND		CONCAT(FORMAT(@RequestDate, N'yyyy-MM-dd'),' 23:59:59') 
			AND [Status] = 1)
				BEGIN
					SELECT 1;
				END
			ELSE
				BEGIN
					SELECT 0;
				END
		END
	ELSE
	BEGIN
		IF EXISTS(SELECT ID FROM [tbl_DCS_CashOnHand] 
			WHERE 
			[BranchCode] = @BranchCode 
			AND [InsertedDate] 
			BETWEEN CONCAT(FORMAT(@RequestDate, N'yyyy-MM-dd'),' 00:00:00') 
			AND		CONCAT(FORMAT(@RequestDate, N'yyyy-MM-dd'),' 23:59:59') 
			AND [Status] = 1
			AND ID <> @pID
			)
				BEGIN
					SELECT 1;
				END
			ELSE
				BEGIN
					SELECT 0;
				END
		END
	END
		
	
END
GO
------------------------------------------------------------
CREATE PROCEDURE [dbo].[spGet_UserRoleByUsernamePassword]
   (
       @pUserName varchar(50),
       @pPassWord varchar(200)
   )
AS
BEGIN
   -- SET NOCOUNT ON added to prevent extra result sets from
   -- interfering with SELECT statements.
   SET NOCOUNT ON
   SELECT userrole FROM dbo.tbl_DCS_SystemUser WHERE UserName = @pUserName and Password = @pPassWord and Active = 1
END


