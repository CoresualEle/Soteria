using Godot;
using System;

public class UserPreferences : Node
{

    private readonly string configFileName = "user://preferences.cfg";
    private ConfigFile config;

    public override void _Ready()
    {
        this.config = new ConfigFile();
    }

    public void LoadPreferences()
    {
        if (this.config is null)
        {
            this.config = new ConfigFile();
        }
        var err = this.config.Load(configFileName);
        if (err != Error.Ok)
        {
            return;
        }
        
        var musicVolume = (float)this.config.GetValue("general","music_volume", -8f);
        var musicIndex = AudioServer.GetBusIndex("Music");
        AudioServer.SetBusVolumeDb(musicIndex, musicVolume);

        var soundVolume = (float)this.config.GetValue("general","sound_volume", -8f);
        var soundIndex = AudioServer.GetBusIndex("Effects");
        AudioServer.SetBusVolumeDb(soundIndex, soundVolume);


        var fullscreen = (bool)this.config.GetValue("general","fullscreen", false);
        OS.WindowFullscreen = fullscreen;
    }

    public void SavePreferences()
    {
        if (this.config is null)
        {
            this.config = new ConfigFile();
        }

        var musicIndex = AudioServer.GetBusIndex("Music");
        var musicVolume = AudioServer.GetBusVolumeDb(musicIndex);
        this.config.SetValue("general", "music_volume", musicVolume);
        
        var soundIndex = AudioServer.GetBusIndex("Effects");
        var soundVolume = AudioServer.GetBusVolumeDb(soundIndex);
        this.config.SetValue("general", "sound_volume", soundVolume);
        
        var fullscreen = OS.WindowFullscreen;
        this.config.SetValue("general", "fullscreen", fullscreen);

        this.config.Save(configFileName);
    }
}
