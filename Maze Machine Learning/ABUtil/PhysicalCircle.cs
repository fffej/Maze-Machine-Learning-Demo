using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Machine_Learning.ABUtil
{
    public class PhysicalCircle : Physical
    {
        public double Radius
        {
            get { return Math.Sqrt(Area/ExtraMath.Pi); }
            set { Area = ExtraMath.Pi*value*value; }
        }

        public PhysicalCircle(Vector pos, double mass, double radius)
            :base(pos, mass, 0)
        {
            Radius = radius;
        }

        public PhysicalCircle()
            :this(new Vector(), 0, 0)
        {
        }
    }
}
