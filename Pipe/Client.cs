using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pipe
{
    public static class Client
    {
        public static void Run()
        {
            using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "channel", PipeDirection.InOut);
            pipeClient.Connect();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Client connected to server");
            Console.ForegroundColor = ConsoleColor.White;
            byte[] bytes = new byte[Unsafe.SizeOf<Data>()];
            pipeClient.Read(bytes, 0, bytes.Length);
            Data received_data = Unsafe.As<byte, Data>(ref bytes[0]);
            Console.WriteLine("Number: " + received_data.number);
            Console.WriteLine("Age: " + received_data.age);
            received_data.number += received_data.age;
            byte[] modified_bytes = new byte[Unsafe.SizeOf<Data>()];
            Unsafe.As<byte, Data>(ref modified_bytes[0]) = received_data;
            pipeClient.Write(modified_bytes, 0, modified_bytes.Length);
            Console.ReadKey();
        }
    }
}
