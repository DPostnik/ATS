
using System;
using System.Collections.Generic;
using System.Linq;
using ATS_EPAM_HOMETASK_3.ATS.enums;
using ATS_EPAM_HOMETASK_3.ATS.Interfaces;

namespace ATS_EPAM_HOMETASK_3.ATS
{
    class Station
    {
        private readonly Dictionary<string, Port> ports;

        private readonly List<CallInfo> calls;
        public event EventHandler<CallInfo> CallEvent;

        public Station()
        {
            ports = new Dictionary<string, Port>();
            calls = new List<CallInfo>();
        }

        public void RegisterTerminal(ITerminal terminal)
        {
            Port port = new Port();
            ports.Add(terminal.PhoneNumber, port);
            terminal.Port = port;
            port.RegisterEventHandlersForTerminal(terminal);
        }

        public virtual void RegisterPortEventHandlers(IPort port)
        {
            port.OutgoingCall += OnOutgoingCall;
            port.Answer += OnIncomingCallAnswer;
            port.Drop += OnCallDrop;
        }

        private void OnOutgoingCall(object sender, CallEventArgs arg)
        {
            var receiverPort = GetPort(arg.TargetNumber);
            if (receiverPort != null)
            {
                if (receiverPort.State == PortState.Free)
                {
                    receiverPort.GetCall(arg);
                    arg.State = CallState.Unprocessed;
                    RegUnprocessedCall(arg);
                }
                else
                {
                    Console.WriteLine("Receiver port is " + receiverPort.State);
                }
            }
        }
        private Port GetPort(string number)
        {
            return ports.TryGetValue(number, out var port) ? port : null;
        }

        public void RegUnprocessedCall(CallEventArgs arg)
        {
            calls.Add(new CallInfo
            {
                Source = arg.SourceNumber,
                Target = arg.TargetNumber,
                CallDate = DateTime.Now,
                Duration = TimeSpan.Zero,
                CallState = arg.State
            });
        }
        private void OnIncomingCallAnswer(object sender, CallEventArgs arg)
        {
            arg.State = CallState.Processed;
            RegAnsweredCall(arg);
        }
        private void RegAnsweredCall(CallEventArgs args)
        {
            var call = calls.FirstOrDefault(x => x.Source == args.SourceNumber && x.Target == args.TargetNumber);
            if (call == null) return;
            call.CallDate = DateTime.Now;
            call.CallState = args.State;
        }


        private void OnCallDrop(object sender, CallEventArgs arg)
        {
            if (arg.State != CallState.Processed) return;
            var fromPort = GetPort(arg.SourceNumber);
            var toPort = GetPort(arg.TargetNumber);
            if (fromPort != null && fromPort.State == PortState.Busy)
                fromPort.State = PortState.Free;
            if (toPort != null && toPort.State == PortState.Busy)
                toPort.State = PortState.Free;

            RegDroppedCall(arg);
            arg.SourceNumber = string.Empty;
            arg.TargetNumber = string.Empty;
        }

        private void RegDroppedCall(CallEventArgs arg)
        {
            var call = calls.FirstOrDefault(x => x.Source == arg.SourceNumber && x.Target == arg.TargetNumber);
            if (call == null) return;
            calls.Remove(call);

            call.Duration = DateTime.Now - call.CallDate;
            CallEvent?.Invoke(this, call);
        }
    }
}

