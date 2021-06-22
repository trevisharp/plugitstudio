using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Model.Script
{
    public class ScriptAnalyzer
    {
        public async Task<CustomComponentPrototype> Compile(string code)
        {
            CustomComponentPrototype ccp = new CustomComponentPrototype();
            StringBuilder sb = new StringBuilder();
            sb.Append(code);
            
            void addbehavior(string name, string arguments)
            {
                sb = code.Contains(name)
                    ? sb.Replace(name, $"void {name}({arguments})")
                    : sb.Append($"\nvoid {name}({arguments}) {{ }}");
            }
            
            addbehavior("draw", "Graphics g, int width, int height, Dictionary<string, object> state");
            addbehavior("tick", "Dictionary<string, object> state");
            addbehavior("load", "");
            addbehavior("start", "Dictionary<string, object> state");

            sb = sb
                .Replace("clear", "g.Clear")
                .Replace("fillrect", "g.FillRectangle")
                .Replace("color", "Color.FromArgb");

            sb.Append(@"new object[]{ 
                     (Action<Graphics, int, int, Dictionary<string, object>>)draw,
                     (Action<Dictionary<string, object>>)tick,
                     (Action)load,
                     (Action<Dictionary<string, object>>)start,
                      }");

            code = sb.ToString();
            
            var options = ScriptOptions.Default
                .AddReferences(typeof(Graphics).Assembly)
                .AddReferences(typeof(Object).Assembly)
                .AddImports("System")
                .AddImports("System.Collections.Generic")
                .AddImports("System.Drawing");

            var result = await CSharpScript.EvaluateAsync(code, options) as object[];

            ccp.Draw = result[0] as Action<Graphics, int, int, Dictionary<string, object>>;
            ccp.Tick = result[1] as Action<Dictionary<string, object>>;
            ccp.Load = result[2] as Action;
            ccp.Start = result[3] as Action<Dictionary<string, object>>;

            return ccp;
        }
    }
}