// Gnos.Maths
using Gnos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

public class Maths
{
    private Dictionary<string, object> AV = new Dictionary<string, object>();

    public Maths()
    {
    }

    public Maths(Dictionary<string, object> g)
    {
        this.AV = g;
    }

    public string Boolit(string what)
    {
        string koup = what;
        if (!what.Equals("true", StringComparison.OrdinalIgnoreCase) && !what.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            string ra = what.Replace("||", "&&");
            try
            {
                string[] kata = (from f in ra.Split(new string[1]
                {
                    "&&"
                }, StringSplitOptions.RemoveEmptyEntries).Where(delegate (string g)
                {
                    if (!(g != ""))
                    {
                        return g.Trim() != "";
                    }
                    return true;
                })
                                 select f.Trim(')', ' ')).ToArray();
                koup = kata.Aggregate(koup, (string current, string t) => current.Replace(t, this.Fstb(t))).Replace("||", " || ").Replace("&&", " && ");
                string[] ling = koup.Split(Array.Empty<char>());
                string[] array = ling;
                foreach (string am in array)
                {
                    string kez2 = am.Trim();
                    if (kez2.EndsWith("\")", StringComparison.Ordinal))
                    {
                        kez2 = kez2.Substring(1, kez2.Length - 3);
                        koup = koup.Replace(am.Trim(), kez2);
                    }
                }
                return koup;
            }
            catch
            {
                return koup;
            }
        }
        return what;
    }

    public string Fstb(string wha)
    {
        string spliter;
        if (wha.Contains("=="))
        {
            spliter = "==";
            goto IL_0118;
        }
        if (wha.Contains("!="))
        {
            spliter = "!=";
            goto IL_0118;
        }
        if (wha.Contains("<"))
        {
            spliter = "<";
            goto IL_0118;
        }
        if (wha.Contains(">"))
        {
            spliter = ">";
            goto IL_0118;
        }
        if (wha.Contains("<="))
        {
            spliter = "<=";
            goto IL_0118;
        }
        if (wha.Contains(">="))
        {
            spliter = ">=";
            goto IL_0118;
        }
        if (wha.Contains("$contains$"))
        {
            spliter = "$contains$";
            goto IL_0118;
        }
        if (wha.Contains("$startswith$"))
        {
            spliter = "$startswith$";
            goto IL_0118;
        }
        if (wha.Contains("$endswith$"))
        {
            spliter = "$endswith$";
            goto IL_0118;
        }
        if (wha.Contains("$Contains$"))
        {
            spliter = "$Contains$";
            goto IL_0118;
        }
        if (wha.Contains("$StartsWith$"))
        {
            spliter = "$StartsWith$";
            goto IL_0118;
        }
        if (wha.Contains("$EndsWith$"))
        {
            spliter = "$EndsWith$";
            goto IL_0118;
        }
        return "0";
        IL_0118:
        string what = wha.Replace(string.Format(" {0} ", spliter), string.Format("{0}", spliter));
        string[] kama = (from d in what.Trim().Split(new string[1]
        {
            spliter
        }, StringSplitOptions.RemoveEmptyEntries)
                         select new SystemCore(this.AV).Strad(d.Trim(), "+", false)).ToArray();
        fr:
        return string.Join(spliter, kama);

    }

