﻿using System;
using Tecnomatix.Engineering;

namespace RobworldToolboxCe.Common
{
    /// <summary>
    /// Provides static mathematical methods 
    /// </summary>
    internal static class RwMath
    {
        /// <summary>
        /// Converts an angle fom radians to degrees
        /// </summary>
        /// <param name="radians">Value in radians</param>
        /// <returns>The value in degrees</returns>
        internal static double Rad2Deg(double radians)
        {
            return (radians / Math.PI) * 180.0;
        }

        /// <summary>
        /// Converts an angle from degrees to radians
        /// </summary>
        /// <param name="degrees">Value in degrees</param>
        /// <returns>The value in radians</returns>
        internal static double Deg2Rad(double degrees)
        {
            return (degrees / 180.0) * Math.PI;
        }

        /// <summary>
        /// Rounds a double value to the nearest integral
        /// </summary>
        /// <param name="value">The value that has to be rounded</param>
        /// <returns></returns>
        internal static double Round(double value)
        {
            return Math.Round(value, 3);
        }

        /// <summary>
        /// Create a TxTransformation object in RPY representation
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        /// <param name="rx">The rx rotation</param>
        /// <param name="ry">The ry rotation</param>
        /// <param name="rz">The rz rotation</param>
        /// <param name="convertToRadians">Convert the rotation values to radians</param>
        /// <returns>The TxTransformation object</returns>
        internal static TxTransformation CreateRpyTransformation(double x, double y, double z, double rx, double ry, double rz, bool convertToRadians)
        {
            TxVector translation = CreateVector(x, y, z, false);
            TxVector rotation = CreateVector(rx, ry, rz, true);
            return new TxTransformation(translation, rotation, TxTransformation.TxRotationType.RPY_XYZ);
        }

        /// <summary>
        /// Create a TxVector object
        /// </summary>
        /// <param name="v1">The first value</param>
        /// <param name="v2">The second value</param>
        /// <param name="v3">The third value</param>
        /// <param name="convertToRadians">Convert the values to radians</param>
        /// <returns></returns>
        internal static TxVector CreateVector(double v1, double v2, double v3, bool convertToRadians)
        {
            TxVector vector;
            if(convertToRadians)
            {
                vector = new TxVector(Deg2Rad(v1), Deg2Rad(v2), Deg2Rad(v3));
            }
            else
            {
                vector = new TxVector(v1, v2, v3);
            }
            return vector;
        }
    }
}
