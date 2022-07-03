public interface State<T> {
	void EnterState(T data);
	void StayState(T data);
	void ExitState(T data);
}
