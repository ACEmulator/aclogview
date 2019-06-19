using System;
using System.Collections.Generic;
using System.Net;

namespace aclogview
{
    static class ServerList
    {
        public static List<Server> Servers = new List<Server>
        {
            new Server("Darktide",      new HashSet<string> { "198.252.160.71", "198.252.160.72"}, true),
            new Server("Frostfell",     new HashSet<string> { "198.252.160.73", "198.252.160.74"}, true),
            new Server("Harvestgain",   new HashSet<string> { "198.252.160.75"}, true),
            new Server("Leafcull",      new HashSet<string> { "198.252.160.76"}, true),
            new Server("Morningthaw",   new HashSet<string> { "198.252.160.77"}, true),
            new Server("Solclaim",      new HashSet<string> { "198.252.160.78"}, true),
            new Server("Thistledown",   new HashSet<string> { "198.252.160.79"}, true),
            new Server("Verdantine",    new HashSet<string> { "198.252.160.80"}, true),
            new Server("WintersEbb",    new HashSet<string> { "198.252.160.81"}, true),
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
