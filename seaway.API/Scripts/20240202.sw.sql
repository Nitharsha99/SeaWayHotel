/*
   
   Creator: Nitharsha
   Purpose: create new tables
   Date   : 02/02/2024

*/

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create Customer table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Customer(
    CustomerId INT IDENTITY  NOT NULL  PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	ContactNo VARCHAR(20) NOT NULL,
	NIC_No VARCHAR(20) NULL,
	PassportNo VARCHAR(50) NULL
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create AdminLogin table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE AdminLogin(
    LoginId INT IDENTITY NOT NULL PRIMARY KEY,
	Username VARCHAR(50) NOT NULL,
	AdminId INT NOT NULL,
	LoginTime DATETIME NOT NULL,
	LogoutTime DATETIME NOT NULL,
	CONSTRAINT FK_Admin FOREIGN KEY (AdminId) REFERENCES Admin(AdminId)
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create PicDocuments table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE PicDocuments(
    PicDocumentId INT IDENTITY NOT NULL PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Value VARBINARY(MAX) NOT NULL,
	InsertedTime DATETIME NOT NULL
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create Activities table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Activities(
    ActivityId INT IDENTITY NOT NULL PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Description VARCHAR(1000) NOT NULL,
	PictureId INT NULL,
	InsertedTime DATETIME NOT NULL,
	CONSTRAINT FK_ActivityPic FOREIGN KEY (PictureId) REFERENCES PicDocuments(PicDocumentId)
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create Booking table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Bookings(
   BookingId INT IDENTITY NOT NULL PRIMARY KEY,
   CustomerId INT NOT NULL,
   GuestCount INT NOT NULL,
   RoomCount INT NOT NULL,
   Date DATETIME NOT NULL,
   CheckinTime DATETIME NOT NULL,
   CheckoutTime DATETIME NOT NULL,
   InsertedTime DATETIME NOT NULL,
   CONSTRAINT FK_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId)
);


-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create Rooms table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Room(
    RoomId INT IDENTITY NOT NULL PRIMARY KEY,
	PictureId INT NULL,
	CountOfMaxGuest INT NOT NULL,
	Price FLOAT NOT NULL,
	DiscountPercent FLOAT NULL,
	DiscountPrice FLOAT NULL,
	IsActive BIT NOT NULL,
	InsertedTime DATETIME NOT NULL,
	CONSTRAINT FK_RoomPic FOREIGN KEY (PictureId) REFERENCES PicDocuments(PicDocumentId)
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create BookingRoom table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE BookingRoom(
    BookingRoomId INT IDENTITY NOT NULL PRIMARY KEY,
	BookingId INT NOT NULL,
	RoomId INT NOT NULL,
	InsertedTime DATETIME NOT NULL,
	CONSTRAINT FK_Booking FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId),
	CONSTRAINT FK_Room FOREIGN KEY (RoomId) REFERENCES Room(RoomId)
);


-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create Offers table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE Offers(
    OfferId INT IDENTITY NOT NULL PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Description VARCHAR(1000) NOT NULL,
	IsActive BIT NOT NULL,
	IsRoomOffer BIT NOT NULL,
	ValidFrom DATETIME NOT NULL,
	ValidTo DATETIME NOT NULL,
	PictureId INT NULL,
	Price FLOAT NOT NULL,
	DiscountPercent FLOAT NULL,
	DiscountPrice FLOAT NULL,
	InsertedTime DATETIME NOT NULL,
	CONSTRAINT FK_OfferPic FOREIGN KEY (PictureId) REFERENCES PicDocuments(PicDocumentId)
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create RoomPicture table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE RoomPicture(
   RoomPictureId INT IDENTITY NOT NULL PRIMARY KEY,
   IsActive BIT NOT NULL,
   RoomId INT NOT NULL,
   PictureId INT NOT NULL,
   InsertedTime DATETIME NOT NULL,
   CONSTRAINT FK_room_picture FOREIGN KEY (PictureId) REFERENCES PicDocuments(PicDocumentId),
   CONSTRAINT FK_roomforpic FOREIGN KEY (RoomId) REFERENCES Room(RoomId)
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create OfferPicture table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE OfferPicture(
   OfferPictureId INT IDENTITY NOT NULL PRIMARY KEY,
   IsActive BIT NOT NULL,
   OfferId INT NOT NULL,
   PictureId INT NOT NULL,
   InsertedTime DATETIME NOT NULL,
   CONSTRAINT FK_offer_picture FOREIGN KEY (PictureId) REFERENCES PicDocuments(PicDocumentId),
   CONSTRAINT FK_offer FOREIGN KEY (OfferId) REFERENCES Offers(OfferId)
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create ActivityPicture table ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE ActivityPicture(
   ActivityPictureId INT IDENTITY NOT NULL PRIMARY KEY,
   IsActive BIT NOT NULL,
   ActivityId INT NOT NULL,
   PictureId INT NOT NULL,
   InsertedTime DATETIME NOT NULL,
   CONSTRAINT FK_picture FOREIGN KEY (PictureId) REFERENCES PicDocuments(PicDocumentId),
   CONSTRAINT FK_activity FOREIGN KEY (ActivityId) REFERENCES Activities(ActivityId)
);

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Add new column to Activities table ----------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Activities' AND COLUMN_NAME = 'IsActive')
BEGIN
    ALTER TABLE Activities
    ADD IsActive BIT NOT NULL
END


-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Add new column to PicDocuments table ----------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PicDocuments' AND COLUMN_NAME = 'IsActive')
BEGIN
    ALTER TABLE PicDocuments
    ADD IsActive BIT NOT NULL
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PicDocuments' AND COLUMN_NAME = 'PicType')
BEGIN
    ALTER TABLE PicDocuments
    ADD PicType VARCHAR(20) NOT NULL
END