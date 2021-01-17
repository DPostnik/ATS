using System;
using System.Collections.Generic;


namespace ATS_EPAM_HOMETASK_3.Billing
{
    class Report
    {
        //IList<CallInfo>
        public IClient Client { get; set; }

        public ICollection<BillInfo> Calls { get; set; }

        public Report(Client client, List<BillInfo> info)
        {
            Client = client;
            Calls = info;
        }

    }
}
