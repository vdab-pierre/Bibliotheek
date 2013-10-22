using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace Tools
{
    public static class Netwerk
    {
        public static bool AssertConnect(string host){

            bool connected;

            Ping ping = new Ping();
            PingReply reply=null;
            try
            {
                reply = ping.Send(host);
            }
            catch
            {
                return false;
            }

            connected = (reply.Status == IPStatus.Success);

            return connected; // possibly won't return false because of exceptions
        }

    }
}