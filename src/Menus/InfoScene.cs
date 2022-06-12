using Godot;
using Soteria;

namespace Soteria.Menus {
    public class InfoScene : CanvasLayer
    {
        private GameVariables gameVariables;
        
        public string Title;
        public string Description;

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            var titleLabel = this.GetNode<Label>("Container/Title");
            titleLabel.Text = this.Title;
            var descriptionLabel = this.GetNode<Label>("Container/Description");
            descriptionLabel.Text = this.Description;
            var closeButton = this.GetNode<Button>("CloseButton");

            closeButton.Connect(nameof(BaseButton._Pressed), this, nameof(this.OnCloseButtonPressed));
            this.gameVariables.SetTimeScale(0);
        }

        private void OnCloseButtonPressed()
        {
            // TODO Maybe set timescale back to old value
            this.QueueFree();
        }
    }
}
