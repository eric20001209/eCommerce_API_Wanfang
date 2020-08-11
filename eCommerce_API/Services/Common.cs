using eCommerce_API.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace eCommerce_API.Services
{
	public class Common
	{

        public static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        /// <summary>
        /// generate 0-z random string
        /// </summary>
        /// <param name="length">string length</param>
        /// <returns>Random String :)</returns>
        public static string GenerateRandomString(int length)
        {
            string checkCode = String.Empty;
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                checkCode += constant[rd.Next(36)].ToString();
            }
            return checkCode;
        }

    }
}
