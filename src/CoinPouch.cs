using Godot;
using System;

public partial class CoinPouch : TextureButton {
	bool isHiddin = true;
	RichTextLabel coinsEarned;
	public override void _Ready() {
		coinsEarned = GetNode<RichTextLabel>("/root/World/CanvasLayer/CoinsAmount");
		coinsEarned.Hide();
		MouseEntered += () => {
			coinsEarned.Show();
		};
		MouseExited += () => {
			coinsEarned.Hide();
		};
	}
}
