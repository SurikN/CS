using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯabcdefghijklmnopqrstuvwxyzабвгґдеєжзиіїйклмнопрстуфхцчшщьюя0123456789 '\\\"!@#№$%^&*()-_=+[]{}/|,.<>?~\n";
            Dictionary<char, int> chars = new Dictionary<char, int>();
            Console.Write("Path: ");
            string str = @Console.ReadLine();

            StreamReader sr = File.OpenText(str);

            foreach (char c in alphabet)
            {
                chars.Add(c, 0);
            }

            double amountOfChars = CountChars(chars, sr);
            Dictionary<char, double> charFrequency;
            charFrequency = CountFrequency(chars, amountOfChars, alphabet);

            Console.WriteLine("Frequencies of symbol:");
            foreach (char c in charFrequency.Keys)
            {

                if (c != '\n')
                {
                    Console.Write(c);
                    Console.WriteLine(": " + charFrequency[c]);
                }
                else
                {
                    Console.WriteLine("Line breaks: " + charFrequency[c]);
                }

            }
            double entrophy = CountEntrophy(charFrequency, alphabet);
            Console.WriteLine("Enthropy: " + entrophy);
            Console.WriteLine("Info amount: " + entrophy * amountOfChars + " b (" + (entrophy * amountOfChars) / 8 + " B)");
            Console.WriteLine("File length: " + sr.BaseStream.Length + " B");

            Console.ReadKey();
        }
        static int CountChars(Dictionary<char, int> chars, StreamReader sr)
        {
            int counter = 0;
            string str;
            while (!sr.EndOfStream)
            {
                str = sr.ReadLine();
                foreach (char c in str)
                {
                    try
                    {
                        chars[c]++;
                    }
                    catch
                    {
                        chars.Add(c, 1);
                    }
                }
                counter += str.Length;
                chars['\n']++;
            }
            return counter;
        }
        static Dictionary<char, double> CountFrequency(Dictionary<char, int> chars, double amountOfChars, string alphabet)
        {
            Dictionary<char, double> frequency = new Dictionary<char, double>();
            foreach (char c in alphabet)
            {
                frequency.Add(c, chars[c] / amountOfChars);
            }

            return frequency;
        }
        static double CountEntrophy(Dictionary<char, double> chars, string alef)
        {
            double entrophy = 0.0;
            foreach (char c in alef)
            {
                if (chars[c] != 0)
                {
                    entrophy -= chars[c] * Math.Log(chars[c], 2);
                }
            }
            return entrophy;
        }
    }
}