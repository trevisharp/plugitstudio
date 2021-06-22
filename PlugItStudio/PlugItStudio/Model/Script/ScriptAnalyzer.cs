using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Model.Script
{
    public class ScriptAnalyzer
    {
        public async Task<CustomComponentPrototype> Compile(string code)
        {
            CustomComponentPrototype ccp = new CustomComponentPrototype();

            code += "\n\nnew object[] { ";
            if (code.Contains(("behavior draw")))
            {
                code = code.Replace("behavior draw", 
                           "Action<Graphics, int, int, State> draw = (g, width, height, state) => {")
                       + "draw,";
            }
            else code += "null,";
            
            if (code.Contains(("behavior tick")))
            {
                code = code.Replace("behavior tick", "Action<State> tick = (state) => {")
                       + "tick,";
            }
            else code += "null,";
            
            if (code.Contains(("behavior load")))
            {
                code = code.Replace("behavior load", "Action<State> load = (state) => {")
                       + "load,";
            }
            else code += "null,";
            
            if (code.Contains(("behavior start")))
            {
                code = code.Replace("behavior start", "Action<State> start = (state) => {")
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

            return ccp;
        }
    }
}