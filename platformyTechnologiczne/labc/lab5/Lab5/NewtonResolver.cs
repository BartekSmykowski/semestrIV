using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class NewtonResolver
    {

        public static int licznik(int N, int K)
        {
            int result = 1;
            for(int i = N-K+1; i <= N; i++)
            {
                result *= i;
            }
            return result;
        }

        public static int mianownik(int K)
        {
            int result = 1;
            for(int i = 1; i <= K; i++)
            {
                result *= i;
            }
            return result;
        }

    }
}
