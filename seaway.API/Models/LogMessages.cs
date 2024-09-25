using Newtonsoft.Json.Serialization;

namespace seaway.API.Models
{
    public static class LogMessages
    {
        public const string GetRoomDataError = "An exception occurred while get all data from Room --> ";
        public const string DeleteRoomError = "An exception occurred while deleting room --> ";

        public const string GetActivityDataError = "An exception occurred while get all data from Activity --> ";
        public const string DeleteActivityError = "An exception occurred while deleting activity --> ";

        public const string GetOfferDataError = "An exception occurred while get all data from Offers --> ";
        public const string DeleteOfferError = "An exception occurred while deleting offer --> ";

        public const string GetAdminDataError = "An exception occurred while get all data from Admin --> ";

        public const string GetBookingDataError = "An exception occurred while get all data from Bookings --> ";

        public const string GetRoomCategoryDataError = "An exception occurred while get all data from RoomCategory --> ";
        public const string DeleteRoomCategoryError = "An exception occurred while deleting room category --> ";

        public const string InvalidIdError = "Invalid Id Passed";
        public const string EmptyDataSetError = "Empty data array passed";
        public const string DeleteImageError = "An exception occurred while deleting images --> ";
        public const string InsertDataError = "An exception occurred while inserting new data --> ";
        public const string UpdateDataError = "An exception occurred while updating exist data --> ";
        public const string FindDataByIdError = "An exception occurred while get data with Id = ";
        public const string StatusChangeError = "An exception occurred while changing Active status --> ";
        public const string LoginError = "An exception occurred while login --> ";

        public const string NewRecordCreated = "Successfully Created New Record";
        public const string RecordUpdated = "Successfully Updated the Exist Record";

        public const string AllAdminRetrieve = "SuccessFully All Admin Data retrieved";
        public const string AllOfferRetrieve = "SuccessFully All Offer Data retrieved";
        public const string AllRoomsRetrieve = "SuccessFully All Room Data retrieved";
    }

    public static class DisplayMessages
    {
        public const string NullInput = "Nullable Input Data Provided";
        public const string InvalidId = "Invalid Id Provided";
        public const string EmptyExistData = "There is no any data with this Id";
        public const string InsertingError = "Issue in Inserting data";

        public const string ExistNameError = "This name already exists";
        public const string ExistNumberError = "This number already exists";

        public const string StatusChangeError = "Error on changing active status";
        public const string DeletingError = "Issue While delete the data";
        public const string CloudinaryError = "Issue in Cloudinary asset deleting";
        public const string ImageDeleteError = "Issue in deleting image from Database";
    }
}
