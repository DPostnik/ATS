
using ATS_EPAM_HOMETASK_3.ATS.enums;

namespace ATS_EPAM_HOMETASK_3.ATS
{
    class CallEventArgs
    {
        public string SourceNumber { get; set; }
        public string TargetNumber { get; set; }
        public CallState State { get; set; }
    }
}
