using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HNet
{
    /// <summary>
    /// 数据得收发
    /// </summary>
    public class AsyncSession
    {
        Socket skt;

        public enum AsyncSessionState
        {
            None,
            Connnected,
            DisCOnnected,
        }


        public AsyncSessionState sessionState= AsyncSessionState.None;
        public void InitSession(Socket skt)
        {
            bool result = false;
            try
            {
                this.skt = skt;

                AsyncPackage pack= new AsyncPackage();

                skt.BeginReceive(pack.headBuff, 0, AsyncPackage.headLen, SocketFlags.None, new AsyncCallback(RecvHeadData), pack);

                result = true;

                sessionState = AsyncSessionState.Connnected;
            }
            catch(Exception ex)
            {
                AsyncTool.ErrorLog(ex.Message);
            }
            finally
            {
                OnConnnected(result);
            }
        }

        void RecvHeadData(IAsyncResult ar)
        {
            try
            {
                if(skt == null || skt.Connected == false)
                {
                    AsyncTool.WarnLog("socket is null or not connected");

                    return;
                }

                AsyncPackage pack = ar.AsyncState as AsyncPackage;

                int len = skt.EndReceive(ar);

                if(len == 0)
                {
                    AsyncTool.ColorLog(AsyncTool.AsyncLogColor.Yellow,"远程连接下线");
                    CloseSession();
                    return;
                }
                else
                {
                    pack.headIndex += len;
                    if(pack.headIndex < AsyncPackage.headLen)
                    {
                        skt.BeginReceive(pack.headBuff,pack.headIndex,AsyncPackage.headLen - pack.headIndex,SocketFlags.None,
                            new AsyncCallback(RecvHeadData), pack );
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception e)
            {

                AsyncTool.WarnLog("RecvHeadData",e.Message);

                CloseSession();
            }
        }

        void OnConnnected(bool result)
        {
            AsyncTool.Log("CLinet Online:" + result);
        }

        public void CloseSession()
        { 
        }
    }
}
