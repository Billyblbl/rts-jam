using Godot;

public class OrderButton : TextureButton {
	public System.Func<ControlToolState> tool;

	public OrderButton(OrderBlueprint order, ControlSystem controls) {
		TextureNormal = order.icon;
		tool = () => order.Instantiate<ControlToolState>(controls);
		// controls
		Expand = true;
		Connect("pressed", this, nameof(OnPressed));
		Name = order.ResourceName;
		GD.Print(string.Format("{0}:{1}", nameof(OrderButton), order.ResourceName));
	}

	public void OnPressed() {
		tool?.Invoke();
	}

}
