using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TourManagerModels;
using System.IO;

namespace TourManager.DAL
{
    public class FileHandler : IFileAccess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FileHandler()
        {
             
        }

        public string NewFileEntry(string path, Byte[] imgBytes, string prefix="", string postfix = ".jpg")
        {
            DirectoryInfo dif = new DirectoryInfo(path);
            FileInfo[] files = dif.GetFiles();
            String imgID = (files.Length + 1).ToString();

            string fileName = prefix = imgID + postfix;
            string finalPath = path + fileName;

            using (FileStream fs = new FileStream(finalPath, FileMode.Create))
            {
                fs.Write(imgBytes, 0, imgBytes.Length);
            }
            log.Info("Successfully saved image -" + fileName);
            return dif.FullName + "\\" + fileName;
        }

        public string NewFileEntry(string path, string content, string prefix = "", string postfix = ".txt")
        {
            throw new NotImplementedException();
        }

        public void DeleteEntryByName(string pathToFile, string postfix="")
        {
            FileInfo file = new FileInfo(pathToFile);

            if(postfix != "")
            {
                if (file.Exists && file.Extension.Contains(postfix))
                {
                    File.Delete(file.FullName);
                }
            }
            else
            {
                if (file.Exists)
                {
                    File.Delete(file.FullName);
                }
            }
            log.Info("Successfully Deleted File " + file.Name + " at " + file.DirectoryName);            
        }
    }
}
