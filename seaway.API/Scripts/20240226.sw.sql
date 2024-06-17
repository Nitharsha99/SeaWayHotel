/*
   
   Creator: Nitharsha
   Purpose: change column field and create some new SPs
   Date   : 26/02/2024

*/

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------change IsActive column ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'IsActive')
BEGIN
ALTER TABLE PicDocuments
DROP COLUMN IsActive
END
GO

IF Not EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Activities' and column_name = 'IsActive')
BEGIN
ALTER TABLE Activities
Add IsActive bit
END
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------create Insert Activity with pic SP ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertActivityWithPic]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertActivityWithPic
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 26-02-2024
-- Description:	Insert activity details
-- =============================================

CREATE PROCEDURE InsertActivityWithPic
	@ActivityName NVARCHAR(100),
	@ActivityDescription NVARCHAR(500),
    @IsActive BIT,
    @PicType NVARCHAR(30),
    @PicName NVARCHAR(100),
    @PicValue VARBINARY(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InsertedActivityId INT;
    DECLARE @InsertedPicDocumentId INT;

    -- Insert into Activity table
	IF NOT EXISTS(SELECT 1 FROM Activities WHERE Name=@ActivityName AND IsActive=1)
	BEGIN
		INSERT INTO Activities(Name, Description, IsActive, InsertedTime)
		VALUES (@ActivityName, @ActivityDescription, @IsActive, GETDATE());

		SET @InsertedActivityId = SCOPE_IDENTITY(); -- Get the ID of the newly inserted activity

        -- Insert into PicDocument table
		INSERT INTO PicDocuments (picTypeId, picType, Name, Value, InsertedTime)
		VALUES (@InsertedActivityId, @PicType, @PicName, @PicValue, GETDATE());

		SET @InsertedPicDocumentId = SCOPE_IDENTITY(); -- Get the ID of the newly inserted PicDocument

		-- Return the IDs of the inserted records
		SELECT @InsertedActivityId AS InsertedActivityId, @InsertedPicDocumentId AS InsertedPicDocumentId;
	END
    
END
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------create upload image SP ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UploadImage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UploadImage
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 26-02-2024
-- Description:	Uploading Images
-- =============================================

CREATE PROCEDURE UploadImage
	@PicTypeId INT,
	@PicType NVARCHAR(30),
	@PicName NVARCHAR(100),
	@PicValue VARBINARY(MAX)
AS
BEGIN

	SET NOCOUNT ON;

    INSERT INTO PicDocuments (PicTypeId, PicType, Name, Value, InsertedTime)
    VALUES (@PicTypeId, @PicType, @PicName, @PicValue, GETDATE());
END
GO