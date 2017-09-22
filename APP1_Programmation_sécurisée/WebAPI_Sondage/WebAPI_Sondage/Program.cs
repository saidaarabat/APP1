﻿using System;
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
               .UseKestrel(options => {

				   options.Listen(IPAddress.Loopback, 8080, listenOptions =>
				   {
					   // Uncomment the following to enable Nagle's algorithm for this endpoint.
					   //listenOptions.NoDelay = false;

					   listenOptions.UseConnectionLogging();
				   });

				   options.Listen(IPAddress.Loopback, 8081, listenOptions =>
				   {
					   listenOptions.UseHttps("localhost.pfx", "password");
					   listenOptions.UseConnectionLogging();
				   });

				   options.UseSystemd();
        })
               .UseIISIntegration()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseStartup<Startup>()
		.Build();
    }
}
