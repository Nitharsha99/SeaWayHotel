﻿using Newtonsoft.Json.Serialization;

namespace seaway.API.Models
{
    public static class LogMessages
    {
        public const string GetRoomDataError = "An exception occurred while get all data from Room --> ";
        public const string FindRoomByIdError = "An exception occurred while get room data with Id = ";
        public const string DeleteRoomError = "An exception occurred while deleting room --> ";

        public const string GetActivityDataError = "An exception occurred while get all data from Activity --> ";
        public const string FindActivityByIdError = "An exception occurred while get activity data with Id = ";
        public const string DeleteActivityError = "An exception occurred while deleting activity --> ";

        public const string GetOfferDataError = "An exception occurred while get all data from Activity --> ";
        public const string FindOfferByIdError = "An exception occurred while get activity data with Id = ";
        public const string DeleteOfferError = "An exception occurred while deleting activity --> ";

        public const string InvalidIdError = "Invalid Id Passed";
        public const string EmptyDataSetError = "Empty data array passed";
        public const string DeleteImageError = "An exception occurred while deleting images --> ";
        public const string InsertDataError = "An exception occurred while inserting new data --> ";
        public const string UpdateDataError = "An exception occurred while updating exist data --> ";
        public const string StatusChangeError = "An exception occurred while changing Active status --> ";
    }

    public static class DisplayMessages
    {
        public const string NullInput = "Nullable Input Data Provided";
        public const string InvalidId = "Invalid Id Provided";
        public const string EmptyExistData = "There is no any data with this Id --> ";

        public const string StatusChangeError = "Error on changing active status";
        public const string DeletingError = "Issue While delete the data";
        public const string CloudinaryError = "Issue in Cloudinary asset deleting";
    }
}