using Godot;
using System.Linq;

public class DirectionalAnimatedSprite : AnimatedSprite {

	[Export] public Vector2 originDirection = Vector2.Right;
	[Export] public SpriteFrames[] animationStates = new SpriteFrames[0];
	[Export] public float turnSpeed = 10f;
	Vector2 _direction;
	Vector2 targetDirection;

	public Vector2 direction { get => _direction; set => targetDirection = value; }

	int stateIndex = 0;
	public State state { set {
		if (value == null) {
			stateIndex = 0;
			return;
		}

		var s = animationStates
			.Select((f, i) => (f, i))
			.FirstOrDefault(((SpriteFrames f, int i) e) => e.f.ResourceName == value.GetType().Name);
		if (s != default) {
			// GD.Print(string.Format("Animation state found for behavior state {0}, index {1}", value.GetType().Name, s.i));
			stateIndex = s.i;
		} else {
			// GD.Print(string.Format("Animation state not found for behavior state {0}", value.GetType().Name));
			stateIndex = -1;
		}
	}}

	public float originAngle { get => originDirection.Angle(); }
	public float angle { get => direction.Angle(); }
	public float turns { get => (angle - originAngle) / (2*Mathf.Pi); }
	public int IndexIn(SpriteFrames frames) => Mathf.RoundToInt((turns * (frames.Animations.Count - 1) + frames.Animations.Count - 1) % (frames.Animations.Count - 1));


	public string label;

	public override void _Process(float delta) {
		_direction = _direction.LinearInterpolate(targetDirection, delta * turnSpeed);
		try {
			if (stateIndex >= 0 && stateIndex < animationStates.Length) {
				Frames = animationStates[stateIndex];
				Animation = Frames.GetAnimationNames()[IndexIn(Frames)];
				label = string.Format("Direction : {2}\nAnimationStateIndex : {0}\nAnimationTurnIndex : {1}\n", stateIndex, IndexIn(Frames), direction);
			} else {
				label = string.Format("Direction : {1}\nAnimationStateIndex : {0}\n", stateIndex, direction);
			}
		} catch (System.Exception) {
			GD.Print(string.Format("Direction : {2}\nAnimationStateIndex : {0}\nAnimationTurnIndex : {1}\n", stateIndex, IndexIn(Frames), direction));
			throw;
		}


		base._Process(delta);
	}

}
