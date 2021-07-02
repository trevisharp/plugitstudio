using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Text.RegularExpressions;

namespace Model.Script
{
    public class ScriptAnalyzer
    {
        public async Task<CustomComponentPrototype> Compile(string code)
        {
            CustomComponentPrototype ccp = new CustomComponentPrototype();
            StringBuilder sb = new StringBuilder();
            sb.Append("var __rand = new Random(DateTime.Now.Millisecond);\n");
            sb.Append(code);

            void addbehavior(string name, string arguments)
            {
                sb = code.Contains("behavior " + name)
                    ? sb.Replace("behavior " + name, $"void {name}({arguments})")
                    : sb.Append($"\nvoid {name}({arguments}) {{ }}");
            }
            
            addbehavior("draw", "Graphics g, int width, int height, Dictionary<string, object> state");
            addbehavior("tick", "Dictionary<string, object> state, Action<string> create");
            addbehavior("load", "Action<string> create");
            addbehavior("start", "Dictionary<string, object> state");
            addbehavior("up", "Point point, Dictionary<string, object> state, Action<string> create");
            addbehavior("move", "Point point, Dictionary<string, object> state, Action<string> create");
            addbehavior("down", "Point point, Dictionary<string, object> state, Action<string> create");
            addbehavior("key", "Keys keys, bool alt, bool ctrl, bool shift, Dictionary<string, object> state, Action<string> create");

            sb = sb
                .Replace("clear", "g.Clear")
                .Replace("random", "__rand.Next")   
                .Replace("fillrect", "g.FillRectangle")
                .Replace("brush", "new SolidBrush")
                .Replace("color", "Color.FromArgb");

            sb.Append("\n\n" + @"new object[]{ 
                     (Action<Graphics, int, int, Dictionary<string, object>>)draw,
                     (Action<Dictionary<string, object>, Action<string>>)tick,
                     (Action<Action<string>>)load,
                     (Action<Dictionary<string, object>>)start,
                     (Action<Point, Dictionary<string, object>, Action<string>>)up,
                     (Action<Point, Dictionary<string, object>, Action<string>>)move,
                     (Action<Point, Dictionary<string, object>, Action<string>>)down,
                     (Action<Keys, bool, bool, bool, Dictionary<string, object>, Action<string>>)key
                      }");

            code = sb.ToString();

            Regex regex = new Regex("(create)[ ]+[a-zA-Z_][a-zA-Z_0-9]*");
            foreach (Match match in regex.Matches(code))
            {
                code = code.Replace(match.Value, "create(\"" + 
                    match.Value.Substring(6).TrimStart() + "\");");
            }

            regex = new Regex("(state)[ ]*(\\.)[ ]*[a-zA-Z_][a-zA-Z_0-9]*");
            foreach (Match match in regex.Matches(code))
            {
                code = code.Replace(match.Value, "state[\"" +
                    match.Value.Substring(5).TrimStart().Substring(1).TrimStart() + "\"]");
            }

            var options = ScriptOptions.Default
                .AddReferences(typeof(Graphics).Assembly)
                .AddReferences(typeof(Object).Assembly)
                .AddReferences(typeof(Keys).Assembly)
                .AddImports("System")
                .AddImports("System.Windows.Forms")
                .AddImports("System.Collections.Generic")
                .AddImports("System.Drawing");

            var result = await CSharpScript.EvaluateAsync(code, options) as object[];

            ccp.Draw = result[0] as Action<Graphics, int, int, Dictionary<string, object>>;
            ccp.Tick = result[1] as Action<Dictionary<string, object>, Action<string>>;
            ccp.OnLoad = result[2] as Action<Action<string>>;
            ccp.Start = result[3] as Action<Dictionary<string, object>>;
            ccp.Up = result[4] as Action<Point, Dictionary<string, object>, Action<string>>;
            ccp.Down = result[5] as Action<Point, Dictionary<string, object>, Action<string>>;
            ccp.Move = result[6] as Action<Point, Dictionary<string, object>, Action<string>>;
            ccp.Key = result[7] as Action<Keys, bool, bool, bool, Dictionary<string, object>, Action<string>>;

            return ccp;
        }
    }
}