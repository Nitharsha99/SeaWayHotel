IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateRoom]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateRoom
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateRoom]    Script Date: 18/05/2024 12:24:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 13/05/2024
-- Description:	Update room details
-- =============================================

/******************************************************************

    MODIFICATIONS

001        Nitharsha              18/05/2024                  change amount while updating room


******************************************************************/
CREATE PROCEDURE [dbo].[UpdateRoom] 
	@roomId	INT,
	@roomName NVARCHAR(100) = NULL,
	@guestCount INT = NULL,
	@price DECIMAL(18,2) = NULL,
	@discountPercent DECIMAL(18,2) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @discountAmount DECIMAL(18,2);

	IF EXISTS(SELECT 1 FROM Room WHERE RoomId = @roomId)
	BEGIN

		SET @discountAmount = ISNULL(@discountPercent, (SELECT DiscountPercent FROM Room WHERE RoomId = @roomId)) *  ISNULL(@price, (SELECT Price FROM Room WHERE RoomId = @roomId)) / 100;

		UPDATE Room
		SET RoomName = ISNULL(@roomName, RoomName),
            CountOfMaxGuest = ISNULL(@guestCount, CountOfMaxGuest),
            Price = ISNULL(@price, Price),
            DiscountPercent = ISNULL(@discountPercent, DiscountPercent),
            DiscountPrice = @discountAmount
        WHERE RoomID = @roomID
	END
END
GO


--------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Room' and column_name = 'IsActive')
BEGIN
ALTER TABLE Room
DROP COLUMN IsActive
END
GO
