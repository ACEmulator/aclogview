using System;
using System.Collections.Generic;
using System.Net;

namespace aclogview
{
    class Server
    {
        public readonly string Name;
        public readonly HashSet<IPAddress> IPAddresses;
        public readonly bool IsRetail;

        public Server(string name, HashSet<string> ipAddresses, bool isRetail)
        {
            Name = name;
            IPAddresses = new HashSet<IPAddress>();
            foreach (var ipAddress in ipAddresses)
                IPAddresses.Add(IPAddress.Parse(ipAddress));
            IsRetail = isRetail;
        }

        public Server(string name, HashSet<IPAddress> ipAddresses, bool isRetail)
        {
            Name = name;
            IPAddresses = ipAddresses;
            IsRetail = isRetail;
        }

        public Server(string name, IPAddress ipAddress, bool isRetail) :this (name, new HashSet<IPAddress> { ipAddress }, isRetail)
        {
        }

        public Server(string name, string ipAddress, bool isRetail) : this(name, IPAddress.Parse(ipAddress), isRetail)
        {
        }
    }
}
