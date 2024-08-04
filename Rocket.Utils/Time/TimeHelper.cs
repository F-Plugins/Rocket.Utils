using System;
using System.Collections.Generic;
using System.Linq;

namespace Feli.Rocket.Utils.Time;

public class TimeHelper
{
    public static TimeSpan ParseTime(params string[] args)
    {
        Dictionary<char, TimeSpan> timeUnitMap = new Dictionary<char, TimeSpan>
        {
            {'s', TimeSpan.FromSeconds(1)},
            {'m', TimeSpan.FromMinutes(1)},
            {'h', TimeSpan.FromHours(1)},
            {'d', TimeSpan.FromDays(1)}
        };

        TimeSpan totalDuration = TimeSpan.Zero;

        foreach (string timeString in args)
        {
            char unit = timeString.Last(); // Get the last character (unit)
            if (timeUnitMap.TryGetValue(unit, out TimeSpan unitTimeSpan))
            {
                if (!int.TryParse(timeString.Substring(0, timeString.Length - 1), out var value))
                    continue;

                totalDuration += TimeSpan.FromTicks(value * unitTimeSpan.Ticks);
            }
        }

        if (totalDuration != TimeSpan.Zero)
        {
            totalDuration += TimeSpan.FromSeconds(1);
        }

        if (totalDuration == TimeSpan.Zero && args.Length > 0 && double.TryParse(args[0], out var result) && result > 0)
        {
            totalDuration = TimeSpan.FromSeconds(result);
        }

        return totalDuration;
    }

    public static string FormatTime(TimeSpan timeSpan)
    {
        Dictionary<TimeSpan, string> timeUnitMap = new Dictionary<TimeSpan, string>
        {
            { TimeSpan.FromDays(1), "d" },
            { TimeSpan.FromHours(1), "h" },
            { TimeSpan.FromMinutes(1), "m" },
            { TimeSpan.FromSeconds(1), "s" }
        };

        List<string> formattedParts = new List<string>();

        foreach (var pair in timeUnitMap)
        {
            int value = (int)(timeSpan.Ticks / pair.Key.Ticks);
            if (value > 0)
            {
                formattedParts.Add($"{value}{pair.Value}");
                timeSpan = timeSpan.Subtract(TimeSpan.FromTicks(value * pair.Key.Ticks));
            }
        }

        return string.Join(" ", formattedParts);
    }
}
