using Godot;
using System;

public class animatedSpriteLabel : Label {

	public override void _Process(float delta) {
		Text = GetParent<DirectionalAnimatedSprite>().label;
		base._Process(delta);
	}
}
