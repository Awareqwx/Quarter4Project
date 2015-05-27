using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//A collision library with functions including map segments, circles, lines, and rectangles, as well as collisions between all of them.

namespace Quarter4Project
{
    public static class Collision
    {

        #region Structs

        /// <summary>
        /// Creates map segments
        /// </summary>
        /// <param name="mapSegment">Points to Vectors to Lines</param>
        public struct mapSegment
        {

            public mapSegment(Point a, Point b)
            {
                p1 = a;
                p2 = b;
            }
            public Point p1;
            public Point p2;

            public Vector2 getVector()
            {
                return new Vector2(p2.X - p1.X, p2.Y - p1.Y);
            }

            public Line2D getLine()
            {
                return new Line2D(new Vector2(p1.X, p1.Y), unitVector(new Vector2(p2.X - p1.X, p2.Y - p1.Y)));
            }

            public Rectangle collisionRect()
            {
                return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
            }

        }

        public struct Line2D
        {
            public Vector2 P;
            public Vector2 V;

            public Line2D(Vector2 p, Vector2 v)
            {
                P = p;
                V = v;
            }

            public float yInt()
            {
                return (-V.Y * P.X + V.X * P.Y) / V.X;
            }

            public float Slope()
            {
                return V.Y / V.X;
            }

        }

        public struct Circle
        {
            public Vector2 P;
            public double R;

            public Circle(Vector2 p, double r)
            {
                P = p;
                R = r;
            }
        }

        #endregion

        #region Vector Methods

        public static float magnitude(Vector2 v)
        {
            return (float)Math.Sqrt((v.X * v.X) + (v.Y * v.Y));
        }

        public static Vector2 unitVector(Vector2 v)
        {
            if (v != Vector2.Zero)
            {
                return new Vector2((v.X / (float)magnitude(v)), (v.Y / (float)magnitude(v)));
            }
            return Vector2.Zero;

        }

