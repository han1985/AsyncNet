using HNetProtocal;
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
                    AsyncTool.ColorLog(AsyncTool.AsyncLogColor.Yellow, "远程连接正常下线");
                    CloseSession();
            
                }
                else
                {
                    pack.headIndex += len;
                    if(pack.headIndex < AsyncPackage.headLen)
                    {
                        skt.BeginReceive(
                            pack.headBuff,
                            pack.headIndex,
                            AsyncPackage.headLen - pack.headIndex,
                            SocketFlags.None,
                            new AsyncCallback(RecvHeadData), 
                            pack );
                    }
                    else
                    {
                        pack.InitBodyBuff();
                        skt.BeginReceive(
                            pack.bodyBuff,
                            0,
                            pack.bodyLen,
                            SocketFlags.None,
                            new AsyncCallback(RecvBodyData),
                            pack
                            );
                    }
                }
            }
            catch (Exception e)
            {

                AsyncTool.WarnLog("RecvHeadData",e.Message);

                CloseSession();
            }
        }

        void RecvBodyData(IAsyncResult ar)
        {
            try
            {
                if (skt == null || skt.Connected == false)
                {
                    AsyncTool.WarnLog("socket is null or not connected");

                    return;
                }

                AsyncPackage pack = ar.AsyncState as AsyncPackage;

                int len = skt.EndReceive(ar);

                if (len == 0)
                {
                    AsyncTool.ColorLog(AsyncTool.AsyncLogColor.Yellow, "远程连接正常下线");
                    CloseSession();
                   
                }
                pack.bodyIndex += len;
                if(pack.bodyIndex < pack.bodyLen)
                {
                    skt.BeginReceive(
                        pack.bodyBuff,
                        pack.bodyIndex,
                        pack.bodyLen-pack.bodyIndex,
                        SocketFlags.None,
                        new AsyncCallback(RecvBodyData),
                        pack
                        );
                }
                else
                {
                    //反序列化
                    AsyncMsg msg = AsyncTool.DeSerialize(pack.bodyBuff);
                    OnRecvMsg(msg);

                    pack.RessetData();
                    skt.BeginReceive(
                        pack.headBuff,
                        0,AsyncPackage.headLen,
                        SocketFlags.None,
                        new AsyncCallback(RecvHeadData),
                        pack 
                        );
                }
            }
            catch (Exception e)
            {
                AsyncTool.WarnLog("RecvBodyData", e.Message);

                CloseSession();
            }
        }

        public bool SendMsg(AsyncMsg msg)
        {
            byte[] data = AsyncTool.PackLenInfo(AsyncTool.Serialize(msg));
            return SendMsg(data);
        }

        public bool SendMsg(byte[] data)
        {
            bool result = false;

            if (this.sessionState != AsyncSessionState.Connnected)
            {
                AsyncTool.WarnLog("sendMsg  网络没连接");
                
            }
            else
            {
                NetworkStream ns = null;
                try
                {
                    ns = new NetworkStream(skt);
                    if (ns.CanWrite)
                    {
                        ns.BeginWrite(
                            data,
                            0,
                            data.Length,
                            new AsyncCallback(SendCB),
                            ns
                            );
                    }

                    result = true;
                }
                catch(Exception es)
                {
                    AsyncTool.ErrorLog(es.Message);
                }  
            }
            
            return result;
        }

        void SendCB(IAsyncResult ar)
        {
            NetworkStream ns = ar.AsyncState as NetworkStream;
            try
            {
                ns.EndWrite(ar);
                ns.Flush();
                ns.Close();
            }
            catch(Exception ex)
            {
                AsyncTool.ErrorLog("SendCB", ex.Message);
            }


        }

        void OnConnnected(bool result)
        {
            AsyncTool.Log("CLinet Online:" + result);
        }

        void OnRecvMsg(AsyncMsg msg)
        {
            AsyncTool.Log("RecvMsg:" + msg.helloMsg);
        }

        public void CloseSession()
        { 
        }
    }
}
