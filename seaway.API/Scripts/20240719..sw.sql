IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Activities' and column_name = 'PictureId')
BEGIN
ALTER TABLE Activities
DROP COLUMN PictureId
END
GO

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

/*
  Modifications

  0001    27-02-2024         Change insert only activity details
  0002    19-07-2024         add created, updated details

*/

CREATE PROCEDURE [dbo].[InsertActivityWithPic]
	@ActivityName NVARCHAR(100),
	@ActivityDescription NVARCHAR(500),
    @IsActive BIT,
	@createdBy NVARCHAR(100)             --0002
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InsertedActivityId INT;

    -- Insert into Activity table
	IF NOT EXISTS(SELECT 1 FROM Activities WHERE Name = @ActivityName and IsActive = 1)
	BEGIN
		    INSERT INTO Activities(Name, Description, IsActive, Created, CreatedBy, Updated, UpdatedBy)
			VALUES (@ActivityName, @ActivityDescription, 1, GETDATE(), @createdBy, GETDATE(), @createdBy);           --0002

			SET @InsertedActivityId = SCOPE_IDENTITY(); -- Get the ID of the newly inserted activity

			-- Return the IDs of the inserted records
			SELECT @InsertedActivityId AS activityId;
	END

END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'UpdatedBy')
BEGIN
ALTER TABLE PicDocuments
DROP COLUMN UpdatedBy
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'Updated')
BEGIN
ALTER TABLE PicDocuments
DROP COLUMN Updated
END
GO


/****** Object:  StoredProcedure [dbo].[UploadImage]    Script Date: 19/07/2024 19:33:25 ******/
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

/*
    Modifications

	001       10/04/2024      Nitharsha         Add Cloudinary public id  and change PicValue type
	002       10/04/2024      Nitharsha         convert the string value of pic value
	003       19/07/2024      Nitharsha         Add Created details
*/

CREATE PROCEDURE [dbo].[UploadImage]
	@PicTypeId INT,
	@PicType NVARCHAR(30),
	@PicName NVARCHAR(100),
	@PicValue IMAGE,                  --002
	@PublicId NVARCHAR(100),                   --001
	@CreatedBy NVARCHAR(100)              --003
AS
BEGIN

	SET NOCOUNT ON;

	select @PicValue as Original

	--001, --003
	INSERT INTO PicDocuments (PicTypeId, PicType, Name, Value, Created, CreatedBy, CloudinaryPublicId)
    VALUES (@PicTypeId, @PicType, @PicName, @PicValue, GETDATE(), @CreatedBy, @PublicId)


END
GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateActivity]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateActivity
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateActivity]    Script Date: 19/07/2024 20:11:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kavishan
-- Create date: 25/05/2024
-- Description:	Update Activity details
-- =============================================

/*
	Modifications

	001          19/07/2024         Nitharsha             add updated details

*/


CREATE PROCEDURE [dbo].[UpdateActivity] 
	@ActivityId	INT,
	@Name NVARCHAR(100) = NULL,
	@Description NVARCHAR(1000) = NULL,
	@IsActive BIT = NULL,
	@UpdatedBy NVARCHAR(100)                         --001
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM Activities WHERE ActivityId = @ActivityId)
	BEGIN
		UPDATE Activities
		SET Name = ISNULL(@Name, Name),
            Description = ISNULL(@Description, Description),
            IsActive = ISNULL(@IsActive, IsActive),
			UpdatedBy = @UpdatedBy,                       --001
			Updated = GETDATE()                          --001
        WHERE ActivityId = @ActivityId
	END
END
GO




