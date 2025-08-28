# WTForecast

WTForecast is a cross-platform weather application built with [.NET MAUI](https://learn.microsoft.com/dotnet/maui/).  
This project is created **for educational and learning purposes only** and is intended only for learning and experimentation with:

    .NET MAUI
    REST APIs
    MVVM architectur
    Weather data visualization

It is not meant for production or commercial use.

---

## Features
- Fetches weather forecasts from the [MET Norway Locationforecast API](https://api.met.no/weatherapi/locationforecast/2.0/documentation)  
- Displays hourly and daily forecasts with charts  
- Supports mobile and desktop builds (iOS, Android, Windows, macOS)  
- Experimentation with data binding, charting, and UI in .NET MAUI  

---

## Configuration

WTForecast supports configuration via **appsettings.json**.
This file contains default values that apply to all environments.

For development and personal overrides, you can create an optional **appsettings.Development.json** file.
This file is ignored by Git (listed in .gitignore) so you can safely store your own values without affecting others or remote builds.

Example

```appsettings.json
{
  "WeatherApi": {
    "BaseUrl": "https://api.met.no/weatherapi/locationforecast/2.0/compact",
    "UserAgent": "WTForecast/1.0 (https://github.com/YOUR_GUTHUB_REPO; YOUR@EMAIL.com""
  }
}
```
If appsettings.Development.json does not exist, the app will fall back to appsettings.json.
All API requests to `https://api.met.no/weatherapi/locationforecast/2.0/` must include a **custom User-Agent header**.  
Without it, requests may return `403 Forbidden`.

## Data Source & Attribution

Weather data is provided by the **Norwegian Meteorological Institute (MET Norway)**.  
© MET Norway, licensed under:  
- [Norwegian Licence for Open Government Data (NLOD) 2.0](https://data.norge.no/nlod/en/2.0)  
- [Creative Commons Attribution 4.0 International (CC BY 4.0)](https://creativecommons.org/licenses/by/4.0/)

> Credit: “Data from MET Norway”  

Weather icons are provided by **Yr.no** under the [MIT License](https://opensource.org/licenses/MIT).  
© Yr.no  

---
## License

This project is licensed under the MIT License

.

Weather data © MET Norway, under NLOD 2.0 and CC BY 4.0.
Weather icons © Yr.no, under MIT license.