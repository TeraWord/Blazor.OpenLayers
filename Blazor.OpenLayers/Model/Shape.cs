using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Shape : Feature
    {
        public Shape() { Kind = this.GetType().Name; ID = Guid.NewGuid(); }

        public string Kind { get; set; }

        public Guid ID { get; set; }

        public bool Popup { get; set; }
        
        [JsonIgnore]
        public string Label { get => GetProperty<string>("label"); set => Properties["label"] = value; }

        [JsonIgnore]
        public string Title { get => GetProperty<string>("title"); set => Properties["title"] = value; }

        [JsonIgnore]
        public string Content { get => GetProperty<string>("content"); set => Properties["content"] = value; }

        public double TextScale { get; set; } = 1;

        public string Color { get; set; } = "#FFFFFF";

        public string BorderColor { get; set; } = "#FFFFFF";

        public int BorderSize { get; set; } = 1;

        public string BackgroundColor { get; set; } = "#000000";

        public double Radius { get; set; }

        private T GetProperty<T>(string key)
        {
            return Properties.ContainsKey(key) ? (T)Properties[key] : default(T);
        }
    }
}
