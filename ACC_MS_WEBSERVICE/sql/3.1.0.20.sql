USE [LCDB_01]
GO
/****** Object:  StoredProcedure [dbo].[prcValidateUserAndAccess]    Script Date: 09/05/2019 1:52:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[prcValidateUserAndAccess]
	(
		@Username varchar(50),
		@UserPass varchar(200),
		@KioskID varchar(4),
		@TerminalMAC varchar(20),
		@RequestingBranch char(5)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Response varchar(100)

	IF NOT EXISTS(SELECT * FROM tbl_Kiosk WHERE KioskID = @KioskID)
		BEGIN
			SET @Response = '1|KioskID is not valid'
		END
	ELSE
		IF NOT EXISTS(SELECT * FROM tbl_Kiosk WHERE MacAddress = @TerminalMAC)
			BEGIN
				SET @Response = '1|Terminal identifier is not valid'
			END
		ELSE
			IF NOT EXISTS(SELECT * FROM tbl_Kiosk WHERE KioskID = @KioskID AND MacAddress = @TerminalMAC)
				BEGIN
					SET @Response = '1|KioskID and Terminal ID are not matched'
				END
			ELSE				
				--IF NOT EXISTS(SELECT * FROM tbl_DCS_KioskBank WHERE KioskID = @KioskID)
				--	BEGIN
				--		SET @Response = '1|KioskID have no assigned bank'
				--	END
				--ELSE	
				IF EXISTS(SELECT * FROM tbl_DCS_SystemUser WHERE UserName = @Username AND ISNULL(Active,0)=0)
						BEGIN
							SET @Response = '1|User is unauthorized'
						END
				ELSE
					IF EXISTS(SELECT * FROM tbl_DCS_SystemUser WHERE UserName = @Username AND ISNULL(Block,0)=1)
							BEGIN
								SET @Response = '1|User is unauthorized'
							END
					ELSE
						IF NOT EXISTS(SELECT * FROM tbl_DCS_SystemUser WHERE UserName = @Username AND Password = @UserPass)
							BEGIN
								SET @Response = '1|Invalid credential'
							END
						ELSE
							IF NOT EXISTS(SELECT * FROM tbl_DCS_SystemUser WHERE UserName = @Username AND Active = 1)
								BEGIN
									SET @Response = '1|Account is not active'
								END
							ELSE
								BEGIN
									DECLARE @UserID int, @UserRole varchar(50), @FullName varchar(150), @IdNumber varchar(150)					

									SELECT @UserID = ID,  @UserRole =  userrole, 
										   @FullName = FirstName + ' ' + ISNULL(MiddleName,'') + ' ' + LastName,
										   @IdNumber = idnumber
									FROM tbl_DCS_SystemUser WHERE UserName = @Username							

									IF NOT EXISTS(SELECT * FROM tbl_DCS_UserBranchSchedule WHERE UserID = @UserID AND requesting_branchcode = @RequestingBranch AND  GETDATE() BETWEEN SchedDateStart AND SchedDateEnd)--SchedDateStart >= GETDATE() AND SchedDateEnd <= GETDATE())
										BEGIN
											SET @Response = '1|Account have no access on this PAGIBIG branch or have no schedule'
										END
									ELSE
										IF NOT EXISTS(SELECT * FROM tbl_DCS_BranchKiosk WHERE requesting_branchcode = @RequestingBranch AND  KioskID = @KioskID)
											BEGIN
												SET @Response = '1|Terminal have no access on this PAGIBIG branch or have no schedule'
											END
										ELSE
											BEGIN
												--DECLARE @BankID	 int
												--SELECT TOP 1 @BankID = BankID FROM tbl_DCS_KioskBank WHERE KioskID = @KioskID

												SET @Response = '0|' + STR(@UserID,LEN(@UserID)) + '|' + RTRIM(@FullName) + '|' + @UserRole + '|'  + @IdNumber    -- + '|' + STR(@BankID,LEN(@BankID))
											END
								END							
	
		SELECT @Response 

END

