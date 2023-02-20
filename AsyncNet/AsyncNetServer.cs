using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HNet
{
    public class AsyncNetServer
    {
        private Socket skt = null;
        public int backLog = 10;
        List<AsyncSession> AsyncSessionList = null;
        public void StartServer(string ip, int port)
        {
            try
            {
                skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                skt.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                skt.Listen(backLog);
                AsyncTool.ColorLog(AsyncTool.AsyncLogColor.Green,"Server Start...");
                skt.BeginAccept(new AsyncCallback(ClientConnectCB), null);
            }
            catch (Exception es)
            {
                AsyncTool.ErrorLog(es.Message);
            }
        }

        void ClientConnectCB(IAsyncResult ar)
        {
            AsyncSession session = new AsyncSession();

            try
            {
                Socket clientSkt = skt.EndAccept(ar);

                AsyncTool.Log("new Client online.");

                if(clientSkt.Connected)
                {
                    lock (AsyncSessionList)
                    {
                        AsyncSessionList.Add(session);
                    }
                    session.InitSession(clientSkt);
                }

                //开始接收下一个新客户端得连接
                skt.BeginAccept(new AsyncCallback(ClientConnectCB), null);
            }
            catch (Exception es)
            {
                AsyncTool.ErrorLog("ClientConnectCB", es.Message);
            }
          
        }
    }
}
