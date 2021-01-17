using System;
using ATS_EPAM_HOMETASK_3.ATS;


namespace ATS_EPAM_HOMETASK_3.Billing
{
    class BillInfo
    {
        public CallInfo CallInfo { get; set; }
        public double Cost { get; set; }
        public IClient SourceClient { get; set; }
        public IClient TargetClient { get; set; }

    }
}