    public bool Ifit(string what)
    {
        string koup = what;
        if (!what.Equals("true", StringComparison.OrdinalIgnoreCase) && !what.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                koup = koup.Replace("||", " || ").Replace("&&", " && ");
                string[] ling = koup.Split(Array.Empty<char>());
                string[] array = ling;
                foreach (string t in array)
                {
                    string kez2 = t.Trim();
                    if (kez2.EndsWith("\")", StringComparison.Ordinal))
                    {
                        kez2 = kez2.Substring(1, kez2.Length - 3);
                        koup = koup.Replace(t.Trim(), kez2);
                    }
                }
            }
            catch (Exception)
            {
            }
            return this.Pifit(koup);
        }
        return bool.Parse(what);
    }

    private bool Pifit(string what)
    {
        string koup2 = what;
        try
        {
            string[] kata = (from f in what.Replace("||", "&&").Split(new string[1]
            {
                "&&"
            }, StringSplitOptions.RemoveEmptyEntries).Where(delegate (string g)
            {
                if (!(g != ""))
                {
                    return g.Trim() != "";
                }
                return true;
            })
                             select f.Trim(')', ' ')).ToArray();
            koup2 = kata.Aggregate(koup2, (string current, string t) => current.Replace(t, this.Stb(t))).Replace("||", " || ").Replace("&&", " && ");
            string[] ling = koup2.Split(Array.Empty<char>());
            string[] array = ling;
            foreach (string aat in array)
            {
                string kez2 = aat.Trim();
                if (kez2.EndsWith("\")", StringComparison.Ordinal))
                {
                    kez2 = kez2.Substring(1, kez2.Length - 3);
                    koup2 = koup2.Replace(aat.Trim(), kez2);
                }
            }
        }
        catch
        {
        }
        string text = koup2.Replace(" ", "").Replace("1", "").Replace("&", "");
        if (text != null && text.Length == 0)
        {
            return true;
        }
        if (text == "0")
        {
            return false;
        }
        if (!koup2.Contains("&&") && koup2.Contains("1"))
        {
            return true;
        }
        koup2 = string.Join(" ", koup2.Split(Array.Empty<char>()));
        return this.Gate(koup2);
    }

    private string Stb(string wha)
    {
        if (wha.Equals("True", StringComparison.OrdinalIgnoreCase))
        {
            return "1";
        }
        if (wha.Equals("False", StringComparison.OrdinalIgnoreCase))
        {
            return "0";
        }
        string spliter;
        if (wha.Contains("=="))
        {
            spliter = "==";
            goto IL_013e;
        }
        if (wha.Contains("!="))
        {
            spliter = "!=";
            goto IL_013e;
        }
        if (wha.Contains("<"))
        {
            spliter = "<";
            goto IL_013e;
        }
        if (wha.Contains(">"))
        {
            spliter = ">";
            goto IL_013e;
        }
        if (wha.Contains("<="))
        {
            spliter = "<=";
            goto IL_013e;
        }
        if (wha.Contains(">="))
        {
            spliter = ">=";
            goto IL_013e;
        }
        if (wha.Contains("$contains$"))
        {
            spliter = "$contains$";
            goto IL_013e;
        }
        if (wha.Contains("$startswith$"))
        {
            spliter = "$startswith$";
            goto IL_013e;
        }
        if (wha.Contains("$endswith$"))
        {
            spliter = "$endswith$";
            goto IL_013e;
        }
        if (wha.Contains("$Contains$"))
        {
            spliter = "$Contains$";
            goto IL_013e;
        }
        if (wha.Contains("$StartsWith$"))
        {
            spliter = "$StartsWith$";
            goto IL_013e;
        }
        if (wha.Contains("$EndsWith$"))
        {
            spliter = "$EndsWith$";
            goto IL_013e;
        }
        return "0";
        IL_013e:
        string what = wha.Replace(string.Format(" {0} ", spliter), string.Format("{0}", spliter));
        string[] kama = (from d in what.Trim().Split(new string[1]
        {
            spliter
        }, StringSplitOptions.RemoveEmptyEntries)
                         select new SystemCore(this.AV).Strad(d.Trim(), "+", false)).ToArray();
        if (spliter == "==")
        {
            if (kama[0].Equals(kama[1]))
            {
                return "1";
            }
            if (this.Dubexpress(kama[0].Trim()).Equals(this.Dubexpress(kama[1].Trim())))
            {
                return "1";
            }
            if (new SystemCore(this.AV).Strad(kama[0], "+", false).Equals(new SystemCore(this.AV).Strad(kama[1], "+", false)))
            {
                return "1";
            }
            if (this.Dubexpress(new SystemCore(this.AV).Numit(kama[0].Trim())) == this.Dubexpress(new SystemCore(this.AV).Numit(kama[1].Trim())))
            {
                return "1";
            }
            if (this.AV.ContainsKey(kama[0]) && this.AV.ContainsKey(kama[1]) && this.AV[kama[0]].Equals(this.AV[kama[1]]))
            {
                return "1";
            }
        }
        else if (spliter == "!=")
        {
            if (kama[0] != kama[1])
            {
                return "1";
            }
            if (this.Dubexpress(kama[0].Trim()) != this.Dubexpress(kama[1].Trim()))
            {
                return "1";
            }
            if (new SystemCore(this.AV).Strad(kama[0], "+", false) != new SystemCore(this.AV).Strad(kama[1], "+", false))
            {
                return "1";
            }
            if (this.Dubexpress(new SystemCore(this.AV).Numit(kama[0].Trim())) != this.Dubexpress(new SystemCore(this.AV).Numit(kama[1].Trim())))
            {
                return "1";
            }
            if (this.AV.ContainsKey(kama[0]) && this.AV.ContainsKey(kama[1]) && !this.AV[kama[0]].Equals(this.AV[kama[1]]))
            {
                return "1";
            }
        }
        else if (spliter == "<")
        {
            if (double.Parse(kama[0]) < double.Parse(kama[1]))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(kama[0].Trim())) < double.Parse(this.Dubexpress(kama[1].Trim())))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[0].Trim()))) < double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[1].Trim()))))
            {
                return "1";
            }
        }
        else if (spliter == ">")
        {
            if (double.Parse(kama[0]) > double.Parse(kama[1]))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(kama[0].Trim())) > double.Parse(this.Dubexpress(kama[1].Trim())))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[0].Trim()))) > double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[1].Trim()))))
            {
                return "1";
            }
        }
        else if (spliter == "<=")
        {
            if (double.Parse(kama[0]) <= double.Parse(kama[1]))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(kama[0].Trim())) <= double.Parse(this.Dubexpress(kama[1].Trim())))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[0].Trim()))) <= double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[1].Trim()))))
            {
                return "1";
            }
        }
        else if (spliter == ">=")
        {
            if (double.Parse(kama[0]) >= double.Parse(kama[1]))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(kama[0].Trim())) >= double.Parse(this.Dubexpress(kama[1].Trim())))
            {
                return "1";
            }
            if (double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[0].Trim()))) >= double.Parse(this.Dubexpress(new SystemCore(this.AV).Numit(kama[1].Trim()))))
            {
                return "1";
            }
        }
        else
        {
            if (spliter.Equals("$contains$", StringComparison.Ordinal) && kama[0].Contains(kama[1]))
            {
                return "1";
            }
            if (spliter.Equals("$startswith$", StringComparison.Ordinal) && kama[0].StartsWith(kama[1], StringComparison.Ordinal))
            {
                return "1";
            }
            if (spliter.Equals("$endswith$", StringComparison.Ordinal) && kama[0].EndsWith(kama[1], StringComparison.Ordinal))
            {
                return "1";
            }
        }
        try
        {
            string a2 = (!this.AV.ContainsKey(kama[0])) ? new SystemCore(this.AV).Strad(kama[0], "+", false) : (this.AV[kama[0]] as string);
            string b2 = (!this.AV.ContainsKey(kama[0])) ? new SystemCore(this.AV).Strad(kama[1], "+", false) : (this.AV[kama[1]] as string);
            if (a2 != null && spliter.Equals("$contains$", StringComparison.Ordinal))
            {
                string text = a2;
                string text2 = b2;
                if (text2 == null)
                {
                    throw new InvalidOperationException();
                }
                if (text.Contains(text2))
                {
                    return "1";
                }
            }
            if (a2 != null && spliter.Equals("$startswith$", StringComparison.Ordinal))
            {
                string text3 = a2;
                string text4 = b2;
                if (text4 == null)
                {
                    throw new InvalidOperationException();
                }
                if (text3.StartsWith(text4, StringComparison.Ordinal))
                {
                    return "1";
                }
            }
            if (a2 != null && spliter.Equals("$endswith$", StringComparison.Ordinal))
            {
                string text5 = a2;
                string text6 = b2;
                if (text6 == null)
                {
                    throw new InvalidOperationException();
                }
                if (text5.EndsWith(text6, StringComparison.Ordinal))
                {
                    return "1";
                }
            }
        }
        catch (Exception)
        {
        }
        try
        {
            string a = new SystemCore(this.AV).Strad(this.AV[kama[0]] as string, "+", false);
            string b = new SystemCore(this.AV).Strad(this.AV[kama[1]] as string, "+", false);
            if (spliter.Equals("$contains$", StringComparison.Ordinal) && a.Contains(b))
            {
                return "1";
            }
            if (spliter.Equals("$startswith$", StringComparison.Ordinal) && a.StartsWith(b, StringComparison.Ordinal))
            {
                return "1";
            }
            if (spliter.Equals("$endswith$", StringComparison.Ordinal) && a.EndsWith(b, StringComparison.Ordinal))
            {
                return "1";
            }
        }
        catch (Exception)
        {
        }
        return "0";
    }

    private bool Gate(string rawD)
    {
        string ra2 = rawD;
        try
        {
            string[] piu = (from g in ra2.Split('(', ')')
                            where g.Trim() != ""
                            select g).ToArray();
            ra2 = piu.Aggregate(ra2, (string current, string t) => current.Replace(string.Format("({0})", t), Maths.Aor(t).ToString()));
        }
        catch
        {
        }
        ra2 = ra2.Replace(" ", "").Replace("&&", " && ").Replace("||", " || ");
        while (ra2.Contains(" "))
        {
            string[] tam = ra2.Split(Array.Empty<char>());
            string chk = string.Format("{0} {1} {2}", tam[0], tam[1], tam[2]);
            ra2 = ra2.Replace(chk, Maths.Aor(chk).ToString()).Trim();
        }
        return ra2 == "1";
    }

    private static int Aor(string exp)
    {
        int a = 0;
        if (exp.Length == 1)
        {
            return int.Parse(exp);
        }
        string[] kami = (from g in exp.Replace(" ", "").Replace("&&", " && ").Replace("||", " || ")
            .Split(new string[1]
            {
                " "
            }, StringSplitOptions.RemoveEmptyEntries)
                         select g.Trim(' ', ')')).ToArray();
        if (kami[1] != "&&")
        {
            if (kami[1] != "||")
            {
                return a;
            }
            if (int.Parse(kami[0]) != 1 && int.Parse(kami[1]) != 1)
            {
                return 0;
            }
            return 1;
        }
        return int.Parse(kami[0]) * int.Parse(kami[2]);
    }

    public string Dubexpress(string sol)
    {
        string timb5 = this.purify(sol).Replace(" ", "");
        string[] g = timb5.Split('(', ')');
        for (int j = g.Length - 1; j >= 0; j--)
        {
            if (timb5.Contains(string.Format("({0})", g[j])))
            {
                timb5 = timb5.Replace(string.Format("({0})", g[j]), this.Dubexpress(g[j]));
            }
        }
        if (timb5.Contains("(") && timb5.Contains(")"))
        {
            timb5 = this.Dubexpress(timb5);
        }
        if (timb5.StartsWith("-", StringComparison.Ordinal))
        {
            char[] simba = timb5.ToCharArray();
            simba[0] = 'M';
            timb5 = new string(simba);
        }
        timb5 = timb5.Replace("*-", "*M");
        timb5 = timb5.Replace("--", "+");
        while (timb5.Contains("++"))
        {
            timb5 = timb5.Replace("++", "+");
        }
        while (timb5.Contains("**"))
        {
            timb5 = timb5.Replace("**", "*");
        }
        timb5 = timb5.Replace("+", " + ").Replace("-", " - ").Replace("*", " * ")
            .Replace("/", " / ")
            .Replace("^", " ^ ")
            .Replace("%", " % ");
        timb5 = timb5.Replace("M", "-");
        string[] tupid = timb5.Split(Array.Empty<char>());
        string dumper = tupid[0];
        for (int i = 1; i < tupid.Length; i += 2)
        {
            double ye = 0.0;
            switch (tupid[i])
            {
                case "+":
                    ye = double.Parse(dumper) + double.Parse(tupid[i + 1]);
                    break;
                case "-":
                    ye = double.Parse(dumper) - double.Parse(tupid[i + 1]);
                    break;
                case "*":
                    ye = double.Parse(dumper) * double.Parse(tupid[i + 1]);
                    break;
                case "/":
                    ye = double.Parse(dumper) / double.Parse(tupid[i + 1]);
                    break;
                case "^":
                    ye = Math.Pow(double.Parse(dumper), double.Parse(tupid[i + 1]));
                    break;
                case "%":
                    ye = double.Parse(dumper) % double.Parse(tupid[i + 1]);
                    break;
            }
            dumper = ye.ToString(CultureInfo.CurrentCulture);
        }
        return dumper;
    }

    public string Floatexpress(string sol)
    {
        string timb5 = this.purify(sol).Replace(" ", "");
        string[] g = timb5.Split('(', ')');
        for (int j = g.Length - 1; j >= 0; j--)
        {
            if (timb5.Contains(string.Format("({0})", g[j])))
            {
                timb5 = timb5.Replace(string.Format("({0})", g[j]), this.Floatexpress(g[j]));
            }
        }
        if (timb5.Contains("(") && timb5.Contains(")"))
        {
            timb5 = this.Floatexpress(timb5);
        }
        if (timb5.StartsWith("-", StringComparison.Ordinal))
        {
            char[] simba = timb5.ToCharArray();
            simba[0] = 'M';
            timb5 = new string(simba);
        }
        timb5 = timb5.Replace("*-", "*M");
        timb5 = timb5.Replace("--", "+");
        while (timb5.Contains("++"))
        {
            timb5 = timb5.Replace("++", "+");
        }
        while (timb5.Contains("**"))
        {
            timb5 = timb5.Replace("**", "*");
        }
        timb5 = timb5.Replace("+", " + ").Replace("-", " - ").Replace("*", " * ")
            .Replace("/", " / ")
            .Replace("^", " ^ ")
            .Replace("%", " % ");
        timb5 = timb5.Replace("M", "-");
        string[] tupid = timb5.Split(Array.Empty<char>());
        string dumper = tupid[0];
        for (int i = 1; i < tupid.Length; i += 2)
        {
            float ye = 0f;
            switch (tupid[i])
            {
                case "+":
                    ye = float.Parse(dumper) + float.Parse(tupid[i + 1]);
                    break;
                case "-":
                    ye = float.Parse(dumper) - float.Parse(tupid[i + 1]);
                    break;
                case "*":
                    ye = float.Parse(dumper) * float.Parse(tupid[i + 1]);
                    break;
                case "/":
                    ye = float.Parse(dumper) / float.Parse(tupid[i + 1]);
                    break;
                case "%":
                    ye = float.Parse(dumper) % float.Parse(tupid[i + 1]);
                    break;
            }
            dumper = ye.ToString(CultureInfo.CurrentCulture);
        }
        return dumper;
    }

    public string Longexpress(string sol)
    {
        string timb5 = this.purify(sol).Replace(" ", "");
        string[] g = timb5.Split('(', ')');
        for (int j = g.Length - 1; j >= 0; j--)
        {
            if (timb5.Contains(string.Format("({0})", g[j])))
            {
                timb5 = timb5.Replace(string.Format("({0})", g[j]), this.Longexpress(g[j]));
            }
        }
        if (timb5.Contains("(") && timb5.Contains(")"))
        {
            timb5 = this.Longexpress(timb5);
        }
        if (timb5.StartsWith("-", StringComparison.Ordinal))
        {
            char[] simba = timb5.ToCharArray();
            simba[0] = 'M';
            timb5 = new string(simba);
        }
        timb5 = timb5.Replace("*-", "*M");
        timb5 = timb5.Replace("--", "+");
        while (timb5.Contains("++"))
        {
            timb5 = timb5.Replace("++", "+");
        }
        while (timb5.Contains("**"))
        {
            timb5 = timb5.Replace("**", "*");
        }
        timb5 = timb5.Replace("+", " + ").Replace("-", " - ").Replace("*", " * ")
            .Replace("/", " / ")
            .Replace("^", " ^ ")
            .Replace("%", " % ");
        timb5 = timb5.Replace("M", "-");
        string[] tupid = timb5.Split(Array.Empty<char>());
        string dumper = tupid[0];
        for (int i = 1; i < tupid.Length; i += 2)
        {
            long ye = 0L;
            switch (tupid[i])
            {
                case "+":
                    ye = long.Parse(dumper) + long.Parse(tupid[i + 1]);
                    break;
                case "-":
                    ye = long.Parse(dumper) - long.Parse(tupid[i + 1]);
                    break;
                case "*":
                    ye = long.Parse(dumper) * long.Parse(tupid[i + 1]);
                    break;
                case "/":
                    ye = long.Parse(dumper) / long.Parse(tupid[i + 1]);
                    break;
                case "^":
                    ye = long.Parse(Math.Pow((double)long.Parse(dumper), (double)long.Parse(tupid[i + 1])).ToString(CultureInfo.CurrentCulture));
                    break;
                case "%":
                    ye = long.Parse(dumper) % long.Parse(tupid[i + 1]);
                    break;
            }
            dumper = ye.ToString();
        }
        return dumper;
    }

    public string Intexpress(string sol)
    {
        string timb5 = this.purify(sol).Replace(" ", "");
        string[] g = timb5.Split('(', ')');
        for (int j = g.Length - 1; j >= 0; j--)
        {
            if (timb5.Contains(string.Format("({0})", g[j])))
            {
                timb5 = timb5.Replace(string.Format("({0})", g[j]), this.Intexpress(g[j]));
            }
        }
        if (timb5.Contains("(") && timb5.Contains(")"))
        {
            timb5 = this.Intexpress(timb5);
        }
        if (timb5.StartsWith("-", StringComparison.Ordinal))
        {
            char[] simba = timb5.ToCharArray();
            simba[0] = 'M';
            timb5 = new string(simba);
        }
        timb5 = timb5.Replace("*-", "*M");
        timb5 = timb5.Replace("--", "+");
        while (timb5.Contains("++"))
        {
            timb5 = timb5.Replace("++", "+");
        }
        while (timb5.Contains("**"))
        {
            timb5 = timb5.Replace("**", "*");
        }
        timb5 = timb5.Replace("+", " + ").Replace("-", " - ").Replace("*", " * ")
            .Replace("/", " / ")
            .Replace("^", " ^ ")
            .Replace("%", " % ");
        timb5 = timb5.Replace("M", "-");
        string[] tupid = timb5.Split(Array.Empty<char>());
        string dumper = tupid[0];
        for (int i = 1; i < tupid.Length; i += 2)
        {
            BigInteger ye = 0;
            switch (tupid[i])
            {
                case "+":
                    ye = BigInteger.Parse(dumper) + BigInteger.Parse(tupid[i + 1]);
                    break;
                case "-":
                    ye = BigInteger.Parse(dumper) - BigInteger.Parse(tupid[i + 1]);
                    break;
                case "*":
                    ye = BigInteger.Parse(dumper) * BigInteger.Parse(tupid[i + 1]);
                    break;
                case "/":
                    ye = BigInteger.Parse(dumper) / BigInteger.Parse(tupid[i + 1]);
                    break;
                case "^":
                    ye = BigInteger.Pow(BigInteger.Parse(dumper), int.Parse(tupid[i + 1]));
                    break;
                case "%":
                    ye = BigInteger.Parse(dumper) % BigInteger.Parse(tupid[i + 1]);
                    break;
            }
            dumper = ye.ToString();
        }
        return dumper;
    }

    private string purify(string wuh)
    {
        char[] wa = wuh.ToCharArray();
        for (int i = 0; i < wa.Length; i++)
        {
            try
            {
                if (wa[i] == wa[i + 1])
                {
                    switch (wa[i])
                    {
                        case ',':
                            break;
                        case '+':
                        case '-':
                            wa[i] = '?';
                            wa[i + 1] = '+';
                            break;
                        case '*':
                            wa[i] = '?';
                            wa[i + 1] = '^';
                            break;
                    }
                }
                else
                {
                    if (wa[i] == '+' && wa[i + 1] == '-')
                    {
                        goto IL_0071;
                    }
                    if (wa[i] == '-' && wa[i + 1] == '+')
                    {
                        goto IL_0071;
                    }
                }
                goto end_IL_000c;
                IL_0071:
                wa[i] = '?';
                wa[i + 1] = '-';
                end_IL_000c:;
            }
            catch
            {
            }
        }
        return new string(wa).Replace("?", "");
    }
}
