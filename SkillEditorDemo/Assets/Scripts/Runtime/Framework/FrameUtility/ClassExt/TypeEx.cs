using System;

namespace Module.Utility
{
    public static class TypeEx
    {
        /// <summary>
        ///     获取默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object DefaultForType(this Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        public static string DefaultValueStringForType(this Type targetType)
        {
            if (targetType.IsClass)
                return "null";
            return $"default({targetType.FullName})";
        }

        /// <summary>
        ///     获取类型的简化名称
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string GetSimpleTypeName(string typeName)
        {
            var result = typeName;
            if (typeName.Contains(".")) result = typeName.Substring(typeName.LastIndexOf(".", StringComparison.Ordinal) + 1);
            return result;
        }

        /// <summary>
        ///     获取类型名，可使用在代码中
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(this Type type)
        {
            var replace = type.FullName?.Replace('+', '.');
            return replace;
        }

        /// <summary>
        ///     判断是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }
    }
}