using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Model.Script
{
    public class CustomComponent : Component
    {
        private Dictionary<string, object> state = new Dictionary<string, object>();
        
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

        public override Component Clone(Dictionary<string, object> state)
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

        //Refactor
        public static async Task<CustomComponent> Create(string code)
        {
            code += "\n\nnew object[] { ";
            if (code.Contains(("behavior draw")))
            {
                code = code.Replace("behavior draw", 
                           "Action<Graphics, int, int, Dictionary<string, object>> draw = (g, width, height, state) => {")
                       + "draw,";
            }
            else code += "null,";
            
            if (code.Contains(("behavior tick")))
            {
                code = code.Replace("behavior tick", "Action<Dictionary<string, object>> tick = (state) => {")
                       + "tick,";
            }
            else code += "null,";
            
            if (code.Contains(("behavior load")))
            {
                code = code.Replace("behavior load", "Action<Dictionary<string, object>> load = (state) => {")
                       + "load,";
            }
            else code += "null,";
            
            if (code.Contains(("behavior start")))
            {
                code = code.Replace("behavior start", "Action<Dictionary<string, object>> start = (state) => {")
                       + "start,";
            }
            else code += "null,";
            
            code = code
                .Replace("create()", "")
                .Replace("end", "};")
                .Replace("clear", "g.Clear")
                .Replace("fillrect", "g.FillRectangle")
                .Replace("color", "Color.FromArgb")
                             + " }";

            var options = ScriptOptions.Default
                .AddReferences(typeof(Graphics).Assembly)
                .AddReferences(typeof(Object).Assembly)
                .AddImports("System")
                .AddImports("System.Collections.Generic")
                .AddImports("System.Drawing");

            var result = await CSharpScript.EvaluateAsync(code, options) as object[];
            CustomComponent cc = new CustomComponent();
            if (result == null)
                return cc;

            cc.draw = result[0] as Action<Graphics, int, int, Dictionary<string, object>>;
            cc.tick = result[1] as Action<Dictionary<string, object>>;
            cc.load = result[2] as Action;
            cc.start = result[3] as Action<Dictionary<string, object>>;

            return cc;
        }
    }
}