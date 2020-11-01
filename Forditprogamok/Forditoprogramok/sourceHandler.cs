using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Forditoprogramok
{
    class sourceHandler
    {
        private string path1, path2 = "";
        private string content = "";

        public string Path1
        {
            get { return this.path1; }
            set { this.path1 = value; }
        }

        public string Path2
        {
            get { return this.path2; }
            set { this.path2 = value; }
        }

        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        public sourceHandler(string path1, string path2)
        {
            this.path1 = path1;
            this.path2 = path2;
        }

        public void openFileToRead()
        {
            try
            {
                StreamReader SR = new StreamReader(File.OpenRead(this.path1));
                content = SR.ReadToEnd();

                SR.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void replaceText(string from, string to) {
            while (content.Contains(from)) {
                content = content.Replace(from, to);
            }
        }


        List<string> symbolTable = new List<string>();
        int symbolIndex = 0;

        string changeVariableAndConstants(string varAndConstName)
        {
            symbolTable.Add(varAndConstName);
            symbolIndex += 1;
            string result = "00" + symbolIndex.ToString();
            return result.Substring(result.Length - 3);
        }

        Dictionary<string, string> fromTo = new Dictionary<string, string>();

        public void replaceFirst()
        {
            fromTo.Add("  ", " ");
            fromTo.Add("\n", " ");
            fromTo.Add("    ", " ");
            fromTo.Add(" {", "{");
            fromTo.Add(" }", "}");
            fromTo.Add("} ", "}");
            fromTo.Add("{ ", "{");
            fromTo.Add(" (", "(");
            fromTo.Add("( ", "(");
            fromTo.Add(" )", ")");
            fromTo.Add(") ", ")");
            fromTo.Add(" ;", ";");
            fromTo.Add("; ", ";");
            fromTo.Add(" =", "=");
            fromTo.Add("= ", "=");
            fromTo.Add("IF", " 10 ");
            fromTo.Add("for", " 20 ");
            fromTo.Add("while", " 30 ");
            fromTo.Add("switch", " 31 ");
            fromTo.Add("case", " 32 ");
            fromTo.Add("else", " 34 ");
            fromTo.Add("(", " 40 ");
            fromTo.Add(")", " 50 ");
            fromTo.Add("==", " 60 ");
            fromTo.Add("=", " 61 ");
            fromTo.Add("{", " 70 ");
            fromTo.Add("}", " 80 ");
            fromTo.Add("+", " 81 ");
            fromTo.Add("++", " 82 ");
            fromTo.Add("-", " 83 ");
            fromTo.Add("--", " 84 ");
            fromTo.Add("-=", " 85 ");
            fromTo.Add("+=", " 86 ");
            fromTo.Add(">", " 87 ");
            fromTo.Add("<", " 88 ");
            fromTo.Add(">=", " 89 ");
            fromTo.Add("<=", " 90 ");

            foreach (KeyValuePair<string, string> kvp in fromTo)
            {
                replaceText(kvp.Key, kvp.Value);
            }
        }

        public void replaceContent()
        {
            string blockComment = @"/[*][\s\S]?[*]/";
            string lineComment = @"//[^\n\r][\n\r]";

            string fromNum = @"\d+";
            string fromVar = @"[A-Za-z-_]+";

            content = Regex.Replace(content, blockComment, "  ");
            content = Regex.Replace(content, lineComment, "  ");


            content = Regex.Replace(content, fromNum, changeVariableAndConstants("$1"));
            content = Regex.Replace(content, fromVar, changeVariableAndConstants("$1"));

            foreach (var x in fromTo)
            {
                while (content.Contains(x.Key))
                {
                    content = content.Replace(x.Key, x.Value);
                }
            }

        }
        public void openFileToWrite()
        {
            try
            {
                StreamWriter SW = new StreamWriter(File.Open(this.path2, FileMode.Create));
                SW.WriteLine(content);
                SW.Flush();
                SW.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
