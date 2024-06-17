IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewOffer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertNewOffer
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 17/06/2024
-- Description:	Insert new Offer
-- =============================================
CREATE PROCEDURE InsertNewOffer 
	@offerName NVARCHAR(100),
	@description NVARCHAR(1000),
	@isRoomOffer BIT,
	@validFrom DATETIME,
	@validTo DATETIME,
	@price DECIMAL(18, 2),
	@discountPercentage DECIMAL(18,2)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @discountAmount DECIMAL(18,2);
	DECLARE @newOfferId INT;

	IF NOT EXISTS(SELECT 1 FROM Offers WHERE Name = @offerName AND IsActive = 1)
	BEGIN

		SET @discountAmount = (@price * @discountPercentage) / 100;

		INSERT INTO Offers(Name, Description, IsActive, IsRoomOffer, ValidFrom, ValidTo, Price, DiscountPercent, DiscountPrice, InsertedTime)
		VALUES(@offerName, @description, 1, @isRoomOffer, @validFrom, @validTo, @price, @discountPercentage, @discountAmount, GETDATE());

		SET @newOfferId = SCOPE_IDENTITY();

		SELECT @newOfferId AS offerId;
	END
END
GO
