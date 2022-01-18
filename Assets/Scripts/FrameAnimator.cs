using System.Collections.Generic;
using UnityEngine;

public class FrameAnimator : MonoBehaviour {

	[Header("Runtime")]
	[SerializeField]
	private Stack<FrameAnimatorState> _animatorStates;
	
}

public class FrameAnimatorState {
	public string name;
	public bool canBeInterrupted;
	public Texture animationSheet;
	public float playbackSpeed;
}

public enum FrameAnimatorStateEnterStrategy {
	Wait,
	Interrupt,
	Ignore
}

public enum FrameAnimatorStateExitStrategy {
	Stop,
	Continue,
	Loop
}