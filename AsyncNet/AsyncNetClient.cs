using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HNet
{
    public class AsyncNetClient
    {
        Socket skt = null;
        AsyncSession session = null;

        public void StartClient(string ip, int port)
        {
            try
            {
                skt = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                AsyncTool.ColorLog(AsyncTool.AsyncLogColor.Green,"Client start ....");
                EndPoint pt = new IPEndPoint(IPAddress.Parse(ip), port);
                skt.BeginConnect(pt,new AsyncCallback(ServerConnectCB),null);
            }
            catch(Exception es) {
                AsyncTool.ErrorLog(es.Message);
            }
        }

        void ServerConnectCB(IAsyncResult ar)
        {
            session= new AsyncSession();

            try
            {
                skt.EndConnect(ar);

                if(skt.Connected)
                {
                    session.InitSession(skt);
                }


            }
            catch(Exception es)
            {
                AsyncTool.ErrorLog(es.Message);
            }
        }
    }
}
