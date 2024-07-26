IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'PicType')
BEGIN
ALTER TABLE PicDocuments
ALTER COLUMN PicType INT NOT NULL
END
GO