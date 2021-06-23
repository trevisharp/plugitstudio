using System;

namespace Model
{
    public static class GlobalOperations
    {
        public static Action<string> CreateBehavior { get; set; }
        public static void Create(string componentname)
        {
            if (CreateBehavior != null)
                CreateBehavior(componentname);
        }
    }
}