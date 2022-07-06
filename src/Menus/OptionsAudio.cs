using Godot;

namespace Soteria.Menus
{
    public class OptionsAudio : CanvasLayer
    {
        private HSlider musicVolumeSlider;
        private Label musicVolumeValue;

        private HSlider soundVolumeSlider;
        private Label soundVolumeValue;

        [Signal]
        public delegate void BackButtonPressed();

        public override void _Ready()
        {
            this.musicVolumeSlider = this.GetNode<HSlider>("VBoxContainer/MusicVolumeContainer/Slider");
            this.musicVolumeValue = this.GetNode<Label>("VBoxContainer/MusicVolumeContainer/Value");
            this.soundVolumeSlider = this.GetNode<HSlider>("VBoxContainer/SoundVolumeContainer/Slider");
            this.soundVolumeValue = this.GetNode<Label>("VBoxContainer/SoundVolumeContainer/Value");

            this.musicVolumeSlider.GrabFocus();

            this.musicVolumeSlider.Connect("value_changed", this, nameof(this.ChangeMusicVolume));
            this.soundVolumeSlider.Connect("value_changed", this, nameof(this.ChangeSoundVolume));

            var musicIndex = AudioServer.GetBusIndex("Music");
            var musicValue = this.VolumeToValue(AudioServer.GetBusVolumeDb(musicIndex));
            this.musicVolumeSlider.Value = musicValue;
            this.musicVolumeValue.Text = ((int)musicValue).ToString();

            var soundIndex = AudioServer.GetBusIndex("Effects");
            var soundValue = this.VolumeToValue(AudioServer.GetBusVolumeDb(soundIndex));
            this.soundVolumeSlider.Value = soundValue;
            this.soundVolumeValue.Text = ((int)soundValue).ToString();

            var backButton = this.GetNode<Button>("VBoxContainer/BackButton");
            backButton.Connect("pressed", this, nameof(this._on_BackButton_pressed));
        }

        private void _on_BackButton_pressed()
        {
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
            return -80f + 7.4f * value;
        }

        private void ChangeMusicVolume(float value)
        {
            this.musicVolumeValue.Text = ((int)value).ToString();
            var musicIndex = AudioServer.GetBusIndex("Music");
            AudioServer.SetBusVolumeDb(musicIndex, this.ValueToVolume(value));
        }

        private void ChangeSoundVolume(float value)
        {
            this.soundVolumeValue.Text = ((int)value).ToString();
            var soundIndex = AudioServer.GetBusIndex("Effects");
            AudioServer.SetBusVolumeDb(soundIndex, this.ValueToVolume(value));
        }
    }
}