namespace ApplicationUnderTest.Constant
{
    using ApplicationUnderTest.Enum;
    using System.Collections.Generic;

    public static class Mappings
    {

        public static readonly IDictionary<TripType, string> TripTypeToStringMapping =
            new Dictionary<TripType, string>
            {
                {TripType.Oneway, "One-way" },
                {TripType.RoundTrip, "Round trip" }
            };
    }
}
