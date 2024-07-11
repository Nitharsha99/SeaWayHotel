IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomCategories]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomCategories
END
GO

-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 11/07/2024
-- Description:	Get all details of room categories
-- =============================================
CREATE PROCEDURE GetAllRoomCategories
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all room details along with associated picture details
    SELECT 
	R.CategoryId, 
	R.RoomName,
	R.CountOfMaxGuest,
	R.Price,
	R.DiscountPercent,
	R.DiscountPrice,
	R.Created,
	R.CreatedBy,
	R.Updated,
	R.UpdatedBy
    FROM RoomCategory R
END
GO
 ---------------------------------------------------------------------------------------------------------------------------------------------------
 ---------------------------------------------------------------------------------------------------------------------------------------------------

 IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewRoomCategory]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertNewRoomCategory
END
GO

-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 11/07/2024
-- Description:	Insert new room Category
-- =============================================
CREATE PROCEDURE InsertNewRoomCategory
	@roomName NVARCHAR(250),
	@guestCountMax INT,
	@price DECIMAL(18, 2),
	@discountPercentage DECIMAL(18,2) = null,
	@createdBy NVARCHAR(100)
AS
BEGIN
	
	DECLARE @discountPrice DECIMAL(18,2) = null;
	DECLARE @InsertedCategoryId INT;                             

	IF NOT EXISTS(SELECT 1 FROM RoomCategory WHERE RoomName = @roomName)             
	BEGIN
		
		SET @discountPrice = (@price * @discountPercentage)/ 100;

		INSERT INTO RoomCategory(RoomName, CountOfMaxGuest, Price, DiscountPrice, DiscountPercent, Created, CreatedBy, Updated, UpdatedBy)                --003
		VALUES(@roomName, @guestCountMax, @price, @discountPrice, @discountPercentage, GETDATE(), @createdBy, GETDATE(), @createdBy)                      

		SET @InsertedCategoryId = SCOPE_IDENTITY();

		SELECT @InsertedCategoryId AS roomCategoryId;

	END

END
GO

---------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomCategoryWithPicDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomCategoryWithPicDetails
END
GO

-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 11/07/2024
-- Description:	Get all details of room category with those pics
-- =============================================
CREATE PROCEDURE GetAllRoomCategoryWithPicDetails
    @categoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all room details along with associated picture details
	IF EXISTS(SELECT 1 FROM RoomCategory WHERE CategoryId = @categoryId)
	BEGIN
	       SELECT 
			R.CategoryId, 
			R.RoomName,
			R.CountOfMaxGuest,
			R.Price,
			R.DiscountPercent,
			R.DiscountPrice,
			R.Created,
			R.CreatedBy,
			R.Updated,
			R.UpdatedBy,
			PD.PicType,
			PD.Name AS PicName,
			PD.Value AS PicValue,
			PD.PicTypeId,
			PD.CloudinaryPublicId
			FROM RoomCategory R
			LEFT JOIN PicDocuments PD ON R.CategoryId = PD.PicTypeID AND PD.PicType = 'Room'
			WHERE @categoryId = r.CategoryId                                                  
	END
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateRoomCategory]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateRoomCategory
END
GO

-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 11/07/2024
-- Description:	Update room category details
-- =============================================
CREATE PROCEDURE UpdateRoomCategory
	@categoryId	INT,
	@roomName NVARCHAR(100) = NULL,
	@guestCount INT = NULL,
	@price DECIMAL(18,2) = NULL,
	@discountPercent DECIMAL(18,2) = NULL,
	@updatedBy NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @discountAmount DECIMAL(18,2);

	IF EXISTS(SELECT 1 FROM RoomCategory WHERE CategoryId = @categoryId)
	BEGIN

		SET @discountAmount = ISNULL(@discountPercent, (SELECT DiscountPercent FROM RoomCategory WHERE CategoryId = @categoryId)) 
		                             *  ISNULL(@price, (SELECT Price FROM RoomCategory WHERE CategoryId = @categoryId)) / 100;

		UPDATE RoomCategory
		SET RoomName = ISNULL(@roomName, RoomName),
            CountOfMaxGuest = ISNULL(@guestCount, CountOfMaxGuest),
            Price = ISNULL(@price, Price),
            DiscountPercent = ISNULL(@discountPercent, DiscountPercent),
            DiscountPrice = @discountAmount,
			Updated = GETDATE(),
			UpdatedBy = @updatedBy
        WHERE CategoryId = @categoryId
	END
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteRoomCategoryWithPics]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE DeleteRoomCategoryWithPics
END
GO

-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 11/07/2024
-- Description:	Delete RoomCategory and related Pics of Category
-- =============================================
CREATE PROCEDURE DeleteRoomCategoryWithPics
	@categoryId INT
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @isCategoryRemove BIT = 0;

	IF EXISTS(SELECT 1 FROM RoomCategory WHERE CategoryId = @categoryId)
	BEGIN
		DELETE FROM RoomCategory WHERE CategoryId = @categoryId

		SET @isCategoryRemove = 1

		IF(@isCategoryRemove = 1)
		BEGIN
			IF EXISTS(SELECT 1 FROM PicDocuments WHERE PicTypeId = @categoryId AND PicType = 'Room')
			BEGIN
				DELETE FROM PicDocuments WHERE PicTypeId = @categoryId AND PicType = 'Room'
			END
		END
	END
END
GO
