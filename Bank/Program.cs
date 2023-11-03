using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bank
{
    public class Bank
    {
        private dynamic storage = new ExpandoObject();
        private object lockObject = new object();

        public void ChangeName(string name)
        {
            Task.Run(() =>
            {
                lock (lockObject)
                {
                    storage.name = name;
                    Console.WriteLine("Name changed to: " + storage.name);
                    SaveStorageToJsonFile("storage.json");
                }
            });
        }

        public void ChangeMoney(int money)
        {
            Task.Run(() =>
            {
                lock (lockObject)
                {
                    storage.money = money;
                    Console.WriteLine("Money changed to: " + storage.money);
                    SaveStorageToJsonFile("storage.json");
                }
            });
        }

        public void ChangePercent(int percent)
        {
            Task.Run(() =>
            {
                lock (lockObject)
                {
                    storage.percent = percent;
                    Console.WriteLine("Percent changed to: " + storage.percent);
                    SaveStorageToJsonFile("storage.json");
                }
            });
        }

        public void LoadStorageFromJsonFile()
        {
            Task.Run(() =>
            {
                lock (lockObject)
                {
                    if (File.Exists("storage.json"))
                    {
                        string json = File.ReadAllText("storage.json");
                        storage = JsonConvert.DeserializeObject<ExpandoObject>(json);
                        Console.WriteLine("Storage loaded from file.");
                        PrintStorage();
                    }
                    else
                    {
                        Console.WriteLine("Файл не найден.");
                    }
                }
            });
        }

        public void PrintStorage()
        {
            foreach (var property in ((IDictionary<string, object>)storage))
            {
                Console.WriteLine($"{property.Key}: {property.Value}");
            }
            Console.WriteLine("\n");
        }

        public void SaveStorageToJsonFile(string filePath)
        {
            lock (lockObject)
            {
                string json = JsonConvert.SerializeObject(storage, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();
            bank.LoadStorageFromJsonFile();
            Task.Delay(1000).Wait();

            bank.ChangeName("New Name");
            bank.ChangeMoney(1000);
            bank.ChangePercent(5);

            Task.Delay(2000).Wait();

            Console.ReadLine();
        }
    }
}
