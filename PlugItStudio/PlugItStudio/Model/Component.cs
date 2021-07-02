using System.Drawing;
using System.Windows.Forms;

namespace Model
{
    public abstract class Component
    {
        public abstract State State { get; protected set; }
        public object this[string key]
        {
            get => this.State[key];
            set => this.State[key] = value;
        }
        
        public abstract void Tick();
        public abstract void Load();
        public abstract void Start();
        public abstract void MouseEnter();
        public abstract void MouseLeave();
        public abstract void MouseUp(Point p);
        public abstract void MouseDown(Point p);
        public abstract void MouseMove(Point p);
        public abstract void Draw(Graphics g, int width, int height);
        public abstract void KeyDown(Keys key, bool alt, bool ctrl, bool shift);
        public abstract Component Clone(State state);
    }
}