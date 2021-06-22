using System;
using System.Drawing;
using System.Windows.Forms;

namespace Model.Script
{
    public class CustomComponent : Component
    {
        public CustomComponent(State state = null, 
            Action<State> tick = null,
            Action load = null,
            Action<State> start = null,
            Action<State> enter = null,
            Action<State> leave = null,
            Action<Point, State> up = null,
            Action<Point, State> down = null,
            Action<Point, State> move = null,
            Action<Graphics, int, int, State> draw = null,
            Action<Keys, bool, bool, bool, State> key = null)
        {
            this.state = state ?? new State();
            this.tick = tick;
            this.load = load;
            this.start = start;
            this.enter = enter;
            this.leave = leave;
            this.up = up;
            this.down = down;
            this.move = move;
            this.draw = draw;
            this.key = key;
        }

        private State state;
        public override State State
        {
            get => state;
            protected set => state = value;
        }

        private Action<State> tick;
        public override void Tick()
        {
            if (tick != null)
                tick(state);
        }

        private Action load;
        public override void Load()
        {
            if (load != null)
                load();
        }
        
        private Action<State> start;
        public override void Start()
        {
            if (start != null)
                start(state);
        }

        private Action<State> enter;
        public override void MouseEnter()
        {
            if (enter != null)
                enter(state);
        }

        private Action<State> leave;
        public override void MouseLeave()
        {
            if (leave != null)
                leave(state);
        }

        private Action<Point, State> up;
        public override void MouseUp(Point p)
        {
            if (up != null)
                up(p, state);
        }

        private Action<Point, State> down;
        public override void MouseDown(Point p)
        {
            if (down != null)
                down(p, state);
        }

        private Action<Point, State> move;
        public override void MouseMove(Point p)
        {
            if (move != null)
                move(p, state);
        }

        private Action<Graphics, int, int, State> draw;
        public override void Draw(Graphics g, int width, int height)
        {
            if (draw != null)
                draw(g, width, height, state);
        }

        private Action<Keys, bool, bool, bool, State> key;
        public override void KeyDown(Keys keys, bool alt, bool ctrl, bool shift)
        {
            if (key != null)
                key(keys, alt, ctrl, shift, state);
        }

        public override Component Clone(State state)
        {
            CustomComponent cc = new CustomComponent();
            cc.tick = tick;
            cc.down = down;
            cc.draw = draw;
            cc.enter = enter;
            cc.leave = leave;
            cc.key = key;
            cc.load = load;
            cc.move = move;
            cc.start = start;
            cc.up = up;
            cc.Start();
            return cc;
        }
    }
}