using System;
using System.Collections.Generic;
using UnityEngine;

public class FrameAnimator : MonoBehaviour {

	[Header("Setup")]
	public bool useUnscaledTime;
	public bool playOnAwake = true;
	public bool loop = true;
	public float fps;
	public List<Texture2D> frames;
	public bool autoRefresh;
	public Material material;

	[Header("Runtime")]
	[SerializeField]
	private bool _isPlaying;
	[SerializeField]
	private float _startProgress;
	[SerializeField]
	private float _startTime;
	[SerializeField]
	private float _currProgress;
	[SerializeField]
	private int _currIndex;
	[SerializeField]
	private Texture2D _lastFrame;
	[SerializeField]
	private Texture2D _currFrame;

	public float AnimationLength => fps == .0f ? 1f : frames.Count / fps;
	public bool NeedsRefresh => _lastFrame != _currFrame;
	public Texture2D CurrentFrame => _currFrame;

	private void Awake() {
		if (playOnAwake) Play();
	}

	private void Update() {
		if (!_isPlaying || frames == null || frames.Count == 0) return;

		_lastFrame = _currFrame;

		var time = useUnscaledTime ? Time.unscaledTime : Time.time;
		var length = AnimationLength;
		_currProgress = (time - _startTime) / length + _startProgress;

		if (_currProgress > 1.0f) {
			if (!loop) {
				_currFrame = frames[^1];
				_isPlaying = false;
				return;
			}
			
			_currProgress = _currProgress - Mathf.Floor(_currProgress);
			_currProgress = Mathf.Clamp01(_currProgress);
		}

		_currIndex = (int) (_currProgress * frames.Count);
		_currIndex = Mathf.Clamp(_currIndex, 0, frames.Count - 1);

		_currFrame = frames[_currIndex];

		if (autoRefresh && NeedsRefresh && material) material.mainTexture = _currFrame;
	}

	public void Reset() {
		_isPlaying = false;
		_startProgress = .0f;
		_startTime = .0f;
		_currProgress = .0f;
		_currIndex = 0;
		_lastFrame = null;
		_currFrame = null;
	}
	
	public bool Stop() {
		if (!_isPlaying) return false;
		_isPlaying = false;
		return true;
	}

	public void Play(bool interrupt = false) => Play(.0f, interrupt);

	public bool Play(float startProgress, bool interrupt = false) {
		if (_isPlaying && !interrupt) return false;
		_isPlaying = true;
		_startProgress = Mathf.Clamp01(startProgress);
		_startTime = useUnscaledTime ? Time.unscaledTime : Time.time;
		_lastFrame = null;
		_currFrame = null;
		return true;
	}

	public bool Resume() => Play(_currProgress);
}