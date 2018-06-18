using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Gnos
{
    internal class SystemCore
    {
        #region Core Control

        public SystemCore(string cod, string s, Dictionary<string, string> ra)
        {
            var code = cod;
            SetDefaultVariables();
            Methodz = ra;
            IntepreteBlockOfCode(code, s, false);
        }

        public SystemCore(Dictionary<string, object> di)
        {
            _allVariables = di;
            SetDefaultVariables();
        }

        public SystemCore()
        {
            Methodz = new Dictionary<string, string>();
            SetDefaultVariables();
        }
        

        private void SetDefaultVariables()
        {
            Set("endl", Environment.NewLine, "String");
            Set("null", "", "String");
            Set(";", "000000000000000000000000000000000000000000000000000000000000000000000000000000001", "String");
            Set("|", "000000000000000000000000000000000000000000000000000000000000000000000000000000010", "String");
            Set("'", "000000000000000000000000000000000000000000000000000000000000000000000000000000011", "String");
            Set("<<", "000000000000000000000000000000000000000000000000000000000000000000000000000000100", "String");
        }

        #endregion Core Control

        public void IntepreteBlockOfCode(string code, string splitby, bool old)
        {
            //print << "Hello " + "World" << " ya";
            //pause();
            try
            {
                var g = new Regex("\"(.*?)\"").Matches(code);
                for (var i = g.Count - 1; i >= 0; i--)
                {
                    code = code.Replace(g[i].Value, g[i].Value.Replace(";", _strings[";"]));
                    code = code.Replace(g[i].Value, g[i].Value.Replace("|", _strings["|"]));
                    code = code.Replace(g[i].Value, g[i].Value.Replace("'", _strings["'"]));
                    code = code.Replace(g[i].Value, g[i].Value.Replace("<<", _strings["<<"]));
                }
                var items = new Regex("else(.*){").Matches(code);
                for (var index = items.Count - 1; index >= 0; index--)
                {
                    var item = items[index];
                    code = code.Replace(item.Value, "else{");
                }
            }
            catch
            {
                // ignored
            }
            var yuh = code.Split(new[] { splitby }, StringSplitOptions.RemoveEmptyEntries).Select(g => g.Trim()).ToArray();
            for (var y = 0; y < yuh.Length; y++)
            {
                try
                {
                    if (y < yuh.Length)
                    {
                        try
                        {
                            IntepreteCode(yuh[y]);
                        }
                        catch
                        {
                            // ignored+
                        }
                    }
                    else break;
                }
                catch { break; }
            }
            if (old == false) { Environment.Exit(0); }
        }

        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void IntepreteCode(string it)
        {
           
            try
            {
                //print array info doesnt work
                var ite = it.Trim();

                #region Manipulating methods

                if (it.Trim('(', ')').Equals("cls", StringComparison.OrdinalIgnoreCase)) { Console.Clear();return; }
                else if (it.Trim('(', ')').Equals("exit", StringComparison.OrdinalIgnoreCase)) { Environment.Exit(0); }
                else if (ite.StartsWith("reverse", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 8).Trim(')');
                    var name = ite;
                    if (_strings.ContainsKey(name))
                    {
                        var dzz = ((string)_allVariables[name]).ToCharArray();
                        Array.Reverse(dzz);
                        Set(name, new string(dzz), "gen");
                        return;
                    }
                    else if (_stringsA.ContainsKey(name))
                    {
                        var dzz = _allVariables[name] as string[];
                        Array.Reverse(dzz ?? throw new InvalidOperationException());
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_intsA.ContainsKey(name))
                    {
                        var dzz = _allVariables[name] as BigInteger[];
                        Array.Reverse(dzz ?? throw new InvalidOperationException());
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_doublesA.ContainsKey(name))
                    {
                        var dzz = _allVariables[name] as double[];
                        Array.Reverse(dzz ?? throw new InvalidOperationException());
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_floatsA.ContainsKey(name))
                    {
                        var dzz = (_allVariables[name] as float[]);
                        Array.Reverse(dzz ?? throw new InvalidOperationException());
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_longsA.ContainsKey(name))
                    {
                        var dzz = (_allVariables[name] as long[]);
                        Array.Reverse(dzz ?? throw new InvalidOperationException());
                        Set(name, dzz, "gen");
                        return;
                    }
                }
                else if (ite.StartsWith("replace", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 8).Trim(')');
                    var kati = ite.Split(',');
                    var name = kati[0];
                    var a = Strad(kati[1], "+", false);
                    var b = Strad(kati[2], "+", false);
                    Set(name, ((string) _allVariables[name]).Replace(a, b), "gen");
                    return;
                }
                else if (ite.StartsWith("remove", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 7).Trim(')');
                    var kati = ite.Split(',');
                    var name = kati[0];
                    var a = int.Parse(new Maths().Intexpress(Numit(kati[1])).Trim());
                    var b = int.Parse(new Maths().Intexpress(Numit(kati[2])).Trim());
                    Set(name, ((string) _allVariables[name]).Remove(a, b), "gen");
                    return;
                }
                else if (ite.StartsWith("swap", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 5).Trim(')');

                    var kati = ite.Split(',');
                    var name = kati[0];
                    var a = int.Parse(new Maths().Intexpress(Numit(kati[1])).Trim());
                    var b = int.Parse(new Maths().Intexpress(Numit(kati[2])).Trim());
                    if (_strings.ContainsKey(name))
                    {
                        var dzz = ((string) _allVariables[name]).ToCharArray();
                        var aa = dzz[a];
                        var bb = dzz[b];
                        dzz[a] = bb;
                        dzz[b] = aa;
                        Set(name, new string(dzz), "gen");
                        return;
                    }
                    else if (_stringsA.ContainsKey(name))
                    {
                        var dzz = (string[])_allVariables[name];
                        if (dzz != null)
                        {
                            var aa = dzz[a];
                            var bb = dzz[b];
                            dzz[a] = bb;
                            dzz[b] = aa;
                        }
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_intsA.ContainsKey(name))
                    {
                        var dzz = _allVariables[name] as BigInteger[];
                        if (dzz != null)
                        {
                            var aa = dzz[a];
                            var bb = dzz[b];
                            dzz[a] = bb;
                            dzz[b] = aa;
                        }
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_doublesA.ContainsKey(name))
                    {
                        var dzz = _allVariables[name] as double[];
                        if (dzz != null)
                        {
                            var aa = dzz[a];
                            var bb = dzz[b];
                            dzz[a] = bb;
                            dzz[b] = aa;
                        }
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_floatsA.ContainsKey(name))
                    {
                        var dzz = _allVariables[name] as float[];
                        if (dzz != null)
                        {
                            var aa = dzz[a];
                            var bb = dzz[b];
                            dzz[a] = bb;
                            dzz[b] = aa;
                        }
                        Set(name, dzz, "gen");
                        return;
                    }
                    else if (_longsA.ContainsKey(name))
                    {
                        if (_allVariables[name] is long[] dzz)
                        {
                            var aa = dzz[a];
                            var bb = dzz[b];
                            dzz[a] = bb;
                            dzz[b] = aa;
                            Set(name, dzz, "gen");
                            return;
                        }
                        
                    }
                }
                else if (ite.StartsWith("split", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 6).Trim(')');

                    var kati = ite.Split(',');
                    var name = kati[0];
                    var arrNam = kati[1];
                    var splitBy = Strad(kati[2], "+", false);
                    var arr = _strings[name].Split(new[] { splitBy }, StringSplitOptions.RemoveEmptyEntries).Where(r => r.Trim(splitBy.ToCharArray()) != "").ToArray();
                    Set(arrNam, arr, "StringA");
                    return;
                }
                else if (ite.StartsWith("pause", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 6).Trim(')');
                    if (ite == "")
                    {
                        Console.Write(@"Press any key to continue . . . .");
                    }
                    else
                    {
                        IntepreteCode("print << " + ite);
                    }
                    Console.ReadKey(); Console.WriteLine();
                    return;
                }
                else if (ite.StartsWith("work_", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 6).Trim(')');
                    IntepreteCode(Strad(ite, "+", false));
                    return;
                }
                else if (ite.StartsWith("work", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 5).Trim(')');
                    IntepreteCode(ite);
                    return;
                }
                else if (ite.StartsWith("start", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 6).Trim(')');
                    Process.Start(Strad(string.Join(" ", ite).Trim(), "+", false));
                    return;
                }

                #endregion Manipulating methods

                #region IO

                #region Dir

                else if (ite.StartsWith("delete", StringComparison.OrdinalIgnoreCase))
                {
                    ite = Strad(ite.Remove(0, 7).Trim(')'), "+", false);

                    if (File.Exists(ite)) { File.Delete(ite); }
                    else if (Directory.Exists(ite)) { Directory.Delete(ite); }
                    return;
                }
                else if (ite.StartsWith("create", StringComparison.OrdinalIgnoreCase))
                {
                    ite = Strad(ite.Remove(0, 7).Trim(')'), "+", false);
                    Directory.CreateDirectory(ite);
                    return;
                }
                else if (ite.Trim('(', ')').Equals("dir", StringComparison.OrdinalIgnoreCase))
                {
                    var v = Directory.GetDirectories(Directory.GetCurrentDirectory());
                    foreach (var t in v)
                    {
                        Console.WriteLine(t);
                    }
                    return;
                }
                else if (ite.StartsWith("dir", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 4).Trim(')');
                    var kati = ite.Split(',').Select(g => Strad(g.Trim(), "+", false)).ToArray();
                    Set(kati[1].Trim(), Directory.GetDirectories(Strad(kati[0], "+", false)), "StringA");
                    return;
                }
                else if (ite.StartsWith("cd", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 3).Trim(')');
                    var kati = ite.Split(',').Select(g => Strad(g.Trim(), "+", false)).ToArray();
                    Directory.SetCurrentDirectory(Strad(kati[0], "+", false));
                    return;
                }

                #endregion Dir

                #region Files

                else if (ite.StartsWith("open", StringComparison.OrdinalIgnoreCase) || ite.StartsWith("read", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 5).Trim(')');
                    var kati = ite.Split(',').Select(g => Strad(g.Trim(), "+", false)).ToArray();
                    try { Set(kati[1], File.ReadAllText(kati[0]), "String"); }
                    catch { Set(kati[1], "", "String"); }
                    return;
                }
                else if (ite.StartsWith("write", StringComparison.OrdinalIgnoreCase))
                {
                    //write("osi.txt",keem)
                    ite = ite.Remove(0, 6).Trim(')');
                    var kati = ite.Split(',').Select(g => Strad(g.Trim(), "+", false)).ToArray();
                    File.WriteAllText(kati[0], kati[1]);
                    return;
                }
                else if (ite.StartsWith("readlines", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 10).Trim(')');
                    var kati = ite.Split(',').Select(g => Strad(g.Trim(), "+", false)).ToArray();

                    try
                    {
                        Set(kati[1].Trim(), File.ReadAllText(kati[0]).Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToArray(), "StringA");
                    }
                    catch
                    {
                        // ignored
                    }
                    return;
                }
                else if (ite.Trim('(', ')').Equals("getfiles", StringComparison.OrdinalIgnoreCase))
                {
                    var v = Directory.GetFiles(Directory.GetCurrentDirectory());
                    foreach (var t in v)
                    {
                        Console.WriteLine(t);
                    }
                    return;
                }
                else if (ite.StartsWith("getfiles", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Remove(0, 9).Trim(')');
                    var kati = ite.Split(',').Select(g => Strad(g.Trim(), "+", false)).ToArray();
                    Set(kati[1].Trim(), Directory.GetFiles(Strad(kati[0], "+", false)), "StringA");
                    return;
                }

                #endregion Files

                #endregion IO

                #region Input Output

                else if (ite.StartsWith("print", StringComparison.OrdinalIgnoreCase))
                {
                    var bits = ite.Substring(6).Trim(' ').Split(new[] { "<<" }, StringSplitOptions.RemoveEmptyEntries).Where(h => h != "").ToArray();

                    if (bits.Length > 1)
                    {
                        foreach (var t in bits)
                        {
                            Console.Write(Strad(t.Trim(), "+", false));
                        }
                    }
                    else
                    {
                        ite = ite.Substring(6).Trim(' ').Substring(2);
                        Console.Write(Strad(ite.Trim(), "+", false));
                    }
                    return;
                }
                else if (ite.ToLower().StartsWith("input", StringComparison.OrdinalIgnoreCase))
                {
                    var tz = ite.Substring(8).Trim();
                    if (!ite.Contains(">>")) { tz = ite.Trim().Substring(6).Trim(); }
                    if (_strings.ContainsKey(tz)) { Set(tz, Console.ReadLine(), "String"); }
                    else if (_ints.ContainsKey(tz)) { Set(tz, BigInteger.Parse(Console.ReadLine() ?? throw new InvalidOperationException()), "Int"); }
                    else if (_doubles.ContainsKey(tz)) { Set(tz, double.Parse(Console.ReadLine() ?? throw new InvalidOperationException()), "Double"); }
                    else if (_longs.ContainsKey(tz)) { Set(tz, long.Parse(Console.ReadLine() ?? throw new InvalidOperationException()), "Longs"); }
                    else if (_floats.ContainsKey(tz)) { Set(tz, float.Parse(Console.ReadLine() ?? throw new InvalidOperationException()), "Float"); }
                    else if (_chars.ContainsKey(tz)) { Set(tz, Console.ReadKey().KeyChar, "Char"); }
                    else
                    {
                        try
                        {
                            var tka = tz.Split('[', ']').Select(f => f.Trim()).ToArray();
                            var index = int.Parse(tka[1].Trim());
                            var namee = tka[0].Trim();
                            if (_stringsA.ContainsKey(namee)) { _stringsA[namee][index] = Console.ReadLine(); }
                            else if (_intsA.ContainsKey(namee)) { _intsA[namee][index] = BigInteger.Parse(Console.ReadLine() ?? throw new InvalidOperationException()); }
                            else if (_doublesA.ContainsKey(namee)) { _doublesA[namee][index] = double.Parse(Console.ReadLine() ?? throw new InvalidOperationException()); }
                            else if (_longsA.ContainsKey(namee)) { _longsA[namee][index] = long.Parse(Console.ReadLine() ?? throw new InvalidOperationException()); }
                            else if (_floatsA.ContainsKey(namee)) { _floatsA[namee][index] = float.Parse(Console.ReadLine() ?? throw new InvalidOperationException()); }
                            else if (_charsA.ContainsKey(namee)) { var g = Console.ReadKey(); _charsA[namee][index] = g.KeyChar; }
                        }
                        catch { Set(tz, Console.ReadLine(), "String"); }
                    }
                    return;
                }

                #endregion Input Output

                #region conditional statements

                else if (ite.ToLower().StartsWith("if", StringComparison.OrdinalIgnoreCase))
                {
                    var wee = ite.Substring(2).Trim().Split(new[] { "{", "}" }, StringSplitOptions.RemoveEmptyEntries).Where(f => f.Trim() != "").ToArray();
                    var tsi = new Maths(_allVariables).Boolit(wee[0].Trim('(', ')'));
                    wee[0] = "";
                    wee = wee.Select(g => g.Trim()).Where(h => h != "").ToArray();
                    var code = string.Join("", wee).Trim('{', '}');
                    if (ite.Replace(" ", "").Contains("else{"))
                    {
                        var tee = ite.Split(new[] {"else{"}, StringSplitOptions.RemoveEmptyEntries)[1].Trim('}').Trim();
                        code = ite.Trim().Replace(tee, "").Trim("else{".ToCharArray()).Replace("else{}", "").Trim();
                        IntepreteBlockOfCode(new Maths(_allVariables).Ifit(tsi.Trim()) ? code : tee, "|", true);
                    }
                    else
                    {
                        if (new Maths(_allVariables).Ifit(tsi.Trim())) { IntepreteBlockOfCode(code, "|", true); }
                    }
                    return;
                }
                else if (ite.ToLower().StartsWith("switch", StringComparison.CurrentCulture))
                {
                    try
                    {
                        var firn = ite.ToLower().Trim().Substring(6);
                        var chize = firn.Split(')')[0].Trim('(').Trim();
                        // switch(raji) this is raji
                        chize = Strad(chize, "+", false).Trim();
                        firn = firn.Substring($"({chize})".Length).Trim('{', '}');

                        var cases = new List<string>();

                        foreach (var item in firn.Split(new[] { "break::" }, StringSplitOptions.RemoveEmptyEntries)) { try { cases.Add(item); }
                            catch
                            {
                                // ignored
                            }
                        }
                        for (var i = 0; i < cases.Count; i++)
                        {
                            try
                            {
                                if (!cases[i].Trim().StartsWith("::", StringComparison.Ordinal)) continue;
                                cases[i - 1] += cases[i]; cases[i] = "";
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        cases = cases.Where(g => g.Trim() != "").ToList();
                        var casesAnDcodes = new Dictionary<string, string>();
                        foreach (var t1 in cases)
                        {
                            try
                            {
                                var spta = t1.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                var checkers = spta[0].Trim();//'1'
                                var tik = checkers.Split().Where(g => g.Trim() != "").Select(d => d.Trim()).ToArray();
                                tik[0] = "";
                                tik = tik.Where(t => t != "").ToArray();
                                checkers = string.Join(" ", tik).Trim();
                                checkers = Strad(checkers, "+", false).Trim();

                                if (checkers.StartsWith("case", StringComparison.OrdinalIgnoreCase))
                                {
                                    checkers = checkers.Trim().Remove(0, 4).Trim();
                                    checkers = Strad(checkers, "+", false).Trim();
                                }

                                spta[0] = "";
                                var code = string.Join("", spta);
                                casesAnDcodes.Add(checkers, code.Trim());
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        foreach (var iteq in casesAnDcodes.Keys)
                        {
                            var item = iteq;
                            if (item.StartsWith("case", StringComparison.OrdinalIgnoreCase)) { item = Strad(item.Remove(0, 4).Trim(), "+", false); }

                            if (item.Trim() == "") continue;
                            try
                            {
                                if (item.Trim() == chize || item.ToLower() == "default")
                                {
                                    var lunni = casesAnDcodes[item].Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Select(h => h.Trim()).Where(na => na.Trim() != "").ToArray();
                                    foreach (var t in lunni)
                                    {
                                        try { IntepreteCode(t); }
                                        catch
                                        {
                                            // ignored
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                    return;
                }
                #endregion conditional statements

                #region Method Control

                else if (ite.StartsWith("let", StringComparison.OrdinalIgnoreCase))
                {
                    /*let g() => {
                     *  pause()|
                     * };
                     */
                    var c = ite.Remove(0, 4);
                    var name = c.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("()", "").Trim();
                    c = c.Remove(0, c.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[0].Length).Trim('=', '>', '{', '}', ' ');
                    try { Methodz.Add(name, c.Replace("|", ";")); } catch { Methodz[name] = c.Replace("|", ";"); }
                    return;
                }
                else if (Methodz.ContainsKey(ite.Trim(' ', '(', ')')))
                {
                    IntepreteBlockOfCode(Methodz[ite.Trim(' ', '(', ')')], ";", true);
                    return;
                }

                #endregion Method Control

                #region Loops

                else if (it.StartsWith("while", StringComparison.OrdinalIgnoreCase))
                {
                    var wee = ite.Substring(5).Trim().Split(new[] { "{", "}" }, StringSplitOptions.RemoveEmptyEntries).Where(f => f.Trim() != "").ToArray();
                    var tsie = new Maths(_allVariables).Boolit(wee[0]).Trim('(', ')');
                    var tsi = new Maths(_allVariables).Boolit(tsie);
                    //1

                    wee[0] = "";
                    wee = wee.Select(g => g.Trim()).Where(h => h != "").ToArray();
                    var dumper = string.Join("", wee);

                    var code = dumper.Trim('{', '}');
                    var whila = new Maths().Ifit(tsi.Trim());
                    while (whila)
                    {
                        try
                        {
                            IntepreteBlockOfCode(code, "|", true);
                            tsi = new Maths(_allVariables).Boolit(tsie);
                            whila = new Maths().Ifit(tsi.Trim());
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                else if (ite.StartsWith("for", StringComparison.OrdinalIgnoreCase))
                {
                    var tz = ite.Substring(4).Trim();
                    var wee = tz.Split(new[] { "{", "}" }, StringSplitOptions.RemoveEmptyEntries).Where(f => f.Trim() != "").ToArray();
                    var strip = wee[0].Trim();

                    IntepreteCode(strip.Split('|')[0].Trim());

                    var tsie = strip.Split('|')[1].Trim('(', ')');

                    var tsi = new Maths(_allVariables).Boolit(tsie);

                    var loopCode = strip.Split('|')[2].Trim();
                    loopCode = loopCode.Substring(0, loopCode.Length - 1);
                    //1

                    wee[0] = "";
                    wee = wee.Select(g => g.Trim()).Where(h => h != "").ToArray();
                    var dumper = string.Join("", wee);

                    var code = dumper.Trim('{', '}');
                    var whila = new Maths().Ifit(tsi.Trim());
                    while (whila)
                    {
                        try
                        {
                            IntepreteBlockOfCode(code, "|", true);
                            tsi = new Maths(_allVariables).Boolit(tsie);
                            whila = new Maths().Ifit(tsi.Trim());
                            IntepreteCode(loopCode);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                else if (ite.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                {
                    ite = ite.Trim().Remove(0, 1);
                    var count = int.Parse(ite.Split(new[] { "::" }, StringSplitOptions.None)[0]);
                    var code = ite.Split(new[] { "::" }, StringSplitOptions.None)[1].Trim('{', '}', ' ').Trim();
                    var a = 1;
                    while (a < count)
                    {
                        IntepreteBlockOfCode(code, "|", true);
                        a++;
                    }
                }

                #endregion Loops

                //note stuff to use fr if statement is in the Math class
                // end of this method is in variable manipulation

                #region Variable Manipulation

                else if (ite.EndsWith("++", StringComparison.OrdinalIgnoreCase)) IntepreteCode(ite.Replace("++", " += 1"));
                else if (ite.EndsWith("--", StringComparison.OrdinalIgnoreCase)) IntepreteCode(ite.Replace("--", " -= 1"));
                else
                {
                    var ya = ite.Split();
                    if (ya[0].Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var ce = Strad(ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim(), "+", false);
                            Set(ya[1].Trim(), ce, "String");
                        }
                        catch { Set(ya[1].Trim(), "", "String"); }

                    }
                    else if (ya[0].Equals("string[]", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim();
                        var neu = cde.Split(',').Select(g => Strad(g.Trim(), "+", false)).ToArray();
                        _allVariables.Add(ya[1].Trim(), neu); _stringsA.Add(ya[1].Trim(), neu);
                        Set(ya[1].Trim(), neu, "StringA");
                    }
                    else if (ya[0].Equals("int", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var cde = new Maths(_allVariables).Intexpress(Numit(ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim()));
                            Set(ya[1].Trim(), BigInteger.Parse(cde), "Int");
                        }
                        catch { Set(ya[1].Trim(), 0, "Int"); }
                    }
                    else if (ya[0].Equals("int[]", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim();
                        var nea = cde.Split(',').Select(f => BigInteger.Parse(new Maths(_allVariables).Intexpress(Numit(f)))).ToArray();
                        Set(ya[1].Trim(), nea, "IntA");
                    }
                    else if (ya[0].Equals("double", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var cde = new Maths(_allVariables).Dubexpress(Numit(ite.Remove(0, $"{ya[0]} {ya[1]} =".Length)).Trim());
                            Set(ya[1].Trim(), double.Parse(cde), "Double");
                        }
                        catch { Set(ya[1].Trim(), 0, "Double"); }
                    }
                    else if (ya[0].Equals("double[]", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim();
                        var neae = cde.Split(',').Select(f => double.Parse(new Maths(_allVariables).Dubexpress(Numit(f)))).ToArray();
                        Set(ya[1].Trim(), neae, "DoubleA");
                    }
                    else if (ya[0].Equals("char", StringComparison.OrdinalIgnoreCase))
                    {
                        try { Set(ya[1].Trim(), char.Parse(Strad(ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim().Replace("'", ""), "+", false)), "Char"); }
                        catch { Set(ya[1].Trim(), ' ', "Char"); }
                    }
                    else if (ya[0].Equals("char[]", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = Strad(ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim(), "+", false);
                        Set(ya[1].Trim(), cde.ToCharArray(), "CharA");
                    }
                    else if (ya[0].Equals("long", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var cde = new Maths(_allVariables).Longexpress(Numit(ite.Remove(0, $"{ya[0]} {ya[1]} =".Length)).Trim());
                            Set(ya[1].Trim(), long.Parse(cde), "Long");
                        }
                        catch (Exception) { Set(ya[1].Trim(), 0, "Long"); }
                    }
                    else if (ya[0].Equals("long[]", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim();
                        var neaea = cde.Split(',').Select(f => long.Parse(new Maths(_allVariables).Longexpress(Numit(f)))).ToArray();
                        Set(ya[1].Trim(), neaea, "LongA");
                    }
                    else if (ya[0].Equals("float", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var cde = new Maths(_allVariables).Floatexpress(Numit(ite.Remove(0, $"{ya[0]} {ya[1]} =".Length)).Trim());
                            Set(ya[1].Trim(), float.Parse(cde), "Float");
                        }
                        catch (Exception) { Set(ya[1].Trim(), 0, "Float"); }
                    }
                    else if (ya[0].Equals("float[]", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim();
                        var ne = cde.Split(',').Select(f => float.Parse(new Maths(_allVariables).Longexpress(Numit(f)))).ToArray();
                        Set(ya[1].Trim(), ne, "FloatA");
                    }
                    else if (ya[0].Equals("bool", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim();
                        cde = new Maths(_allVariables).Boolit(cde);
                        Set(ya[1].Trim(), new Maths(_allVariables).Ifit(cde), "Bool");
                    }
                    else if (ya[0].Equals("bool[]", StringComparison.OrdinalIgnoreCase))
                    {
                        var cde = ite.Remove(0, $"{ya[0]} {ya[1]} =".Length).Trim();
                        var kai = cde.Split(',').Select(f => new Maths(_allVariables).Ifit(new Maths(_allVariables).Boolit(f))).ToArray();
                        Set(ya[1].Trim(), kai, "BoolA");
                    }
                    else if (ya[0].Equals("title", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        Console.Title = Strad(string.Join(" ", ya), "+", false);
                    }
                    else if (ya[0].Equals("wait", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        Thread.Sleep(int.Parse(new Maths().Intexpress(Numit(string.Join(" ", ya).Trim()))));
                    }
                    else if (ya[0].Equals("start", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        Process.Start(Strad(string.Join(" ", ya).Trim(), "+", false));
                    }
                    else if (ya[0].Equals("load", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        var entry = new Entry(File.ReadAllText(Strad(string.Join(" ", ya).Trim(), "+", false)));
                        entry.Invalidate();
                    }
                    else if (ya[0].Equals("bg", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Strad(string.Join(" ", ya).Trim(), "+", false));
                    }
                    else if (ya[0].Equals("fg", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Strad(string.Join(" ", ya).Trim(), "+", false));
                    }
                    else if (ya[0].Equals("cd", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        Directory.SetCurrentDirectory(Strad(string.Join(" ", ya).Trim(), "+", false));
                    }
                    else if (ya[0].Equals("dir", StringComparison.OrdinalIgnoreCase))
                    {
                        ya[0] = "";
                        IntepreteCode($"dir(\"{Strad(string.Join(" ", ya).Trim(), "+", false)}\")");
                    }
                    else if (ya[1] == "=")
                    {
                        var name = ya[0].Trim();
                        if (name.Contains("["))
                        {
                            name = ya[0];
                            var index = int.Parse(new Maths().Intexpress(Numit(name.Split('[')[1].Trim(']'))));
                            name = name.Split('[')[0].Trim();

                            ya[0] = ""; ya[1] = "";

                            var data = Strad(string.Join("", ya).Trim(), "+", false);
                            if (_strings.ContainsKey(name))
                            {
                                data = Strad(string.Join("", ya).Trim(), "+", false);
                                _strings[name].ToCharArray()[index] = ((char[]) _allVariables[name])[index] = Char.Parse(data.Trim());
                            }
                            else if (_stringsA.ContainsKey(name))
                            {
                                data = Strad(string.Join(" ", ya).Trim(), "+", false);
                                _stringsA[name][index] = ((string[])_allVariables[name])[index] = data;
                            }
                            else if (_intsA.ContainsKey(name))
                            {
                                ((BigInteger[])_allVariables[name])[index] = BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                _intsA[name][index] = BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                            }
                            else if (_doublesA.ContainsKey(name))
                            {
                                ((double[])_allVariables[name])[index] = double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                                _doublesA[name][index] = double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                            }
                            else if (_longsA.ContainsKey(name))
                            {
                                ((long[])_allVariables[name])[index] = long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                _longsA[name][index] = long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                            }
                            else if (_floatsA.ContainsKey(name))
                            {
                                ((float[])_allVariables[name])[index] = float.Parse(new Maths().Floatexpress(Numit(data)));
                                _floatsA[name][index] = float.Parse(new Maths().Floatexpress(Numit(data)));
                            }
                        }
                        else
                        {
                            ya[0] = "";
                            ya[1] = "";
                            var kong = string.Join("", ya).Trim();
                            if (_strings.ContainsKey(name))
                            {
                                ya = ya.Where(t => t != "").ToArray();
                                kong = Strad(string.Join(" ", ya).Trim(), "+", false);
                                _strings[name] = kong; _allVariables[name] = _strings[name];
                            }
                            else if (_ints.ContainsKey(name))
                            {
                                _ints[name] = BigInteger.Parse(new Maths().Intexpress(Numit(kong)).Trim()); _allVariables[name] = _ints[name];
                            }
                            else if (_doubles.ContainsKey(name))
                            {
                                _doubles[name] = double.Parse(new Maths().Dubexpress(Numit(kong)).Trim()); _allVariables[name] = _doubles[name];
                            }
                            else if (_longs.ContainsKey(name))
                            {
                                _longs[name] = long.Parse(new Maths().Longexpress(Numit(kong)).Trim()); _allVariables[name] = _longs[name];
                            }
                            else if (_floats.ContainsKey(name))
                            {
                                _floats[name] = float.Parse(new Maths().Floatexpress(Numit(kong))); _allVariables[name] = _floats[name];
                            }
                        }
                    }
                    else if (ya[1] == "+=")
                    {
                        var name = ya[0].Trim();
                        if (name.Contains("["))
                        {
                            name = ya[0];
                            var index = int.Parse(new Maths().Intexpress(Numit(name.Split('[')[1].Trim(']'))));

                            name = name.Split('[')[0].Trim();

                            ya[0] = "";
                            ya[1] = "";
                            var data = string.Join("", ya).Trim();
                            data = Strad(data, "+", false);
                            if (_stringsA.ContainsKey(name))
                            {
                                data = string.Join(" ", ya).Trim();
                                data = Strad(data, "+", false);
                                ((string[]) _allVariables[name])[index] += data;
                                _stringsA[name][index] += data;
                            }
                            else if (_intsA.ContainsKey(name))
                            {
                                try
                                {
                                    ((BigInteger[]) _allVariables[name])[index] += BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                    _intsA[name][index] += BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                }
                                catch (Exception)
                                {
                                    ((BigInteger[]) _allVariables[name])[index] += BigInteger.Parse(data);
                                    _intsA[name][index] += BigInteger.Parse(data);
                                }
                            }
                            else if (_doublesA.ContainsKey(name))
                            {
                                try
                                {
                                    ((double[]) _allVariables[name])[index] += double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                                    _doublesA[name][index] += double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                                }
                                catch
                                {
                                    ((double[]) _allVariables[name])[index] += double.Parse(data);
                                    _doublesA[name][index] += double.Parse(data);
                                }
                            }
                            else if (_longsA.ContainsKey(name))
                            {
                                try
                                {
                                    ((long[]) _allVariables[name])[index] += long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                    _longsA[name][index] += long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                }
                                catch
                                {
                                    ((long[]) _allVariables[name])[index] += long.Parse(data);
                                    _longsA[name][index] += long.Parse(data);
                                }
                            }
                            else if (_floatsA.ContainsKey(name))
                            {
                                try
                                {
                                    ((float[]) _allVariables[name])[index] += float.Parse(new Maths().Floatexpress(Numit(data)));
                                    _floatsA[name][index] += float.Parse(new Maths().Floatexpress(Numit(data)));
                                }
                                catch
                                {
                                    ((float[])_allVariables[name])[index] += float.Parse(data);
                                    _floatsA[name][index] += float.Parse(data);
                                }
                            }
                        }
                        else
                        {
                            ya[0] = "";
                            ya[1] = "";
                            var kong = string.Join(" ", ya).Trim();
                            if (_strings.ContainsKey(name))
                            {
                                _strings[name] += Strad(kong, "+", false); _allVariables[name] = _strings[name];
                            }
                            if (Methodz.ContainsKey(name))
                            {
                                //main += k() + d() + s();
                                if (kong.Contains("+"))
                                {
                                    var yami = kong.Split('+').Select(f => f.Trim(' ', '(', ')')).ToArray();
                                    foreach (string am in yami)
                                    {
                                        Methodz[name] += Methodz[am.Trim()];
                                    }
                                }
                                else
                                {
                                    Methodz[name] += kong.Trim();
                                }
                            }

                            if (_ints.ContainsKey(name))
                            {
                                try
                                {
                                    _ints[name] += BigInteger.Parse(new Maths().Intexpress(Numit(kong)).Trim()); _allVariables[name] = _ints[name];
                                }
                                catch
                                {
                                    _ints[name] += BigInteger.Parse(kong); _allVariables[name] = _ints[name];
                                }
                            }

                            if (_doubles.ContainsKey(name))
                            {
                                try
                                {
                                    _doubles[name] += double.Parse(new Maths().Dubexpress(Numit(kong)).Trim()); _allVariables[name] = _doubles[name];
                                }
                                catch
                                {
                                    _doubles[name] += double.Parse(kong); _allVariables[name] = _doubles[name];
                                }
                            }

                            if (_longs.ContainsKey(name))
                            {
                                try
                                {
                                    _longs[name] += long.Parse(new Maths().Longexpress(Numit(kong)).Trim()); _allVariables[name] = _longs[name];
                                }
                                catch
                                {
                                    _longs[name] += long.Parse(kong); _allVariables[name] = _longs[name];
                                }
                            }

                            if (_floats.ContainsKey(name))
                            {
                                try
                                {
                                    _floats[name] += float.Parse(new Maths().Floatexpress(Numit(kong))); _allVariables[name] = _floats[name];
                                }
                                catch
                                {
                                    _floats[name] += float.Parse(kong); _allVariables[name] = _floats[name];
                                }
                            }

                            if (_stringsA.ContainsKey(name))
                            {
                                var neus = kong.Split(',').Select(t => Strad(t, "+", false)).ToArray();
                                var neww = new string[_stringsA[name].Length + neus.Length];
                                _stringsA[name].CopyTo(neww, 0);
                                neus.CopyTo(neww, _stringsA[name].Length);
                                Set(name, neww, "StringA");
                            }
                            if (_intsA.ContainsKey(name))
                            {
                                var neus = kong.Split(',').Select(t => BigInteger.Parse(new Maths().Intexpress(Numit(t)))).ToArray();
                                var neww = new BigInteger[_intsA[name].Length + neus.Length];
                                _intsA[name].CopyTo(neww, 0);
                                neus.CopyTo(neww, _intsA[name].Length);
                                Set(name, neww, "IntA");
                            }
                            if (_doublesA.ContainsKey(name))
                            {
                                var neus = kong.Split(',').Select(t => double.Parse(new Maths().Dubexpress(Numit(t)))).ToArray();
                                var neww = new double[_doublesA[name].Length + neus.Length];
                                _doublesA[name].CopyTo(neww, 0);
                                neus.CopyTo(neww, _doublesA[name].Length);
                                Set(name, neww, "DoubleA");
                            }
                            if (_longsA.ContainsKey(name))
                            {
                                var neus = kong.Split(',').Select(t => long.Parse(new Maths().Longexpress(Numit(t)))).ToArray();
                                var neww = new long[_longsA[name].Length + neus.Length];
                                _longsA[name].CopyTo(neww, 0);
                                neus.CopyTo(neww, _longsA[name].Length);
                                Set(name, neww, "LongA");
                            }
                            if (_floatsA.ContainsKey(name))
                            {
                                var neus = kong.Split(',').Select(t => float.Parse(new Maths().Floatexpress(Numit(t)))).ToArray();
                                var neww = new float[_floatsA[name].Length + neus.Length];
                                _floatsA[name].CopyTo(neww, 0);
                                neus.CopyTo(neww, _floatsA[name].Length);
                                Set(name, neww, "FloatA");
                            }
                        }
                    }
                    else if (ya[1] == "-=")
                    {
                        var name = ya[0].Trim();
                        if (name.Contains("["))
                        {
                            name = ya[0];
                            var index = int.Parse(new Maths().Intexpress(Numit(name.Split('[')[1].Trim(']'))));

                            name = name.Split('[')[0].Trim();

                            ya[0] = "";
                            ya[1] = "";
                            var data = string.Join("", ya).Trim();
                            data = Strad(data, "+", false);
                            if (_intsA.ContainsKey(name))
                            {
                                ((BigInteger[])_allVariables[name])[index] -= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                _intsA[name][index] -= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                            }
                            else if (_doublesA.ContainsKey(name))
                            {
                                ((double[]) _allVariables[name])[index] -= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                                _doublesA[name][index] -= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                            }
                            else if (_longsA.ContainsKey(name))
                            {
                                ((long[]) _allVariables[name])[index] -= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                _longsA[name][index] -= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                            }
                            else if (_floatsA.ContainsKey(name))
                            {
                                ((float[]) _allVariables[name])[index] -= float.Parse(new Maths().Floatexpress(Numit(data)));
                                _floatsA[name][index] -= float.Parse(new Maths().Floatexpress(Numit(data)));
                            }
                        }
                        else
                        {
                            ya[0] = "";
                            ya[1] = "";
                            var kong = string.Join("", ya).Trim();
                            if (_ints.ContainsKey(name))
                            {
                                _ints[name] -= BigInteger.Parse(new Maths().Intexpress(Numit(kong)).Trim()); _allVariables[name] = _ints[name];
                            }

                            if (_doubles.ContainsKey(name))
                            {
                                _doubles[name] -= double.Parse(new Maths().Dubexpress(Numit(kong)).Trim()); _allVariables[name] = _doubles[name];
                            }

                            if (_longs.ContainsKey(name))
                            {
                                _longs[name] -= long.Parse(new Maths().Longexpress(Numit(kong)).Trim()); _allVariables[name] = _longs[name];
                            }

                            if (_floats.ContainsKey(name))
                            {
                                _floats[name] -= float.Parse(new Maths().Floatexpress(Numit(kong))); _allVariables[name] = _floats[name];
                            }
                        }
                    }
                    else if (ya[1] == "*=")
                    {
                        var name = ya[0].Trim();
                        if (name.Contains("["))
                        {
                            name = ya[0];
                            var index = int.Parse(new Maths().Intexpress(Numit(name.Split('[')[1].Trim(']'))));

                            name = name.Split('[')[0].Trim();

                            ya[0] = "";
                            ya[1] = "";
                            var data = string.Join("", ya).Trim();
                            data = Strad(data, "+", false);
                            if (_intsA.ContainsKey(name))
                            {
                                ((BigInteger[]) _allVariables[name])[index] *= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                _intsA[name][index] *= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                            }
                            else if (_doublesA.ContainsKey(name))
                            {
                                ((double[]) _allVariables[name])[index] *= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                                _doublesA[name][index] *= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                            }
                            else if (_longsA.ContainsKey(name))
                            {
                                ((long[]) _allVariables[name])[index] *= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                _longsA[name][index] *= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                            }
                            else if (_floatsA.ContainsKey(name))
                            {
                                ((float[]) _allVariables[name])[index] *= float.Parse(new Maths().Floatexpress(Numit(data)));
                                _floatsA[name][index] *= float.Parse(new Maths().Floatexpress(Numit(data)));
                            }
                        }
                        else
                        {
                            ya[0] = "";
                            ya[1] = "";
                            var kong = string.Join("", ya).Trim();
                            if (_ints.ContainsKey(name))
                            {
                                _ints[name] *= BigInteger.Parse(new Maths().Intexpress(Numit(kong)).Trim()); _allVariables[name] = _ints[name];
                            }

                            if (_doubles.ContainsKey(name))
                            {
                                _doubles[name] *= double.Parse(new Maths().Dubexpress(Numit(kong)).Trim()); _allVariables[name] = _doubles[name];
                            }

                            if (_longs.ContainsKey(name))
                            {
                                _longs[name] *= long.Parse(new Maths().Longexpress(Numit(kong)).Trim()); _allVariables[name] = _longs[name];
                            }

                            if (_floats.ContainsKey(name))
                            {
                                _floats[name] *= float.Parse(new Maths().Floatexpress(Numit(kong))); _allVariables[name] = _floats[name];
                            }
                        }
                    }
                    else if (ya[1] == "/=")
                    {
                        var name = ya[0].Trim();
                        if (name.Contains("["))
                        {
                            name = ya[0];
                            var index = int.Parse(new Maths().Intexpress(Numit(name.Split('[')[1].Trim(']'))));

                            name = name.Split('[')[0].Trim();

                            ya[0] = "";
                            ya[1] = "";
                            var data = string.Join("", ya).Trim();
                            data = Strad(data, "+", false);
                            if (_intsA.ContainsKey(name))
                            {
                                ((BigInteger[]) _allVariables[name])[index] /= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                _intsA[name][index] /= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                            }
                            else if (_doublesA.ContainsKey(name))
                            {
                                ((double[]) _allVariables[name])[index] /= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                                _doublesA[name][index] /= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                            }
                            else if (_longsA.ContainsKey(name))
                            {
                                ((long[]) _allVariables[name])[index] /= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                _longsA[name][index] /= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                            }
                            else if (_floatsA.ContainsKey(name))
                            {
                                ((float[]) _allVariables[name])[index] /= float.Parse(new Maths().Floatexpress(Numit(data)));
                                _floatsA[name][index] /= float.Parse(new Maths().Floatexpress(Numit(data)));
                            }
                        }
                        else
                        {
                            ya[0] = "";
                            ya[1] = "";
                            var kong = string.Join("", ya).Trim();
                            if (_ints.ContainsKey(name))
                            {
                                _ints[name] /= BigInteger.Parse(new Maths().Intexpress(Numit(kong)).Trim()); _allVariables[name] = _ints[name];
                            }

                            if (_doubles.ContainsKey(name))
                            {
                                _doubles[name] /= double.Parse(new Maths().Dubexpress(Numit(kong)).Trim()); _allVariables[name] = _doubles[name];
                            }

                            if (_longs.ContainsKey(name))
                            {
                                _longs[name] /= long.Parse(new Maths().Longexpress(Numit(kong)).Trim()); _allVariables[name] = _longs[name];
                            }

                            if (_floats.ContainsKey(name))
                            {
                                _floats[name] /= float.Parse(new Maths().Floatexpress(Numit(kong))); _allVariables[name] = _floats[name];
                            }
                        }
                    }
                    else if (ya[1] == "%=")
                    {
                        var name = ya[0].Trim();
                        if (name.Contains("["))
                        {
                            name = ya[0];
                            var index = int.Parse(new Maths().Intexpress(Numit(name.Split('[')[1].Trim(']'))));

                            name = name.Split('[')[0].Trim();

                            ya[0] = "";
                            ya[1] = "";
                            var data = string.Join("", ya).Trim();
                            data = Strad(data, "+", false);
                            if (_intsA.ContainsKey(name))
                            {
                                ((BigInteger[]) _allVariables[name])[index] %= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                _intsA[name][index] %= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                            }
                            else if (_doublesA.ContainsKey(name))
                            {
                                ((double[]) _allVariables[name])[index] %= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                                _doublesA[name][index] %= double.Parse(new Maths().Dubexpress(Numit(data)).Trim());
                            }
                            else if (_longsA.ContainsKey(name))
                            {
                                ((long[]) _allVariables[name])[index] %= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                _longsA[name][index] %= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                            }
                            else if (_floatsA.ContainsKey(name))
                            {
                                ((float[]) _allVariables[name])[index] %= float.Parse(new Maths().Floatexpress(Numit(data)));
                                _floatsA[name][index] %= float.Parse(new Maths().Floatexpress(Numit(data)));
                            }
                        }
                        else
                        {
                            ya[0] = "";
                            ya[1] = "";
                            var kong = string.Join("", ya).Trim();
                            if (_ints.ContainsKey(name))
                            {
                                _ints[name] %= BigInteger.Parse(new Maths().Intexpress(Numit(kong)).Trim()); _allVariables[name] = _ints[name];
                            }

                            if (_doubles.ContainsKey(name))
                            {
                                _doubles[name] %= double.Parse(new Maths().Dubexpress(Numit(kong)).Trim()); _allVariables[name] = _doubles[name];
                            }

                            if (_longs.ContainsKey(name))
                            {
                                _longs[name] %= long.Parse(new Maths().Longexpress(Numit(kong)).Trim()); _allVariables[name] = _longs[name];
                            }

                            if (_floats.ContainsKey(name))
                            {
                                _floats[name] %= float.Parse(new Maths().Floatexpress(Numit(kong))); _allVariables[name] = _floats[name];
                            }
                        }
                    }
                    else if (ya[1] == "^=")
                    {
                        var name = ya[0].Trim();
                        if (name.Contains("["))
                        {
                            name = ya[0];
                            var index = int.Parse(new Maths().Intexpress(Numit(name.Split('[')[1].Trim(']'))));

                            name = name.Split('[')[0].Trim();

                            ya[0] = "";
                            ya[1] = "";
                            var data = string.Join("", ya).Trim();
                            data = Strad(data, "+", false);
                            if (_intsA.ContainsKey(name))
                            {
                                ((BigInteger[]) _allVariables[name])[index] ^= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                                _intsA[name][index] ^= BigInteger.Parse(new Maths().Intexpress(Numit(data)).Trim());
                            }
                            else if (_longsA.ContainsKey(name))
                            {
                                ((long[]) _allVariables[name])[index] ^= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                                _longsA[name][index] ^= long.Parse(new Maths().Longexpress(Numit(data)).Trim());
                            }
                        }
                        else
                        {
                            ya[0] = "";
                            ya[1] = "";
                            var kong = string.Join("", ya).Trim();
                            if (_ints.ContainsKey(name))
                            {
                                _ints[name] ^= BigInteger.Parse(new Maths().Intexpress(Numit(kong)).Trim()); _allVariables[name] = _ints[name];
                            }

                            if (_longs.ContainsKey(name))
                            {
                                _longs[name] ^= long.Parse(new Maths().Longexpress(Numit(kong)).Trim()); _allVariables[name] = _longs[name];
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }// this is the ending bracket for the method work... please dont get confused.

        #region Variables and Setting

        public Dictionary<string, string> Methodz;
        private Dictionary<string, object> _allVariables = new Dictionary<string, object>();
        private Dictionary<string, bool> _bools = new Dictionary<string, bool>();
        private Dictionary<string, bool[]> _boolsA = new Dictionary<string, bool[]>();
        private Dictionary<string, char> _chars = new Dictionary<string, char>();
        private Dictionary<string, char[]> _charsA = new Dictionary<string, char[]>();
        private Dictionary<string, double> _doubles = new Dictionary<string, double>();
        private Dictionary<string, double[]> _doublesA = new Dictionary<string, double[]>();
        private Dictionary<string, float> _floats = new Dictionary<string, float>();
        private Dictionary<string, float[]> _floatsA = new Dictionary<string, float[]>();
        private Dictionary<string, BigInteger> _ints = new Dictionary<string, BigInteger>();
        private Dictionary<string, BigInteger[]> _intsA = new Dictionary<string, BigInteger[]>();
        private Dictionary<string, long> _longs = new Dictionary<string, long>();
        private Dictionary<string, long[]> _longsA = new Dictionary<string, long[]>();
        private Dictionary<string, string> _strings = new Dictionary<string, string>();
        private Dictionary<string, string[]> _stringsA = new Dictionary<string, string[]>();

        private void Set(string name, object data, string type)
        {
            switch (type)
            {
                case "Bool":
                    if (this._bools.ContainsKey(name))
                    {
                        this._bools[name] = (bool)data;
                    }
                    else
                    {
                        this._bools.Add(name, (bool)data);
                    }
                    break;
                case "BoolA":
                    if (this._boolsA.ContainsKey(name))
                    {
                        this._boolsA[name] = (bool[])data;
                    }
                    else
                    {
                        this._boolsA.Add(name, (bool[])data);
                    }
                    break;
                case "Char":
                    if (this._chars.ContainsKey(name))
                    {
                        this._chars[name] = (char)data;
                    }
                    else
                    {
                        this._chars.Add(name, (char)data);
                    }
                    break;
                case "CharA":
                    if (this._charsA.ContainsKey(name))
                    {
                        this._charsA[name] = (char[])data;
                    }
                    else
                    {
                        this._charsA.Add(name, (char[])data);
                    }
                    break;
                case "Int":
                    if (this._ints.ContainsKey(name))
                    {
                        this._ints[name] = BigInteger.Parse(new Maths().Intexpress(this.Numit(data.ToString())).Trim());
                    }
                    else
                    {
                        this._ints.Add(name, BigInteger.Parse(new Maths().Intexpress(this.Numit(data.ToString())).Trim()));
                    }
                    break;
                case "IntA":
                    if (this._intsA.ContainsKey(name))
                    {
                        this._intsA[name] = (from ka in (BigInteger[])data
                                             select BigInteger.Parse(new Maths().Intexpress(this.Numit(ka.ToString())).Trim())).ToArray();
                    }
                    else
                    {
                        this._intsA.Add(name, (from ka in (BigInteger[])data
                                               select BigInteger.Parse(new Maths().Intexpress(this.Numit(ka.ToString())).Trim())).ToArray());
                    }
                    break;
                case "Double":
                    if (this._doubles.ContainsKey(name))
                    {
                        this._doubles[name] = double.Parse(this.Strad(data.ToString(), "kama", false).Trim());
                    }
                    else
                    {
                        this._doubles.Add(name, double.Parse(this.Strad(data.ToString(), "+", false)));
                    }
                    break;
                case "DoubleA":
                    if (this._doublesA.ContainsKey(name))
                    {
                        this._doublesA[name] = (from g in (double[])data
                                                select double.Parse(this.Strad(g.ToString(CultureInfo.CurrentCulture), "+", false))).ToArray();
                    }
                    else
                    {
                        this._doublesA.Add(name, (from g in (double[])data
                                                  select double.Parse(this.Strad(g.ToString(CultureInfo.CurrentCulture), "kama", false))).ToArray());
                    }
                    break;
                case "Float":
                    if (this._floats.ContainsKey(name))
                    {
                        this._floats[name] = float.Parse(new Maths().Floatexpress(this.Numit(data.ToString())).Trim());
                    }
                    else
                    {
                        this._floats.Add(name, float.Parse(new Maths().Floatexpress(this.Numit(data.ToString().Trim()))));
                    }
                    break;
                case "FloatA":
                    if (this._floatsA.ContainsKey(name))
                    {
                        this._floatsA[name] = (from g in (float[])data
                                               select float.Parse(new Maths().Floatexpress(this.Numit(g.ToString(CultureInfo.CurrentCulture))).Trim())).ToArray();
                    }
                    else
                    {
                        this._floatsA.Add(name, (from g in (float[])data
                                                 select float.Parse(new Maths().Floatexpress(this.Numit(g.ToString(CultureInfo.CurrentCulture))).Trim())).ToArray());
                    }
                    break;
                case "LongA":
                    if (this._longsA.ContainsKey(name))
                    {
                        this._longsA[name] = (from g in (long[])data
                                              select long.Parse(new Maths().Longexpress(this.Numit(g.ToString(CultureInfo.CurrentCulture))).Trim())).ToArray();
                    }
                    else
                    {
                        this._longsA.Add(name, (from g in (long[])data
                                                select long.Parse(new Maths().Longexpress(this.Numit(g.ToString(CultureInfo.CurrentCulture))).Trim())).ToArray());
                    }
                    break;
                case "String":
                    if (this._strings.ContainsKey(name))
                    {
                        this._strings[name] = (string)data;
                    }
                    else
                    {
                        this._strings.Add(name, (string)data);
                    }
                    break;
                case "StringA":
                    if (this._stringsA.ContainsKey(name))
                    {
                        this._stringsA[name] = (string[])data;
                    }
                    else
                    {
                        this._stringsA.Add(name, (string[])data);
                    }
                    break;
            }
            this._allVariables[name] = data;
            if (!(type != "gen"))
            {
                if (this._strings.ContainsKey(name))
                {
                    this._strings[name] = (string)data;
                }
                if (this._stringsA.ContainsKey(name))
                {
                    this._stringsA[name] = (string[])data;
                }
                if (this._chars.ContainsKey(name))
                {
                    this._chars[name] = (char)data;
                }
                if (this._charsA.ContainsKey(name))
                {
                    this._charsA[name] = (char[])data;
                }
                if (this._ints.ContainsKey(name))
                {
                    this._ints[name] = BigInteger.Parse(new Maths().Intexpress(this.Numit(data.ToString())).Trim());
                }
                if (this._intsA.ContainsKey(name))
                {
                    this._intsA[name] = (from h in (BigInteger[])data
                                         select BigInteger.Parse(new Maths().Intexpress(this.Numit(h.ToString())).Trim())).ToArray();
                }
                if (this._doubles.ContainsKey(name))
                {
                    this._doubles[name] = double.Parse(new Maths().Dubexpress(this.Numit(data.ToString())).Trim());
                }
                if (this._doublesA.ContainsKey(name))
                {
                    this._doublesA[name] = (from h in (double[])data
                                            select double.Parse(new Maths().Dubexpress(this.Numit(h.ToString(CultureInfo.CurrentCulture))).Trim())).ToArray();
                }
                if (this._floats.ContainsKey(name))
                {
                    this._floats[name] = float.Parse(new Maths().Floatexpress(this.Numit(data.ToString())));
                }
                if (this._floatsA.ContainsKey(name))
                {
                    this._floatsA[name] = (from h in (float[])data
                                           select float.Parse(new Maths().Floatexpress(this.Numit(h.ToString(CultureInfo.CurrentCulture))).Trim())).ToArray();
                }
                if (this._longs.ContainsKey(name))
                {
                    this._longs[name] = long.Parse(new Maths().Longexpress(this.Numit(data.ToString())));
                }
                if (this._longsA.ContainsKey(name))
                {
                    this._longsA[name] = (from h in (long[])data
                                          select long.Parse(new Maths().Longexpress(this.Numit(h.ToString())).Trim())).ToArray();
                }
                if (this._bools.ContainsKey(name))
                {
                    this._bools[name] = (bool)data;
                }
                if (this._boolsA.ContainsKey(name))
                {
                    this._boolsA[name] = (bool[])data;
                }
            }
        }

        #endregion Variables and Setting

        #region Core

        public string Numit(string what)
        {
            var wha = Strad(what, " ", true);
            try
            {
                MatchCollection hee = new Regex("%\\w*").Matches(wha);
                foreach (Match chk in hee)
                {
                    wha = wha.Replace(chk.Value, Strad(chk.Value, "", false));
                }
                foreach (var item in _allVariables.Keys) { try { wha = wha.Replace(item, Convert.ToString(_allVariables[item])); }
                    catch
                    {
                        // ignored
                    }
                }
                foreach (var item in _longsA.Keys)
                {
                    wha = item.Aggregate(wha, (current, w) => current.Replace(w, item[w]));
                }
                foreach (var item in _intsA.Keys)
                {
                    wha = item.Aggregate(wha, (current, w) => current.Replace(w, item[w]));
                }
                foreach (var item in _doublesA.Keys)
                {
                    wha = item.Aggregate(wha, (current, w) => current.Replace(w, item[w]));
                }
                foreach (var item in _floatsA.Keys)
                {
                    wha = item.Aggregate(wha, (current, w) => current.Replace(w, item[w]));
                }
            }
            catch
            {
                // ignored
            }
            return wha.Trim();
        }

        public string Strad(string codee, string splitby, bool show)
        {
            var code = codee
            .Replace(_strings[";"], ";")
            .Replace(_strings["|"], "|")
            .Replace(_strings["<<"], "<<")
            .Replace(_strings["'"], "'");

            //"hello" + " world"
            var ye = code.Split(new[] { splitby }, StringSplitOptions.RemoveEmptyEntries).Select(g => g.Trim()).ToArray();
            try
            {
                for (var i = 0; i < ye.Length; i++)
                {
                    try
                    {
                        ye[i] = ye[i].Trim();
                        if ((ye[i].StartsWith("\"", StringComparison.Ordinal) && ye[i].EndsWith("\"", StringComparison.Ordinal)) || ye[i].StartsWith("\'", StringComparison.Ordinal) && ye[i].EndsWith("\'", StringComparison.Ordinal))
                        {
                            ye[i] = ye[i].Trim('\'', '"');
                            ye[i] = ye[i].Replace(@"\" + "n", Environment.NewLine)
                                         .Replace(@"\" + "a", "\a")
                                         .Replace(@"\" + "b", "\b")
                                         .Replace(@"\" + "f", "\f")
                                         .Replace(@"\" + "r", "\r")
                                         .Replace(@"\" + "t", "\t")
                                         .Replace(@"\" + "v", "\v");
                        }
                        else if (ye[i].Trim().StartsWith("%", StringComparison.Ordinal))
                        {
                            var name = ye[i].Replace("%", "").Trim();
                            if (_strings.ContainsKey(name)) ye[i] = _strings[name].Length.ToString();
                            else if (_stringsA.ContainsKey(name)) ye[i] = _stringsA[name].Length.ToString();
                            else if (_charsA.ContainsKey(name)) ye[i] = _charsA[name].Length.ToString();
                            else if (_doublesA.ContainsKey(name)) ye[i] = _doublesA[name].Length.ToString();
                            else if (_intsA.ContainsKey(name)) ye[i] = _intsA[name].Length.ToString();
                            else if (_longsA.ContainsKey(name)) ye[i] = _longsA[name].Length.ToString();
                            else if (_floatsA.ContainsKey(name)) ye[i] = _floatsA[name].Length.ToString();
                            else if (_boolsA.ContainsKey(name)) ye[i] = _boolsA[name].Length.ToString();
                            else if (_doubles.ContainsKey(name)) ye[i] = _doubles[name].ToString(CultureInfo.CurrentCulture).Length.ToString();
                            else if (_ints.ContainsKey(name)) ye[i] = _ints[name].ToString().Length.ToString();
                            else if (_longs.ContainsKey(name)) ye[i] = _longs[name].ToString().Length.ToString();
                            else if (_floats.ContainsKey(name)) ye[i] = _floats[name].ToString(CultureInfo.CurrentCulture).Length.ToString();
                            else if(name.Contains("[") && name.Contains("]"))
                            {
                                string dada = Strad(name.Replace("%", ""), "", false);
                                ye[i] = dada.Length.ToString();
                            }
                        }
                        else if (ye[i].StartsWith("replace", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 8).Trim(')');
                            var kati = ye[i].Split(',');
                            var name = kati[0].Trim();
                            var a = Strad(kati[1], "+", false);
                            var b = Strad(kati[2], "+", false);
                            if (_allVariables.ContainsKey(name))
                            {
                                ye[i] = (_allVariables[name] as string).Replace(a, b);
                            }
                            else
                            {
                                ye[i] = Strad(name, "+", false).Replace(a, b);
                            }
                        }
                        else if (ye[i].StartsWith("reverse", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 8).Trim(')');
                            var name = ye[i];
                            if (_strings.ContainsKey(name))
                            {
                                var dzz = (_allVariables[name] as string).ToCharArray();
                                Array.Reverse(dzz);
                                ye[i] = new string(dzz);
                            }
                            else
                            {
                                var dzz = Strad(name, "+", false).ToCharArray();
                                Array.Reverse(dzz);
                                ye[i] = new string(dzz);
                            }
                        }
                        else if (ye[i].StartsWith("remove", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 7).Trim(')');
                            var kati = ye[i].Split(',');
                            var name = kati[0].Trim();
                            var a = int.Parse(new Maths().Intexpress(Numit(kati[1])).Trim());
                            var b = int.Parse(new Maths().Intexpress(Numit(kati[2])).Trim());
                            if (_allVariables.ContainsKey(name))
                            {
                                ye[i] = (_allVariables[name] as string).Remove(a, b);
                            }
                            else
                            {
                                ye[i] = Strad(name, "+", false).Remove(a, b);
                            }
                        }
                        else if (ye[i].StartsWith("swap", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 5).Trim(')');

                            var kati = ye[i].Split(',');
                            var name = kati[0].Trim();

                            var a = int.Parse(new Maths().Intexpress(Numit(kati[1])).Trim());
                            var b = int.Parse(new Maths().Intexpress(Numit(kati[2])).Trim());
                            var dzz = Strad(name, "+", false).ToCharArray();
                            if (_allVariables.ContainsKey(name)) { dzz = (_allVariables[name] as string).ToCharArray(); }
                            var aa = dzz[a]; var bb = dzz[b]; dzz[a] = bb; dzz[b] = aa;
                            ye[i] = new string(dzz);
                        }
                        else if (ye[i].StartsWith("join", StringComparison.OrdinalIgnoreCase))
                        {
                            //join(kami,",")
                            ye[i] = ye[i].Remove(0, 5).Trim(')');
                            var kati = ye[i].Split(',');
                            ye[i] = String.Join(Strad(kati[1].Trim(), "+", false), _stringsA[kati[0].Trim()]);
                        }
                        else if (ye[i].StartsWith("sol", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 4).Trim(')');
                            ye[i] = new Maths(_allVariables).Dubexpress(Numit(ye[i]));
                        }
                        else if (ye[i].StartsWith("name", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 5).Trim(')');
                            ye[i] = Strad(ye[i], "+", false);
                            foreach (var VARIABLE in _allVariables.Keys)
                            {
                                if (_allVariables[VARIABLE].Equals(ye[i]))
                                {
                                    ye[i] = VARIABLE;
                                    break;
                                }
                            }
                        }
                        else if (ye[i].StartsWith("lower", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 6).Trim(')');
                            var name = ye[i];
                            if (_allVariables.ContainsKey(name)) { ye[i] = Convert.ToString(_allVariables[name] as string).ToLower(); }
                            else { ye[i] = Strad(name, "+", false).ToLower(); }
                        }
                        else if (ye[i].StartsWith("upper", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 6).Trim(')');
                            var name = ye[i];
                            if (_allVariables.ContainsKey(name)) { ye[i] = Convert.ToString(_allVariables[name] as string).ToUpper(); }
                            else { ye[i] = Strad(name, "+", false).ToUpper(); }
                        }
                        else if (ye[i].StartsWith("open", StringComparison.OrdinalIgnoreCase) || ye[i].StartsWith("read", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = ye[i].Remove(0, 5).Trim(')');
                            ye[i] = File.ReadAllText(Strad(ye[i], "+", false).ToUpper());
                        }
                        else if (ye[i].Trim(')', '(', ' ').Equals("date", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = DateTime.Now.Date.ToShortDateString();
                        }
                        else if (ye[i].Trim(')', '(', ' ').Equals("time", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = DateTime.Now.TimeOfDay.ToString();
                        }
                        else if (ye[i].Trim(')', '(', ' ').Equals("day", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = DateTime.Now.Date.DayOfWeek.ToString();
                        }
                        else if (ye[i].Trim(')', '(', ' ').Equals("month", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = DateTime.Now.Date.DayOfYear.ToString();
                        }
                        else if (ye[i].Trim(')', '(', ' ').Equals("year", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = DateTime.Now.Date.Year.ToString();
                        }
                        else if (ye[i].Equals("random", StringComparison.OrdinalIgnoreCase))
                        {
                            ye[i] = (new Random().NextDouble() * 10).ToString();
                        }
                        else
                        {
                            if (_allVariables.ContainsKey(ye[i].Trim()))
                            {
                                ye[i] = _allVariables[ye[i].Trim()].ToString();
                            }
                            else if (ye[i].Contains("[") && ye[i].Contains("]"))
                            {
                                var yami = ye[i].Split('[')[0].Trim();
                                var index = int.Parse(new Maths().Intexpress(Numit(ye[i].Split('[')[1].Trim(']'))));
                                if (_stringsA.ContainsKey(yami))
                                {
                                    ye[i] = _stringsA[yami][index];
                                }
                                else if (_strings.ContainsKey(yami))
                                {
                                    ye[i] = _strings[yami][index].ToString();
                                }
                                else if (_charsA.ContainsKey(yami))
                                {
                                    ye[i] = _charsA[yami][index].ToString();
                                }
                                else if (_intsA.ContainsKey(yami))
                                {
                                    ye[i] = _intsA[yami][index].ToString();
                                }
                                else if (_doublesA.ContainsKey(yami))
                                {
                                    ye[i] = _doublesA[yami][index].ToString(CultureInfo.CurrentCulture);
                                }
                                else if (_longsA.ContainsKey(yami))
                                {
                                    ye[i] = _longsA[yami][index].ToString();
                                }
                                else if (_boolsA.ContainsKey(yami))
                                {
                                    ye[i] = _boolsA[yami][index].ToString();
                                }
                                else if (_floatsA.ContainsKey(yami))
                                {
                                    ye[i] = _floatsA[yami][index].ToString(CultureInfo.CurrentCulture);
                                }
                                else
                                {
                                    ye[i] = ye[i];
                                }
                            }

                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch
            {
                // ignored
            }
            ye = ye.Where(k => k != "").ToArray();
            return show ? string.Join($" {splitby} ", ye).Trim($" {splitby} ".ToCharArray()) : string.Join("", ye);
        }

        #endregion Core

        #endregion Variable Manipulation
    }
}