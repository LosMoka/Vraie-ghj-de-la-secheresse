﻿using System;

namespace Model
{
    public class Vector2<T>
    {
        private Point2<T> m_origin, m_dest;

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

        public T x()
        {
            dynamic dx = m_dest.x, ox = m_origin.x;
            return dx-ox;
        }
        public T y()
        {
            dynamic dy = m_dest.y, oy = m_origin.y;
            return dy - oy;
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
    }
}