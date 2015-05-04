using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public static class Utilities
    {
        public static Boolean inBetween(double d, double l, double r)
        {
            return (d >= l && d <= r);
        }

        public static Boolean sameSign(double[] d)
        {
            Boolean b = true;
            if(d.Length == 1)
            {
                return true;
            }
            for(int i = 1; i < d.Length; i++)
            {
                if(Math.Sign(d[0]) != Math.Sign(d[i]))
                {
                    b = false;
                }
            }
            return b;
        }
    }
}
