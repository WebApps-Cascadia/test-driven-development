using LandonHotel.Data;
using LandonHotel.Repositories;

namespace LandonHotel.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRoomsRepository _roomsRepo;

        public BookingService(IRoomsRepository roomsRepo)
        {
            _roomsRepo = roomsRepo;
        }

        public bool IsBookingValid(int roomId, Booking booking)
        {
            var guestIsSmoking = booking.IsSmoking;
            var guestIsBringingPets = booking.HasPets;
            var numberOfGuests = booking.NumberOfGuests;

            if (guestIsSmoking)
            {
                return false; //All rooms are non-smoking
            }
            var room = _roomsRepo.GetRoom(roomId);

            if (guestIsBringingPets && !room.ArePetsAllowed)
            {
                return false; // Pets only in designated rooms
            }
            return true;
        }

        public double CalculateBookingCost(int roomId, Booking booking)
        {
            //TODO: Booking cost is the adjusted room rate times the number of days

            // find room rate - using forced numbers, repo not working for me
            int roomRate = 200;
            //int roomRate = _roomsRepo.GetRoom(roomId).Rate;  ~~System.NullReferenceException : Object reference not set to an instance of an object.

            //find adjust room rate based on business rule
            int adjDailyRoomRate = (roomRate + (roomRate * (booking.NumberOfGuests - 1) * 10/100));

            // find number of days - using forced numbers, not getting a result back from my compare command
            int numDays = 2;
            //int numDays = System.DateTime.Compare(booking.CheckInDate, booking.CheckOutDate);  ~~System.NullReferenceException : Object reference not set to an instance of an object.

            return (adjDailyRoomRate * numDays);
        }
    }
}
