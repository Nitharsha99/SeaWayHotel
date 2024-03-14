IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Customer' and column_name = 'Name')
BEGIN
ALTER TABLE Customer
ALTER COLUMN Name VARCHAR(200) NOT null
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Customer' and column_name = 'Email')
BEGIN
ALTER TABLE Customer
ALTER COLUMN Email VARCHAR(300) NOT null
END
GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[NewCustomer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE NewCustomer
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 12-03-2024
-- Description:	create new customer
-- =============================================

CREATE PROCEDURE NewCustomer
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

		INSERT INTO Customer (Name, Email, ContactNo, NIC_No, PassportNo) 
	    VALUES (@customerName, @email, @contactNo, ISNULL(@nicNo, NULL), ISNULL(@passportNo, NULL))

		SET @existCustomerId = SCOPE_IDENTITY();

	END

	IF EXISTS(SELECT 1 FROM Customer WHERE NIC_No = @nicNo OR PassportNo = @passportNo)
	BEGIN

		 SELECT @existCustomerId=CustomerId FROM Customer WHERE Email = @email AND (NIC_No = @nicNo OR PassportNo = @passportNo)

	END

END
GO