IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateOffer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateOffer
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 19/06/2024
-- Description:	Update the Offer details
-- =============================================
CREATE PROCEDURE UpdateOffer 
	@offerId INT,
	@offerName NVARCHAR(100) = NULL,
	@description NVARCHAR(1000) = NULL,
	@isRoomOffer BIT = NULL,
	@validFrom DATETIME = NULL,
	@validTo DATETIME = NULL,
	@price DECIMAL(18, 2) = NULL,
	@discountPercent DECIMAL(18,2) = NULL,
	@isActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @discountPrice DECIMAL(18,2);

	IF EXISTS(SELECT 1 FROM Offers WHERE OfferId = @offerId)
	BEGIN
		
		SET @discountPrice = ISNULL(@discountPercent, (SELECT DiscountPercent FROM Offers WHERE OfferId = @offerId)) *  
		                     ISNULL(@price, (SELECT Price FROM Offers WHERE OfferId = @offerId)) / 100;

		UPDATE Offers
		SET Name = ISNULL(@offerName, Name),
		    Description = ISNULL(@description, Description),
			ValidFrom = ISNULL(@validFrom, ValidFrom),
			ValidTo = ISNULL(@validTo, ValidTo),
			Price = ISNULL(@price, Price),
			DiscountPercent = ISNULL(@discountPercent, DiscountPercent),
			DiscountPrice = @discountPrice,
			IsActive = ISNULL(@isActive, IsActive),
			IsRoomOffer = ISNULL(@isRoomOffer, IsRoomOffer)
		WHERE OfferId = @offerId
		
	END
END
GO
