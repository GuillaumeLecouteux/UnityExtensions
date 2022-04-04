using System;
using UnityEngine;

namespace JauntyBear.UnityExtensions
{
	/// <summary>
	/// Methods for additional math functions.
	/// </summary>
	public static class MathfExtensions
	{
		#region Constants

		public static readonly float Sqrt3 = Mathf.Sqrt(3);

        #endregion

        #region Static Methods

        //Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
        //Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
        //same plane, use ClosestPointsOnTwoLines() instead.
        //http://wiki.unity3d.com/index.php/3d_Math_functions
        public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {

            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parrallel
            if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }

        public static bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

            if (d == 0.0f)
            {
                return false;
            }

            var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
            var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

            if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
            {
                return false;
            }

            intersection.x = p1.x + u * (p2.x - p1.x);
            intersection.y = p1.y + u * (p2.y - p1.y);

            return true;
        }


        // https://forum.unity.com/threads/line-intersection.17384/
        public static bool SegmentSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 intersection)
        {
            float Ax, Bx, Cx, Ay, By, Cy, d, e, f, num/*,offset*/;
            float x1lo, x1hi, y1lo, y1hi;
            Ax = p2.x - p1.x;
            Bx = p3.x - p4.x;
            // X bound box test/
            if (Ax < 0)
            {
                x1lo = p2.x; x1hi = p1.x;
            }
            else
            {
                x1hi = p2.x; x1lo = p1.x;
            }

            if (Bx > 0)
            {
                if (x1hi < p4.x || p3.x < x1lo) return false;
            }
            else
            {
                if (x1hi < p3.x || p4.x < x1lo) return false;
            }
            Ay = p2.y - p1.y;
            By = p3.y - p4.y;
            // Y bound box test//
            if (Ay < 0)
            {
                y1lo = p2.y; y1hi = p1.y;
            }
            else
            {
                y1hi = p2.y; y1lo = p1.y;
            }
            if (By > 0)
            {
                if (y1hi < p4.y || p3.y < y1lo) return false;

            }
            else
            {
                if (y1hi < p3.y || p4.y < y1lo) return false;
            }
            Cx = p1.x - p3.x;
            Cy = p1.y - p3.y;
            d = By * Cx - Bx * Cy;  // alpha numerator//
            f = Ay * Bx - Ax * By;  // both denominator//
            // alpha tests//
            if (f > 0)
            {
                if (d < 0 || d > f) return false;
            }
            else
            {
                if (d > 0 || d < f) return false;
            }
            e = Ax * Cy - Ay * Cx;  // beta numerator//
            // beta tests //
            if (f > 0)
            {
                if (e < 0 || e > f) return false;
            }
            else
            {
                if (e > 0 || e < f) return false;
            }

            // check if they are parallel
            if (f == 0) return false;
            // compute intersection coordinates //
            num = d * Ax; // numerator //
            //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;   // round direction //

            //    intersection.x = p1.x + (num+offset) / f;
            intersection.x = p1.x + num / f;
            num = d * Ay;
            //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;
            //    intersection.y = p1.y + (num+offset) / f;
            intersection.y = p1.y + num / f;
            return true;
        }


        /// <summary>
        /// Extension method to check if a layer is in a layermask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}

		/// <summary>
		/// Linearly interpolates between two values between 0 and 1 if values wrap around from 1 back to 0.
		/// </summary>
		/// <remarks>This is useful, for example, in lerping between angles.</remarks>
		/// <example>
		/// <code>float angleInRad1 = 1;
		/// float angleInRad2 = 5;
		/// float revolution = Mathf.PI * 2;
		/// float interpolation = WLerp(angleInRad1 / revolution, angleInRad2 / revolution, 0.5f);
		/// 
		/// //interpolation == (5 + 1 + Mathf.PI * 2)/2 = 3 + Mathf.PI
		/// </code>
		/// </example>
		public static float Wlerp01(float v1, float v2, float t)
		{
			Debug.Assert(InRange(v1, 0, 1), "v1 is not in [0, 1)");
			Debug.Assert(InRange(v2, 0, 1), "v2 is not in [0, 1)");

			if (Mathf.Abs(v1 - v2) <= 0.5f)
			{
				return Mathf.Lerp(v1, v2, t);
			}
			else if (v1 <= v2)
			{
				return Frac(Mathf.Lerp(v1 + 1, v2, t));
			}
			else
			{
				return Frac(Mathf.Lerp(v1, v2 + 1, t));
			}
		}

		/// <summary>
		/// Tests whether the given value lies in the range [0, 1).
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns><c>true</c> if the given value is equal or greater than 0 and smaller than 1, <c>false</c> otherwise.</returns>
		public static bool InRange01(float value)
		{
			return InRange(value, 0, 1);
		}

		/// <summary>
		/// Tests whether the given value lies in the half-open interval specified by its endpoints, that is, whether the value
		/// lies in the interval <c>[closedLeft, openRight)</c>.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="closedLeft">The left end of the interval.</param>
		/// <param name="openRight">The right end of the interval.</param>
		/// <returns><c>true</c> if the given value is equal or greater than <c>closedLeft</c> and smaller than <c>openRight</c>, <c>false</c> otherwise.</returns>
		public static bool InRange(float value, float closedLeft, float openRight)
		{
			return value >= closedLeft && value < openRight;
		}

		/// <summary>
		/// Mod operator that also works for negative m.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <param name="n">The n.</param>
		/// <returns>System.Int32.</returns>
		public static int FloorMod(int m, int n)
		{
			if (m >= 0)
			{
				return m % n;
			}

			return (m - 2 * m * n) % n;
		}

		/// <summary>
		/// Mod operator that also works for negative m.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <param name="n">The n.</param>
		/// <returns>System.Int32.</returns>
		public static float FloorMod(float m, float n)
		{
			if (m >= 0)
			{
				return m % n;
			}

			return (m % n) + n;
		}

		/// <summary>
		/// Floor division that also work for negative m.
		/// </summary>
		/// <param name="m">The m.</param>
		/// <param name="n">The n.</param>
		/// <returns>System.Int32.</returns>
		public static int FloorDiv(int m, int n)
		{
			if (m >= 0)
			{
				return m / n;
			}

			int t = m / n;

			if (t * n == m)
			{
				return t;
			}

			return t - 1;
		}

		/// <summary>
		/// Returns the fractional part of a floating point number.
		/// </summary>
		/// <param name="x">The number to get the fractional part of.</param>
		/// <returns>The fractional part of the given number.</returns>
		/// <remarks>The result is always the number minus the number's floor.</remarks>
		public static float Frac(float x)
		{
			return x - Mathf.Floor(x);
		}

		/// <summary>
		/// Returns the sign function evaluated at the given value.
		/// </summary>
		/// <returns>1 if the given value is positive, -1 if it is negative, and 0 if it is 0.</returns>
		public static int Sign(float x)
		{
			if (x > 0) return 1;
			if (x < 0) return -1;

			return 0;
		}

		/// <summary>
		/// Returns the sign function evaluated at the given value.
		/// </summary>
		/// <returns>1 if the given value is positive, -1 if it is negative, and 0 if it is 0.</returns>
		public static int Sign(int p)
		{
			if (p > 0) return 1;
			if (p < 0) return -1;

			return 0;
		}
		#endregion

		#region Obsolete
		[Obsolete("Use FloorDiv instead")]
		public static int Div(int m, int n)
		{
			return FloorDiv(m, n);
		}

		[Obsolete("Use FloorMod instead")]
		public static int Mod(int m, int n)
		{
			return FloorMod(m, n);
		}

		[Obsolete("Use FloorMod instead")]
		public static float Mod(float m, float n)
		{
			return FloorMod(m, n);
		}

		/// <summary>
		/// Returns the highest integer equal to the given float.
		/// </summary>
		[Obsolete("Use Mathf.FloorToInt")]
		public static int FloorToInt(float x)
		{
			return Mathf.FloorToInt(x);
		}

		[Obsolete("Use Frac instead.")]
		public static float Wrap01(float value)
		{
			int n = Mathf.FloorToInt(value);
			float result = value - n;

			return result;
		}
		#endregion
	}
}
