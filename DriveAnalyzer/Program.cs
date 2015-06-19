using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DriveAnalyzerLibrary;

namespace DriveAnalyzer
{
    public static class Program
    {      

        static void Main(string[] args)
        {
            DriveAnalyzerLibrary.DriveAnalyzer da = new DriveAnalyzerLibrary.DriveAnalyzer();
            
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Console.WriteLine("Searching {0}", path);            

            DirectoryInfo dr = new DirectoryInfo(path);

            da.EnumerateSubDirectories(dr);            

            Console.WriteLine("Found {0:N0} files. Total size {1:N} bytes.", da.GetNumberOfFiles(), da.GetTotalSizeOfFiles());
            Console.WriteLine("Press enter to enumerate the list.");

            Console.ReadLine();

            da.SortByFileSizeAscending();
            foreach (var item in da.AllFiles)
            {
                Console.WriteLine("{0} - {1:N2}", item.Key, item.Value);               
            }

            Console.WriteLine("Press enter to exit the application.");

            Console.ReadLine();
        }

        

        

    }
}
