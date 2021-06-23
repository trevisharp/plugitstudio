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
    private List<ComponentPrototype> _prototypes = new List<ComponentPrototype>();
    private List<Component> _components = new List<Component>();
    
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

        GlobalOperations.CreateBehavior = s =>
        {
            foreach (var prototype in this._prototypes)
            {
                if (prototype.Name == s)
                    this._components.Add(prototype.Instance());
            }
        };

        this.PreviewKeyDown += async (sender, e) =>
        {
            if (e.KeyCode == Keys.Oemtilde)
            {
                if (e.Control)
                    Application.Exit();
                else if (e.Shift)
                {
                }
                else if (e.Alt)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var file in ofd.FileNames)
                        {
                            try
                            {
                                var plugin = await Plugin.Open(file);
                                this._prototypes.AddRange(plugin.Components);
                            }
                            catch { }
                        }
                        foreach (var prototype in this._prototypes)
                            prototype.Load();
                    }
                }
            }
            else
            {
                foreach (var component in _components)
                    component.KeyDown(e.KeyCode, e.Alt, e.Control, e.Shift);
            }
            foreach (var component in _components)
                component.Tick();
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
            foreach (var component in _components)
                component.Tick();
            foreach (var component in _components)
                component.Draw(g, pb.Width, pb.Height);
            pb.Refresh();
        };
    }
}