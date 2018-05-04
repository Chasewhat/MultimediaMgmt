using System;
using System.Runtime.InteropServices;

namespace Common.Helper
{
    /// <summary>
    /// Struct结构体序列化帮助类
    /// </summary>
    public class StructHelper
    {
        /// <summary>
        /// 将指定数据对象封送到非托管内存块中,并将内存块中的数据读到byte[]数组中
        /// </summary>
        /// <param name="structObj">数据对象</param>
        /// <returns>byte[]数组</returns>
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                //将数据从托管对象封送到非托管内存块
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        /// <summary>
        /// 将byte[]数组拷贝到指定大小的非托管内存块中,并从非托管内存块中序列化解析指定类型的托管对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="bytes">byte数组</param>
        /// <returns>指定类型对象</returns>
        public static T BytesToStruct<T>(byte[] bytes)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                //将数据从非托管内存块封送到新分配的指定类型的托管对象
                return (T)Marshal.PtrToStructure(buffer, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
