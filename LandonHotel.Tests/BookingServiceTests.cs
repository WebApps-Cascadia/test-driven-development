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

        //Business Rule: Each additional person in a room costs an extra 10%.
        // TODO: Compute

        [Fact]
        public void IsBookingCostForAdditionalPersonCorrect()
        {
            //Arrange (create Subject and setup roomRepo; calculate cost using the rule)
            var bookingCostCalculatedFromBusinessRule = 1.0;
            //Act (use the service method CalculateBookingCost)
            var bookingCostFromService = 0.0;
            //Assert
            Assert.Equal(bookingCostCalculatedFromBusinessRule, bookingCostFromService); //Start with a false assertion see the test fail
        }
    }
}
