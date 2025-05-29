using System;
using System.Collections.Generic;
using System.Linq;

namespace WTForecast
{
    public static class WeatherMood
    {
        private static readonly SymbolCode[] PleasantWeather =
        {
            SymbolCode.ClearskyDay,
            SymbolCode.ClearskyNight,
            SymbolCode.FairDay,
            SymbolCode.FairNight,
            SymbolCode.PartlycloudyDay,
            SymbolCode.PartlycloudyNight
        };

        private static readonly SymbolCode[] MehWeather =
        {
            SymbolCode.Cloudy,
            SymbolCode.Fog,
            SymbolCode.ShowersDay,
            SymbolCode.ShowersNight,
            SymbolCode.RainshowersDay,
            SymbolCode.RainshowersNight,
            SymbolCode.SleetshowersDay,
            SymbolCode.SleetshowersNight,
            SymbolCode.SnowshowersDay,
            SymbolCode.SnowshowersNight
        };

        private static readonly SymbolCode[] NastyWeather =
        {
            SymbolCode.Rain,
            SymbolCode.Heavyrain,
            SymbolCode.HeavyrainAndThunder,
            SymbolCode.Sleet,
            SymbolCode.Heavysleet,
            SymbolCode.HeavysleetAndThunder,
            SymbolCode.Snow,
            SymbolCode.Heavysnow,
            SymbolCode.HeavysnowAndThunder,
            SymbolCode.Thunderstorm,
            SymbolCode.ThunderstormLight,
            SymbolCode.RainAndThunder,
            SymbolCode.SleetAndThunder,
            SymbolCode.SnowAndThunder
        };

        private static readonly string[] PleasantMessages =
        {
            "It's a perfect day for doing nothing outside!",
            "Sun's out! Go blind with joy. 😎",
            "The sky called. It said, 'You're welcome.'",
            "Weather so good, your ex might text you.",
            "Get outside before the clouds change their mind.",
            "Sunshine and chill — nature's best combo.",
            "Even your grumpy neighbor is smiling today.",
            "Vitamin D overdose in progress. Proceed outdoors.",
            "This weather was handcrafted by angels.",
            "No umbrella, no worries, just vibes.",
            "This weather deserves a standing ovation.",
            "Your plants are dancing. So should you.",
            "Perfect day to pretend you jog.",
            "Warning: Extreme levels of pleasantness ahead.",
            "You’ve got no excuse to stay inside today.",
            "Even your weather app blushed.",
            "It’s like the sky got a promotion.",
            "Time to Instagram the sky again.",
            "It's like the weather won the lottery and shared the prize.",
            "The sun called in rich today.",
            "Even the breeze is flirting with you.",
            "Weather so nice, even your inbox chilled out.",
            "It's so nice out, your coffee wants to be iced.",
            "Mother Nature’s showing off again.",
            "Today's forecast: sunglasses and main character energy.",
            "Sky’s bluer than your browser in incognito mode.",
            "The weather’s so good it cured your group chat.",
            "This is what mood-boosting looks like IRL.",
            "A day so fine, even your Wi-Fi’s jealous.",
            "Warning: spontaneous whistling may occur.",
            "It’s like the clouds took the day off too.",
            "Sunlight so smooth, it should be illegal.",
            "The weather’s vibing harder than your playlist.",
            "Outside just hit 'legendary' difficulty—in a good way.",
            "This day was clearly beta-tested by angels.",
            "Even your introversion is reconsidering."
        };

        private static readonly string[] PleasantMessagesClosers = {
            "Don't waste it indoors.",
    "Put on those shades!",
    "Grab an ice cream—doctor's orders.",
    "You earned this sunshine.",
    "Even your plants are happy.",
    "Charge your solar-powered good vibes.",
    "The sun ships you and happiness.",
    "Call in sick to go frolic.",
    "Pet a dog. You deserve it.",
    "Nature approves of your existence today.",
    "Let your shadow tag along for fun.",
    "Smile like you mean it—or fake it.",
    "Your weather horoscope: fabulous.",
    "Do something reckless like smiling at strangers.",
    "Birds are chirping your theme song.",
    "Even Mondays are scared of this day.",
    "Feel free to skip responsibilities.",
    "Soak it up before it changes its mind.",
    "Perfect day to ghost your calendar.",
    "Just don’t forget sunscreen, hero.",
            "Don’t make your couch do all the work today.",
"Let the sunlight roast your worries.",
"Permission granted to strut for no reason.",
"Give your walls a break. Go outside.",
"Let the breeze do your hair today.",
"Reality called—it’s nicer than your screen.",
"Be the person your weather thinks you are.",
"Your serotonin called. It's outside.",
"Skip the small talk, go chase some rays.",
"If you needed a sign, this weather is it.",
"Outfit idea: whatever makes you happiest.",
"Let your shadow out for a walk.",
"This weather deserves your best smile.",
"Be the sunshine someone else avoids.",
"Reward yourself with a popsicle and a walk.",
"Just go outside and thank the sky.",
"Don’t let the good vibes expire.",
"Take your happy outside for a test drive.",
"Put the 'awe' in 'awesome day'."

        };


