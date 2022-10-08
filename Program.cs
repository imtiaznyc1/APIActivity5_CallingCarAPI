using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ApiCall
{
    public class Car{
        [JsonProperty("Count")]
        public int Count{get; set;}

        [JsonProperty("Results")]
        public List<Results> Results = new List<Results>();
    }

    public class Results{
        [JsonProperty("Model_Name")]
        public string? Model_Name{get; set;}

        [JsonProperty("Make_ID")]
        public int Make_ID;

        [JsonProperty("Model_ID")]
        public int Model_ID;
    }

    class Program{
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepositories();
        }

        private static async Task ProcessRepositories()
        {
            while (true){
                try{
                    Console.WriteLine("Hello there wonderful human being. We will be using the car API! Write a car manufacturer name to see list of models, the make id, and the model id!");
                    var mk = Console.ReadLine();
                    if(string.IsNullOrEmpty(mk)){
                        break;
                    }
                    var result = await client.GetAsync("https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMake/" + mk.ToLower() + "?format=json");
                    var resultRead = await result.Content.ReadAsStringAsync();
                
                    //Console.WriteLine(resultRead);
                    var c = JsonConvert.DeserializeObject<Car>(resultRead);
                    Console.WriteLine(c.Results);
                    foreach(var i in c.Results){
                        Console.WriteLine("--------");
                        Console.WriteLine("Model name: " + i.Model_Name);
                        Console.WriteLine("Make ID: " + i.Make_ID);
                        Console.WriteLine("Model ID: " + i.Model_ID);
                        Console.WriteLine("--------");
                    }
                }catch (Exception){
                    Console.WriteLine("No result was found! Try again :(");
                }

            }
            
        }
    }
}