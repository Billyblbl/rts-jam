using Godot;
using System.Collections.Generic;
using System.Linq;

// #nullable enable

public class ControlSystem : Node2D {

	[Export] public string selectActionName;
	[Export] public string orderToolAction;
	[Export] public string orderToolCancel;
	[Export] public string additiveModifier;
	[Export] public string controlGroupEdit;
	[Export] public string contextOrder;
	[Export] public string[] controlGroupKeys;
	[Export] public Color selectionRectangleColorFill = new Color(0f, 1f, 0f, .5f);
	[Export] public Color selectionRectangleColorBorder = Colors.Green;

	[Signal] public delegate void SelectionChanged();
	public event SelectionChanged OnSelectionChanged;

	public ControlToolState currentTool = null;

	Rect2 selectionRect;
	Vector2 selectStart;
	Vector2 selectEnd;
	bool dragging = false;

	public Controllable[] selection = new Controllable[0];
	public Controllable[] hovered = new Controllable[0];
	public Controllable[][] controlGroups = new Controllable[10][];

	// public Texture cursor { set {
	// 	if (value == null) {
	// 		Input.SetDefaultCursorShape();
	// 	} else {
	// 		Input.SetCustomMouseCursor(value, hotspot:value.GetSize()/2f);
	// 	}
	// }}

	public void GiveOrderToSelection(Order order, bool additive) {
		foreach (var controllable in selection) {
			controllable.GiveOrder(order, additive);
		}
	}

	public void GiveOrderToSelection(Order order) => GiveOrderToSelection(order, additiveOrder);

	public void ClearOrdersInSelection() {
		foreach (var controllable in selection) {
			controllable.StopAllOrders();
		}
	}

	public bool additiveOrder { get => Input.IsActionPressed(additiveModifier); }

	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventMouseMotion eventMouseMotion) {
			dragging = Input.IsActionPressed(selectActionName);
		} else if (@event.IsAction(selectActionName)) {
			// GD.Print(string.Format("{0}, {1}", nameof(_UnhandledInput), @event));
			var wasPressed = select;
			select = @event.IsPressed();
			var mousePos = GetGlobalMousePosition();

			HideIndicators();
			if (select && !wasPressed) {
				selectStart = mousePos;
				selectEnd = mousePos;
			} else if (!select && wasPressed) {
				var additive = Input.IsActionPressed(additiveModifier);
				selection = ResolveSelection(additive ? selection : null);
				dragging = false;
				OnSelectionChanged?.Invoke();
			}
			ShowIndicators();

		} else if (selection != null && selection.Length > 0 && @event.IsAction(contextOrder) && @event.IsPressed()) {
			GiveOrderToSelection(ResolveContextOrder(GetGlobalMousePosition()), additive: Input.IsActionPressed(additiveModifier));
		} else {
			foreach (var (action, i) in controlGroupKeys.Select((a, i) => (a, i))) {
				if (@event.IsAction(action) && @event.IsPressed()) {
					if (Input.IsActionPressed(controlGroupEdit)) {
						controlGroups[i] = selection;
					} else {
						HideIndicators();
						selection = controlGroups[i];
						ShowIndicators();
					}
				}
			}
		}
	}

	public Order ResolveContextOrder(Vector2 position) {
		//TODO different unit orders

		var target = GetSelectableUnderCursor(position);
		if (target != null) return new AttackOrder(target.GetParent<Node2D>());

		return new MoveOrder(position);
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

	public override void _Ready() {
		base._Ready();
		OnSelectionChanged += () => EmitSignal(nameof(SelectionChanged));
		currentTool = new ControlToolState();
		currentTool.controls = this;
	}

	bool select;
	public override void _Process(float delta) {

		var mousePos = GetGlobalMousePosition();

		selectEnd = mousePos;
		selectionRect = RectContaining(selectStart, selectEnd);

		HideIndicators();
		hovered = ResolveSelection();
		ShowIndicators();
		Update();
	}

	Controllable[] ResolveSelection(Controllable[] oldSelection = null) {
		var newSelection = oldSelection != null ? new List<Controllable>(oldSelection) : new List<Controllable>();
		if (dragging) {
			newSelection.AddRange(GetSelectablesInRect(selectionRect).Where(s => !newSelection.Contains(s)));
		}
		var underCursor = GetSelectableUnderCursor(selectEnd);
		if (underCursor != null && !newSelection.Contains(underCursor)) newSelection.Add(underCursor);
		return newSelection.ToArray();
	}

	Controllable[] GetSelectablesInRect(Rect2 rect) => Controllable.Population.Where(s => rect.HasPoint(s.GetParent<Node2D>().GlobalPosition)).ToArray();
	Controllable GetSelectableUnderCursor(Vector2 cursor) => Controllable.Population
			.Where(s => s.selectArea.HasPoint(s.GetParent<Node2D>().ToLocal(cursor)))
			.Aggregate<Controllable, Controllable>(null, (smaller, next) => smaller != null && (smaller.GetParent<Node2D>().GlobalPosition - cursor).Length() < (next.GetParent<Node2D>().GlobalPosition - cursor).Length() ? smaller : next);

	public override void _Draw() {
		if (dragging) {
			DrawRect(selectionRect, selectionRectangleColorFill, filled: true);
			DrawRect(selectionRect, selectionRectangleColorBorder, filled: false, width: 2);
		}
	}
}
