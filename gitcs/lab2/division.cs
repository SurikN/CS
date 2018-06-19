using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_division
{
    class Program
    {
        static void Main(string[] args)
        {
            int a, b;
            Console.WriteLine("Enter divident:");
            a = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter divisor:");
            b = int.Parse(Console.ReadLine());
            string s = String.Empty;
            long rem;
            int q;
            RemShiftDivider.Devide(a, b, out s, out rem, out q);
            Console.WriteLine("{0}/{1} = {2}, remainder = {3} - my result",a,b,q,rem);
            Console.WriteLine("{0}/{1} = "+(a/b)+", remainder = "+(a%b)+" - compiller",a,b);
            Console.WriteLine(s) ;
            Console.ReadLine();
        }
    }

     public static class RemShiftDivider
    {
        public static /*(string def, long remainder, int quotient)*/ void Devide(int dividend, int divisor, out string definition, out long remainder, out int quotient)
        {
            int lenght;
            long r = (long)Math.Abs(dividend);
            string def = "divident: "+Convert.ToString(dividend, 2)+"\ndivisor: "+Convert.ToString(divisor, 2)+"\n";
            long b = ((long)Math.Abs(divisor)) << 32;
            int q = 0;
            def += "remainder = "+Convert.ToString(r, 2)+"\n";
            for (lenght = 0; dividend != 0; dividend >>= 1, lenght++) ;
            def += "Shift the remainder register to "+(32 - lenght - 1)+" bits left to optimize algorithm\n\n";
            r <<= (32 - lenght - 1);
            for (int i = 0; i < lenght + 1; i++)
            {

                r <<= 1;
                def += "shift left remainder to 1 bit "+Convert.ToString(r, 2)+"\n";
                def += "subtract  divisor from remainder:\n";
                def += "remainder: "+Convert.ToString(r, 2)+" divisor: "+Convert.ToString(divisor, 2)+"\n";
                r += -b;
                def += "operation result: (remainder) "+Convert.ToString(r, 2)+"\n";
                q <<= 1;
                if (r < 0)
                {
                    r += b;
                    def +="Restoring the remainder original value:\n"+Convert.ToString(r, 2)+"\nShift quotient register left to 1 bit: "+Convert.ToString(q, 2)+"\n\n";

                }
                else
                {
                    q++;
                    def += "Shift quotient register left to 1 bit and plus 1:  "+Convert.ToString(q, 2)+"\n\n";
                }

            }
            r >>= 32;
            if (dividend < 0)
            {
                r = -r;
                q = -q;
            }
            definition = def;
            remainder = r;
            quotient = q;
        }    
    }
}
