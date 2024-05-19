IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateRoom]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateRoom
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 13/05/2024
-- Description:	Update room details
-- =============================================
CREATE PROCEDURE UpdateRoom 
	@roomId	INT,
	@roomName NVARCHAR(100) = NULL,
	@guestCount INT = NULL,
	@price DECIMAL(18,2) = NULL,
	@discountPercent DECIMAL(18,2) = NULL,
    @discountAmount DECIMAL(18,2) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM Room WHERE RoomId = @roomId)
	BEGIN
		UPDATE Room
		SET RoomName = ISNULL(@roomName, RoomName),
            CountOfMaxGuest = ISNULL(@guestCount, CountOfMaxGuest),
            Price = ISNULL(@price, Price),
            DiscountPercent = ISNULL(@discountPercent, DiscountPercent),
            DiscountPrice = ISNULL(@discountAmount, DiscountPrice)
        WHERE RoomID = @roomID
	END
END
GO
