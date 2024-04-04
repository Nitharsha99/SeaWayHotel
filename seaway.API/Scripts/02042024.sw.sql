IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'CloudinaryPublicId')
BEGIN
ALTER TABLE PicDocuments
ADD CloudinaryPublicId	VARCHAR(300) NULL
END
GO