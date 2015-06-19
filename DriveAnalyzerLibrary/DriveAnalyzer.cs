using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveAnalyzerLibrary
{
    public class DriveAnalyzer
    {
        public Dictionary<string, long> AllFiles { get; set; }
        public DriveAnalyzer()
        {
            if (AllFiles == null)
            {
                AllFiles = new Dictionary<string, long>();
            }
        }

        public void EnumerateSubDirectories(DirectoryInfo parentDirectory)
        {
            try
            {
                foreach (FileInfo file in parentDirectory.EnumerateFiles())
                {
                    AllFiles.Add(file.FullName, file.Length);
                }
            }
            catch { }


            try
            {
                var directories = parentDirectory.EnumerateDirectories();
                if (directories.Count() == 0)
                {
                    return;
                }

                foreach (DirectoryInfo dir in directories)
                {
                    EnumerateSubDirectories(dir);
                }
            }
            catch
            {
                return;
            }
        }

        public long GetNumberOfFiles()
        {
            return AllFiles.Count();
        }

        public long GetTotalSizeOfFiles()
        {
            return AllFiles.Sum(f => f.Value);
        }

        public void SortByFileSizeAscending()
        {
            AllFiles = AllFiles.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
        }
    }
}
