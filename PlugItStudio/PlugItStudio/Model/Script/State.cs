using System.Collections.Generic;
using System.Windows.Forms;

namespace Model.Script
{
    public class State
    {
        public object this[string key]
        {
            get => statevalue[key];
            set => statevalue[key] = value;
        }
        
        private Dictionary<string, object> statevalue = new Dictionary<string, object>();
        public void Add(string key, object value)
            => this.statevalue.Add(key, value);
        public static implicit operator State(Dictionary<string, object> dictionary)
        {
            State state = new State();
            foreach (var key in dictionary.Keys)
                state.Add(key, dictionary[key]);
            return state;
        }
        public static implicit operator Dictionary<string, object>(State state)
            => state.statevalue;
    }
}