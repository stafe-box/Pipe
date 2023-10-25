using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pipe
{
    public static class Server
    {
        public static void Run()
        {
            using NamedPipeServerStream pipeServer = new("channel", PipeDirection.InOut);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Waiting for client connection...");
            pipeServer.WaitForConnection();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Client connected");
            Console.ForegroundColor = ConsoleColor.White;
            StreamWriter sw = new(pipeServer)
            {
                AutoFlush = true
            };

            Console.Write("Input number: ");
            int number_r = int.Parse(Console.ReadLine());
            Console.Write("Input age: ");
            int age_r = int.Parse(Console.ReadLine());
            Data msg = new()
            {
                number = number_r,
                age = age_r
            };

            byte[] bytes = new byte[Unsafe.SizeOf<Data>()];
            Unsafe.As<byte, Data>(ref bytes[0]) = msg;
            sw.BaseStream.Write(bytes, 0, bytes.Length);

            byte[] received_bytes = new byte[Unsafe.SizeOf<Data>()];
            sw.BaseStream.Read(received_bytes, 0, received_bytes.Length);
            Data received_data = Unsafe.As<byte, Data>(ref received_bytes[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Received data: \n\tNumber:\t{received_data.number}; \n\tAge:\t{received_data.age};");
            Console.ReadKey();
        }
    }
}
