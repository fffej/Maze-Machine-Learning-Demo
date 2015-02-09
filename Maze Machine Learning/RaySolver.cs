using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maze_Machine_Learning.ABUtil;
using Maze_Machine_Learning.Maze;

namespace Maze_Machine_Learning
{
    public class RaySolver
    {
        public static double CastRayInMaze(Line ray, PaintMaze maze)
        {
            var len = 0.0;
            var wall = new WallP((ray.Origin / maze.CellSize).Coord, Direction.None);

            var east = ray.Angle.InCardinal(Direction.East);
            var south = ray.Angle.InCardinal(Direction.South);

            var projectionAngle = new Vector(
                1.0 / Math.Cos(south ^ east ? ExtraMath.Eta - ray.Angle.Theta % ExtraMath.Eta : ray.Angle.Theta % ExtraMath.Eta),
                1.0 / Math.Cos(south ^ east ? ray.Angle.Theta % ExtraMath.Eta : ExtraMath.Eta - ray.Angle.Theta % ExtraMath.Eta));

            while (!(wall.D != Direction.None && maze[wall]) && len < ray.Angle.Magnitude)
            {
                var gridOffset = wall.C * maze.CellSize;
                var cellSpace = ray.Origin - gridOffset;
                gridOffset.Abs();
                cellSpace.Abs();

                var projection = new Vector(
                    (east ? cellSpace.X : gridOffset.X) * projectionAngle.X,
                    (south ? cellSpace.Y : gridOffset.Y) * projectionAngle.Y);

                len += Math.Min(projection.X, projection.Y);
                wall.D = projection.X < projection.Y ? (east ? Direction.East : Direction.West) : (south ? Direction.South : Direction.North);
                wall.C.X += projection.X < projection.Y ? (east ? 1 : -1) : 0;
                wall.C.Y += projection.X < projection.Y ? (south ? 1 : -1) : 0;
            }

            return len / ray.Angle.Magnitude;
        }

        public static void CastRayInMaze(ref Line ray, PaintMaze maze)
        {
            ray.Cast = CastRayInMaze(ray, maze);
        }

        public static void CastRayInMaze(ref PaintLine ray, PaintMaze maze)
        {
            ray.Cast = CastRayInMaze(ray, maze);
        }

        public static void MoveCircleInMaze(ref PhysicalCircle circle, PaintMaze maze)
        {
            var energy = 1.0;
            while (energy > 0)
            {
                var dist = circle.Radius - ExtraMath.DefaultMoE;
                var angle = circle.Velocity.Theta;
                var length = circle.Velocity.Magnitude*energy;
                var origin = circle.Position;

                var rays = Enumerable.Range(-1, 1).Select(i => new Line(origin + new Angle(angle + ExtraMath.Eta*i, dist), new Angle(angle, length - (1 - Math.Abs(i))*dist))).ToArray();

                foreach (var r in rays) r.Cast = CastRayInMaze(r, maze);

                if (ExtraMath.Compare(rays[0].Cast, 1.0) && ExtraMath.Compare(rays[1].Cast, 1.0) && ExtraMath.Compare(rays[2].Cast, 1.0))
                {
                    circle.Position += new Angle(circle.Velocity.Theta, length);
                    return;
                }

                rays = rays.OrderBy(x => x.Cast).ToArray();
                var colPoint = rays[0].CastPoint();
                Angle mirror;
                Vector offset;

                if (colPoint.X%maze.CellSize.X < colPoint.Y%maze.CellSize.Y)
                {
                    var east = rays[0].Angle.InCardinal(Direction.East);
                    offset = new Vector(east ? -circle.Radius : circle.Radius, 0);
                    mirror = east ? new Angle {Theta = ExtraMath.Tau*0.00} : new Angle {Theta = ExtraMath.Tau*0.50};
                }
                else
                {
                    var south = rays[0].Angle.InCardinal(Direction.South);
                    offset = new Vector(0, south ? -circle.Radius : circle.Radius);
                    mirror = south ? new Angle {Theta = ExtraMath.Tau*0.25} : new Angle {Theta = ExtraMath.Tau*0.75};
                }

                var solution = new Line(origin + new Angle(mirror.Theta, dist), new Angle(angle, length));
                solution.Cast = CastRayInMaze(solution, maze);
                circle.Position = solution.CastPoint() + offset;

                energy = energy - solution.Cast*energy;
            }
        }
    }
}
