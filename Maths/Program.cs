using System;
using System.Numerics;
using System.Linq;
namespace Maths
{
    internal static class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine(ifit(Console.ReadLine()));
            }
        }
        //dubexpress for solving decimals
        //intexpress for solving integ

        public static bool ifit(string what)
        {
            //osi == osi || kami == qazi
            string ra = what;

            
            bool u = false;
               string[] kato = ra.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                string[] kata = ra.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in kato)
                {
                    ra = ra.Replace(item, stb(item));
                }
                foreach (var item in kata)
                {
                    ra = ra.Replace(item, stb(item));
                }
            ra = ra.Replace("||", " || ").Replace("&&", " && ").Replace("  ", " ");
            return (aor(ra) == 1) ? true : false;
        }

        static string stb(string wha)
        {
            //ra == ra
            string what = wha;
            string[] kama = what.Trim().Split();
            switch (kama[1].ToLower())
            {
                case "==":
                    try
                    {
                        if (kama[0].ToString().Contains(".") || kama[2].ToString().Contains("."))
                        {
                            if (Dubexpress(kama[0]) == Dubexpress(kama[2])) { return "1"; } else { return "0"; }
                        }
                        else { if (Intexpress(kama[0]) == Intexpress(kama[2])) { return "1"; } else { return "0"; } }
                    }
                    catch { }
                    if(kama[0] == kama[2]) { return "1"; } else { return "0"; }
                    break;
                case "!=":
                    try
                    {
                        if (kama[0].ToString().Contains(".") || kama[2].ToString().Contains("."))
                        {
                            if (Dubexpress(kama[0]) == Dubexpress(kama[2])) { return "0"; } else { return "1"; }
                        }
                        else { if (Intexpress(kama[0]) == Intexpress(kama[2])) { return "0"; } else { return "1"; } }
                            
                    }
                    catch { }
                    if (kama[0] == kama[2]) { return "0"; } else { return "1"; }
                    break;
                case "<":
                    if (kama[0].ToString().Contains(".") || kama[2].ToString().Contains("."))
                    {
                        if (double.Parse(Intexpress(kama[0])) < double.Parse(Intexpress(kama[2]))) { return "1"; } else { return "0"; }
                    }
                    else
                    {
                        if (BigInteger.Parse(Intexpress(kama[0])) < BigInteger.Parse(Intexpress(kama[2]))) { return "1"; } else { return "0"; }
                    }
                        
                    break;
                case ">":
                    if (kama[0].ToString().Contains(".") || kama[2].ToString().Contains("."))
                    {
                        if (double.Parse(Intexpress(kama[0])) < double.Parse(Intexpress(kama[2]))) { return "0"; } else { return "1"; }
                    }
                    else { if (BigInteger.Parse(Intexpress(kama[0])) < BigInteger.Parse(Intexpress(kama[2]))) { return "0"; } else { return "1"; } }
                        
                    break;
                case "contains":
                    if (kama[0].Contains(kama[2])) { return "1"; } else { return "0"; }
                    break;
                case "startswith":
                    if (kama[0].StartsWith(kama[2])) { return "1"; } else { return "0"; }
                    break;
                case "endswith":
                    if (kama[0].EndsWith(kama[2])) { return "1"; } else { return "0"; }
                    break;
            }
            return "0";

        }
        static bool gate(string rawD)
        {
            //use for if statement bruv
            //pattern:
            //0 - false
            //1 - true
            // || - or
            // && - and
            // 0 && 1 || 1 && 0
            //0 && !1 || (1 && 0) || 1
            // 0 && 1 || 1 && 0 || 1 && 0
            string ra = rawD;
            foreach (var s in ra.Split('(',')'))
            {
                ra = ra.Replace($"({s})", aor(s).ToString());
            }
            ra = ra.Replace(" ", "")
                   .Replace("&&", " && ")
                   .Replace("||", " || ");
            while(ra.Contains(" "))
            {
                string[] tam = ra.Split();
                string chk = $"{tam[0]} {tam[1]} {tam[2]}";
                ra = ra.Replace(chk, aor(chk).ToString()).Trim();
            }

            return (ra == "1") ? true : false;

        }
        static int aor(string exp)
        {
            int a = 0;
            //0 && 1
            //0 || 1
            if(exp.Length == 1) { return int.Parse(exp); }
            string[] kami = exp.Replace(" ","").Replace("&&"," && ").Replace("||", " || ").Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).Select(g => g = g.Trim()).ToArray();
            switch (kami[1])
            {
                case "&&":
                    a = int.Parse(kami[0]) * int.Parse(kami[2]);
                    break;
                case "||":
                    a = int.Parse(kami[0]) + int.Parse(kami[2]);
                    a = (a > 1) ? 1 : a;
                    break;
            }
            return a;
        }
        private static string Zurk(string shu)
        {
            
            string shun = shu;
            string[] shema = shun.Split('(', ')');
            foreach (var item in shema)
            {
                if (item.Replace(" ", "") != "")
                    try { shun = shun.Replace(item, Intexpress(item)); } catch { }
            }
            return shun;
        }
        private static string Zurka(string shu)
        {

            string shun = shu;
            string[] shema = shun.Split('(', ')');
            foreach (var item in shema)
            {
                if (item.Replace(" ", "") != "")
                    try { shun = shun.Replace(item, Dubexpress(item)); } catch { }
            }
            return shun;
        }
        public static string Dubexpress(string expression)
        {
            //45+4/2*(4*2)*2

            string shun = expression.Replace(" ", "");
            if (shun.Contains("(") && shun.Contains(")"))
            {
                shun = Zurka(shun);
            }
            //( (180) + (180) ) / 2
            //((90+90)+(90+90))/2
            if (shun.Contains("(("))
            {
                shun = shun.Replace("((", "( (").Replace("))", ") )");
                shun = Zurka(shun);
            }
            try
            {
                double.Parse(expression.Trim());
                return expression.Trim();
            }
            catch { }

            shun = shun.Replace("+", " + ")
                       .Replace("-", " - ")
                       .Replace("*", " * ")
                       .Replace("/", " / ")
                       .Replace("^", " ^ ");

            while (shun.Contains(" "))
            {
                //4 + 2
                string[] shama = shun.Split();
                double a = 0;
                double b = 0;
                try
                {
                    try
                    {
                        a = double.Parse(shama[0]);
                        b = double.Parse(shama[2]);
                    }
                    catch (Exception)
                    {
                        a = double.Parse(shama[1]);
                        b = double.Parse(shama[3]);
                    }
                }
                catch (Exception)
                {

                    try
                    {
                        shun = shun.Replace(shama[0], shama[0].Trim('(', ')'));
                        shun = shun.Replace(shama[2], shama[2].Trim('(', ')'));
                        a = double.Parse(shama[0].Trim('(', ')'));
                        b = double.Parse(shama[2].Trim('(', ')'));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            shun = shun.Replace(shama[1], shama[1].Trim('(', ')'));
                            shun = shun.Replace(shama[3], shama[3].Trim('(', ')'));
                            a = double.Parse(shama[1].Trim('(', ')'));
                            b = double.Parse(shama[3].Trim('(', ')'));
                        }
                        catch
                        {
                            shama = shun.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            a = double.Parse(shama[0]);
                            b = double.Parse(shama[2]);
                            shun = shun.Replace("  ", " ");
                        }
                    }

                }
                switch (shama[1])
                {
                    case "+":
                        shun = shun.Replace($"{a} {shama[1]} {b}", (a + b).ToString());
                        break;

                    case "-":
                        shun = shun.Replace($"{a} {shama[1]} {b}", (a - b).ToString());
                        break;

                    case "/":
                        shun = shun.Replace($"{a} {shama[1]} {b}", (a / b).ToString());
                        break;

                    case "*":
                        shun = shun.Replace($"{a} {shama[1]} {b}", (a * b).ToString());
                        break;

                    case "^":
                        shun = shun.Replace($"{a} {shama[1]} {b}", (System.Math.Pow(a, double.Parse(b.ToString()))).ToString());
                        break;
                    case "%":
                        shun = shun.Replace($"{a} {shama[1]} {b}", (a % b).ToString());
                        break;
                }
                shun = shun.Trim();

            }

            return shun;
        }
        public static string Intexpress(string expression)
        {
            //45+4/2*(4*2)*2

            string shun = expression.Replace(" ", "");
            if (shun.Contains("(") && shun.Contains(")"))
            {
                shun = Zurk(shun);
            }
            //( (180) + (180) ) / 2
            //((90+90)+(90+90))/2
            if (shun.Contains("(("))
            {
                shun = shun.Replace("((", "( (").Replace("))", ") )");
                shun = Zurk(shun);
            }
            try
            {
                BigInteger.Parse(expression.Trim());
                return expression.Trim();
            }
            catch { }

            shun = shun.Replace("+", " + ")
                       .Replace("-", " - ")
                       .Replace("*", " * ")
                       .Replace("/", " / ")
                       .Replace("^", " ^ ");

            while (shun.Contains(" "))
            {
                    //4 + 2
                    string[] shama = shun.Split();
                BigInteger a = 0;
                BigInteger b = 0;
                try
                {
                    try
                    {
                        a = BigInteger.Parse(shama[0]);
                        b = BigInteger.Parse(shama[2]);
                    }
                    catch (Exception)
                    {
                        a = BigInteger.Parse(shama[1]);
                        b = BigInteger.Parse(shama[3]);
                    }
                }
                catch (Exception)
                {
                 
                    try
                    {
                        shun = shun.Replace(shama[0], shama[0].Trim('(', ')'));
                        shun = shun.Replace(shama[2], shama[2].Trim('(', ')'));
                        a = BigInteger.Parse(shama[0].Trim('(',')'));
                        b = BigInteger.Parse(shama[2].Trim('(', ')'));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            shun = shun.Replace(shama[1], shama[1].Trim('(', ')'));
                            shun = shun.Replace(shama[3], shama[3].Trim('(', ')'));
                            a = BigInteger.Parse(shama[1].Trim('(', ')'));
                            b = BigInteger.Parse(shama[3].Trim('(', ')'));
                        }
                        catch 
                        {
                            shama = shun.Split(new string[] {" " },StringSplitOptions.RemoveEmptyEntries);
                            a = BigInteger.Parse(shama[0]);
                            b = BigInteger.Parse(shama[2]);
                            shun = shun.Replace("  ", " ");
                        }
                    }

                }
                    switch (shama[1])
                    {
                        case "+":
                            shun = shun.Replace($"{a} {shama[1]} {b}", (a + b).ToString());
                            break;

                        case "-":
                            shun = shun.Replace($"{a} {shama[1]} {b}", (a - b).ToString());
                            break;

                        case "/":
                            shun = shun.Replace($"{a} {shama[1]} {b}", (a / b).ToString());
                            break;

                        case "*":
                            shun = shun.Replace($"{a} {shama[1]} {b}", (a * b).ToString());
                            break;

                        case "^":
                            shun = shun.Replace($"{a} {shama[1]} {b}", (BigInteger.Pow(a, int.Parse(b.ToString()))).ToString());
                            break;
                    case "%":
                        shun = shun.Replace($"{a} {shama[1]} {b}", (a % b).ToString());
                        break;
                }
                    shun = shun.Trim();
               
            }

            return shun;
        }
    }
}