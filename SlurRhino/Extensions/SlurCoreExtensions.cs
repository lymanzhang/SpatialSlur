﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpatialSlur.SlurCore;

using Rhino.Geometry;

/*
 * Notes
 */
 
namespace SpatialSlur.SlurRhino
{
    /// <summary>
    /// 
    /// </summary>
    public static class SlurCoreExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Point2d ToPoint2d(this Vec2d vector)
        {
            return new Point2d(vector.X, vector.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Point2f ToPoint2f(this Vec2d vector)
        {
            return new Point2f((float)vector.X, (float)vector.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2d ToVector2d(this Vec2d vector)
        {
            return new Vector2d(vector.X, vector.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vec2d ToVec2d(this Point2d point)
        {
            return new Vec2d(point.X, point.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vec2d ToVec2d(this Vector2d vector)
        {
            return new Vec2d(vector.X, vector.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vec2d ToVec2d(this Point2f point)
        {
            return new Vec2d(point.X, point.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vec2d ToVec2d(this Vector2f vector)
        {
            return new Vec2d(vector.X, vector.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Vec4d Apply(this Transform xform, Vec4d vector)
        {
            return new Vec4d(
             vector.X * xform.M00 + vector.Y * xform.M01 + vector.Z * xform.M02 + vector.W * xform.M03,
             vector.X * xform.M10 + vector.Y * xform.M11 + vector.Z * xform.M12 + vector.W * xform.M13,
             vector.X * xform.M20 + vector.Y * xform.M21 + vector.Z * xform.M22 + vector.W * xform.M23,
             vector.W
             );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xform"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vec3d Apply(this Transform xform, Vec3d vector)
        {
            return new Vec3d(
               vector.X * xform.M00 + vector.Y * xform.M01 + vector.Z * xform.M02,
               vector.X * xform.M10 + vector.Y * xform.M11 + vector.Z * xform.M12,
               vector.X * xform.M20 + vector.Y * xform.M21 + vector.Z * xform.M22
               );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xform"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vec3d ApplyPosition(this Transform xform, Vec3d vector)
        {
            return new Vec3d(
                vector.X * xform.M00 + vector.Y * xform.M01 + vector.Z * xform.M02 + xform.M03,
                vector.X * xform.M10 + vector.Y * xform.M11 + vector.Z * xform.M12 + xform.M13,
                vector.X * xform.M20 + vector.Y * xform.M21 + vector.Z * xform.M22 + xform.M23
                );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmorph"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vec3d MorphPoint(this SpaceMorph xmorph, Vec3d vector)
        {
            return xmorph.MorphPoint(vector.ToPoint3d()).ToVec3d();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Point3d ToPoint3d(this Vec3d vector)
        {
            return new Point3d(vector.X, vector.Y, vector.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Point3f ToPoint3f(this Vec3d vector)
        {
            return new Point3f((float)vector.X, (float)vector.Y, (float)vector.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3d ToVector3d(this Vec3d vector)
        {
            return new Vector3d(vector.X, vector.Y, vector.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3f ToVector3f(this Vec3d vector)
        {
            return new Vector3f((float)vector.X, (float)vector.Y, (float)vector.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vec3d ToVec3d(this Point3d point)
        {
            return new Vec3d(point.X, point.Y, point.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vec3d ToVec3d(this Vector3d vector)
        {
            return new Vec3d(vector.X, vector.Y, vector.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vec3d ToVec3d(this Point3f point)
        {
            return new Vec3d(point.X, point.Y, point.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vec3d ToVec3d(this Vector3f vector)
        {
            return new Vec3d(vector.X, vector.Y, vector.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static Domain ToDomain(this Interval interval)
        {
            return new Domain(interval.T0, interval.T1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static Interval ToInterval(this Domain domain)
        {
            return new Interval(domain.T0, domain.T1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static BoundingBox ToBoundingBox(this Domain2d domain)
        {
            Domain x = domain.X;
            Domain y = domain.Y;
            return new BoundingBox(x.T0, y.T0, 0.0, x.T1, y.T1, 0.0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bbox"></param>
        /// <returns></returns>
        public static Domain2d ToDomain2d(this BoundingBox bbox)
        {
            Vec3d p0 = bbox.Min.ToVec3d();
            Vec3d p1 = bbox.Max.ToVec3d();
            return new Domain2d(p0, p1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static BoundingBox ToBoundingBox(this Domain3d domain)
        {
            Domain x = domain.X;
            Domain y = domain.Y;
            Domain z = domain.Z;
            return new BoundingBox(x.T0, y.T0, z.T0, x.T1, y.T1, z.T1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bbox"></param>
        /// <returns></returns>
        public static Domain3d ToDomain3d(this BoundingBox bbox)
        {
            Vec3d p0 = bbox.Min.ToVec3d();
            Vec3d p1 = bbox.Max.ToVec3d();
            return new Domain3d(p0, p1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Domain3d ToDomain3d(this Line line)
        {
            return new Domain3d(line.From.ToVec3d(), line.To.ToVec3d());
        }
    }
}
