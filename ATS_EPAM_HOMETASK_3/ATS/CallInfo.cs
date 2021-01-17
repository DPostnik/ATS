
using System;
using ATS_EPAM_HOMETASK_3.ATS.enums;

namespace ATS_EPAM_HOMETASK_3.ATS
{
    class CallInfo
    {
        public string Target { get; set; }
        public string Source { get; set; }
        public DateTime CallDate { get; set; }
        public TimeSpan Duration { get; set; }
        public CallState CallState { get; set; }
    }
}
