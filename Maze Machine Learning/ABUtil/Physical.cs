using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Machine_Learning.ABUtil
{
    public class Physical
    {
        public Line Inertia { get; set; }
        public Angle Acceleration { get; set; }
        public double Mass { get; set; }
        public double Area { get; set; }
        public const double AirDensity = 1.2041;
        public const double AirCoefficient = 0.47;
        public const double AirFriction = AirDensity*AirCoefficient;

        public Vector Position
        {
            get { return Inertia.Origin; }
            set { Inertia.Origin = value; }
        }

        public Angle Velocity
        {
            get { return Inertia.Angle; }
            set { Inertia.Angle = value; }
        }

        public Angle Force
        {
            get { return Mass*Acceleration; }
            set { Acceleration = value/Mass; }
        }

        public void Stop()
        {
            Velocity.Magnitude = 0;
            Acceleration.Magnitude = 0;
        }

        public void Accelerate(double time)
        {
            // Account for air friction. This is not accurate to real world air friction but it acts as a good trade off for complexity.
            Force += new Angle(-Velocity.Theta,
                Math.Max(Velocity.Magnitude, 0.5*AirFriction*Velocity.Magnitude*Velocity.Magnitude*Area));

            Velocity += Acceleration*time;
        }

        public Physical(Vector pos, double mass, double area)
        {
            Position = pos;
            Mass = mass;
            Area = area;
        }

        public Physical()
            :this(new Vector(), 1, 1)
        {}
    }
}
