using Godot;
using System.Collections.Generic;

// #nullable enable

public class Selectable : Node2D {

	[Export] public NodePath hoverIndicatorPath;
	[Export] public NodePath selectIndicatorPath;
	[Export] public Rect2 selectArea;

	public Sprite hoverIndicator;
	public Sprite selectIndicator;

	public override void _Ready() {
		base._Ready();
		hoverIndicator = GetNode<Sprite>(hoverIndicatorPath);
		selectIndicator = GetNode<Sprite>(selectIndicatorPath);
	}

	public override void _EnterTree() {
		base._EnterTree();
		Population.Add(this);
	}

	public override void _ExitTree() {
		Population.Remove(this);
		base._ExitTree();
	}

	public static List<Selectable> Population = new List<Selectable>();

#if !GODOT_EXPORT
	public override void _Draw() {
		base._Draw();
		DrawRect(selectArea, Colors.Red, filled:false);
	}

#endif
}
