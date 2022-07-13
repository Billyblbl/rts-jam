using Godot;
using System.Collections.Generic;
using System.Linq;

// #nullable enable

public class Controllable : Node {
	[Export] public NodePath hoverIndicatorPath;
	[Export] public NodePath selectIndicatorPath;
	[Export] public Rect2 selectArea;
	[Export] public OrderBlueprint[] availableOrders = new OrderBlueprint[0];

	public Sprite hoverIndicator;
	public Sprite selectIndicator;
	public BehaviorState currentBehavior;

	Queue<Order> orders = new Queue<Order>();
	public Order currentOrder { get => orders.Count > 0 ? orders.Peek() : null; }

	public void GiveOrder(Order order, bool additive = false) {
		order.participants++;
		if (!additive) StopAllOrders();
		orders.Enqueue(order);
	}

	bool idle = true;
	public void StopOrder(Order order) {
		if (order == null) return;
		if (order == currentOrder) {
			order.Stop(this);
			order.participants--;
			orders.Dequeue();
			idle = true;
		} else {
			order.participants--;
			//!this is icky and probably slow but Queue doesn't allow for changes in the middle, might consider using a simple list instead
			orders = new Queue<Order>(orders.Where(o => o != order));
		}

	}

	public void StopAllOrders() {
		if (!idle) currentOrder.Stop(this);
		foreach (var cancelled in orders) cancelled.participants--;
		orders.Clear();
		idle = true;
	}

	public override void _Ready() {
		base._Ready();
		hoverIndicator = GetNode<Sprite>(hoverIndicatorPath);
		selectIndicator = GetNode<Sprite>(selectIndicatorPath);
		currentBehavior = new BehaviorState(GetParent<Node2D>());
	}

	public override void _EnterTree() {
		base._EnterTree();
		Population.Add(this);
	}

	public override void _ExitTree() {
		Population.Remove(this);
		base._ExitTree();
	}

	public override void _Process(float delta) {
		if (idle) {
			while(orders.Count > 0 && !currentOrder.Start(this)) {
				currentOrder.participants--;
				orders.Dequeue();
			}
			idle = orders.Count == 0;
		}
		if (!idle && !currentOrder.Update(this)) {
			StopOrder(currentOrder);
			if (orders.Count > 0) currentOrder.Start(this);
			else idle = true;
		}
	}

	public static List<Controllable> Population = new List<Controllable>();

}
