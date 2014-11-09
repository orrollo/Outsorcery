﻿/*
    License: http://www.apache.org/licenses/LICENSE-2.0
 */
namespace Outsorcery.ExampleServer
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>The local end point for demonstration purposes</summary>
        private static readonly IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444);

        /// <summary>
        /// The console application entry point.
        /// </summary>
        public static void Main()
        {
            BasicUsageExample(new CancellationToken()).Wait();
        }

        /// <summary>
        /// An example of the most basic usage of the server.
        /// When implementing your own, don't forget to reference your work item library,
        /// the server needs it to understand the client requests and do the work!
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// An awaitable task.
        /// </returns>
        public static async Task BasicUsageExample(CancellationToken cancellationToken)
        {
            Console.WriteLine("====================");
            Console.WriteLine("Basic Server Example");
            Console.WriteLine("====================");

            await new TcpWorkServer(LocalEndPoint)
                                    .Run(cancellationToken)
                                    .ConfigureAwait(false);
        }
    }
}
