﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Model.Script
{
    public class CustomComponent : Component
    {
        public CustomComponent(State state = null, 
            Action<Dictionary<string, object>> tick = null,
            Action load = null,
            Action<Dictionary<string, object>> start = null,
            Action<Dictionary<string, object>> enter = null,
            Action<Dictionary<string, object>> leave = null,
            Action<Point, Dictionary<string, object>> up = null,
            Action<Point, Dictionary<string, object>> down = null,
            Action<Point, Dictionary<string, object>> move = null,
            Action<Graphics, int, int, Dictionary<string, object>> draw = null,
            Action<Keys, bool, bool, bool, Dictionary<string, object>> key = null)
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

        private Action<Dictionary<string, object>> tick;
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
        
        private Action<Dictionary<string, object>> start;
        public override void Start()
        {
            if (start != null)
                start(state);
        }

        private Action<Dictionary<string, object>> enter;
        public override void MouseEnter()
        {
            if (enter != null)
                enter(state);
        }

        private Action<Dictionary<string, object>> leave;
        public override void MouseLeave()
        {
            if (leave != null)
                leave(state);
        }

        private Action<Point, Dictionary<string, object>> up;
        public override void MouseUp(Point p)
        {
            if (up != null)
                up(p, state);
        }

        private Action<Point, Dictionary<string, object>> down;
        public override void MouseDown(Point p)
        {
            if (down != null)
                down(p, state);
        }

        private Action<Point, Dictionary<string, object>> move;
        public override void MouseMove(Point p)
        {
            if (move != null)
                move(p, state);
        }

        private Action<Graphics, int, int, Dictionary<string, object>> draw;
        public override void Draw(Graphics g, int width, int height)
        {
            if (draw != null)
                draw(g, width, height, state);
        }

        private Action<Keys, bool, bool, bool, Dictionary<string, object>> key;
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