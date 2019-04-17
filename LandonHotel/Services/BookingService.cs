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
            Room bookedRoom = _roomsRepo.GetRoom(roomId);
            double guestNumRate;
            //Checks to see if there are more than one guests
            if(booking.NumberOfGuests == 1)
            {
                guestNumRate = 1;
            }
            else
            {
                guestNumRate = bookedRoom.Rate * ((booking.NumberOfGuests - 1) * (10 / 100));
            }

            double totalBookingCost = 
                bookedRoom.Rate + guestNumRate * (booking.CheckOutDate - booking.CheckInDate).TotalDays;

            return totalBookingCost;
        }
    }
}
