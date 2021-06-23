namespace Model
{
    public abstract class ComponentPrototype
    {
        public string Name { get; set; }
        public abstract Component Instance();
        public abstract void Load();
    }
}