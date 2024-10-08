IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewOffer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertNewOffer
END
GO
/****** Object:  StoredProcedure [dbo].[InsertNewOffer]    Script Date: 15/08/2024 20:32:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 17/06/2024
-- Description:	Insert new Offer
-- =============================================

/*
Modifications
	
	001                 Nitharsha            15/08/2024                      add createdBy, created parameters
*/
CREATE PROCEDURE [dbo].[InsertNewOffer] 
	@offerName NVARCHAR(100),
	@description NVARCHAR(1000),
	@isRoomOffer BIT,
	@validFrom DATETIME,
	@validTo DATETIME,
	@price DECIMAL(18, 2),
	@discountPercentage DECIMAL(18,2),
	@createdBy NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @discountAmount DECIMAL(18,2);
	DECLARE @newOfferId INT;

	IF NOT EXISTS(SELECT 1 FROM Offers WHERE Name = @offerName AND IsActive = 1)
	BEGIN

		SET @discountAmount = (@price * @discountPercentage) / 100;

		INSERT INTO Offers
		       (Name, Description, IsActive, IsRoomOffer, 
			   ValidFrom, ValidTo, Price, DiscountPercent, DiscountPrice, 
			   Created, CreatedBy, Updated, UpdatedBy)
		VALUES 
				(@offerName, @description, 1, 
				@isRoomOffer, @validFrom, @validTo, @price, @discountPercentage, @discountAmount, 
				GETDATE(), @createdBy, GETDATE(), @createdBy);

		SET @newOfferId = SCOPE_IDENTITY();

		SELECT @newOfferId AS offerId;
	END

END
GO