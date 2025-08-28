
using System.Reflection;
using System.Runtime.Serialization;

namespace WTForecast.Models
{

    public enum SymbolCode
    {
        [EnumMember(Value = "clearsky_day")]
        ClearskyDay,

        [EnumMember(Value = "clearsky_night")]
        ClearskyNight,

        [EnumMember(Value = "cloudy")]
        Cloudy,

        [EnumMember(Value = "fair_day")]
        FairDay,

        [EnumMember(Value = "fair_night")]
        FairNight,

        [EnumMember(Value = "fog")]
        Fog,

        [EnumMember(Value = "heavyrain")]
        Heavyrain,

        [EnumMember(Value = "heavyrainandthunder")]
        HeavyrainAndThunder,

        [EnumMember(Value = "heavyrainshowers_day")]
        HeavyrainshowersDay,

        [EnumMember(Value = "heavyrainshowers_night")]
        HeavyrainshowersNight,

        [EnumMember(Value = "heavysleet")]
        Heavysleet,

        [EnumMember(Value = "heavysleetandthunder")]
        HeavysleetAndThunder,

        [EnumMember(Value = "heavysleetshowers_day")]
        HeavysleetshowersDay,

        [EnumMember(Value = "heavysleetshowers_night")]
        HeavysleetshowersNight,

        [EnumMember(Value = "heavysnow")]
        Heavysnow,

        [EnumMember(Value = "heavysnowandthunder")]
        HeavysnowAndThunder,

        [EnumMember(Value = "heavysnowshowers_day")]
        HeavysnowshowersDay,

        [EnumMember(Value = "heavysnowshowers_night")]
        HeavysnowshowersNight,

        [EnumMember(Value = "partlycloudy_day")]
        PartlycloudyDay,

        [EnumMember(Value = "partlycloudy_night")]
        PartlycloudyNight,

        [EnumMember(Value = "rain")]
        Rain,

        [EnumMember(Value = "rainandthunder")]
        RainAndThunder,

        [EnumMember(Value = "rainshowers_day")]
        RainshowersDay,

        [EnumMember(Value = "rainshowers_night")]
        RainshowersNight,

        [EnumMember(Value = "sleet")]
        Sleet,

        [EnumMember(Value = "sleetandthunder")]
        SleetAndThunder,

        [EnumMember(Value = "sleetshowers_day")]
        SleetshowersDay,

        [EnumMember(Value = "sleetshowers_night")]
        SleetshowersNight,

        [EnumMember(Value = "snow")]
        Snow,

        [EnumMember(Value = "snowandthunder")]
        SnowAndThunder,

        [EnumMember(Value = "snowshowers_day")]
        SnowshowersDay,

        [EnumMember(Value = "snowshowers_night")]
        SnowshowersNight,

        [EnumMember(Value = "showers_day")]
        ShowersDay,

        [EnumMember(Value = "showers_night")]
        ShowersNight,

        [EnumMember(Value = "thunderstorm")]
        Thunderstorm,

        [EnumMember(Value = "thunderstorm_light")]
        ThunderstormLight,
    }





    public static class SymbolCodeExtensions
    {
        public static TEnum? ParseEnumByEnumMemberValue<TEnum>(string value) where TEnum : struct, Enum
        {
            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = field.GetCustomAttribute<EnumMemberAttribute>();
                if (attr != null && string.Equals(attr.Value, value, StringComparison.OrdinalIgnoreCase))
                    return (TEnum)field.GetValue(null)!;
            }
            return null;
        }

        public static string GetMeaning(this SymbolCode code) => code switch
        {
            SymbolCode.ClearskyDay => "Clear sky (day)",
            SymbolCode.ClearskyNight => "Clear sky (night)",
            SymbolCode.Cloudy => "Cloudy",
            SymbolCode.FairDay => "Fair weather (day)",
            SymbolCode.FairNight => "Fair weather (night)",
            SymbolCode.Fog => "Fog",
            SymbolCode.Heavyrain => "Heavy rain",
            SymbolCode.HeavyrainAndThunder => "Heavy rain and thunder",
            SymbolCode.HeavyrainshowersDay => "Heavy rain showers (day)",
            SymbolCode.HeavyrainshowersNight => "Heavy rain showers (night)",
            SymbolCode.Heavysleet => "Heavy sleet",
            SymbolCode.HeavysleetAndThunder => "Heavy sleet and thunder",
            SymbolCode.HeavysleetshowersDay => "Heavy sleet showers (day)",
            SymbolCode.HeavysleetshowersNight => "Heavy sleet showers (night)",
            SymbolCode.Heavysnow => "Heavy snow",
            SymbolCode.HeavysnowAndThunder => "Heavy snow and thunder",
            SymbolCode.HeavysnowshowersDay => "Heavy snow showers (day)",
            SymbolCode.HeavysnowshowersNight => "Heavy snow showers (night)",
            SymbolCode.PartlycloudyDay => "Partly cloudy (day)",
            SymbolCode.PartlycloudyNight => "Partly cloudy (night)",
            SymbolCode.Rain => "Rain",
            SymbolCode.RainAndThunder => "Rain and thunder",
            SymbolCode.RainshowersDay => "Rain showers (day)",
            SymbolCode.RainshowersNight => "Rain showers (night)",
            SymbolCode.Sleet => "Sleet",
            SymbolCode.SleetAndThunder => "Sleet and thunder",
            SymbolCode.SleetshowersDay => "Sleet showers (day)",
            SymbolCode.SleetshowersNight => "Sleet showers (night)",
            SymbolCode.Snow => "Snow",
            SymbolCode.SnowAndThunder => "Snow and thunder",
            SymbolCode.SnowshowersDay => "Snow showers (day)",
            SymbolCode.SnowshowersNight => "Snow showers (night)",
            SymbolCode.ShowersDay => "Showers (day)",
            SymbolCode.ShowersNight => "Showers (night)",
            SymbolCode.Thunderstorm => "Thunderstorm",
            SymbolCode.ThunderstormLight => "Light thunderstorm",
            _ => "Unknown"
        };
    }
}
