using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_float
{
    class Program
    {
        static void Main(string[] args)
        {
            //BinaryNumber n1 = new BinaryNumber("11110");
            //BinaryNumber n2 = new BinaryNumber("111");
            //Console.WriteLine(n1.ToString() + " - " + n2.ToString() + " = " + (n1 - n2).ToString());
            Console.WriteLine("Enter multiplicand in binary form:");
            BinaryFloat32 multiplicand = new BinaryFloat32(Console.ReadLine());
            Console.WriteLine("Enter multiplier in binary form:");
            BinaryFloat32 multiplier = new BinaryFloat32(Console.ReadLine());
            Console.WriteLine("Result:\n" + (multiplicand * multiplier).ToString());
            Console.ReadLine();
        }
    }

    class BinaryNumber
    {
        protected bool[] number;

        public bool Contains(bool value)
        {
            return number.Contains(value);
        }

        public bool this[int n]
        {
            get { return number[n]; }
            set { number[n] = value; }
        }

        public BinaryNumber(string num)
        {
            number = new bool[num.Length];
            for (int i = 0; i < num.Length; i++)
            {
                switch (num[i])
                {
                    case '1':
                        number[num.Length - i - 1] = true;
                        break;
                    case '0':
                        number[num.Length - i - 1] = false;
                        break;
                    default:
                        throw new ApplicationException("Binary number can contain only '1' and '0'!");
                }
            }
        }

        public BinaryNumber(int length)
        {
            number = new bool[length];
        }

        public BinaryNumber() : this(32) { }

        public int Length
        {
            get { return number.Length; }
        }

        public static BinaryNumber operator *(BinaryNumber n1, BinaryNumber n2)
        {
            BinaryNumber result = new BinaryNumber(n1.Length + n2.Length);
            for (int i = 0; i < n1.Length; i++)
                result[i] = n1[i];
            bool carry = false;
            for (int i = 0; i < n1.Length; i++)
            {
                if (result[0])
                {
                    for (int k = 0; k < n2.Length; k++)
                    {
                        if (carry)
                        {
                            if (n2[k] && result[k + n1.Length])//1+1+carry1
                            {
                                result[k + n1.Length] = true;
                            }
                            else if (n2[k] || result[k + n1.Length])//1+0+carry1
                            {
                                result[k + n1.Length] = false;
                            }
                            else//0+0+carry1
                            {
                                result[k + n1.Length] = true;
                                carry = false;
                            }
                        }
                        else
                        {

                            if (n2[k] && result[k + n1.Length])//1+1+carry0
                            {
                                result[k + n1.Length] = false;
                                carry = true;
                            }
                            else if (n2[k] || result[k + n1.Length])//1+0+carry0
                            {
                                result[k + n1.Length] = true;
                            }
                            else//0+0+carry
                            {
                                result[k + n1.Length] = false;
                            }
                        }
                    }
                }
                result = Shift(result, -1);
            }
            return result;
        }

        public static BinaryNumber operator +(BinaryNumber n1, BinaryNumber n2) //110 1
        {
            BinaryNumber result = new BinaryNumber(Math.Max(n1.Length, n2.Length) + 1),//0000
                temp1 = new BinaryNumber(result.Length - 1),//110
                temp2 = new BinaryNumber(result.Length - 1);//001
            for (int i = 0; i < n1.Length; i++)
                temp1[i] = n1[i];
            for (int i = 0; i < n2.Length; i++)
                temp2[i] = n2[i];

            bool carry = false;
            for (int i = 0; i < result.Length - 1; i++)//0to3
            {
                if (carry)
                {
                    if (temp1[i] & temp2[i])
                        result[i] = true;
                    if (temp1[i] ^ temp2[i])
                    {
                        result[i] = false;
                    }
                    if (!temp1[i] & !temp2[i])
                    {
                        result[i] = true;
                        carry = false;
                    }
                }
                else//110 001
                {
                    if (temp1[i] & temp2[i])
                    {
                        result[i] = false;
                        carry = true;
                    }
                    if (temp1[i] ^ temp2[i])
                    {
                        result[i] = true;
                    }
                    if (!temp1[i] & !temp2[i])
                        result[i] = false;
                }
            }
            result[result.Length - 1] = carry;
            return result;
        }

        //help method for multiplying
        private static BinaryNumber Shift(BinaryNumber array, int shift)
        {
            BinaryNumber temp = new BinaryNumber(array.Length);
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

        public override string ToString()
        {
            string result = string.Empty;
            for (int i = 0; i < this.Length; i++)
            {
                if (this[this.Length - i - 1])
                    result += "1";
                else result += "0";
            }
            return result;
        }

        public static BinaryNumber DecToBin(int n)
        {
            BinaryNumber result;
            if (n == 0)
                result = new BinaryNumber("0");
            else
            {
                int size = 0;
                for (int i = 0; i < 32; i++)
                {
                    if (Math.Pow(2, i) > n)
                    {
                        size = i;
                        break;
                    }
                }
                if (size == 0)
                    throw new ApplicationException("Too big number!");
                result = new BinaryNumber(size);
                for (int i = result.Length - 1; i >= 0; i--)
                {
                    if (Math.Pow(2, i) <= n)
                    {
                        n -= ((int)Math.Pow(2, i));
                        result[i] = true;
                    }
                }
            }
            return result;
        }

        public static BinaryNumber Truncate(BinaryNumber num)
        {
            int temp = 0;
            for (int i = num.Length; i > 0; i--)
            {
                if (num[i - 1])
                {
                    temp = i;
                    break;
                }
            }
            BinaryNumber result = new BinaryNumber(temp);
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = num[i];
            }
            return result;
        }

        public static BinaryNumber Truncate(BinaryNumber num, int length)
        {
            BinaryNumber result = new BinaryNumber(length);
            for (int i = 0; i < Math.Min(result.Length, num.Length); i++)
                result[i] = num[i];
            return result;
        }

        public static BinaryNumber HalfInvert(BinaryNumber num)
        {
            BinaryNumber result = new BinaryNumber(num.Length);
            for (int i = 0; i < result.Length; i++)
                result[i] = !num[i];
            return result;
        }

        public static BinaryNumber Invert(BinaryNumber num, int size)
        {
            if (size < num.Length)
                throw new ApplicationException("Wrong size!");
            BinaryNumber result = new BinaryNumber(size);
            for (int i = 0; i < num.Length; i++)
                result[i] = num[i];
            BinaryNumber temp = HalfInvert(result);
            BinaryNumber temp1 = DecToBin(1);
            BinaryNumber temp2 = temp + temp1;
            int n = num.Length;
            return Truncate(temp2, size);
        }

        public static BinaryNumber operator -(BinaryNumber n1, BinaryNumber n2)
        {

            BinaryNumber temp = Invert(n2, n1.Length);
            BinaryNumber temp1 = n1 + temp;
            return Truncate(temp1, Math.Max(n1.Length, n2.Length));
        }
    }

    class BinaryFloat32 : BinaryNumber
    {
        public BinaryFloat32(string num)
        {
            if (num.Length != 32)
                throw new ApplicationException("Binary floating point number is 32 bits format!");
            number = new bool[num.Length];
            for (int i = 0; i < num.Length; i++)
            {
                switch (num[i])
                {
                    case '1':
                        number[i] = true;
                        break;
                    case '0':
                        number[i] = false;
                        break;
                    default:
                        throw new ApplicationException("Binary number can contain only '1' and '0'!");
                }
            }
        }

        public BinaryFloat32()
        {
            number = new bool[32];
        }

        //additional properties
        public bool IsInfinity
        {
            get
            {
                for (int i = 0; i < Exponent.Length; i++)
                    if (!Exponent[i])
                        return false;
                return true;
            }
        }

        public bool IsZero
        {
            get
            {
                for (int i = 0; i < Exponent.Length; i++)
                    if (Exponent[i])
                        return false;
                return true;
            }
        }

        //main properties
        public BinaryNumber Exponent
        {
            get
            {
                BinaryNumber result = new BinaryNumber(8);
                for (int i = 0; i < 8; i++)
                {
                    result[i] = number[8 - i];
                }
                return result;
            }

            private set
            {
                for (int i = 0; i < 8; i++)
                {
                    this[8 - i] = value[i];
                }
            }
        }

        public BinaryNumber Mantissa
        {
            get
            {
                BinaryNumber result = new BinaryNumber(23);
                for (int i = 0; i < 23; i++)
                {
                    result[i] = number[31 - i];
                }
                return result;
            }

            private set
            {
                for (int i = 0; i < 23; i++)
                {
                    this[31 - i] = value[i];
                }
            }
        }

        public bool Sign
        {
            get { return this[0]; }
            set { this[0] = value; }
        }

        public static BinaryFloat32 operator *(BinaryFloat32 n1, BinaryFloat32 n2)
        {
            BinaryFloat32 result = new BinaryFloat32();
            if (n1.IsZero || n2.IsZero)
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = false;
                return result;
            }
            if (n1.IsInfinity || n2.IsInfinity)
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = true;
                return result;
            }
            //setting sign
            result.Sign = n1.Sign ^ n2.Sign;

            //finding result mantissa
            //-creating mantissa with added hidden 1
            int needExp = 0;
            BinaryNumber temp_m1 = new BinaryNumber("1" + n1.Mantissa.ToString());
            BinaryNumber temp_m2 = new BinaryNumber("1" + n2.Mantissa.ToString());

            BinaryNumber temp_m = temp_m1 * temp_m2;
            if (temp_m[temp_m.Length - 1])
                needExp = 1;
            //11.0101010101010101010101010101010101010101010101
            BinaryNumber NewM = new BinaryNumber(23);
            for (int i = 45 + needExp; i > 45+needExp-23; i--)
            {
                NewM[i-23-needExp] = temp_m[i];
            }

            result.Mantissa = NewM;

            ///finding exponent
            BinaryNumber NewExp = Truncate(n1.Exponent + n2.Exponent + DecToBin(needExp) - DecToBin(127));
            if (NewExp.Length > 8 || !NewExp.Contains(false))
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = true;
            }
            else
                result.Exponent = Truncate(NewExp, 8);
            return result;
        }

        public override string ToString()
        {
            string sign;
            if (Sign)
                sign = "1";
            else
                sign = "0";
            return sign + " " + Exponent.ToString() + " " + Mantissa.ToString();
        }
    }
}
