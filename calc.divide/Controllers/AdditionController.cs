using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using System.Collections.Generic;
using System;
using static calc.add.Addition;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace calc.divide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionController : ControllerBase
    {
        static string[] endpoints = null;
        // GET: api/<AdditionController>
        [HttpGet]
        public async Task<object> Get()
        {
#if DEBUG
            endpoints = new string[1] {
                "http://localhost:5001"};
#else
            endpoints = new string[1] {
                Environment.GetEnvironmentVariable("AdditionEndpoint")};
#endif


            try
            {
                Console.WriteLine("Starting gRPC..");
                Console.WriteLine();
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
                using var AdditionChannel = GrpcChannel.ForAddress(endpoints[0]);
                var op1 = new Random().Next(0, 50);
                var op2 = new Random().Next(0, 50);

                var AdditionClient = new AdditionClient(AdditionChannel);

                var result = await AdditionClient.AddAsync(new calc.add.AdditionRequest
                {
                    Op1 = op1,
                    Op2 = op2
                });
                Console.WriteLine(result);
                Console.WriteLine();
                Console.WriteLine("Finishing gRPC..");
                return result;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message+ "//////-----((( "+ex.InnerException);
            }

            return null;
            
        }


    }
}
