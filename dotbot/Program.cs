namespace dotbot
{
    using System;
    using System.Linq;
    using Microsoft.Owin.Hosting;
    using Owin;

    static class Program
    {
        public static int Port = 8888;

        static void Main(string[] args)
        {
            var options = new StartOptions
            {
                ServerFactory = "Nowin",
                Port = Port
            };

            using (WebApp.Start<Startup>(options))
            {
                Console.WriteLine("Running a http server on port " + Port);
                Console.ReadKey();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(async context =>
            {
                if (context.Request.Method == "POST" && context.Request.Path.Value == "/start")
                {
                    Console.WriteLine("Start...");
                } 
                else if (context.Request.Method == "GET" && context.Request.Path.Value == "/move")
                {
                    var random = new Random();
                    var moves = new string[] { "ROCK", "PAPER", "SCISSORS" };
                    var move = moves[random.Next(0, moves.Length)];
                    context.Response.ContentType = "text/plain";
                    Console.WriteLine("myMove: " + move);
                    await context.Response.WriteAsync(move);
                } 
                else if (context.Request.Method == "POST" && context.Request.Path.Value == "/move")
                {
                    var formData = await context.Request.ReadFormAsync();
                    var lastOpponentMove = formData.FirstOrDefault(x => x.Key == "lastOpponentMove").Value[0];
                    Console.WriteLine("lastOpponentMove: " + lastOpponentMove);
                }

                context.Response.StatusCode = 404;
            });
        }
    }
}
