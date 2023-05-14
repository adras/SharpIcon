using SharpIconLib;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles("TESTDATA-DONOTCOMMIT");

            SharpIcon.Load(File.OpenRead(files[0]));
        }
    }
}