using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace TicketOfficeService.Tests
{
    [TestFixture]
    public class TicketOfficeTests
    {
        private const string Url = "http://127.0.0.1:8083";
        private const string Interpreter = "dotnet";
        private const string ReservationScript = "Reserve.dll";

        [Test]
        public async Task TestReserveSeatsViaPost()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("train_id", "express_2000"),
                    new KeyValuePair<string, string>("seat_count", "4")
                });

                var response = await client.PostAsync(Url + "/reserve", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var reservation = JObject.Parse(responseContent);

                Assert.AreEqual("express_2000", reservation["train_id"].ToString());
                Assert.AreEqual(4, reservation["seats"].ToObject<string[]>().Length);
                Assert.AreEqual("1A", reservation["seats"].ToObject<string[]>()[0]);
                Assert.AreEqual("75bcd15", reservation["booking_reference"].ToString());
            }
        }

        [Test]
        public void TestReserveSeatsViaCmd()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Interpreter,
                    Arguments = $"{ReservationScript} express2000 4",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var reservation = JObject.Parse(output);

            Assert.AreEqual("express_2000", reservation["train_id"].ToString());
            Assert.AreEqual(4, reservation["seats"].ToObject<string[]>().Length);
            Assert.AreEqual("1A", reservation["seats"].ToObject<string[]>()[0]);
            Assert.AreEqual("75bcd15", reservation["booking_reference"].ToString());
        }
    }
}
