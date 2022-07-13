public class StopOrderTool : ControlToolState {

	public override void _Ready() {
		controls.ClearOrdersInSelection();
		Exitstate();
	}

}
