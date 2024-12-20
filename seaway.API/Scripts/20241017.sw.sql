IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetBookingDetailsById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetBookingDetailsById
END
GO

/****** Object:  StoredProcedure [dbo].[GetBookingDetailsById]    Script Date: 17/10/2024 19:44:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 05/10/2024
-- Description:	Get all details about booking 
-- =============================================

/*
	MODIFICATIONS

	001            Nitharsha                     17/10/2024                 solved the issue: get same room details fo different booking id 
*/

CREATE PROCEDURE [dbo].[GetBookingDetailsById] 
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
		LEFT JOIN Customer C ON B.CustomerId = C.CustomerId 
		LEFT JOIN BookingRooms BR ON BR.BookingId = B.BookingId                                      --001
		LEFT JOIN Rooms R ON R.RoomId = BR.RoomId
		LEFT JOIN RoomCategory RC ON R.RoomTypeId = RC.CategoryId
		WHERE B.BookingId = @bookingId
   END

END
GO
