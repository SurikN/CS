using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace ConsoleApplication8
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] dict = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
                                     'P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d',
                                     'e','f','g','h','i','j','k','l','m','n','o','p','q','r','s',
                                     't','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
                                     '8','9','+','/','=' };
            Console.WriteLine("Enter the path to the file:");
            byte[] bytes = ReadFile(Console.ReadLine());
            List<char> result = new List<char>();
            Console.WriteLine("Enter the path for saving:");
            Encode(result, bytes, dict);
            using (StreamWriter sw = new StreamWriter(Console.ReadLine()))
            {
                sw.Write(new String(result.ToArray()));
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        static byte[] ReadFile(string path)
        {
            byte[] r;
            using (BinaryReader sr = new BinaryReader(File.Open(path, FileMode.Open), Encoding.UTF8))
            {
                r = sr.ReadBytes((int)sr.BaseStream.Length);
            }
            return r;

        }

        static void Encode(List<char> list, byte[] bytes, char[] chars)
        {
            int len3 = bytes.Length % 3;
            int i;
            int temp;
            for (i = 0; i < bytes.Length - len3; i += 3)
            {
                //0xfc = 0000 0000 1111 1100
                //0x03 = 0000 0000 0000 0011
                //0xf0 = 0000 0000 1111 0000
                //0x0f = 0000 0000 0000 1111
                //0xc0 = 0000 0000 1100 0000
                //                                                                              01011010 11001100 10110110
                list.Add(chars[(bytes[i] & 0xfc) >> 2]);                                      //010110
                list.Add(chars[((bytes[i] & 0x03) << 4) | ((bytes[i + 1] & 0xf0) >> 4)]);     //101100
                list.Add(chars[((bytes[i + 1] & 0x0f) << 2) | ((bytes[i + 2] & 0xc0) >> 6)]); //110010
                list.Add(chars[(bytes[i + 2] & 0x3f)]);                                       //110110               

            }
            i = bytes.Length - len3;
            switch (len3)
            {
                case 2:

                    list.Add(chars[(bytes[i] & 0xfc) >> 2]);
                    list.Add(chars[((bytes[i] & 0x03) << 4) | ((bytes[i + 1] & 0xf0) >> 4)]);
                    list.Add(chars[((bytes[i + 1] & 0x0f) << 2)]);
                    list.Add(chars[64]);
                    break;
                case 1:
                    list.Add(chars[(bytes[i] & 0xfc) >> 2]);
                    list.Add(chars[((bytes[i] & 0x03) << 4)]);
                    list.Add(chars[64]);
                    list.Add(chars[64]);
                    break;
            }
        }

    }
}