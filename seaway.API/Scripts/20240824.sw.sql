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
