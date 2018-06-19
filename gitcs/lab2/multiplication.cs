using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lab2_multiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            bool carry = false;
            BitArray Mcand = new BitArray(32);
            BitArray Register = new BitArray(64);

            Console.WriteLine("Enter Multiplicator: ");
            DecToBit(int.Parse(Console.ReadLine()),Register);

            Console.WriteLine("Enter Multiplicand: ");
            DecToBit(int.Parse(Console.ReadLine()), Mcand);

            for (int i = 0; i < 32; i++)
            {
                if (Register[0])
                {
                    for (int k = 0; k < 32; k++)
                    {
                        if (carry)
                        {
                            if (Mcand[k] && Register[k + 32])//1+1+carry1
                            {
                                Register[k + 32] = true;
                            }
                            else if (Mcand[k] || Register[k + 32])//1+0+carry1
                            {
                                Register[k + 32] = false;
                            }
                            else//0+0+carry1
                            {
                                Register[k + 32] = true;
                                carry = false;
                            }
                        }
                        else
                        {

                            if (Mcand[k] && Register[k + 32])//1+1+carry0
                            {
                                Register[k + 32] = false;
                                carry = true;
                            }
                            else if (Mcand[k] || Register[k + 32])//1+0+carry0
                            {
                                Register[k + 32] = true;
                            }
                            else//0+0+carry
                            {
                                Register[k + 32] = false;
                            }
                        }
                    }
                }
                Console.WriteLine("Shifting: "+BitToString(Register));
                Register = Shift(Register, -1);
            }
            Console.WriteLine(BitToDec(Register));
        }

        static BitArray Shift(BitArray array, int shift)
        {
            BitArray temp = new BitArray(array.Length);
            if (shift > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    try
                    {
                        temp[i + shift] = array[i];
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    try
                    {
                        temp[i + shift] = array[i];
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
            return temp;
        }

        static string BitToString(BitArray array)
        {
            string result = String.Empty;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (i == 32)
                    result += "|";
                if (array[i])
                    result += "1";
                else
                    result += "0";
            }
            return result;
        }

        static int BitToDec(BitArray array)
        {
            double result = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                    result += Math.Pow(2, i);
            }
            return (int)(Math.Round(result));
        }

        static void DecToBit(int dec, BitArray array)
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (Math.Pow(2, i) <= dec)
                {
                    array[i] = true;
                    dec -= (int)Math.Pow(2, i);
                }
            }
        }
    }
}
