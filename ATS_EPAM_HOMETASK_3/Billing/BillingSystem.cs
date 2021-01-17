using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ATS_EPAM_HOMETASK_3.ATS;
using ATS_EPAM_HOMETASK_3.ATS.enums;


namespace ATS_EPAM_HOMETASK_3.Billing
{
    class BillingSystem
    {
        private ICollection<IClient> clients = new List<IClient>();
        private ICollection<BillInfo> infos = new List<BillInfo>();


        public virtual void RegisterStationEventHandlers(Station station)
        {
            station.CallEvent += OnCallHappened;
        }
        
        private void OnCallHappened(object sender, CallInfo call)
        {
            BillInfo extendedCall = new BillInfo
            {
                CallInfo = call,
                TargetClient = GetClientByPhoneNumber(call.Target),
                SourceClient = GetClientByPhoneNumber(call.Source),
            };
            if (call.CallState == CallState.Processed)
            {
                extendedCall.Cost = call.Duration.Seconds * TariffPlan.CostPerMinute / 60.0;
                extendedCall.SourceClient.Balance -= extendedCall.Cost;
            }
            infos.Add(extendedCall);
        }

        public IClient GetClientByPhoneNumber(string phoneNumber)
        {
            return clients.FirstOrDefault(x => x.Terminal.PhoneNumber.Equals(phoneNumber));
        }

        public void AddClient(IClient client)
        {
            clients.Add(client);
        }
        public Report CreateReport(Client client, DateTime from)
        {
            var calls = GetCalls(client).Intersect(GetCalls(from)).ToList();
            return new Report(client, calls);
        }

        
        public List<BillInfo> GetCalls(DateTime date)
        {
            return infos.Where(x => x.CallInfo.CallDate >= date).ToList();
        }
        public List<BillInfo> GetCalls(IClient client)
        {
            return infos.Where(x => x.TargetClient.Equals(client) || x.SourceClient.Equals(client)).ToList();
        }

        public List<BillInfo> GetIncomingCalls(IClient subscriber)
        {
            return infos.Where(x => x.TargetClient.Equals(subscriber)).ToList();
        }

        public List<BillInfo> GetOutgoingCalls(IClient subscriber)
        {
            return infos.Where(x => x.SourceClient.Equals(subscriber)).ToList();
        }

        public void SortCallsByDate(Report report)
        {
            report.Calls = report.Calls.OrderBy(x => x.CallInfo.CallDate).ToList();
        }
        public void SortCallsByCost(Report report)
        {
            report.Calls = report.Calls.OrderBy(x => x.Cost).ToList();
        }

    }
}
