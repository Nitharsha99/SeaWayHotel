IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[NewCustomer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE NewCustomer
END
GO
/****** Object:  StoredProcedure [dbo].[NewCustomer]    Script Date: 28/09/2024 23:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 12-03-2024
-- Description:	create new customer
-- =============================================

/*
	MODIFICATIONS

	001           Nitharsha                28/09/2024                  add create, update field
*/

CREATE PROCEDURE [dbo].[NewCustomer]
	@customerName NVARCHAR(200),
	@email NVARCHAR(300),
	@contactNo NVARCHAR(25),
	@nicNo NVARCHAR(15) = NULL,
	@passportNo NVARCHAR(50) = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @existCustomerId INT;

	IF NOT EXISTS(SELECT 1 FROM Customer WHERE NIC_No = @nicNo OR PassportNo = @passportNo)
	BEGIN

		INSERT INTO Customer (Name, Email, ContactNo, NIC_No, PassportNo, Created, Updated) 
	    VALUES (@customerName, @email, @contactNo, ISNULL(@nicNo, NULL), ISNULL(@passportNo, NULL), GETDATE(), GETDATE())

		SET @existCustomerId = SCOPE_IDENTITY();

		SELECT @existCustomerId AS customerId;
	END

END
GO

---------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------

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
-- Create date: 28/09/2024
-- Description:	Create new booking
-- =============================================
CREATE PROCEDURE NewBooking 
	@guestCount INT,
	@roomCount INT,
	@customerId INT,
	@date DATETIME,
	@checkIn DATETIME,
	@checkOut DATETIME
AS
BEGIN
	
	SET NOCOUNT ON;

    INSERT INTO Bookings (CustomerId, BookingDate, CheckinDateTime, CheckoutDateTime, GuestCount, RoomCount, Created, Updated)
	VALUES (@customerId, @date, @checkIn, @checkOut, @guestCount, @roomCount, GETDATE(), GETDATE());
END
GO

