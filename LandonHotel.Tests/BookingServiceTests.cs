using LandonHotel.Data;
using LandonHotel.Repositories;
using LandonHotel.Services;
using Moq;
using Xunit;

namespace LandonHotel.Tests
{
    public class BookingServiceTests
    {
        private Mock<IRoomsRepository> roomRepo;

        private BookingService Subject()
        {
            roomRepo = new Mock<IRoomsRepository>();

            return new BookingService(roomRepo.Object);
        }

        // Business Rule: No Smoking in any room; deny "smoking-room" bookings
        // TODO: simplify the two test into one using InlineData and [Theory] for the two cases
        [Fact]
        public void IsBookingValid_NonSmoking_Valid()
        {
            //Arrange
            var service = Subject();
            //Act
            var isValid = service.IsBookingValid(1, new Booking { IsSmoking = false });
            //Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsBookingValid_Smoking_Invalid()
        {
            //Arrange
            var service = Subject();
            //Act
            var isValid = service.IsBookingValid(1, new Booking { IsSmoking = true });
            //Assert
            Assert.False(isValid);
        }

        //Business Rule: Pets are only allowed in rooms with "ArePetsAllowed" marked
        [Theory]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        public void IsBookingValid_Pets(bool areAllowed, bool hasPets, bool result)
        {
            //Arrange
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { ArePetsAllowed = areAllowed });
            //Act
            var isValid = service.IsBookingValid(1, new Booking { HasPets = hasPets });
            //Assert
            Assert.Equal(isValid, result);
        }

        //Business Rule: Each additional person increases the room rate by 10% over the regular rate.
        [Fact]
        public void IsBookingCostForAdditionalPersonsCorrect()
        {
            //Arrange
            var service = Subject();
            const int guestCount = 3, rate = 200, numberDays = 2;
            System.DateTime checkin = System.DateTime.Now;
            System.DateTime checkout = checkin.AddDays(numberDays);

            //TODO: Calculate total booking cost using the constant values and the business rule
            //           cost per day = rate + rate * guestCount * 10/100

            var totalBookingCost = numberDays *(rate + (rate * ((guestCount - 1) * .1))); //dummy value to start

            //Act
            //TODO: Setup roomRepo and create new Booking using the constants
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { Rate = rate });

            Booking booking = new Booking { CheckInDate = checkin,
                                            CheckOutDate = checkout,
                                            NumberOfGuests = guestCount } ;
            var bookingCostFromService = service.CalculateBookingCost(1, booking);

            //Assert
            Assert.Equal(totalBookingCost, bookingCostFromService);
        }
    }
}
