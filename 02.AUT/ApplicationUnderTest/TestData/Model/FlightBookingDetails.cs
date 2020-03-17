namespace ApplicationUnderTest.TestData.Model
{
    using System;

    using Framework.Core.TestData;
    
    public class FlightBookingDetails : ITestData
    {
        public string TripType { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string Class { get; set; }

        public DateTime TravelDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public int NumberOfAdults { get; set; }

        public int NumberOfChildren { get; set; }

        public int NumberOfInfantsInSeat { get; set; }

        public int NumberOfInfantsOnLap { get; set; }
    }
}
