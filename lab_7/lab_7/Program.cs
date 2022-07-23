using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Benchmarks;
using System;
using System.Linq;

namespace Benchmarks
{

    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class MultBechmarks
    {
        static int count = 10000;
        static float[] weightsFloat = GetRandomWeightsFloats();
        static short[] weightsShorts = GetRandomWeightsShorts();
        static short randShortInput = (short)new Random().Next(88, 120);
        static float randFloatInput = (float)(new Random().NextDouble() + 0.88d);

        private static float[] GetRandomWeightsFloats()
        {
            Random random = new Random();
            float[] randNums = new float[count];
            for (int i = 0; i < count; i++)
            {
                float randNum = (float)random.NextDouble();
                randNums[i] = randNum;
            }
            return randNums;
        }

        private static short[] GetRandomWeightsShorts()
        {
            Random random = new Random();
            short[] randNums = new short[count];
            for (int i = 0; i < count; i++)
            {
                short randNum = (short)random.Next(-32768, 32768);
                randNums[i] = randNum;
            }
            return randNums;
        }


        [Benchmark]
        public void MultiplyByFloat()
        {
            Mult(randFloatInput);
        }
        [Benchmark]
        public void MultiplyDivideByte()
        { 
            MultiplyDivideByte(randShortInput);
        }

        private void Mult(float a)
        {
            for (int i = 0; i < count; i++)
            {
                float weight = weightsFloat[i];
                weight *= a;
                weightsFloat[i] = weight;
            }
        }

        public void MultiplyDivideByte(short a)
        {
            for (int i = 0; i < count; i++)
            {
                float weight = weightsShorts[i];
                weight = a * weight / 1000;
                weightsFloat[i] = weight;
            }
        }
    }
}


namespace lab_7
{


    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<MultBechmarks>();
        }
        public static async void Run()
        {
            string connectionString = @"Data Source=D:\WSEI\ProgramowanieObjektowe\WSEI_Programowanie_Objektowe\lab_7\lab_7\bin\Debug\netcoreapp3.1\lab_7.db;";

            using (BloggingContext db = new BloggingContext(connectionString))
            {
                Console.WriteLine($"Database ConnectionString: {db.ConnectionString}.");

                // Create
                Console.WriteLine("Inserting a new blog");

                await db.AddAsync(new Blog { Url = "http://blogs.msdn.com/adonet" });
                await db.SaveChangesAsync();

                // Read
                Console.WriteLine("Querying for a blog");

                Blog blog = db.Blogs
                    .OrderBy(b => b.Id)
                    .First();

                // Update
                Console.WriteLine("Updating the blog and adding a post");

                blog.Url = "https://devblogs.microsoft.com/dotnet";
                blog.Posts.Add(new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
                db.SaveChanges();

                // Delete
                Console.WriteLine("Delete the blog");

                db.Remove(blog);
                db.SaveChanges();
            }
        }
    }
}