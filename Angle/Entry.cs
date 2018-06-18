// Gnos.Entry
using Gnos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

internal class Entry : Form
{
    public Dictionary<string, string> Methods
    {
        get;
    } = new Dictionary<string, string>();


    private IEnumerable<string> Methodzz(string of)
    {
        string workon = of.Trim();
        List<string> methodNames = new List<string>();
        MatchCollection f = new Regex("<(.*?)>").Matches(workon);
        for (int j = 0; j < f.Count; j++)
        {
            if (!f[j].Value.Contains("/"))
            {
                methodNames.Add(f[j].Value);
            }
        }
        List<string> meths = new List<string>();
        for (int i = methodNames.Count - 1; i >= 0; i--)
        {
            string item = methodNames[i];
            string[] yami = workon.Split(new string[1]
            {
                item.Replace("<", "")
            }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                string code = string.Format("{0}\n{1}\n{2}", item, yami[1].Trim().Trim('/', '<'), item.Replace("<", "</")).Replace("</</", "</");
                meths.Add(code);
            }
            catch
            {
            }
        }
        return meths.ToArray();
    }

    public Entry(string what)
    {
        Thread.CurrentThread.Priority = ThreadPriority.Highest;
        this.InitializeComponent();
        string[] dzz = (from de in this.Methodzz(what)
                        where de.Trim() != ""
                        select de).ToArray();
        if (dzz.Length != 0)
        {
            for (int i = dzz.Length - 1; i >= 0; i--)
            {
                try
                {
                    string name = new Regex("<(.*)>").Matches(dzz[i])[0].Value.Trim('<', '>');
                    string[] yami = dzz[i].Split(new string[1]
                    {
                        name + ">"
                    }, StringSplitOptions.RemoveEmptyEntries);
                    string code = yami[1].Trim().Trim('/', '<');
                    this.Methods.Add(name, code);
                }
                catch
                {
                }
            }
            try
            {
                SystemCore system = new SystemCore(this.Methods["main"], ";", this.Methods);
                system.IntepreteCode("Exit");
            }
            catch
            {
            }
        }
        else
        {
            Entry.Cmdme();
        }
    }

    public Entry()
    {
    }

    private static void Cmdme()
    {
        SystemCore ma = new SystemCore();
        while (true)
        {
            Console.Write(">> ");
            ma.IntepreteCode(Console.ReadLine());
        }
    }

    private void InitializeComponent()
    {
        base.SuspendLayout();
        this.BackColor = Color.Purple;
        base.ClientSize = new Size(284, 261);
        base.FormBorderStyle = FormBorderStyle.None;
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = "Entry";
        base.Opacity = 0.0;
        base.ShowIcon = false;
        base.ShowInTaskbar = false;
        base.StartPosition = FormStartPosition.CenterScreen;
        base.TransparencyKey = Color.Purple;
        base.WindowState = FormWindowState.Minimized;
        base.Load += this.Entry_Load_1;
        base.ResumeLayout(false);
    }

    private void Entry_Load_1(object sender, EventArgs e)
    {
    }
}
