using System;
using ATS_EPAM_HOMETASK_3.ATS.Interfaces;


namespace ATS_EPAM_HOMETASK_3.Billing
{
    class Client : IClient
    {
        public string Name { get; set; }
        public ITerminal Terminal { get; set; }

        public double Balance { get; set; } = 0;



    }
}
