IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetOfferById
END
GO
/****** Object:  StoredProcedure [dbo].[GetOfferById]    Script Date: 16/08/2024 18:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kavishan
-- Create date: 29/05/2024
-- Description:	get offer details for an id
-- =============================================

/*
	Modifications

	001           Nitharsha               16/08/2024               add created, updated details

*/
CREATE PROCEDURE [dbo].[GetOfferById]
	@OfferId INT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM Offers WHERE OfferId = @OfferId)
	BEGIN
		SELECT 
			O.offerId AS OfferId,
			O.Name AS Name,
			O.Description AS Description,
			O.Price AS Price,
			O.DiscountPercent AS DiscountPercentage,
			O.DiscountPrice AS DiscountAmount,
			O.ValidFrom AS ValidFrom,
			O.ValidTo AS ValidTo,
			O.IsActive AS IsActive,
			O.IsRoomOffer AS IsRoomOffer,
			O.Created AS Created,
			O.CreatedBy AS CreatedBy,
			O.Updated AS Updated,
			O.UpdatedBy AS UpdatedBy
			FROM Offers O
			WHERE (O.OfferId = @OfferId)
	END
END
GO
