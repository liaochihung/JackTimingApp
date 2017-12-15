using System.Collections.Generic;

namespace JackTimingApp.Helper
{
    public class TimingMap
    {
        public string Symbol { get; set; }
        public string Timing { get; set; }

        public List<TimingCharacterElement> Elements { get; set; }
    }
}