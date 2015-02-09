using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Machine_Learning.ABUtil
{
    class ExtraMath
    {
        /// <summary> Standard circle constant. </summary>
        public const double Pi = Math.PI;
        /// <summary> Full turn circle constant. </summary>
        public const double Tau = Math.PI * 2.0;
        /// <summary> The real circle constant. </summary>
        public const double Eta = Math.PI / 2.0;

        /// <summary> The default difference betwean doubles to consider them equal. </summary>
        public const double DefaultMoE = 1.0 / 3.0 - 0.33333333;

        public const int HashPrime = 8191;

        public static bool Compare(double a, double b, double epsilon)
        {
            if (Double.IsNaN(a) || Double.IsNaN(b)) return false;

            return Math.Abs(a - b) < epsilon;
        }

        public static bool Compare(double a, double b)
        {
            return Compare(a, b, DefaultMoE);
        }
    }
}
