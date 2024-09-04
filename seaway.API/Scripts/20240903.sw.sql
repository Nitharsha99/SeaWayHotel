IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[BookingDetailsByRoomId]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE BookingDetailsByRoomId 
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 03/09/2024
-- Description:	Get Booking details of particular room
-- =============================================
CREATE PROCEDURE BookingDetailsByRoomId 
	@roomId	INT
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM BookingRooms WHERE RoomId = @roomId)
	BEGIN
		SELECT 
		BR.BookingId,
		B.BookingDate,
		B.CheckinDateTime,
		B.CheckoutDateTime
		FROM BookingRooms BR
		INNER JOIN Bookings B ON BR.BookingId = B.BookingId
		WHERE BR.RoomId = @roomId AND B.CheckoutDateTime > GETDATE()
		ORDER BY B.BookingDate DESC
	END
END
GO