        private static readonly string[] MehMessages =
        {
        "Mediocre weather, just like your Monday.",
        "Not great, not terrible. Like store-brand cereal.",
        "Could be worse. Could be a Zoom call.",
        "It's the kind of weather you forget while you're in it.",
"Sky's as uninspired as your lunch.",
"A real five-out-of-ten kind of day.",
"Today’s weather is just... available.",
"Not bad enough to complain. Not good enough to notice.",
"The sky's been ghosted by the sun.",
"Mother Nature phoned this one in.",
"It’s giving: ‘eh’.",
"Cloudy with a chance of indifference.",
"The sky’s wearing sweatpants today.",
"This weather wouldn’t even swipe right on itself.",
"The sun clocked out early.",
"Looks like the clouds forgot how to drama.",
"Not ugly. Not cute. Just there.",
"A great day for staring blankly into space.",
"Weather’s stuck in neutral.",
"It’s the ‘unsalted rice’ of weather.",
"The sky's running on 10% battery.",
"Not enough rain for a vibe, not enough sun for joy.",
"A background kind of day.",
"Weather’s doing the bare minimum.",
"Sky said, 'Do what you want, I don’t care.'",
"This day is the elevator music of weather.",
"Today’s vibe: shoulder shrug.",
"Could be worse, could be a group project.",
"Sunshine optional. Enthusiasm not included.",
"Cloudy with a 50% chance of 'meh'.",
"Perfect for passive aggressively sipping coffee.",
"Gray skies and medium emotions.",
"It’s like the weather hit snooze.",
"A day built for vague dissatisfaction.",
"Definitely a beige sweater kind of day.",
"This day brought to you by lukewarm soup.",
"A fine day to cancel plans.",
"The kind of weather where nothing’s urgent.",
"Sky is giving you the silent treatment."

    };

        private static readonly string[] MehMessagesClosers =
{
"Manage your expectations accordingly.",
"Maybe wear socks with sandals. No one cares today.",
"Perfect weather for doing the bare minimum.",
"Honestly, same energy as instant noodles.",
"Could inspire a shrug. Maybe.",
"Go outside, or don’t. It won’t notice.",
"A fine day for scrolling aimlessly.",
"Weather that says, 'Meh, you too.'",
"If beige had a season, this would be it.",
"Try to look mildly alive out there.",
"Call it 'weather' and move on.",
"Like decaf coffee—technically counts.",
"You won't remember this day, and that's fine.",
"Put on a hoodie and pretend you're productive.",
"Take a walk if you’re feeling chaotic.",
"Exist gently."

    };

        private static readonly string[] NastyMessages =
        {
        "Stay in bed. The sky's angry.",
        "Weather update: NOPE.",
        "It’s like the apocalypse but wetter.",
        "The sky’s throwing a tantrum.",
"Feels like the atmosphere is mad at you personally.",
"Rain so rude it came sideways.",
"Today’s forecast: emotional damage.",
"If misery had a climate, this would be it.",
"Umbrella? Useless. Spirit? Broken.",
"Your socks won’t survive.",
"One step outside, instant regret.",
"Weather said: ‘Not today, joy.’",
"It’s like nature caught a cold and sneezed on everything.",
"Today’s goal: survive the commute.",
"Bring snacks. It’s a battle out there.",
"You vs. the wind—good luck.",
"This weather writes passive-aggressive emails.",
"A perfect day to develop a villain origin story.",
"Even the birds called in sick.",
"Sky’s leaking its feelings again.",
"Hope your shoes weren’t new.",
"Nature’s on hard mode today.",
"This day came straight out of a disaster movie.",
"The clouds are judging you and they brought thunder.",
"Feels like the wind has opinions.",
"Everything’s wet, including your soul.",
"A great day to scream into a pillow.",
"Forecast brought to you by chaos.",
"Today’s weather: sponsored by suffering.",
"The outdoors are closed for renovations.",
"It’s not just raining—it’s plotting.",
"The elements demand a blood sacrifice (or at least your umbrella).",
"Mother Nature's in full gremlin mode.",
"This is what emotional instability looks like in cloud form.",
"Step outside and you’ll be exfoliated by the wind.",
"The sun left a note: 'Back never.'",
"It’s raining on your inner peace.",
"Hope you like your weather with a side of despair."

    };
        private static readonly string[] NastyMessagesClosers =
{
"Abandon hope, grab a blanket.",
"Don't even look out the window.",
"Stay indoors unless you're a duck.",
"Perfect day for regretting your choices.",
"Even your umbrella wants a raise.",
"May the odds (and your socks) be ever dry.",
"Time to build that indoor pillow fort.",
"Hide. The weather’s in a mood.",
"Great day to question your life choices.",
"Treat yourself to pajamas and silence.",
"Do not engage. Weather is hostile.",
"Today’s aesthetic: soggy and betrayed.",
"It’s you vs. the elements. Stay down.",
"Netflix just became a survival tool.",
"Your hair never stood a chance.",
"Weather’s throwing hands—stay low.",
"This is why we invented roofs.",
"Say goodbye to good hair days.",
"Bundle up. Emotionally and physically."

        };



        private static readonly Random Rand = new();

        public static string GetFunnyMessage(this SymbolCode code)
        {
            if (PleasantWeather.Contains(code))
                return PleasantMessages[Rand.Next(PleasantMessages.Length)];

            if (MehWeather.Contains(code))
                return MehMessages[Rand.Next(MehMessages.Length)];

            if (NastyWeather.Contains(code))
                return NastyMessages[Rand.Next(NastyMessages.Length)];

            return "Weather unknown. Panic accordingly!";
        }

        public static string GetMoodGroup(this SymbolCode code)
        {
            if (PleasantWeather.Contains(code)) return "Pleasant";
            if (MehWeather.Contains(code)) return "Meh";
            if (NastyWeather.Contains(code)) return "Nasty";
            return "Unknown";
        }
    }
}
