using Godot;
using System.Linq;
using System.Collections.Generic;

public class OrdersPanel : Control {

	[Export] public Slot<ControlSystem> controlSystem;
	[Export] public NodePath[] slots;

	public Control[] slotNodes;

	async void ListenToSelection() {
		if (controlSystem.instance == null) await ToSignal(controlSystem, "changed");
		controlSystem.instance.OnSelectionChanged += delegate {
			ClearButtons();
			if (controlSystem.instance.selection.Length > 0)
				BuildButtons();
		};
	}

	public void ClearButtons() {
		foreach (var slot in slotNodes) {
			foreach (var child in slot.GetChildren().Cast<Node>()) {
				child.QueueFree();
			}
		}
	}

	public void BuildButtons() {
		var buttons = controlSystem.instance.selection
			.Select(c => c.availableOrders)
			.Cast<IEnumerable<OrderBlueprint>>()
			.Aggregate((bl1, bl2) => bl1.Intersect(bl2))
			.Select(o => new OrderButton(o, controlSystem.instance))
			.ToArray();
		for (int i = 0; i < buttons.Length; i++) {
			slotNodes[i].AddChild(buttons[i]);
		}
		GD.Print(string.Format("{0}: {1}", nameof(BuildButtons), buttons.Aggregate("", (a, b) => string.Format("{0}.{1}", a, b.Name))));
	}

	public override void _Ready() {
		base._Ready();
		slotNodes = slots.Select(s => GetNode<Control>(s)).ToArray();
		ListenToSelection();
	}

}
