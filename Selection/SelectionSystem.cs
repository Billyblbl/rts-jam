using Godot;
using System.Collections.Generic;
using System.Linq;

// #nullable enable

public class SelectionSystem : Node2D {

	[Export] public string selectActionName;
	[Export] public string additiveModifier;
	[Export] public Color selectionRectangleColorFill = new Color(0f, 1f, 0f, .5f);
	[Export] public Color selectionRectangleColorBorder = Colors.Green;

	Rect2 selectionRect;
	Vector2 selectStart;
	Vector2 selectEnd;
	bool dragging = false;

	public Selectable[] selection = new Selectable[0];
	public Selectable[] hovered = new Selectable[0];

	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (@event is InputEventMouseMotion eventMouseMotion) {
			dragging = Input.IsActionPressed(selectActionName);
		}
	}

	Rect2 RectContaining(Vector2 A, Vector2 B) {
		var min = new Vector2(Mathf.Min(A.x, B.x), Mathf.Min(A.y, B.y));
		var max = new Vector2(Mathf.Max(A.x, B.x), Mathf.Max(A.y, B.y));
		return new Rect2(min, max - min);
	}

	public override void _Process(float delta) {

		var selectDown = Input.IsActionJustPressed(selectActionName);
		var selectUp = Input.IsActionJustReleased(selectActionName);
		var additive = Input.IsActionPressed(additiveModifier);
		var mousePos = GetGlobalMousePosition();

		selectEnd = mousePos;
		selectionRect = RectContaining(selectStart, selectEnd);

		foreach (var selectable in selection) { selectable.selectIndicator.Hide(); }
		foreach (var selectable in hovered) { selectable.hoverIndicator.Hide(); }

		if (selectDown) {
			selectStart = mousePos;
			selectEnd = mousePos;
		} else if (selectUp) {
			selection = ResolveSelection(additive ? selection : null);
			dragging = false;
		}
		hovered = ResolveSelection();

		foreach (var selectable in selection) { selectable.selectIndicator.Show(); }
		foreach (var selectable in hovered) { selectable.hoverIndicator.Show(); }

		Update();

	}

	Selectable[] ResolveSelection(Selectable[] oldSelection = null) {
		var newSelection = oldSelection != null ? new List<Selectable>(oldSelection) : new List<Selectable>();
		if (dragging) {
			newSelection.AddRange(GetSelectablesInRect(selectionRect).Where(s => !newSelection.Contains(s)));
		} else {
			var underCursor = GetSelectableUnderCursor(selectEnd);
			if (underCursor != null)
			newSelection.Add(underCursor);
		}
		return newSelection.ToArray();
	}

	Selectable[] GetSelectablesInRect(Rect2 rect) => Selectable.Population.Where(s => rect.HasPoint(s.GlobalPosition)).ToArray();
	Selectable	GetSelectableUnderCursor(Vector2 cursor) => Selectable.Population
			.Where(s => s.selectArea.HasPoint(s.ToLocal(cursor)))
			.Aggregate<Selectable, Selectable>(null, (smaller, next) => smaller != null && (smaller.GlobalPosition - cursor).Length() < (next.GlobalPosition - cursor).Length() ? smaller : next);

	public override void _Draw() {
		if (dragging) {
			DrawRect(selectionRect, selectionRectangleColorFill, filled: true);
			DrawRect(selectionRect, selectionRectangleColorBorder, filled: false, width: 2);
		}
	}
}
