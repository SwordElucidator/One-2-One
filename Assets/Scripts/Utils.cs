using System;

public static class Utils
{
    public static long GetTimeStamp(DateTime dateTime)
    {
        TimeSpan ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds);
    }

    public static DateTime GetDateTime(long timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public static bool IsSameDay(long t1, long t2)
    {
        DateTime d1 = GetDateTime(t1);
        DateTime d2 = GetDateTime(t2);
        return (d1.Year == d2.Year) && (d1.DayOfYear == d2.DayOfYear);
    }
    
    public static bool IsSameDay(DateTime d1, long t2)
    {
        DateTime d2 = GetDateTime(t2);
        return (d1.Year == d2.Year) && (d1.DayOfYear == d2.DayOfYear);
    }
}