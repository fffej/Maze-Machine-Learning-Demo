using System;
using System.Drawing;
using System.Windows.Forms;
using Maze_Machine_Learning;

namespace ABUtil
{
    /// <summary>
    /// A vector, angle, and magnitude. A ray. A velocty. A line.
    /// </summary>
    /// <field name="O">Origin.</field>
    /// <field name="v">How far the ray is cast down it's magnitude.</field>
    /// <field name="θ">Angle.</field>
    /// <field name="r">Magnitude.</field>
    public struct Ray
    {
        public CoordD O;
        public double v;
        public double θ;
        public double r;

        public Ray(CoordD origin, double θ, double r)
        {
            O = origin;
            this.r = r;
            v = 1.0;

            this.θ = bound_angle(θ);
        }

        /// <summary>
        /// Ensures that a given angle is between 0 (inclusive) and τ (exclusive).
        /// </summary>
        /// <param name="θ">Angle.</param>
        /// <returns>The correct angle.</returns>
        public static double bound_angle(double θ)
        {
            while (θ < 0) θ += Consts.τ;
            while (θ >= Consts.τ) θ -= Consts.τ;
            return θ;
        }

        public double get_angle()
        {
            return θ;
        }

        public bool going_north()
        {
            return θ >= Consts.η2 && θ < Consts.η4;
        }

        public bool going_east()
        {
            return θ >= Consts.η3 || θ < Consts.η;
        }

        public bool going_south()
        {
            return θ < Consts.η2 && θ >= 0;
        }

        public bool going_west()
        {
            return θ < Consts.η3 && θ >= Consts.η;
        }

        public bool north_quad()
        {
            return θ >= Consts.η * 2.5 && θ < Consts.η * 3.5;
        }

        public bool east_quad()
        {
            return θ >= Consts.η * 3.5 || θ < Consts.η * 0.5;
        }

        public bool south_quad()
        {
            return θ >= Consts.η * 0.5 && θ < Consts.η * 1.5;
        }

        public bool west_quad()
        {
            return θ >= Consts.η * 1.5 && θ < Consts.η * 2.5;
        }

        public void set_angle(double θ)
        {
            this.θ = bound_angle(θ);
        }

        public CoordD end_point()
        {
            return O.by_angle(θ, r);
        }

        public CoordD cast_point()
        {
            return O.by_angle(θ, r * v);
        }

        /// <summary>
        /// Draws with the a default colour.
        /// </summary>
        /// <param name="e">Event data.</param>
        public void draw(PaintEventArgs e)
        {
            draw(e, new Pen(Color.FromArgb(255, 191, 255, 0)));
        }

        /// <summary>
        /// Draws with the alpha channel of a given pen modified by <c>v</c>.
        /// </summary>
        /// <param name="e">Event data.</param>
        /// <param name="p">The <c>Pen</c> to use.</param>
        public void draw(PaintEventArgs e, Pen p)
        {
            Pen p1 = new Pen(Color.FromArgb((int)((1.0 - Math.Max(0.0, v)) * 240) + 15, p.Color.R, p.Color.G, p.Color.B));
            CoordD end = cast_point();

            if (O.x >= e.ClipRectangle.X && O.x < e.ClipRectangle.Width &&
                O.y >= e.ClipRectangle.Y && O.y < e.ClipRectangle.Height)
                e.Graphics.DrawLine(p1,
                    new PointF((float)O.x, (float)O.y),
                    new PointF((float)end.x, (float)end.y));
        }

        /// <summary>
        /// Draws with a given pen and differed length.
        /// </summary>
        /// <param name="e">Event data.</param>
        /// <param name="p">The <c>Pen</c> to use.</param>
        /// <param name="val">Length.</param>
        public void draw(PaintEventArgs e, Pen p, double val)
        {
            CoordD end = O.by_angle(θ, val);

            if (O.x >= e.ClipRectangle.X && O.x < e.ClipRectangle.Width &&
                O.y >= e.ClipRectangle.Y && O.y < e.ClipRectangle.Height)
                e.Graphics.DrawLine(p,
                    new PointF((float)O.x, (float)O.y),
                    new PointF((float)end.x, (float)end.y));
        }
    }
}