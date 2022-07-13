using Godot;

public class MoveOrderTool : ControlToolState {

	public override void _Input(InputEvent @event) {
		if (@event.IsAction(controls.orderToolAction)) {

			GetTree().SetInputAsHandled();

			controls.GiveOrderToSelection(new MoveOrder(controls.GetGlobalMousePosition()));
			if (!controls.additiveOrder) Exitstate();
		} else if (@event.IsAction(controls.orderToolCancel)) {
			GetTree().SetInputAsHandled();
			Exitstate();
		}
	}

}
