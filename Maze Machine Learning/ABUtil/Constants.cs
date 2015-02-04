using System;
using System.Text.RegularExpressions;

namespace Maze_Machine_Learning.ABUtil
{
    namespace Constants
    {
        public struct Strings
        {
            public const string ArrayToSmall2 = "Array is too small, must be atleast two elements in size.";
        }

        public struct Circles
        {
            /// <summary> Standard circle constant. </summary>
            public const double Pi = Math.PI;
            /// <summary> Full turn circle constant. </summary>
            public const double Tau = Math.PI * 2.0;
            /// <summary> The real circle constant. </summary>
            public const double Eta = Math.PI / 2.0;

            /// <summary> One quater turn. </summary>
            public const double Q1 = Math.PI / 2.0;
            /// <summary> Two quater turn. </summary>
            public const double Q2 = Math.PI;
            /// <summary> Three quater turn. </summary>
            public const double Q3 = Math.PI * 1.5;
            /// <summary> Four quater turn. </summary>
            public const double Q4 = Math.PI * 2.0;
        }
    }
}