IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetBookingDetailsById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetBookingDetailsById
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 05/10/2024
-- Description:	Get all details about booking 
-- =============================================
CREATE PROCEDURE GetBookingDetailsById 
	@bookingId INT
AS
BEGIN
	SET NOCOUNT ON;

   IF EXISTS(SELECT 1 FROM Bookings WHERE BookingId = @bookingId)
   BEGIN

		SELECT 
		B.BookingId,
		B.BookingDate,
		B.CheckinDateTime,
		B.CheckoutDateTime,
		B.GuestCount,
		B.RoomCount,
		B.Created,
		B.Updated,
		C.Name,
		C.ContactNo,
		C.Email,
		C.NIC_No,
		C.PassportNo,
		BR.BookingRoomId,
		R.RoomNumber,
		RC.RoomName
		FROM Bookings B 
		JOIN Customer C ON B.CustomerId = C.CustomerId 
		JOIN BookingRooms BR ON B.BookingId = B.BookingId
		JOIN Rooms R ON R.RoomId = BR.RoomId
		JOIN RoomCategory RC ON R.RoomTypeId = RC.CategoryId
		WHERE B.BookingId = @bookingId
   END

END
GO
