using Godot;
using System.Collections.Generic;

namespace Soteria {
    public class Audio : Node2D
    {
        private AudioStreamPlayer musicPlayerPads;
        private AudioStreamPlayer musicPlayerBass;
        private AudioStreamPlayer musicPlayerDrums;
        private AudioStreamPlayer effectsPlayer;
        private Tween tween;

        public override void _Ready()
        {
            this.musicPlayerPads = this.GetNode<AudioStreamPlayer>("music-pads");
            this.musicPlayerBass = this.GetNode<AudioStreamPlayer>("music-bass");
            this.musicPlayerDrums = this.GetNode<AudioStreamPlayer>("music-drums");
            this.effectsPlayer = this.GetNode<AudioStreamPlayer>("effects");
            this.tween = this.GetNode<Tween>("Tween");
        }
    
        public void AddBass()
        {
            this.tween.InterpolateProperty(this.musicPlayerBass, "volume_db", -80, -6, 2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
            this.tween.Start();
        }
    
        public void RemoveAll()
        {
            this.tween.InterpolateProperty(this.musicPlayerBass, "volume_db", -6, -80, 2f, Tween.TransitionType.Sine, Tween.EaseType.In);
            this.RemoveDrums();
            this.tween.Start();
        }
    
        public void AddDrums()
        {
            this.tween.InterpolateProperty(this.musicPlayerDrums, "volume_db", -80, -6, 2f, Tween.TransitionType.Sine, Tween.EaseType.Out);
            this.tween.Start();
        }
        public void RemoveDrums()
        {
            this.tween.InterpolateProperty(this.musicPlayerDrums, "volume_db", -6, -80, 2f, Tween.TransitionType.Sine, Tween.EaseType.In);
            this.tween.Start();
        }


    }
}
