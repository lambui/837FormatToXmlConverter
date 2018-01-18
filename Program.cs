using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading;

namespace edi_app
{
    class Program
    {
        const char seperator = '*';
        const string form_837_folder_name = "837Form";
        const string xml_folder = "XmlExport";
        static List<List<string>> file837 = new List<List<string>>();

        //create form_837_folder_name and xml_folder if not already exist
        static void Init()
        {
            if(Directory.Exists(Directory.GetCurrentDirectory() + "/" + form_837_folder_name) == false)
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/" + form_837_folder_name);
            if (Directory.Exists(Directory.GetCurrentDirectory() + "/" + xml_folder) == false)
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/" + xml_folder);
        }

        static void Main(string[] args)
        {
            Init();
            while(true)
            {
                file837.Clear();
                while(true)
                {
                    Console.WriteLine("Enter 837 file name (ex: test.837) within 837Form folder: ");
                    string filename = Console.ReadLine();
                    if(filename != "")
                        if(ReadFile(filename))
                            break;
                }
                ToXml();
                Console.WriteLine("Would you like to exit (Y/N)?");
                char respond = Console.ReadKey().KeyChar;
                if(respond == 'Y' || respond == 'y')
                    break;
                else
                    Console.WriteLine('\n');
            }
            Console.WriteLine("\nExit!");
            Thread.Sleep(2000);
        }

        //read 837 format file into List<List<string>> file837
        static bool ReadFile(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/" + form_837_folder_name + "/" + filename);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    file837.Add(new List<string>(line.Split(seperator)));
                }
            }
            catch (FileNotFoundException e)
            {
                return false;
            }
            return true;
        }

        //convert List<List<string>> file837 into basic xml format and write xml file in xml_folder
        static void ToXml()
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Form837");
            xml.AppendChild(root);
            foreach (List<string> ls in file837)
            {
                XmlElement child = xml.CreateElement(ls[0]);
                int count = 0;
                foreach (string s in ls)
                {
                    if (count != 0)
                    {
                        XmlElement grandchild = xml.CreateElement(ls[0] + "_" + count);
                        grandchild.InnerText = s;
                        child.AppendChild(grandchild);
                    }
                    count++;
                }
                root.AppendChild(child);
            }
            Console.WriteLine("Enter exported xml file name without extension: ");
            string filename = Console.ReadLine();
            filename += ".xml";
            File.WriteAllText(Directory.GetCurrentDirectory() + "/" + xml_folder + "/" + filename, XElement.Parse(xml.OuterXml).ToString());
            Console.WriteLine("Exported XML file " + filename + " at " + Directory.GetCurrentDirectory());
        }
    }
}
