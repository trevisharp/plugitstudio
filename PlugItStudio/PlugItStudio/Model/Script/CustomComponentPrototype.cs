using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Model.Script
{
    public class CustomComponentPrototype : ComponentPrototype
    {
        public Action OnLoad { get; set; }
        public Action<Dictionary<string, object>> Tick { get; set; }
        public Action<Dictionary<string, object>> Start { get; set; }
        public Action<Dictionary<string, object>> Enter { get; set; }
        public Action<Dictionary<string, object>> Leave { get; set; }
        public Action<Point, Dictionary<string, object>> Up { get; set; }
        public Action<Point, Dictionary<string, object>> Down { get; set; }
        public Action<Point, Dictionary<string, object>> Move { get; set; }
        public Action<Graphics, int, int, Dictionary<string, object>> Draw { get; set; }
        public Action<Keys, bool, bool, bool, Dictionary<string, object>> Key { get; set; }

        public override Component Instance()
        {
            var component = new CustomComponent(null, Tick, 
                OnLoad, Start, Enter, Leave, Up, Down, Move, Draw, Key);
            component.Start();
            return component;
        }

        public override void Load()
        {
            if (OnLoad != null)
                OnLoad();
        }
    }
}