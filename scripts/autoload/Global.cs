using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaPapi.Autoload
{
    public partial class Global : Node
    {
        [Signal]
        public delegate void PlayerGotDroppedEventHandler();

        private bool isInWater;

        public bool IsInWater
        {
            get { return isInWater; }
            set 
            { 
                isInWater = value;
                if (value)
                {
                    EmitSignal(SignalName.PlayerGotDropped);
                }
            }
        }

        public int Treats { get; set; }

        public int Oxygen { get; set; } = 5;
        public int MaxOxygen { get; set; } = 5;
    }
}
