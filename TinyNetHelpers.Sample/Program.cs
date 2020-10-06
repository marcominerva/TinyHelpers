using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TinyNetHelpers.Extensions;
using TinyNetHelpers.Threading;

namespace TinyNetHelpers.Sample
{
    public class Person
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }

    public enum Priority
    {
        [Display(Name = "Bassa")]
        Low,

        [Display(Name = "Media")]
        Medium,

        [Display(Name = "Alta")]
        High
    };

    [Flags]
    public enum ConnectionTypes
    {
        [Display(Name = "Cavo")]
        Wired = 1,
        WiFi = 2,
        Bluetooth = 4,
        [Display(Name = "Via Satellite")]
        Satellite = 8
    };

    internal class Program
    {
        private static readonly AsyncLock syncObject = new AsyncLock();

        private static async Task Main(string[] args)
        {
            var connectionTypes = ConnectionTypes.Wired | ConnectionTypes.Satellite;

            var description = connectionTypes.GetDescription();

            foreach (var connectionType in connectionTypes.GetFlags())
            {
                Foo(connectionType);
            }

            connectionTypes.GetFlags().ForEach(c => Foo(c));

            var priority = Priority.Medium;
            Console.WriteLine($"La priorità è {priority.GetDescription()}");

            //var a = "Marco";
            //var b = "marco";

            //if (a.EqualsIgnoreCase(b))
            //{

            //}

            //var inputName = "";
            //var name = inputName.GetValueOrDefault("sconosciuto");

            var people = GetPeople();

            foreach (var (person, i) in people.WithIndex())
            {
                Console.WriteLine($"Elaboro la persona {i}...");
            }

            //var task = Task.Delay(5000);
            //await task.TimeoutAfterAsync(TimeSpan.FromSeconds(2));

            using (await syncObject.LockAsync())
            {
                await Task.Delay(1000);
            }
        }

        private static void Foo(ConnectionTypes connectionType)
        {
            Console.WriteLine($"Analizzo la connessione {connectionType.ToString()}...");
            // ...
        }

        private static IEnumerable<Person> GetPeople()
        {
            var people = new List<Person>
           {
                new Person { FirstName = "Donald", LastName = "Duck" },
                new Person { FirstName = "Mickey", LastName = "Mouse" },
                new Person { FirstName = "Daisy", LastName = "Duck" }
             };

            return people;
        }
    }
}
