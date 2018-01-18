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
        static List<List<string>> file837 = new List<List<string>>(); 

        static void Main(string[] args)
        {
            while(true)
            {
                file837.Clear();
                while(true)
                {
                    Console.WriteLine("Enter file name with extension: ");
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

        static bool ReadFile(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/" + filename);
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
            File.WriteAllText(filename, XElement.Parse(xml.OuterXml).ToString());
            Console.WriteLine("Exported XML file " + filename + " at " + Directory.GetCurrentDirectory());
        }
    }
}
