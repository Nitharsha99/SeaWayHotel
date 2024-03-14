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

CREATE PROCEDURE InsertNewRoom
	@roomName NVARCHAR(250),
	@guestCountMax INT,
	@price DECIMAL(18, 2),
	@discountPercentage DECIMAL(18,2),
	@isActive BIT
AS
BEGIN
	
	DECLARE @discountPrice DECIMAL(18,2);

	IF NOT EXISTS(SELECT 1 FROM Room WHERE RoomName = @roomName AND IsActive = @isActive)
	BEGIN
		
		SET @discountPrice = (@price * @discountPercentage)/ 100;

		INSERT INTO Room(RoomName, CountOfMaxGuest, Price, DiscountPrice, DiscountPercent, IsActive, InsertedTime)
		VALUES(@roomName, @guestCountMax, @price, @discountPrice, @discountPercentage, @isActive, GETDATE())

	END

END
GO