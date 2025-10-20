using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeraWord.Blazor.OpenLayers;

namespace Demo
{
    public class LayerBaidu : Layer
    { 
        public LayerBaidu()
        {
            Kind = LayerKind.Tile;
            //Url = "http://online1.map.bdimg.com/onlinelabel/?qt=tile&x={x}&y={y}&z={z}&styles=pl&scaler=1";
            Url = "http://online0.map.bdimg.com/tile/?qt=tile&x={x}&y={y}&z={z}&styles=sl&scaler=1";
            //Url = "https://gss{s}.bdstatic.com/8bo_dTSlRsgBo1vgoIiO_jowehsv/tile/?qt=tile&x={x}&y={y}&z={z}&styles=pl&scaler=1&udt=20170927";
            Attributions = "Baidu";
        }
    }
}
