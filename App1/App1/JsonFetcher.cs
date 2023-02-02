using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace App1
{
    public class JsonFetcher
    {

        private static HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => { return true; }; // Ignore SSL errors for local development
            return new HttpClient(httpClientHandler);
        }
        
        public static string GetNotif2()
        {
            string URL = "https://notif2.gdanskiewodociagi.pl/api/csappwersja";
            try
            {
                var data = new[]
                {
                    new KeyValuePair<string, string>("Wersja", "Wersja"),
                };
                var jsonDelays = CreateHttpClient().PostAsync(URL, new FormUrlEncodedContent(data)).Result.Content.ReadAsStringAsync().Result;
                return jsonDelays;
            }
            catch (Exception ex)
            {
                int i = 0;
            }

            return null;
        }
        
        public static DelaysDTO GetDelaysOfStop(int stopId)
        {
            string URL = $"https://ckan2.multimediagdansk.pl/delays?stopId={stopId}";
            try
            {
                var jsonDelays = CreateHttpClient().GetStringAsync(URL).Result;
                var delays = JsonConvert.DeserializeObject<DelaysList>(jsonDelays);
                return DelaysDTO.EntityToDto(delays);
            }
            catch (Exception ex)
            {
                int i = 0;
            }

            return null;
        }
    }

    public class DelaysDTO
    {
        public string LastUpdate { get; set; }

        public List<DelayDTO> Delays { get; set; }


        public static DelaysDTO EntityToDto(DelaysList delaysList)
        {
            var delays = new List<DelayDTO>();
            delaysList.Delay.ForEach(delay => delays.Add(DelayDTO.EntityToDto(delay)));
            return new DelaysDTO()
            {
                LastUpdate = delaysList.LastUpdate,
                Delays = delays
            };
        }
    }

    public class DelaysList
    {
        public string LastUpdate { get; set; }

        public List<Delay> Delay { get; set; }
    }

    public class DelayDTO
    {
        public string Id { get; set; }
        public string Headsign { get; set; }
        public int VehicleCode { get; set; }
        public string TheoreticalTime { get; set; }
        public string EstimatedTime { get; set; }


        public static DelayDTO EntityToDto(Delay delay)
        {
            return new DelayDTO()
            {
                Id = delay.Id,
                Headsign = delay.Headsign,
                VehicleCode = delay.VehicleCode,
                TheoreticalTime = delay.TheoreticalTime,
                EstimatedTime = delay.EstimatedTime
            };
        }
    }

    public class Delay
    {
        public string Id { get; set; }//:"T31R10085",
        public int DelayInSeconds { get; set; }//:13,
        public string EstimatedTime { get; set; }//:"16:17",
        public string Headsign { get; set; }//:"Rumia Partyzantów",
        public int RouteId { get; set; }//:10085,
        public int TripId { get; set; }//:31,
        public string Status { get; set; }//:"REALTIME",
        public string TheoreticalTime { get; set; }//:"16:17",
        public string Timestamp { get; set; }//:"16:11:08",
        public int Trip { get; set; }//:2452275,
        public int VehicleCode { get; set; }//:7091,
        public int VehicleId { get; set; }//:203833
    }
}