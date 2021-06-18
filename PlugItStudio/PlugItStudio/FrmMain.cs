using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

public class FrmMain : Form
{
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
        };

        this.Load += async delegate
        {
            p = new Picture(pb.Width, pb.Height);
            g = p.Graphics;
            g.Clear(c1);
            pb.Image = p.Bitmap;
            tm.Start();
        };

        tm.Tick += delegate
        {
            g.Clear(c1);
            pb.Refresh();
        };
    }
}