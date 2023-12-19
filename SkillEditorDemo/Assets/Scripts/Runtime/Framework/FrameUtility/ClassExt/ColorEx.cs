using System;
using System.Globalization;
using Runtime.Framework.FrameUtility;
using UnityEngine;

namespace Module.Utility
 {
    public class ColorEx
    {
        public static readonly Color EntityNormalColor = SetHex(0x808080FFu); //RGBA          

        /// <summary>
        /// </summary>
        /// <param name="color"></param>
        /// <returns>0xRGBA</returns>
        public static uint GetHex(Color color)
        {
            return ((uint) (color.r * 255) << 24) |
                   ((uint) (color.g * 255) << 16) |
                   ((uint) (color.b * 255) << 8) |
                   (uint) (color.a * 255);
        }

        public static Color SetHex(uint rgba)
        {
            return new Color(
                (0xFF & (rgba >> 24)) / 255f,
                (0xFF & (rgba >> 16)) / 255f,
                (0xFF & (rgba >> 8)) / 255f,
                (0xFF & rgba) / 255f
            );
        }

        public static Color GetColorByHex(string hex)
        {
            uint rgba = 0;
            try
            {
                rgba = uint.Parse(hex, NumberStyles.HexNumber);
            }
            catch (Exception ex)
            {
                FrameworkLog.LogError($"[GetColorByHex]传入的参数不是合法的颜色16进制值,hex={hex}, msg={ex.Message}");
            }

            return SetHex(rgba);
        }
    }
}