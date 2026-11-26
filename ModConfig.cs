using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBrain
{
    public sealed class ModConfig
    {
        public bool CfgEnableBundleBrain { get; set; } = true;
        public bool CfgButtonDraggable { get; set; } = true;
        public int CfgButtonOffsetX { get; set; } = 20;
        public int CfgButtonOffsetY { get; set; } = 40;

    }
}
