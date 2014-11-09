﻿/*
    License: http://www.apache.org/licenses/LICENSE-2.0
 */
namespace Outsorcery.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using FluentAssertions;
    using NUnit.Framework;

    /// <summary>
    /// Outsourced Worker Connection Tests
    /// </summary>
    [TestFixture]
    public class OutsourcedWorkerConnectionTests
    {
        /// <summary>The test stream</summary>
        private static readonly Stream TestStream = new MemoryStream();

        /// <summary>The test objects</summary>
        private static readonly IEnumerable<object> TestObjects = new List<object>
        {
            52,
            35.5f,
            "A test",
            new Tuple<int, string>(10, "Hello"),
            new TestSerializableClass(),
        };

        /// <summary>The test objects</summary>
        private static readonly IEnumerable<IWorkerConnection> OutsourcedWorkerConnections = new List<IWorkerConnection>
        {
            new StreamWorkerConnection(TestStream),
        };

        /// <summary>
        /// Sends and receives object successfully test.
        /// </summary>
        /// <typeparam name="T">The Type of the test value</typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="testValue">The test value.</param>
        [Test]
        public async void SendsAndReceivesObjectSuccessfully<T>(
            [ValueSource("OutsourcedWorkerConnections")]IWorkerConnection connection,
            [ValueSource("TestObjects")]T testValue)
        {
            // Arrange
            var ct = new CancellationToken();
            TestStream.Position = 0; // To start from fresh

            // Act
            await connection.SendObjectAsync(testValue, ct).ConfigureAwait(false);
            TestStream.Position = 0; // To read what we just wrote
            var deserialized = (T)await connection.ReceiveObjectAsync(ct).ConfigureAwait(false);

            // Assert
            deserialized.ShouldBeEquivalentTo(testValue);
        }

        /// <summary>
        /// Test Serializable class
        /// </summary>
        [Serializable]
        public class TestSerializableClass
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestSerializableClass"/> class.
            /// </summary>
            public TestSerializableClass()
            {
                TestString = "Test";
                TestInt = 5;
                TestList = new List<string> { "Hello", "World" };
            }

            /// <summary>
            /// Gets or sets the test string.
            /// </summary>
            /// <value>
            /// The test string.
            /// </value>
            public string TestString { get; set; }

            /// <summary>
            /// Gets or sets the test integer.
            /// </summary>
            /// <value>
            /// The test integer.
            /// </value>
            public int TestInt { get; set; }

            /// <summary>
            /// Gets or sets the test list.
            /// </summary>
            /// <value>
            /// The test list.
            /// </value>
            public List<string> TestList { get; set; }
        }
    }
}
