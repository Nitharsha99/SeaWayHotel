IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateRooms]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateRooms
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 18/04/2024
-- Description:	Update the room details
-- =============================================

CREATE PROCEDURE UpdateRooms 
	@roomId INT,
	@roomName NVARCHAR(250),
	@guestCountMax INT,
	@price DECIMAL(18, 2),
	@discountPercentage DECIMAL(18,2),
	@isActive BIT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @discountAmount DECIMAL(18,2);

	IF(@discountPercentage <> 0)
	BEGIN
	  SET @discountAmount = (@discountPercentage * @price) / 100;
	END

    UPDATE Room 
	SET RoomName = @roomName,
	    CountOfMaxGuest = @guestCountMax,
		Price = @price,
		DiscountPercent = @discountPercentage,
		DiscountPrice = @discountAmount,
		IsActive = @isActive,
		InsertedTime = GETDATE()
	WHERE RoomId = @roomId
END
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomsWithPicDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomsWithPicDetails
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllRoomsWithPicDetails]    Script Date: 4/18/2024 7:34:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 04/04/2024
-- Description:	Get all details of rooms with those pics
-- =============================================

/**************
    modifications

***************/

CREATE PROCEDURE [dbo].[GetAllRoomsWithPicDetails]
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all room details along with associated picture details
    SELECT 
	R.RoomId, 
	R.RoomName,
	R.CountOfMaxGuest,
	R.Price,
	R.DiscountPercent,
	R.DiscountPrice,
	PD.PicType,
	PD.Name AS PicName,
	PD.Value AS PicValue,
	PD.PicTypeId,
	PD.CloudinaryPublicId
    FROM Room R
    LEFT JOIN PicDocuments PD ON R.RoomID = PD.PicTypeID 
	WHERE (R.IsActive = 1) AND (PD.PicType = 'Room')
END
GO

---------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[SearchRooms]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE SearchRooms
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 04/04/2024
-- Description:	Get all details of rooms with those pics using parameters
-- =============================================

CREATE PROCEDURE SearchRooms
     @roomId INT = NULL,                           
	 @guestCount INT = NULL                        
AS
BEGIN
   SET NOCOUNT ON;

   
	/*********************  Create one temp table for Rooms ********************/

	CREATE TABLE #roomTemp(
	     RoomId INT,
		 RoomName NVARCHAR(250),
		 CountOfMaxGuest INT,
		 Price DECIMAL(18,2),
		 DiscountPercent DECIMAL(18,2),
		 DiscountPrice DECIMAL(18,2)
	);

	INSERT INTO #roomTemp(RoomId, RoomName, CountOfMaxGuest, Price, DiscountPercent, DiscountPrice)
	SELECT 	R.RoomId, 
			R.RoomName,
			R.CountOfMaxGuest,
			R.Price,
			R.DiscountPercent,
	        R.DiscountPrice
	FROM Room R
	WHERE (R.IsActive = 1) AND 
	(@roomId IS NULL OR R.RoomId = @roomId)                                                    

	/******************  create table for room pics  ***********************/
	CREATE TABLE #picTemp(
	   PicType NVARCHAR(20),
	   PicName NVARCHAR(150),
	   PicValue IMAGE,
	   PicTypeId INT,
	   CloudinaryPublicId NVARCHAR(100)
	);

	INSERT INTO #picTemp(PicType, PicName, PicValue, PicTypeId, CloudinaryPublicId)
	SELECT PD.PicType,
			PD.Name,
			PD.Value,
			PD.PicTypeId,
			PD.CloudinaryPublicId
	FROM PicDocuments PD
	WHERE PD.PicTypeId = @roomId

	SELECT * FROM #roomTemp;
	SELECT * FROM #picTemp;
END 
GO
