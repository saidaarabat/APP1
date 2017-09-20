using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;

namespace WebAPI_Sondage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
    	    WebHost.CreateDefaultBuilder(args)
    		.UseStartup<Startup>()
    		.UseKestrel(options =>
    		{
    			options.Listen(IPAddress.Loopback, 5000);
    			options.Listen(IPAddress.Loopback, 5001, listenOptions =>
    			{
    				listenOptions.UseHttps("testCert.pfx", "testPassword");
    			});
    		})
    		.Build();
    }
}
