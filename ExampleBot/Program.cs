using System.Threading.Tasks;

namespace ExampleBot_Qmmands
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var bot = new ExampleBot();

            Task.Run(async () =>
            {
                bot.Initialize();
                await bot.StartAsync();

            }).GetAwaiter().GetResult();
        }

    }
}