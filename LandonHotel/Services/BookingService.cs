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
            double numberdays = booking.CheckOutDate.Subtract(booking.CheckInDate).TotalDays;
            var room = _roomsRepo.GetRoom(roomId); 
            var guests = booking.NumberOfGuests - 1;
            //cost per day = rate + rate * guestCount * 10 / 100
            double costperday = room.Rate + room.Rate * guests * .1;

            return (numberdays * costperday);//TotalCost;
        }
    }
}
