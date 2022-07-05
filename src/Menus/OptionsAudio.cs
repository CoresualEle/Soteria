using Godot;
using System.Collections.Generic;

namespace Soteria.Menus
{
    public class OptionsAudio : CanvasLayer
    {
        [Signal]
        public delegate void BackButtonPressed();

        private HSlider musicVolumeSlider;
        private Label musicVolumeValue;

        private HSlider soundVolumeSlider;
        private Label soundVolumeValue;

        public override void _Ready()
        {
            this.musicVolumeSlider = this.GetNode<HSlider>("VBoxContainer/MusicVolumeContainer/Slider");
            this.musicVolumeValue = this.GetNode<Label>("VBoxContainer/MusicVolumeContainer/Value");
            this.soundVolumeSlider = this.GetNode<HSlider>("VBoxContainer/SoundVolumeContainer/Slider");
            this.soundVolumeValue = this.GetNode<Label>("VBoxContainer/SoundVolumeContainer/Value");


            this.musicVolumeSlider.GrabFocus();
            
            this.musicVolumeSlider.Connect("value_changed", this, nameof(ChangeMusicVolume));
            this.soundVolumeSlider.Connect("value_changed", this, nameof(ChangeSoundVolume));
            
            var music_index = AudioServer.GetBusIndex("Music");
            var music_value = this.VolumeToValue(AudioServer.GetBusVolumeDb(music_index));
            this.musicVolumeSlider.Value = music_value;
            this.musicVolumeValue.Text = ((int) music_value).ToString();

            var sound_index = AudioServer.GetBusIndex("Effects");
            var sound_value = this.VolumeToValue(AudioServer.GetBusVolumeDb(sound_index));
            this.soundVolumeSlider.Value = sound_value;
            this.soundVolumeValue.Text = ((int) sound_value).ToString();

            var backButton = this.GetNode<Button>("VBoxContainer/BackButton");
            backButton.Connect("pressed", this, nameof(_on_BackButton_pressed));
        }

        private void _on_BackButton_pressed()
        {
            this.GetNode<UserPreferences>("/root/UserPreferences").SavePreferences();
            this.QueueFree();
            this.EmitSignal(nameof(BackButtonPressed));
        }

        private float VolumeToValue(float volume)
        {
            var value = (volume + 80f) / 7.4f;
            return Mathf.Round(value);
        }

        private float ValueToVolume(float value)
        {
            return -80f + (7.4f*value);
        }

        private void ChangeMusicVolume(float value)
        {
            this.musicVolumeValue.Text = ((int) value).ToString();
            var music_index = AudioServer.GetBusIndex("Music");
            AudioServer.SetBusVolumeDb(music_index, ValueToVolume(value));
        }

        private void ChangeSoundVolume(float value)
        {
            this.soundVolumeValue.Text = ((int) value).ToString();
            var sound_index = AudioServer.GetBusIndex("Effects");
            AudioServer.SetBusVolumeDb(sound_index, ValueToVolume(value));
        }


    }
}
