using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourManager.DAL
{
    public interface IFileAccess
    {
        public string NewFileEntry(string path, Byte[] imgBytes, string prefix = "", string postfix = ".jpg");
        public string NewFileEntry(string path, string content, string prefix = "", string postfix = ".txt");
        public void DeleteEntryByName(string pathToFile, string postfix = "");
    }
}
