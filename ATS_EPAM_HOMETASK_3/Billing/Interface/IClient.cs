using ATS_EPAM_HOMETASK_3.ATS.Interfaces;

namespace ATS_EPAM_HOMETASK_3.Billing
{
    interface IClient
    {
        string Name { get; set; }

        double Balance { get; set; }

        ITerminal Terminal { get; set; }
    }
}