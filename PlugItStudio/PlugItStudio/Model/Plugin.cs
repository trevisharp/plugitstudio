using System.IO;
using Model.Script;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model
{
    public class Plugin
    {
        private List<ComponentPrototype> components = new List<ComponentPrototype>();
        public IEnumerable<ComponentPrototype> Components => this.components;
        
        public static async Task<Plugin> Open(string path)
        {
            Plugin plugin = new Plugin();
            ScriptAnalyzer analyzer = new ScriptAnalyzer();
            using (var archive = ZipFile.OpenRead(path))
            {
                foreach (var entry in archive.Entries)
                {
                    using (var stream = entry.Open())
                    {
                        var reader = new StreamReader(stream);
                        var code = await reader.ReadToEndAsync();
                        plugin.components.Add(await analyzer.Compile(code));
                    }
                }
            }
            foreach (var prototype in plugin.Components)
                prototype.Load();
            return plugin;
        }
    }
}