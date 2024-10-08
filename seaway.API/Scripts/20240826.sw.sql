IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteOfferWithPics]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE DeleteOfferWithPics
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteOfferWithPics]    Script Date: 26/08/2024 17:31:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 21/06/2024
-- Description:	Delete the offer with pictures
-- =============================================

/*
	Modifications

	001          Nitharsha            26/08/2024                  change offer into enum value
*/
CREATE PROCEDURE [dbo].[DeleteOfferWithPics] 
	@offerId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @isOfferRemove BIT = 0;

	IF EXISTS(SELECT 1 FROM Offers WHERE OfferId = @offerId)
	BEGIN 
		DELETE FROM Offers WHERE OfferId = @offerId

		SET @isOfferRemove = 1;

		IF(@isOfferRemove =1)
		BEGIN
			IF EXISTS(SELECT 1 FROM PicDocuments WHERE PicTypeId = @offerId AND PicType = 3)
			BEGIN
				DELETE FROM PicDocuments WHERE PicTypeId = @offerId AND PicType = 3
			END
		END
	END

END
GO
-----------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateOffer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateOffer
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateOffer]    Script Date: 26/08/2024 20:17:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 19/06/2024
-- Description:	Update the Offer details
-- =============================================

/*
	Modification

	001          Nitharsha            26/08/2024                 add updated details
*/
CREATE PROCEDURE [dbo].[UpdateOffer] 
	@offerId INT,
	@offerName NVARCHAR(100) = NULL,
	@description NVARCHAR(1000) = NULL,
	@isRoomOffer BIT = NULL,
	@validFrom DATETIME = NULL,
	@validTo DATETIME = NULL,
	@price DECIMAL(18, 2) = NULL,
	@discountPercent DECIMAL(18,2) = NULL,
	@isActive BIT = NULL,
	@updatedBy NVARCHAR(100)
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
			IsRoomOffer = ISNULL(@isRoomOffer, IsRoomOffer),
			Updated = GETDATE(),                                   --001
			UpdatedBy = @updatedBy                                  --001
		WHERE OfferId = @offerId
		
	END
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetOfferById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetOfferById
END
GO
/****** Object:  StoredProcedure [dbo].[GetOfferById]    Script Date: 26/08/2024 20:40:37 ******/
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
	002           Nitharsha               26/08/2024               add pictures

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
			O.Updated AS Updated,                                                          --001
			O.UpdatedBy AS UpdatedBy,                                                        --001
			PD.PicType,
			PD.Name AS PicName,
			PD.Value AS PicValue,
			PD.PicTypeId,
			PD.CloudinaryPublicId
			FROM Offers O
			LEFT JOIN PicDocuments PD ON O.OfferId = PD.PicTypeID AND PD.PicType = 3                       --002
			WHERE (O.OfferId = @OfferId)
	END
END
GO


