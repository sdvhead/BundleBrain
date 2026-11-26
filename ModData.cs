using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBrain
{
    internal sealed class ModData
    {
        public List<Room> Rooms { get; set; } = new();
        public List<Bundle> Bundles { get; set; } = new();
        public List<BundleOption> BundleOptions { get; set; } = new();


    }

    internal sealed class Room
    {
        public string Name { get; set; } = "";
    }
    internal sealed class Bundle
    {
        public string Name { get; set; } = "";
    }
    internal sealed class BundleOption
    {
        public string Name { get; set; } = "";
    }

}
