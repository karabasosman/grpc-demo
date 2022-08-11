using Grpc.Net.Client;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using static calc.add.Addition;
using static calc.divide.Division;
using static calc.multiply.Multiplication;
using static calc.percentage.Percentage;
using static calc.substract.Substraction;

namespace MathFanboy
{
    class Program
    {
        static string[] endpoints = null;
        
        static async Task Main(string[] args)
        {

#if DEBUG
            endpoints = new string[5] {
                "http://localhost:5001", 
                "http://localhost:5002", 
                "http://localhost:5003", 
                "http://localhost:5004",
                "http://localhost:5005"};
#else
            endpoints = new string[1] {
                Environment.GetEnvironmentVariable("AdditionEndpoint")};
#endif
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            Console.WriteLine("available endpoints");
            foreach(string endpoint in endpoints)
            {
                Console.WriteLine(endpoint);
            }
            Console.WriteLine("Starting gRPC chatting");
            
            using var AdditionChannel = GrpcChannel.ForAddress(endpoints[0]);
      

            var AdditionClient = new AdditionClient(AdditionChannel);


            while (true)
            {
                try
                {
                    Thread.Sleep(2000);                    
                    var position = 0;
                    var op1 = new Random().Next(0, 50);
                    var op2 = new Random().Next(0, 50);
                    switch (position)
                    {
                        case 0:
                            Console.WriteLine("Attempting an addition");
                            Console.WriteLine((await AdditionClient.AddAsync(new calc.add.AdditionRequest
                            {
                                Op1 = op1,
                                Op2 = op2
                            })).Result);
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }            

        }       

    }
}
