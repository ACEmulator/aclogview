using System;
using System.Collections.Generic;
using System.Net;

namespace aclogview
{
    static class ServerList
    {
        public static List<Server> Servers = new List<Server>
        {
            // todo
        };

        public static Server FindBy(string name)
        {
            foreach (var server in Servers)
            {
                if (server.Name == name)
                    return server;
            }

            return null;
        }

        public static List<Server> FindBy(IPAddress ipAddress)
        {
            var results = new List<Server>();

            foreach (var server in Servers)
            {
                if (server.IPAddresses.Contains(ipAddress))
                    results.Add(server);
            }

            return results;
        }

        public static List<Server> FindBy(IpHeader ipHeader, bool isSend)
        {
            IPAddress ipAddress;

            if (!isSend)
                ipAddress = new IPAddress(ipHeader.sAddr.bytes);
            else
                ipAddress = new IPAddress(ipHeader.dAddr.bytes);

            return FindBy(ipAddress);
        }
    }
}
