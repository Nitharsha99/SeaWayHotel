IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllOffers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllOffers
END
GO

-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 18/06/2024
-- Description:	Get all offers details
-- =============================================
CREATE PROCEDURE GetAllOffers
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
	o.OfferId,
	o.Name AS OfferName,
	o.Description,
	o.ValidFrom,
	o.ValidTo,
	o.IsRoomOffer,
	o.Price,
	o.DiscountPercent,
	o.DiscountPrice,
	o.IsActive
	FROM Offers o;
END
GO
