IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[NewAdmin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE NewAdmin
END
GO
/****** Object:  StoredProcedure [dbo].[NewAdmin]    Script Date: 08/10/2024 19:39:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 07/05/2024
-- Description:	add new admin
-- =============================================

/*
	Modifications

	001          23/07/2024            Nitharsha             Add some variables and pic
	002          08/10/2024            Nitharsha             add picture in cloudinary

*/
CREATE PROCEDURE [dbo].[NewAdmin] 
	@username NVARCHAR(100),
	@password NVARCHAR(200),
	@isAdmin BIT,                          --001
	@profilePic IMAGE = NULL,              --001
	@picName NVARCHAR(100) = NULL,         --002
	@publicId NVARCHAR(100) = NULL,        --002
	@created NVARCHAR(100)                 --001
AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT 1 FROM Admin WHERE Username = @username)
	BEGIN
		INSERT INTO Admin(Username, Password, IsAdmin, ProfilePic, PicName, PublicId, CreatedBy, Created, UpdatedBy, Updated)           --002
		VALUES(@username, @password, @isAdmin, @profilePic, @picName, @publicId, @created, GETDATE(), @created, GETDATE())           --001, --002
	END

END
GO


IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Admin' and column_name = 'PicName')
BEGIN
ALTER TABLE Admin
ADD PicName VARCHAR(100) NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Admin' and column_name = 'PublicId')
BEGIN
ALTER TABLE Admin
ADD PublicId VARCHAR(100) NULL
END
GO