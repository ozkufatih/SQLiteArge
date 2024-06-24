namespace Playground.Utils.ConsoleHelper
{
    public static class ConsoleExtensions
    {
        public static void ToColorWriteLine(this string str, ConsoleColor foreColor = ConsoleColor.Gray)
        {
            ConsoleColor currentCol = Console.ForegroundColor;

            Console.ForegroundColor = foreColor;
            Console.WriteLine(str);
            Console.ForegroundColor = currentCol;
        }

        public static void ToColorWrite(this string str, ConsoleColor foreColor = ConsoleColor.Gray)
        {
            ConsoleColor currentCol = Console.ForegroundColor;

            Console.ForegroundColor = foreColor;
            Console.Write(str);
            Console.ForegroundColor = currentCol;
        }
    }
}
