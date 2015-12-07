﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpatialSlur.SlurCore;

namespace SpatialSlur.SlurField
{
    public static class ImplicitSurfaces
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        public static void Gyroid(ScalarField3d field)
        {
            field.SpatialFunction(Gyroid);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        public static void Diamond(ScalarField3d field)
        {
           field.SpatialFunction(Diamond);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        public static void Neovius(ScalarField3d field)
        {
            field.SpatialFunction(Neovius);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        public static void IWP(ScalarField3d field)
        {
            field.SpatialFunction(IWP);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        public static void HybridPW(ScalarField3d field)
        {
            field.SpatialFunction(HybridPW);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double Gyroid(double x, double y, double z)
        {
            return Math.Sin(x) * Math.Cos(y) + Math.Sin(y) * Math.Cos(z) + Math.Sin(z) * Math.Cos(x);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double Diamond(double x, double y, double z)
        {
            double sx = Math.Sin(x);
            double sy = Math.Sin(y);
            double sz = Math.Sin(z);

            double cx = Math.Cos(x);
            double cy = Math.Cos(y);
            double cz = Math.Cos(z);

            return sx * sy * sz + sx * cy * cz + cx * sy * cz + cx * cy * sz;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double Neovius(double x, double y, double z)
        {
            double cx = Math.Cos(x);
            double cy = Math.Cos(y);
            double cz = Math.Cos(z);

            return 3 * (cx + cy + cz) + 4 * cx * cy * cz;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double IWP(double x, double y, double z)
        {
            double cx = Math.Cos(x);
            double cy = Math.Cos(y);
            double cz = Math.Cos(z);

            return cx * cy + cy * cz + cz * cx - cx * cy * cz + 0.25;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double HybridPW(double x, double y, double z)
        {
            double cx = Math.Cos(x);
            double cy = Math.Cos(y);
            double cz = Math.Cos(z);

            return 4 * (cx * cy + cy * cz + cz * cx) - 3 * cx * cy * cz + 1.4;
        }

    }
}
