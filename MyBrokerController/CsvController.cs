using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyBroker.Controller
{
    public class CsvController
    {
        private string _fileName;
        private string _folder;

        public CsvController(string folder,string fileName)
        {
            _fileName = fileName;
            _folder = folder;
        }

        public void AppendLine(string line)
        {
        
            using(StreamWriter writer=new StreamWriter(Path.Combine(_folder,_fileName),true))
            {
                writer.AutoFlush = true;
                writer.WriteLine(line);
            }
        
        }


    }
}
