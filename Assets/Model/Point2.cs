﻿using System;
using System.Text;

namespace Model
{
    public class Point2<T>
    {
        public T x, y;
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("P(");
            stringBuilder.Append(x);
            stringBuilder.Append(";");
            stringBuilder.Append(y);
            stringBuilder.Append(")");
            return stringBuilder.ToString(); 
        }

        public Point2(T x, T y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    public class Point2i : Point2<int> {
        public Point2i(int x, int y) : base(x, y)
        {
        }
        public Point2i(string str) : base(0,0)
        {
            str = str.Split('(')[1];
            string[] tab = str.Split(';');
            string sx = tab[0];
            string sy = tab[1].Split(')')[0];

            x = Int32.Parse(sx);
            y = Int32.Parse(sy);
        }
        
        public static Point2i operator+(Point2i point,Vector2i vec)
        {
            int px = point.x, py = point.y, vx = vec.x(), vy = vec.y();
            Point2i newPoint = new Point2i(px+vx,py+vy);
            return newPoint;
        }
        public static Point2i operator-(Point2i point1,Point2i point2)
        {
            int p1x = point1.x, p1y = point1.y, p2x = point2.x, p2y = point2.y;
            Point2i newPoint = new Point2i(p1x-p2x,p1y-p2y);
            return newPoint;
        }
        public static Point2i operator-(Point2i point,Vector2i vec)
        {
            int px = point.x, py = point.y, vx = vec.x(), vy = vec.y();
            Point2i newPoint = new Point2i(px-vx,py-vy);
            return newPoint;
        }
        public static Point2i operator+(Point2i point1,Point2i point2)
        {
            int p1x = point1.x, p1y = point1.y, p2x = point2.x, p2y = point2.y;
            Point2i newPoint = new Point2i(p1x+p2x,p1y+p2y);
            return newPoint;
        }
        public static Point2i operator/(Point2i point1,double f)
        {
            int p1x = (int) (point1.x/f), p1y = (int) (point1.y/f);
            Point2i newPoint = new Point2i(p1x,p1y);
            return newPoint;
        }

        public bool equals(Point2i obj)
        {
            return obj.x == x && obj.y == y;
        }
    }
    public class Point2d : Point2<double> {
        public Point2d(double x, double y) : base(x, y)
        {
        }
        public Point2d(string str) : base(0,0)
        {
            str = str.Split('(')[1];
            string[] tab = str.Split(';');
            string sx = tab[0];
            string sy = tab[1].Split(')')[0];

            x = Double.Parse(sx);
            y = Double.Parse(sy);
        }
    }
}