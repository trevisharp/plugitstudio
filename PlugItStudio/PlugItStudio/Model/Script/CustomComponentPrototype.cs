using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Model.Script
{
    public class CustomComponentPrototype : ComponentPrototype
    {
        public Action Load { get; set; }
        public Action<State> Tick { get; set; }
        public Action<State> Start { get; set; }
        public Action<State> Enter { get; set; }
        public Action<State> Leave { get; set; }
        public Action<Point, State> Up { get; set; }
        public Action<Point, State> Down { get; set; }
        public Action<Point, State> Move { get; set; }
        public Action<Graphics, int, int, State> Draw { get; set; }
        public Action<Keys, bool, bool, bool, State> Key { get; set; }

        public override Component Instance()
        {
            var component = new CustomComponent(null, Tick, 
                Load, Start, Enter, Leave, Up, Down, Move, Draw, Key);
            component.Start();
            return component;
        }
    }
}