using System.Net;

namespace ElevaniPaymentGateway.Infrastructure.Helpers
{
    public class IPAddressHelper
    {
        static string IPAddress = string.Empty;
        public static string GetIPAddress()
        {
            IPHostEntry host = default(IPHostEntry);
            string hostName = null;
            hostName = Environment.MachineName;
            host = Dns.GetHostEntry(hostName);

            foreach(IPAddress IP in host.AddressList)
            {
                if(IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    IPAddress = IP.ToString();
                }
            }

            return IPAddress;
        }
    }
}
