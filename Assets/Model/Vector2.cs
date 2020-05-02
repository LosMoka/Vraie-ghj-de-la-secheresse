﻿using System;

namespace Model
{
    public class Vector2<T>
    {
        protected Point2<T> m_origin;
        protected Point2<T> m_dest;

        public Vector2(Point2<T> dest)
        {
            m_origin = new Point2<T>( (T) Convert.ChangeType(0, typeof(T)),
                (T) Convert.ChangeType(0, typeof(T)));
            m_dest = new Point2<T>(dest.x,dest.y);
        }
        public Vector2(Point2<T> origin, Point2<T> dest)
        {
            m_origin = new Point2<T>(origin.x,origin.y);
            m_dest = new Point2<T>(dest.x,dest.y);
        }
        public Vector2(T x, T y)
        {
            m_origin = new Point2<T>((T) Convert.ChangeType(0, typeof(T)),
                (T) Convert.ChangeType(0, typeof(T)));
            m_dest = new Point2<T>(x,y);
        }

    }
    public class Vector2i : Vector2<int> {
        public Vector2i(Point2<int> dest) : base(dest)
        {
        }
        public Vector2i(Point2<int> origin, Point2<int> dest) : base(origin, dest)
        {
        }
        public Vector2i(int x, int y) : base(x,y)
        {
        }
        public int x()
        {
            int dx = m_dest.x, ox = m_origin.x;
            return dx-ox;
        }
        public int y()
        {
            int dy = m_dest.y, oy = m_origin.y;
            return dy - oy;
        }
        public int squareEuclidianDistance()
        {
            return x() * x() + y() * y();
        }
    }
    public class Vector2d : Vector2<double> {
        public Vector2d(Point2<double> dest) : base(dest)
        {
        }
        public Vector2d(double x, double y) : base(x,y)
        {
        }
        public double x()
        {
            double dx = m_dest.x, ox = m_origin.x;
            return dx-ox;
        }
        public double y()
        {
            double dy = m_dest.y, oy = m_origin.y;
            return dy - oy;
        }
    }
}