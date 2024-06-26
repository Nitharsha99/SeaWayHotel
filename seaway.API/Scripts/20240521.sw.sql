IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewRoom]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertNewRoom
END
GO
/****** Object:  StoredProcedure [dbo].[InsertNewRoom]    Script Date: 21/05/2024 21:49:30 ******/
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
   003        21/05/2024       Nitharsha           remove IsActive column and change below function
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

	IF NOT EXISTS(SELECT 1 FROM Room WHERE RoomName = @roomName)              --001   --003
	BEGIN
		
		SET @discountPrice = (@price * @discountPercentage)/ 100;

		INSERT INTO Room(RoomName, CountOfMaxGuest, Price, DiscountPrice, DiscountPercent, InsertedTime)                --003
		VALUES(@roomName, @guestCountMax, @price, @discountPrice, @discountPercentage, GETDATE())                      --001  003

		--002
		SET @InsertedRoomId = SCOPE_IDENTITY();

		SELECT @InsertedRoomId AS roomId;

	END

END
GO