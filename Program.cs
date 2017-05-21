using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Chap1;
using System.Net.Http;
using System.Net.Http.Headers;


namespace Chap1
{
	public static class Program
	{
        //[ThreadStatic]
        //public static int _field;

        public static ThreadLocal<int> _field = new ThreadLocal<int>(
        () =>
        {
            return Thread.CurrentThread.ManagedThreadId;
        });
        static HttpClient client = new HttpClient();

		static void Main(string[] args)
		{
            /*
			Thread t = new Thread(new ThreadStart(ThreadMethod));
			//t.IsBackground = true;
			t.Start();

			Thread t1 = new Thread(new ParameterizedThreadStart(ThreadMethodWithParams));
			t1.Start("Hello params");

			for (int i = 0; i < 4; i++) {
				Console.WriteLine("Main thread: do some work");
				Thread.Sleep(0);
			}

			t.Join();
			t1.Join();
			Console.WriteLine("ThreadMethod is finished");
			
			new Thread(() =>
			{
				for (int i = 0; i < 10; i++) 
				{
					Console.WriteLine("Thread A: {0}", _field);
					Thread.Sleep(0);
				}
			}).Start();

			new Thread((ob) =>
			{
				for (int i = 0; i < 10; i++)
				{
					Console.WriteLine("Thread B: {0} og tekst {1}", _field, (String) ob);
					Thread.Sleep(0);
				}
			}).Start("Hello world!");
            */

            /*
            const int FibonacciCalculations = 10;

            // One event is used for each Fibonacci object
            ManualResetEvent[] doneEvents = new ManualResetEvent[FibonacciCalculations];
            Fibonacci[] fibArray = new Fibonacci[FibonacciCalculations];
            Random r = new Random();

            // Configure and launch threads using ThreadPool:
            Console.WriteLine("launching {0} tasks...", FibonacciCalculations);
            for (int i = 0; i < FibonacciCalculations; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                Fibonacci f = new Fibonacci(r.Next(20, 40), doneEvents[i]);
                fibArray[i] = f;
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }

            // Wait for all threads in pool to calculation...
            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All calculations are complete.");

            // Display the results...
            for (int i = 0; i < FibonacciCalculations; i++)
            {
                Fibonacci f = fibArray[i];
                Console.WriteLine("Fibonacci({0}) = {1}", f.N, f.FibOfN);
            }
            */

            RunAsync().Wait();
            
		}

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:5000/api/hello/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Hilsen hilsen = await GetProductAsync("http://localhost:5000/api/hello/test");
            //Thread.Sleep(10000);
            Console.WriteLine("Svaret er: {0}", hilsen.Prompt);
            Console.ReadLine();
        }

        static async Task<Hilsen> GetProductAsync(string path)
        {
            Hilsen product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Hilsen>();
            }
            return product;
        }

        public static void ThreadMethodWithParams(object o) 
		{
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("ParamThread: {0} gang {1}", i, (String) o);
				Thread.Sleep(0);
			}
		}

		public static void ThreadMethod()
		{
			for (int i = 0; i < 10; i++) 
			{
				Console.WriteLine("ThreadProc: {0}", i);
				Thread.Sleep(0);
			}
		}
	}

    public class Hilsen
    {
        public string Prompt { get; set; }
    }
}
