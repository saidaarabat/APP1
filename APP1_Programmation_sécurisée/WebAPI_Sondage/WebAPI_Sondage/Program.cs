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
               .UseKestrel(options => {

                   // Limitation du nombre de connections
                   options.Limits.MaxConcurrentConnections = 4;
                   options.Limits.MaxConcurrentUpgradedConnections = 4;

                   // Limitation de la taille du body pour la requete
                   options.Limits.MaxRequestBodySize = 10 * 1024;

                   // Ligne pour le debug
                   /*options.Listen(IPAddress.Loopback, 8080, listenOptions =>
				   {
					   listenOptions.UseConnectionLogging();
				   });*/

                   // Definition de l ' HTTPS sur le port 8081 et definition de son certificat
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
