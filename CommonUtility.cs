using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bygbit.CommonUtility
{
    public static class CommonUtility
    {
        public static bool MultiStringMatch(List<string> stringToCheck, List<string> string_list)
        {
            if (stringToCheck == null || stringToCheck.Count <= 0) return false;
            if (string_list == null || string_list.Count <= 0) return false;

            foreach (var s in stringToCheck)
            {
                var n = string_list.Count(x => x.Equals(s, StringComparison.OrdinalIgnoreCase));
                if (n <= 0) return false;
            }
            return true;
        }
        public static bool MultiStringMatch(string stringToCheck, string string_list)
        {
            return MultiStringMatch(stringToCheck?.Split(',', ';').ToList(), string_list?.Split(',', ';').ToList());
        }

        public static bool MultiStringMatch(string stringToCheck, List<string> string_list)
        {
            return MultiStringMatch(stringToCheck?.Split(',', ';').ToList(), string_list);
        }

        public static List<string> SplitMultipleLinesString(string lines, bool RemoveEmpty)
        {
            if (string.IsNullOrEmpty(lines)) return new List<string>();
            StringSplitOptions opt = RemoveEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var ss = lines.Split(new string[] { "\r\n" },opt);
            if (ss.Length <=1) ss = lines.Split(new char[] { '\r', '\n' },opt);
            return ss.ToList();
        }

        public static bool MultipleWordsContain(string text, string words,bool AllWords)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(words)) return false;
            var wordList = SplitMultipleLinesString(words, true);
            if (wordList == null ||wordList.Count <= 0) return false;
            var count = wordList.Count(x => text.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
            if (AllWords) return count == wordList.Count;
            return count>=1;
        }

        public static int CleanOldFiles(string folder, int keepDays, bool quiet=true)
        {
            try
            {
                var recentTime = DateTime.Today.AddDays(-keepDays);
                var files = new DirectoryInfo(folder).GetFiles().Where(x=>x.LastWriteTime<recentTime).ToList();
                foreach (var f in files) File.Delete(f.FullName);
                return files.Count;
            }
            catch
            {
                if (!quiet) throw;
                return -1;
            }
        }

        /// <summary>
        /// search multiple string list separated by ' or ; and replace the old one with new one
        /// </summary>
        /// <param name="fullString"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <returns></returns>
        public static string ReplaceEmailString(string fullString, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(fullString) || string.IsNullOrEmpty(newStr)) return fullString;

            var EMPTYEMAIL = "EMPTYEMAIL";
            oldStr = oldStr.Trim().ToUpper();
            string[] list = fullString.Split(new char[] { ',', ';' },StringSplitOptions.RemoveEmptyEntries);
            if (newStr != "{DELETE}")
                list = list.Select(x => x.Trim().ToUpper() == oldStr ? newStr : x).ToArray();
            else
            {
                list = list.Where(x => x.Trim().ToUpper() != oldStr).ToArray();
            }

            if (list.Length <= 0) return EMPTYEMAIL;

            var delimeter = fullString.Contains(";") ? ";" : ",";
            var result = string.Join(delimeter, list);
            return result;
        }

    }
}