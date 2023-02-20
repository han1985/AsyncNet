using System;
using System.Collections.Generic;
using System.Text;

namespace HNet
{
    public class AsyncPackage
    {
        public const int headLen = 4;
        public int bodyLen = 0;
        public int headIndex = 0;
        public int bodyIndex = 0;

        public byte[] headBuff = null;
      
        public byte[] bodyBuff = null;
   
        public AsyncPackage() { 
        
            headBuff = new byte[4];
        }

        public void InitBodyBuff()
        {
            bodyLen = BitConverter.ToInt32(headBuff, 0);
            bodyBuff= new byte[bodyLen];
        }
    }
}
