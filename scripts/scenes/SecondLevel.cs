using AquaPapi.Abstractions;
using Godot;
using System;

namespace AquaPapi.Scenes
{
    public partial class SecondLevel : BaseScene
    {
        

        public override void _Ready()
        {
            base._Ready();
            UserInterface.ShowTreats();
            UserInterface.HideMainButtons();
            UserInterface.ToggleStatsUI(true);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}
