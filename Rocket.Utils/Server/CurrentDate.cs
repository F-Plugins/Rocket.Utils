using System;

namespace Feli.Rocket.Utils.Server;

public class CurrentDate
{
    public static DateTime Now => DateTime.Now;
    public static DateTimeOffset NowOffset => DateTimeOffset.Now;
}
