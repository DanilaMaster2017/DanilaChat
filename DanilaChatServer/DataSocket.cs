using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using DanilaChat;

namespace DanilaChatServer
{
    class DataSocket
    {
        public Human User { get; set; }

        const int BUFFER_SIZE = 1024;

        byte[] buffer = new byte[1024];

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public Socket ClientConnection { get; set; }
    }
}
