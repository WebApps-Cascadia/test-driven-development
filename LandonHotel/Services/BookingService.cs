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

            // find room rate
            int roomRate = _roomsRepo.GetRoom(roomId).Rate;
           
            //find adjust room rate based on business rule
            int adjDailyRoomRate = (roomRate + (roomRate * (booking.NumberOfGuests - 1) * 1/10));

            // find number of days
            int numDays = System.DateTime.Compare(booking.CheckOutDate, booking.CheckInDate);

            return (adjDailyRoomRate * numDays);
        }
    }
}
