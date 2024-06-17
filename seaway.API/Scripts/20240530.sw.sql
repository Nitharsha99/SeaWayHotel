IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'Bookings' AND column_name = 'CheckinTime')
BEGIN
    EXEC sp_rename 'Bookings.CheckinTime', 'CheckinDateTime', 'COLUMN';
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'Bookings' AND column_name = 'CheckoutTime')
BEGIN
    EXEC sp_rename 'Bookings.CheckoutTime', 'CheckoutDateTime', 'COLUMN';
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = 'Bookings' AND column_name = 'Date')
BEGIN
    EXEC sp_rename 'Bookings.Date', 'BookingDate', 'COLUMN';
END
GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[NewBooking]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE NewBooking
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 30/05/2024
-- Description:	Insert the new booking
-- =============================================
CREATE PROCEDURE NewBooking 
   @customerId INT,
   @totalGuestCount INT,
   @roomCount INT,
   @date DATETIME,
   @checkinTimeDate DATETIME,
   @checkoutTimeDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO Bookings (CustomerId, GuestCount, RoomCount, BookingDate, CheckinDateTime, CheckoutDateTime, InsertedTime)
	VALUES (@customerId, @totalGuestCount, @roomCount, @date, @checkinTimeDate, @checkoutTimeDate)
END
GO