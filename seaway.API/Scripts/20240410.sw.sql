IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewRoom]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertNewRoom
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 14-03-2024
-- Description:	Insert new room
-- =============================================

/*
   Modifications----

   001        10/04/2024       Nitharsha           change isactive always active when insert new room   
   002        10/04/2024       Nitharsha           get room id of new room record
*/

CREATE PROCEDURE [dbo].[InsertNewRoom]
	@roomName NVARCHAR(250),
	@guestCountMax INT,
	@price DECIMAL(18, 2),
	@discountPercentage DECIMAL(18,2) = null           
AS
BEGIN
	
	DECLARE @discountPrice DECIMAL(18,2) = null;
	DECLARE @InsertedRoomId INT;                              --002

	IF NOT EXISTS(SELECT 1 FROM Room WHERE RoomName = @roomName AND IsActive = 1)              --001
	BEGIN
		
		SET @discountPrice = (@price * @discountPercentage)/ 100;

		INSERT INTO Room(RoomName, CountOfMaxGuest, Price, DiscountPrice, DiscountPercent, IsActive, InsertedTime)
		VALUES(@roomName, @guestCountMax, @price, @discountPrice, @discountPercentage, 1, GETDATE())                      --001

		--002
		SET @InsertedRoomId = SCOPE_IDENTITY();

		SELECT @InsertedRoomId AS roomId;

	END

END
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UploadImage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UploadImage
END
GO

/****** Object:  StoredProcedure [dbo].[UploadImage]    Script Date: 4/10/2024 1:02:38 PM ******/
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
	002       10/04/2024      Nitharsha         change data type of pic value
*/

CREATE PROCEDURE [dbo].[UploadImage]
	@PicTypeId INT,
	@PicType NVARCHAR(30),
	@PicName NVARCHAR(100),
	@PicValue IMAGE,                  --002
	@PublicId NVARCHAR(100)                   --001
AS
BEGIN

	SET NOCOUNT ON;

	select @PicValue as Original

	--001
	INSERT INTO PicDocuments (PicTypeId, PicType, Name, Value, InsertedTime, CloudinaryPublicId)
    VALUES (@PicTypeId, @PicType, @PicName, @PicValue, GETDATE(), @PublicId)


END
GO
