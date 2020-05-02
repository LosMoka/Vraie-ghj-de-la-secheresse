﻿using System;
using System.IO;

namespace Utils
{
    public class std
    {
        
        public static bool getline(ref string iss, ref string str, char del)
        {
            string[] issSplit = iss.Split(del);
            str = issSplit[0];
            iss = "";
            if (issSplit.Length <= 1) return false;
            
            for (var i = 1; i < issSplit.Length-1; i++)
                iss += issSplit[i] + del;
            iss += issSplit[issSplit.Length - 1];
            return true;

        }

        public static void extractInt(ref string iss, ref int val)
        {
            string iss2 = iss, str2 = "", str3 = "";

            while(str2=="")
                getline(ref iss2, ref str2, '\n');
            while(str3=="")
                getline(ref str2, ref str3, ' ');

            val = Convert.ToInt32(str3);
            if (str2 != "")
                iss = str2 + '\n' + iss2;
            else
                iss = iss2;
        }

        public static void extractFloat(ref string iss, ref float val)
        {
            string iss2 = iss, str2 = "", str3 = "";

            while(str2=="")
                getline(ref iss2, ref str2, '\n');
            while(str3=="")
                getline(ref str2, ref str3, ' ');

            val = (float) Convert.ToDouble(str3);
            if (str2 != "")
                iss = str2 + '\n' + iss2;
            else
                iss = iss2;
        }
        
        public static Stream toStream(string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}