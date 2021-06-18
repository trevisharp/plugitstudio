using System.IO;
using Model.Script;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Model
{
    public class Plugin
    {
        private List<Component> components = new List<Component>();
        public IEnumerable<Component> Components => this.components;
        
        public static async Task<Plugin> Open(string path)
        {
            Plugin plugin = new Plugin();
            using (var archive = ZipFile.OpenRead(path))
            {
                foreach (var entry in archive.Entries)
                {
                    using (var stream = entry.Open())
                    {
                        var reader = new StreamReader(stream);
                        var code = await reader.ReadToEndAsync();
                        plugin.components.Add(await CustomComponent.Create(code));
                    }
                }
            }
            return plugin;
        }
    }
}