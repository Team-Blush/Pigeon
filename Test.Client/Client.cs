namespace Test.Client
{
    using System;
    using System.Linq;

    using Test.Data;

    internal class Client
    {
        private static void Main()
        {
            var context = new TestContext();
            Console.WriteLine(context.Photos.Count());
        }
    }
}