using Godot;
using System.Collections.Generic;
using System.Linq;

// #nullable enable

public class SelectionSystem : Node2D {

	[Export] public string selectActionName;
	[Export] public string additiveModifier;
	[Export] public string controlGroupEdit;
	[Export] public string contextOrder;
	[Export] public string[] controlGroupKeys;
	[Export] public Color selectionRectangleColorFill = new Color(0f, 1f, 0f, .5f);
	[Export] public Color selectionRectangleColorBorder = Colors.Green;

	Rect2 selectionRect;
	Vector2 selectStart;
	Vector2 selectEnd;
	bool dragging = false;

	public Controllable[] selection = new Controllable[0];
	public Controllable[] hovered = new Controllable[0];
	public Controllable[][] controlGroups = new Controllable[10][];

	public void GiveOrderToSelection(Order order, bool additive = false) {
		foreach (var controllable in selection) {
			controllable.GiveOrder(order, additive);
		}
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (@event is InputEventMouseMotion eventMouseMotion) {
			dragging = Input.IsActionPressed(selectActionName);
		} else if (selection != null && selection.Length > 0 && @event.IsAction(contextOrder) && @event.IsPressed()) {
			GiveOrderToSelection(ResolveContextOrder(GetGlobalMousePosition()), additive: Input.IsActionPressed(additiveModifier));
		} else foreach (var (action, i) in controlGroupKeys.Select((a,i) => (a,i))) if (@event.IsAction(action) && @event.IsPressed()) {
			if (Input.IsActionPressed(controlGroupEdit)) {
				controlGroups[i] = selection;
			} else {
				HideIndicators();
				selection = controlGroups[i];
				ShowIndicators();
			}
		}
	}

	public Order ResolveContextOrder(Vector2 position) {
		//TODO different unit orders
		return new MoveOrder(position, this);
	}

	Rect2 RectContaining(Vector2 A, Vector2 B) {
		var min = new Vector2(Mathf.Min(A.x, B.x), Mathf.Min(A.y, B.y));
		var max = new Vector2(Mathf.Max(A.x, B.x), Mathf.Max(A.y, B.y));
		return new Rect2(min, max - min);
	}

	void HideIndicators() {
		if (selection != null) foreach (var selectable in selection) { selectable.selectIndicator.Hide(); }
		if (hovered != null) foreach (var selectable in hovered) { selectable.hoverIndicator.Hide(); }
	}

	void ShowIndicators() {
		if (selection != null) foreach (var selectable in selection) { selectable.selectIndicator.Show(); }
		if (hovered != null) foreach (var selectable in hovered) { selectable.hoverIndicator.Show(); }
	}

	public override void _Process(float delta) {

		var selectDown = Input.IsActionJustPressed(selectActionName);
		var selectUp = Input.IsActionJustReleased(selectActionName);
		var additive = Input.IsActionPressed(additiveModifier);
		var mousePos = GetGlobalMousePosition();

		selectEnd = mousePos;
		selectionRect = RectContaining(selectStart, selectEnd);

		HideIndicators();
		if (selectDown) {
			selectStart = mousePos;
			selectEnd = mousePos;
		} else if (selectUp) {
			selection = ResolveSelection(additive ? selection : null);
			dragging = false;
		}
		hovered = ResolveSelection();
		ShowIndicators();
		Update();
	}

	Controllable[] ResolveSelection(Controllable[] oldSelection = null) {
		var newSelection = oldSelection != null ? new List<Controllable>(oldSelection) : new List<Controllable>();
		if (dragging) {
			newSelection.AddRange(GetSelectablesInRect(selectionRect).Where(s => !newSelection.Contains(s)));
		} else {
			var underCursor = GetSelectableUnderCursor(selectEnd);
			if (underCursor != null)
			newSelection.Add(underCursor);
		}
		return newSelection.ToArray();
	}

	Controllable[] GetSelectablesInRect(Rect2 rect) => Controllable.Population.Where(s => rect.HasPoint(s.GlobalPosition)).ToArray();
	Controllable	GetSelectableUnderCursor(Vector2 cursor) => Controllable.Population
			.Where(s => s.selectArea.HasPoint(s.ToLocal(cursor)))
			.Aggregate<Controllable, Controllable>(null, (smaller, next) => smaller != null && (smaller.GlobalPosition - cursor).Length() < (next.GlobalPosition - cursor).Length() ? smaller : next);

	public override void _Draw() {
		if (dragging) {
			DrawRect(selectionRect, selectionRectangleColorFill, filled: true);
			DrawRect(selectionRect, selectionRectangleColorBorder, filled: false, width: 2);
		}
	}
}
