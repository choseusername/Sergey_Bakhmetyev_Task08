using System;
namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicArray<int> dynamicArray = new DynamicArray<int>(50);
            dynamicArray.Add(505);
            Console.WriteLine("dynamicArray[0] = {0}", dynamicArray[0]);
            Console.ReadKey();
        }
    }
}
