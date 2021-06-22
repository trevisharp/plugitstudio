using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using Model.Script;

public class FrmMain : Form
{
    private Component c;
    
    private Picture p;
    private Graphics g;
    private PictureBox pb;
    private Timer tm;

    private Color c1 = Color.FromArgb(18, 18, 18);
    
    public FrmMain()
    {
        this.WindowState = FormWindowState.Maximized;
        this.FormBorderStyle = FormBorderStyle.None;
        this.KeyPreview = true;

        this.pb = new PictureBox();
        this.pb.Dock = DockStyle.Fill;
        this.Controls.Add(pb);

        this.tm = new Timer();
        this.tm.Interval = 20;

        this.PreviewKeyDown += (sender, e) =>
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
            c.KeyDown(e.KeyCode, e.Alt, e.Control, e.Shift);
        };

        this.Load += async delegate
        {
            p = new Picture(pb.Width, pb.Height);
            g = p.Graphics;
            g.Clear(c1);
            pb.Image = p.Bitmap;

            string code = @"
                int r = 0;
                behavior draw
                {
                    clear(color((byte)(r % 255), 0, 0));
                }

                behavior tick
                {
                    r++;
                }
                
                behavior key
                {
                    if (keys == Keys.A)
                        r = 0;
                    if (keys == Keys.B)
                        r = 128;
                }
            ";
            ScriptAnalyzer sa = new ScriptAnalyzer();
            var prototype = await sa.Compile(code);
            var component = prototype.Instance();
            c = component;
            
            tm.Start();
        };

        tm.Tick += delegate
        {
            g.Clear(c1);
            c.Tick();
            c.Draw(g, pb.Width, pb.Height);
            pb.Refresh();
        };
    }
}