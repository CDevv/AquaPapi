using AquaPapi.Autoload;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaPapi.Abstractions
{
    public partial class BaseScene : Node2D
    {
        protected Global Global { get; set; }
        public Camera2D Camera { get; protected set; }

        public override void _Ready()
        {
            Global = GetNode<Global>("/root/Global");
            Camera = GetNode<Camera2D>("Camera");

            Global.CurrentScene = this;
        }
    }
}
