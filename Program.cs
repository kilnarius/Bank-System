using System;
using System.Threading;
using System.Threading.Tasks;

namespace Банковская_система
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    new Bank().SaveDataBank();
                }
            });
            new Client().UsingProgramm();
        }
    }
}
