namespace Kasi_Server.Utils.Maths
{
    public static class TemperatureConv
    {
        public static decimal DegreesCelsiusToFahrenheit(decimal value)
        {
            return (decimal)1.8 * value + 32;
        }

        public static decimal DegreesCelsiusToThermodynamicTemperature(decimal value)
        {
            return value + (decimal)273.16;
        }

        public static decimal FahrenheitToDegreesCelsius(decimal value)
        {
            return (value - 32) / (decimal)1.8;
        }

        public static decimal FahrenheitToThermodynamicTemperature(decimal value)
        {
            return (value - 32) / (decimal)1.8 + (decimal)273.16;
        }

        public static decimal ThermodynamicTemperatureToDegreesCelsius(decimal value)
        {
            return value - (decimal)273.16;
        }

        public static decimal ThermodynamicTemperatureToFahrenheit(decimal value)
        {
            return (value - (decimal)273.16) * (decimal)1.8 + 32;
        }
    }
}