        public static Vector2 vectorNormal(Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        public static float dotProduct(Vector2 u, Vector2 v)
        {
            return (u.X * v.X) + (u.Y * v.Y);
        }

        public static Vector2 reflectionVector(Vector2 v, Vector2 a)
        {
            Vector2 n = vectorNormal(a);
            float co = -2 * (dotProduct(v, n) / (magnitude(n) * magnitude(n)));
            Vector2 r;
            r.X = v.X + co * n.X;
            r.Y = v.Y + co * n.Y;
            return r;
        }

        /// <summary>
        /// Returns the angle of a vector in radians
        /// </summary>
        public static double angle(Vector2 v)
        {
            double d = Math.Atan2(v.Y, v.X);
            if(d < 0)
            {
                d += Math.PI * 2;
            }
            return d;
        }

        #endregion

        #region Collision Methods

        public static bool CheckSegmentRectangleCollision(mapSegment m, Rectangle r)
        {

            Vector2[] points = new Vector2[] { new Vector2(r.Left, r.Top), new Vector2(r.Left, r.Bottom), new Vector2(r.Right, r.Bottom), new Vector2(r.Right, r.Top) };
            double[] vals = new double[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                vals[i] = (m.p2.Y - m.p1.Y) * points[i].X + (m.p1.X - m.p2.X) * points[i].Y + (m.p2.X * m.p1.Y - m.p1.X * m.p2.Y);
            }
            if(Utilities.sameSign(vals))
            {
                return false;
            }
            if ((m.p1.X < r.Left && m.p2.X < r.Left) || (m.p1.Y < r.Top && m.p2.Y < r.Top) || (m.p1.X > r.Right && m.p2.X > r.Right) || (m.p1.Y > r.Bottom && m.p2.Y > r.Bottom))
            {
                return false;
            }
            return true;
        }

        public static bool CheckPointRectangleCollision(Point p, Rectangle r)
        {
            return (p.X >= r.X && p.Y >= r.Y && p.X <= r.X + r.Width && p.Y <= r.Y + r.Height);
        }

        public static bool CheckCircleSegmentCollision(Circle C, mapSegment S)
        {
            Line2D L;
            L.P.X = S.p1.X;
            L.P.Y = S.p1.Y;
            L.V.X = S.p2.X - S.p1.X;
            L.V.Y = S.p2.Y - S.p1.Y;

            double OH = Math.Abs(((L.V.X * (C.P.Y - L.P.Y)) - (L.V.Y * (C.P.X - L.P.X))) / (Math.Sqrt(L.V.X * L.V.X + L.V.Y * L.V.Y)));

            if (OH <= C.R)
            {
                Vector2 CollisionPoint1;
                Vector2 CollisionPoint2;
                if (L.V.X != 0)
                {
                    double Dv = L.V.Y / L.V.X;
                    double E = (L.V.X * L.P.Y - L.V.Y * L.P.X) / L.V.X - C.P.Y;

                    double a = 1 + Dv * Dv;
                    double b = -2 * C.P.X + 2 * E * Dv;
                    double c = C.P.X * C.P.X + E * E - C.R * C.R;

                    CollisionPoint1.X = (float)((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
                    CollisionPoint2.X = (float)((-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
                    CollisionPoint1.Y = L.Slope() * CollisionPoint1.X + L.yInt();
                    CollisionPoint2.Y = L.Slope() * CollisionPoint2.X + L.yInt();

                    bool cond1 = (Math.Min(S.p1.X, S.p2.X) <= CollisionPoint1.X && CollisionPoint1.X <= Math.Max(S.p1.X, S.p2.X));
                    bool cond2 = (Math.Min(S.p1.Y, S.p2.Y) <= CollisionPoint1.Y && CollisionPoint1.Y <= Math.Max(S.p1.Y, S.p2.Y));
                    bool cond3 = (Math.Min(S.p1.X, S.p2.X) <= CollisionPoint2.X && CollisionPoint2.X <= Math.Max(S.p1.X, S.p2.X));
                    bool cond4 = (Math.Min(S.p1.Y, S.p2.Y) <= CollisionPoint2.Y && CollisionPoint2.Y <= Math.Max(S.p1.Y, S.p2.Y));

                    return (cond1 && cond2) || (cond3 && cond4);
                }
            }
            return false;
        }

        public static bool CheckSegmentSegmentCollision(mapSegment s1, mapSegment s2)
        {
            Line2D l1, l2;

            l1.P = new Vector2(s1.p1.X, s1.p1.Y);
            l2.P = new Vector2(s2.p1.X, s2.p1.Y);
            l1.V.X = s1.p2.X - s1.p1.X;
            l1.V.Y = s1.p2.Y - s1.p1.Y;
            l2.V.X = s2.p2.X - s2.p1.X;
            l2.V.Y = s2.p2.Y - s2.p1.Y;

            Vector2 collisionPoint;

            collisionPoint.X = (l2.yInt() - l1.yInt()) / (l1.Slope() - l2.Slope());
            collisionPoint.Y = l1.Slope() * collisionPoint.X + l1.yInt();

            bool cond1 = (Math.Min(s1.p1.X, s1.p2.X) <= collisionPoint.X && collisionPoint.X <= Math.Max(s1.p1.X, s1.p2.X));
            bool cond2 = (Math.Min(s2.p1.X, s2.p2.X) <= collisionPoint.X && collisionPoint.X <= Math.Max(s2.p1.X, s2.p2.X));
            bool cond3 = (Math.Min(s1.p1.Y, s1.p2.Y) <= collisionPoint.Y && collisionPoint.Y <= Math.Max(s1.p1.Y, s1.p2.Y));
            bool cond4 = (Math.Min(s2.p1.Y, s2.p2.Y) <= collisionPoint.Y && collisionPoint.Y <= Math.Max(s2.p1.Y, s2.p2.Y));


            return cond1 && cond2 && cond3 && cond4;
        }

        public static bool CheckCircleCircleCollision(Circle C1, Circle C2)
        {
            if (C1.R + C2.R >= magnitude(C2.P - C1.P))
            {
                return true;
            }
            return false;
        }

        public static bool CheckCircleRectangleCollision(Circle C, Rectangle R)
        {
            Point[] points = new Point[] { new Point(R.X, R.Y), new Point(R.X + R.Width, R.Y), new Point(R.X, R.Y + R.Height), new Point(R.X + R.Width, R.Y + R.Height) };
            mapSegment[] segs = new mapSegment[] { new mapSegment(points[0], points[2]), new mapSegment(points[2], points[3]), new mapSegment(points[3], points[1]), new mapSegment(points[1], points[0]) };
            Boolean loopBreak = false;
            for (int i = 0; i < segs.Length && !loopBreak; i++)
            {
                loopBreak = CheckCircleSegmentCollision(C, segs[i]);
            }
            return loopBreak;
        }

        #endregion

        #region Other Methods

        public static float getDistance(Vector2 a, Vector2 b)
        {
            float d;
            d = magnitude(a - b);
            return d;
        }

        public static float getDistance(Point a, Point b)
        {
            float d;
            d = magnitude(new Vector2(a.X - b.X, a.Y - b.Y));
            return d;
        }

        #endregion

        public static Point toPoint(Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }
    }
}
