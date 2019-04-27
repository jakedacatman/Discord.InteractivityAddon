using System.Threading.Tasks;

namespace ExampleBot_Qmmands
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var Bot = new ExampleBot();

            Task.Run(async () => {
                Bot.Initialize();
                await Bot.StartAsync();

            }).GetAwaiter().GetResult();
        }

    }
}