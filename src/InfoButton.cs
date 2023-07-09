using Godot;
using System;

public partial class InfoButton : TextureButton {
	ShaderMaterial defaultMaterial;
	ShaderMaterial hoverMaterial;
	public override void _Ready() {
		defaultMaterial = ResourceLoader.Load<ShaderMaterial>("res://assets/shaders/Default.tres");
		hoverMaterial = ResourceLoader.Load<ShaderMaterial>("res://assets/shaders/Hover.tres");
		MouseEntered += () => {
			Material = hoverMaterial;
		};
		MouseExited += () => {
			Material = defaultMaterial;
		};
	}

	public override void _Process(double delta) {
	}
}