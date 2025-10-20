using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Defaults
    {
        public bool AutoPopup { get; set; }

        public string Label { get; set; } 

        public string Color { get; set; } = "#FFFFFF";

        public string BackgroundColor { get; set; } = "#0000FF";

        public string BorderColor { get; set; } = "#FFFFFF";

        public string PopupTitleProperty { get; set; } = "title";

        public string PopupContentProperty { get; set; } = "content";
    }
}
