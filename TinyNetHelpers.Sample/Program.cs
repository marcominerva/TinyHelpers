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

    internal class Program
    {
        private static readonly AsyncLock syncObject = new AsyncLock();

        private static async Task Main(string[] args)
        {
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
