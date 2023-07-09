using Godot;
using System;

public partial class Info : Control {
	InfoButton infoButton;
	bool isHiddin = true;
	public override void _Ready() {
		Hide();
		infoButton = GetNode<InfoButton>("/root/World/CanvasLayer/UISwitcher/InfoButtonWrapper/InfoButton");
		infoButton.Pressed += () => {
			if(isHiddin) {
				Show();
				isHiddin = false;
			} else {
				Hide();
				isHiddin = true;
			}
		};
	}
}
