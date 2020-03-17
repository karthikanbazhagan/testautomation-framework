namespace ApplicationUnderTest.TestData.Container
{
    using System;
    using System.IO;
    using System.Linq;
    using ApplicationUnderTest.Constant;
    using ApplicationUnderTest.Enum;
    using ApplicationUnderTest.TestData.Model;
    using Framework.Core.TestData;
    
    /// <summary>
    /// A Singleton class to ensure that there is only one instance of this type for a complete Test Run
    /// </summary>
    public class FlightBookingDetailsDataContainer : BaseDataContainer<FlightBookingDetails>
    {
        private static FlightBookingDetailsDataContainer Container;

        /// <summary>
        /// A private constructor to ensure that the class can't be instantiated from outside world.
        /// And the data is imported from the CSV file on instantiating the class.
        /// </summary>
        private FlightBookingDetailsDataContainer()
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData\\Source\\FlightBookingDetails.csv");
            ImportData();
        }

        /// <summary>
        /// A static method to get the instance of this class.
        /// </summary>
        /// <returns>
        /// Returns an instance of this class.
        /// Creates a new instance for the first time it is called and returns the created instance henceforth for the full test run.
        /// </returns>
        public static FlightBookingDetailsDataContainer GetInstance()
        {
            if ( Container == null)
            {
                Container = new FlightBookingDetailsDataContainer();                
            }

            return Container;
        }

        /// <summary>
        /// Gets a random FlightBookingDetails for the given Trip Type
        /// </summary>
        /// <param name="tripType">
        /// The TripType to be filtered with. "One-way" or "Round trip"
        /// </param>
        /// <returns>
        /// A FlightBookingDetails record/instance.
        /// </returns>
        public FlightBookingDetails GetFlightBookingDetailsWithTripType(TripType tripType)
        {
            var flightBookingDetails = Data.Where(d => d.TripType == Mappings.TripTypeToStringMapping[tripType]).ToList();
            return flightBookingDetails[new Random().Next(flightBookingDetails.Count)];
        }
    }
}
