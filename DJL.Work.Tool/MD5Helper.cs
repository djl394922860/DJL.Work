using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DJL.Work.Tool
{
    public static class MD5Helper
    {
        /// <summary>
        /// 获取MD5加密字符串
        /// </summary>
        /// <param name="str">被加密字符串</param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            //创建加密对象
            MD5 md5 = MD5.Create();
            //需要将加密的字符串转换为字节数组
            byte[] md5buffer = Encoding.Default.GetBytes(str);
            //加密后返回一个字节数组
            byte[] md5last = md5.ComputeHash(md5buffer);
            //加密后的字节数组遍历得到字符串
            string strnew = "";
            foreach (var item in md5last)
            {
                strnew += item.ToString("x2");
            }
            return strnew;
        }

        /// <summary>
        /// 获取文件MD5加密字符串
        /// </summary>
        /// <param name="stream">加密文件流</param>
        /// <returns></returns>
        public static string GetStreamMD5(Stream stream)
        {
            string strResult = string.Empty;
            string strHashData = string.Empty;
            byte[] arrbytHashValue;
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            arrbytHashValue = oMD5Hasher.ComputeHash(stream); //计算指定Stream 对象的哈希值
            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            strHashData = System.BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData;
            return strResult;
        }

        /// <summary>
        /// 获取加密Salt
        /// </summary>
        /// <returns></returns>
        public static string GetMD5Salt()
        {
            string salt = ConfigurationManager.AppSettings["PasswordSalt"];
            return salt;
        }
    }
}
