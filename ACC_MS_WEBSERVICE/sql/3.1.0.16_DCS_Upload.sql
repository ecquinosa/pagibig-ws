
GO
CREATE TABLE tbl_DCS_Upload (
    ID INT IDENTITY (1, 1),
	Status VARCHAR (25) NOT NULL,
	RefNum  VARCHAR (22) NOT NULL,
	PagIBIGID VARCHAR (12) NOT NULL,
	IsPushCardInfo BIT NOT NULL,
	PushCardInfoDate DateTime NULL,
	IsPackUpData BIT NOT NULL,
	PackUpDataDate DateTime NULL,
	Remarks VARCHAR(100) NULL,
	Username VARCHAR (20) NOT NULL,
	EntryDate DateTime NOT NULL,
	LastUpdate DateTime NOT NULL,
	PRIMARY KEY (ID,RefNum),
	CONSTRAINT FK_tbl_Member_tbl_DCS_Upload FOREIGN KEY (RefNum)
	REFERENCES tbl_Member (RefNum)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);



GO
CREATE PROCEDURE [dbo].[prcSaveDCS_Upload]
	(	
		   @Status varchar (25),
		   @RefNum varchar(22),
           @PagIBIGID varchar (12),
           @IsPushCardInfo bit,
		   @PushCardInfoDate DateTime = null,
		   @IsPackUpData bit,
		   @PackUpDataDate DateTime = null,
		   @Remarks varchar(100),
		   @Username varchar(20)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	IF EXISTS (SELECT RefNum From tbl_DCS_Upload WHERE RefNum = @RefNum) 
	BEGIN
		UPDATE tbl_DCS_Upload
		SET Status = @Status,  IsPushCardInfo = @IsPushCardInfo , PushCardInfoDate= @PushCardInfoDate, IsPackUpData= @IsPackUpData , PackUpDataDate=@PackUpDataDate,Remarks =@Remarks,Username = @Username , LastUpdate = GETDATE()
		WHERE RefNum = @RefNum
	END
	ELSE
	BEGIN
		INSERT INTO tbl_DCS_Upload
		(Status,RefNum,PagIBIGID,IsPushCardInfo,PushCardInfoDate,IsPackUpData,PackUpDataDate,Remarks,Username ,EntryDate,LastUpdate) 
		VALUES
		('NEW',@RefNum,@PagIBIGID,@IsPushCardInfo,@PushCardInfoDate,@IsPackUpData,@PackUpDataDate,@Remarks,@Username,GETDATE(),GETDATE())	
	END
END
--------------------------------------------------------------------------------------------------------------

GO
CREATE PROCEDURE [dbo].[prcSelectDCS_Upload]
	(
			@RefNum varchar(22)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT   ID,Status,RefNum,PagIBIGID,IsPushCardInfo,PushCardInfoDate,IsPackUpData,PackUpDataDate,Remarks,Username,EntryDate,LastUpdate
	FROM     tbl_DCS_Upload
	WHERE   (RefNum = @RefNum)

END
---------------------------------------------------------------------------------------------------------------------
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prcSelectDCS_UploadForUpload]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT   TOP 10 ID,Status,RefNum,PagIBIGID,IsPushCardInfo,PushCardInfoDate,IsPackUpData,PackUpDataDate,Remarks,Username,EntryDate,LastUpdate
	FROM     tbl_DCS_Upload
	WHERE   (Status = 'NEW')
	ORDER BY EntryDate

END

----------------------------------------------------------------------------------------------------------------------

-- Process all existing as uploaded and completed.
INSERT INTO tbl_DCS_Upload (Status,RefNum,PagIBIGID,IsPushCardInfo,PushCardInfoDate,IsPackUpData,PackUpDataDate,Remarks,Username,EntryDate,LastUpdate)
SELECT 'COMPLETE',RefNum,PagIBIGID,0,NULL,0,NULL,'POST INSERT EXISTING DATA',UserName,GETDATE(),GETDATE() FROM tbl_Member


GO
ALTER PROCEDURE [dbo].[prcAddDCS_Card_Account]
	(	
		@RefNum varchar (22),
        @PagIBIGID varchar (12),
        @BankCode varchar (5),
        @CardNo varchar (25),
        @AccountNumber varchar (20),        
        @EntryUsername varchar (20),        
        @LastUpdateUserName varchar (20)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO tbl_DCS_Card_Account
		(RefNum,PagIBIGID,BankCode,CardNo,AccountNumber,EntryDate,EntryUsername,LastUpdate,LastUpdateUserName) 
	VALUES
	--	(@RefNum,@PagIBIGID,@BankCode,@CardNo,@AccountNumber,GETDATE(),@EntryUsername,GETDATE(),@LastUpdateUserName)	
	(@RefNum,@PagIBIGID,@BankCode,@CardNo,@AccountNumber,GETDATE(),@EntryUsername,GETDATE(),@LastUpdateUserName)
	
END

