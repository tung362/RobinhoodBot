using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Tools
    {
        public static bool TimeBetween(string time, string startTime, string endTime)
        {
            TimeSpan start = DateTime.Parse(startTime).TimeOfDay;
            TimeSpan end = DateTime.Parse(endTime).TimeOfDay;
            TimeSpan now = DateTime.Parse(time).TimeOfDay;
            if (start < end) return start <= now && now <= end;
            return !(end < now && now < start);
        }

        public static bool TimeBetween(TimeSpan time, string startTime, string endTime)
        {
            TimeSpan start = DateTime.Parse(startTime).TimeOfDay;
            TimeSpan end = DateTime.Parse(endTime).TimeOfDay;
            TimeSpan now = time;
            if (start < end) return start <= now && now <= end;
            return !(end < now && now < start);
        }
    }
}
