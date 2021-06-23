using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Model.Script
{
    public class CustomComponentPrototype : ComponentPrototype
    {
        public Action<Action<string>> OnLoad { get; set; }
        public Action<Dictionary<string, object>, Action<string>> Tick { get; set; }
        public Action<Dictionary<string, object>> Start { get; set; }
        public Action<Dictionary<string, object>> Enter { get; set; }
        public Action<Dictionary<string, object>> Leave { get; set; }
        public Action<Point, Dictionary<string, object>, Action<string>> Up { get; set; }
        public Action<Point, Dictionary<string, object>, Action<string>> Down { get; set; }
        public Action<Point, Dictionary<string, object>, Action<string>> Move { get; set; }
        public Action<Graphics, int, int, Dictionary<string, object>> Draw { get; set; }
        public Action<Keys, bool, bool, bool, Dictionary<string, object>, Action<string>> Key { get; set; }

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
                OnLoad(GlobalOperations.Create);
        }
    }
}