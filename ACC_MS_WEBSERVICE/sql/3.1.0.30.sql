ALTER TABLE  [LCDB_01].[dbo].[tbl_Kiosk]
ADD Version VARCHAR (30) NULL,
	Anydesk VARCHAR(30) NULL;

GO
CREATE PROCEDURE [dbo].[spSave_KioskVersion]
(
	@pID varchar(10),
	@pVersion varchar(30),
	@pAnyDesk varchar(30)
)
AS
BEGIN
	UPDATE tbl_Kiosk
			set Version = @pVersion,
				Anydesk = @pAnyDesk,
			    LastUpdate = GETDATE()
			WHERE KioskID = @pID
END
