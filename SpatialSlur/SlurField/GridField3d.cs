﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using SpatialSlur.SlurCore;

using static SpatialSlur.SlurField.GridUtil;

/*
 * Notes
 */

namespace SpatialSlur.SlurField
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class GridField3d<T> : Grid3d, IField2d<T>, IField3d<T>, IDiscreteField<T>
        where T : struct
    {
        #region Static

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="path"></param>
        /// <param name="mapper"></param>
        public static void SaveAsImageStack(GridField3d<T> field, string path, Func<T, Color> mapper)
        {
            string dir = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);

            Parallel.For(0, field.CountZ, z =>
            {
                using (Bitmap bmp = new Bitmap(field.CountX, field.CountY, PixelFormat.Format32bppArgb))
                {
                    FieldIO.WriteToImage(field, z, bmp, mapper);
                    bmp.Save(String.Format(@"{0}\{1}_{2}{3}", dir, name, z, ext));
                }
            });
        }

        #endregion


        private readonly T[] _values;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <param name="nz"></param>
        protected GridField3d(Domain3d domain, int nx, int ny, int nz)
          : base(domain, nx, ny, nz)
        {
            _values = new T[Count];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <param name="nz"></param>
        /// <param name="sampleMode"></param>
        /// <param name="wrapMode"></param>
        protected GridField3d(Domain3d domain, int nx, int ny, int nz, SampleMode sampleMode, WrapMode wrapMode)
            : base(domain, nx, ny, nz, sampleMode, wrapMode)
        {
            _values = new T[Count];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <param name="nz"></param>
        /// <param name="sampleMode"></param>
        /// <param name="wrapModeX"></param>
        /// <param name="wrapModeY"></param>
        /// <param name="wrapModeZ"></param>
        protected GridField3d(Domain3d domain, int nx, int ny, int nz, SampleMode sampleMode, WrapMode wrapModeX, WrapMode wrapModeY, WrapMode wrapModeZ)
            : base(domain, nx, ny, nz, sampleMode, wrapModeX, wrapModeY, wrapModeZ)
        {
            _values = new T[Count];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected GridField3d(Grid3d other)
            : base(other)
        {
            _values = new T[Count];
        }


        /// <summary>
        /// 
        /// </summary>
        public T[] Values
        {
            get { return _values; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return _values[i];
        }


        /// <summary>
        /// Returns a deep copy of this field.
        /// </summary>
        /// <returns></returns>
        protected abstract GridField3d<T> DuplicateBase();


        /// <summary>
        /// Returns the value at the given indices.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public T ValueAt(int i, int j, int k)
        {
            return _values[IndexAt(i, j, k)];
        }


        /// <summary>
        /// Returns the value at the given indices.
        /// Assumes the indices are within the valid range.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public T ValueAtUnchecked(int i, int j, int k)
        {
            return _values[IndexAtUnchecked(i, j, k)];
        }


        /// <inheritdoc/>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public T ValueAt(Vec3d point)
        {
            switch (SampleMode)
            {
                case SampleMode.Nearest:
                    return ValueAtNearest(point);
                case SampleMode.Linear:
                    return ValueAtLinear(point);
            }

            throw new NotSupportedException();
        }


        /// <summary>
        /// Returns the value at the given point.
        /// Assumes the point is inside the field's domain.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public T ValueAtUnchecked(Vec3d point)
        {
            switch (SampleMode)
            {
                case SampleMode.Nearest:
                    return ValueAtNearestUnchecked(point);
                case SampleMode.Linear:
                    return ValueAtLinearUnchecked(point);
            }

            throw new NotSupportedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private T ValueAtNearest(Vec3d point)
        {
            return _values[IndexAt(point)];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private T ValueAtNearestUnchecked(Vec3d point)
        {
            return _values[IndexAtUnchecked(point)];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        protected abstract T ValueAtLinear(Vec3d point);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        protected abstract T ValueAtLinearUnchecked(Vec3d point);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract T ValueAt(GridPoint3d point);


        /// <summary>
        /// Sets all values along the boundary of the field to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundary(T value)
        {
            SetBoundaryXY(value);
            SetBoundaryXZ(value);
            SetBoundaryYZ(value);
        }


        /// <summary>
        /// Sets all values along the XY boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryXY(T value)
        {
            int offset = Count - CountXY;

            for (int i = 0; i < CountXY; i++)
                _values[i] = _values[i + offset] = value;
        }


        /// <summary>
        /// Sets all values along the XZ boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryXZ(T value)
        {
            int offset = CountXY - CountX;

            for (int k = 0; k < CountZ; k++)
            {
                int i0 = k * CountXY;
                int i1 = i0 + CountX;

                for (int i = i0; i < i1; i++)
                    _values[i] = _values[i + offset] = value;
            }
        }


        /// <summary>
        /// Sets all values along the YZ boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryYZ(T value)
        {
            int offset = CountX - 1;

            for (int i = 0; i < Count; i += CountX)
                _values[i] = _values[i + offset] = value;
        }


        /// <summary>
        /// Sets all values along the lower XY boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryXY0(T value)
        {
            for (int i = 0; i < CountXY; i++)
                _values[i] = value;
        }


        /// <summary>
        /// Sets all values along the upper XY boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryXY1(T value)
        {
            for (int i = Count - CountXY; i < Count; i++)
                _values[i] = value;
        }


        /// <summary>
        /// Sets all values along the lower YZ boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryXZ0(T value)
        {
            for (int k = 0; k < CountZ; k++)
            {
                int i0 = k * CountXY;
                int i1 = i0 + CountX;

                for (int i = i0; i < i1; i++)
                    _values[i] = value;
            }
        }


        /// <summary>
        /// Sets all values along the upper XZ boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryXZ1(T value)
        {
            int offset = CountXY - CountX;

            for (int k = 0; k < CountZ; k++)
            {
                int i0 = (k + 1) * CountXY - CountX;
                int i1 = i0 + CountX;

                for (int i = i0; i < i1; i++)
                    _values[i + offset] = value;
            }
        }


        /// <summary>
        /// Sets all values along the lower YZ boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryYZ0(T value)
        {
            for (int i = 0; i < Count; i += CountX)
                _values[i] = value;
        }


        /// <summary>
        /// Sets all values along the upper YZ boundary to a given constant.
        /// </summary>
        /// <param name="value"></param>
        public void SetBoundaryYZ1(T value)
        {
            for (int i = CountX - 1; i < Count; i += CountX)
                _values[i] = value;
        }


        /// <summary>
        /// Sets this field to some function of its coordinates.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parallel"></param>
        public void SpatialFunctionXYZ(Func<Vec3d, T> func, bool parallel = false)
        {
            Action<Tuple<int, int>> func2 = range =>
            {
                (int i, int j, int k) = IndicesAt(range.Item1);

                for (int index = range.Item1; index < range.Item2; index++, i++)
                {
                    if (i == CountX) { j++; i = 0; }
                    if (j == CountY) { k++; j = 0; }
                    _values[index] = func(CoordinateAt(i, j, k));
                }
            };

            if (parallel)
                Parallel.ForEach(Partitioner.Create(0, Count), func2);
            else
                func2(Tuple.Create(0, Count));
        }


        /// <summary>
        /// Sets this field to some function of its coordinates.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parallel"></param>
        public void SpatialFunctionXYZ(Func<double, double, double, T> func, bool parallel = false)
        {
            double x0 = Domain.X.T0;
            double y0 = Domain.Y.T0;
            double z0 = Domain.Z.T0;

            Action<Tuple<int, int>> func2 = range =>
            {
                (int i, int j, int k) = IndicesAt(range.Item1);

                for (int index = range.Item1; index < range.Item2; index++, i++)
                {
                    if (i == CountX) { j++; i = 0; }
                    if (j == CountY) { k++; j = 0; }
                    _values[index] = func(i * ScaleX + x0, j * ScaleY + y0, k * ScaleZ + z0);
                }
            };

            if (parallel)
                Parallel.ForEach(Partitioner.Create(0, Count), func2);
            else
                func2(Tuple.Create(0, Count));
        }


        /// <summary>
        /// Sets this field to some function of its normalized coordinates.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parallel"></param>
        public void SpatialFunctionUVW(Func<Vec3d, T> func, bool parallel = false)
        {
            double ti = 1.0 / (CountX - 1);
            double tj = 1.0 / (CountY - 1);
            double tk = 1.0 / (CountZ - 1);

            Action<Tuple<int, int>> func2 = range =>
            {
                (int i, int j, int k) = IndicesAt(range.Item1);

                for (int index = range.Item1; index < range.Item2; index++, i++)
                {
                    if (i == CountX) { j++; i = 0; }
                    if (j == CountY) { k++; j = 0; }

                    _values[index] = func(new Vec3d(i * ti, j * tj, k * tk));
                }
            };

            if (parallel)
                Parallel.ForEach(Partitioner.Create(0, Count), func2);
            else
                func2(Tuple.Create(0, Count));
        }


        /// <summary>
        /// Sets this field to some function of its normalized coordinates.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parallel"></param>
        public void SpatialFunctionUVW(Func<double, double, double, T> func, bool parallel = false)
        {
            double ti = 1.0 / (CountX - 1);
            double tj = 1.0 / (CountY - 1);
            double tk = 1.0 / (CountZ - 1);

            Action<Tuple<int, int>> func2 = range =>
            {
                (int i, int j, int k) = IndicesAt(range.Item1);

                for (int index = range.Item1; index < range.Item2; index++, i++)
                {
                    if (i == CountX) { j++; i = 0; }
                    if (j == CountY) { k++; j = 0; }

                    _values[index] = func(i * ti, j * tj, k * tk);
                }
            };

            if (parallel)
                Parallel.ForEach(Partitioner.Create(0, Count), func2);
            else
                func2(Tuple.Create(0, Count));
        }


        /// <summary>
        /// Sets this field to some function of its indices.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parallel"></param>
        public void SpatialFunctionIJK(Func<int, int, int, T> func, bool parallel = false)
        {
            Action<Tuple<int, int>> func2 = range =>
            {
                (int i, int j, int k) = IndicesAt(range.Item1);

                for (int index = range.Item1; index < range.Item2; index++, i++)
                {
                    if (i == CountX) { j++; i = 0; }
                    if (j == CountY) { k++; j = 0; }
                    _values[index] = func(i, j, k);
                }
            };

            if (parallel)
                Parallel.ForEach(Partitioner.Create(0, Count), func2);
            else
                func2(Tuple.Create(0, Count));
        }


        /// <summary>
        /// Sets this field to the values of another.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="parallel"></param>
        public void Sample(GridField3d<T> other, bool parallel = false)
        {
            if (ResolutionEquals(other))
            {
                _values.Set(other._values);
                return;
            }

            Sample((IField3d<T>)other, parallel);
        }


        /// <summary>
        /// Sets this field to the values of another.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="other"></param>
        /// <param name="converter"></param>
        /// <param name="parallel"></param>
        public void Sample<U>(GridField3d<U> other, Func<U, T> converter, bool parallel = false)
            where U : struct
        {
            if (ResolutionEquals(other))
            {
                other._values.Convert(converter, _values);
                return;
            }

            Sample((IField3d<U>)other, converter, parallel);
        }


        /// <summary>
        /// Sets this field to the values of another.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="parallel"></param>
        public void Sample(IField3d<T> other, bool parallel = false)
        {
            Action<Tuple<int, int>> func = range =>
            {
                (int i, int j, int k) = IndicesAt(range.Item1);

                for (int index = range.Item1; index < range.Item2; index++, i++)
                {
                    if (i == CountX) { j++; i = 0; }
                    if (j == CountY) { k++; j = 0; }
                    _values[index] = other.ValueAt(CoordinateAt(i, j, k));
                }
            };

            if (parallel)
                Parallel.ForEach(Partitioner.Create(0, Count), func);
            else
                func(Tuple.Create(0, Count));
        }


        /// <summary>
        /// Sets this field to the values of another.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="other"></param>
        /// <param name="converter"></param>
        /// <param name="parallel"></param>
        public void Sample<U>(IField3d<U> other, Func<U, T> converter, bool parallel = false)
        {
            Action<Tuple<int, int>> func = range =>
            {
                (int i, int j, int k) = IndicesAt(range.Item1);

                for (int index = range.Item1; index < range.Item2; index++, i++)
                {
                    if (i == CountX) { j++; i = 0; }
                    if (j == CountY) { k++; j = 0; }
                    _values[index] = converter(other.ValueAt(CoordinateAt(i, j, k)));
                }
            };

            if (parallel)
                Parallel.ForEach(Partitioner.Create(0, Count), func);
            else
                func(Tuple.Create(0, Count));
        }


        #region Explicit interface implementations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        T IField2d<T>.ValueAt(Vec2d point)
        {
            return ValueAt(new Vec3d(point.X, point.Y, 0.0));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDiscreteField<T> IDiscreteField<T>.Duplicate()
        {
            return DuplicateBase();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int IList<T>.IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
                if (item.Equals(_values[i])) return i;

            return -1;
        }


        /// <summary>
        /// 
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// 
        /// </summary>
        bool ICollection<T>.Contains(T item)
        {
            for (int i = 0; i < Count; i++)
                if (item.Equals(_values[i])) return true;

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// 
        /// </summary>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// 
        /// </summary>
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        void ICollection<T>.CopyTo(T[] array, int index)
        {
            array.SetRange(_values, index, 0, Count);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
