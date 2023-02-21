using HNetProtocal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNet
{
    internal class AsycNetClientStart
    {
        static void Main(string[] args)
        {
           AsyncNetClient client = new AsyncNetClient();

            client.StartClient("192.168.3.71", 17666);


            while (true)
            {
                string ipt = Console.ReadLine();

                AsyncMsg msg = new AsyncMsg
                {
                    helloMsg = ipt,
                };

                client.session.SendMsg(msg);
            }
            Console.ReadKey();
        }
    }
}
