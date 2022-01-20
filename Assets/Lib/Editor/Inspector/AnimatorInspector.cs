using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UEditor = UnityEditor.Editor;

namespace Lib.Editor.Inspector
{
    [CustomEditor(typeof(Animator), true)]
    public class AnimatorInspector : UEditor
    {
        private int _curIdx;
        private Animator _animator;
        private RuntimeAnimatorController _animatorController;
        private AnimationClip[] _clips;
        private bool _isAnimPlaying;
        private float _updateFrameTime;
        private float _timer;

        private void OnEnable()
        {
            _curIdx = -1;
            _animator = target as Animator;
            ;
            _timer = 0f;
            if (_animator != null) _animatorController = _animator.runtimeAnimatorController;
            _clips = _animatorController ? _animatorController.animationClips : new AnimationClip[0];
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var lastIndex = _curIdx;
            if (_curIdx < 0) _curIdx = 0;

            GUILayout.BeginHorizontal();
            var clip = _clips[_curIdx];
            GUILayout.Label("Select Clip", GUILayout.Width(100));
            _curIdx = EditorGUILayout.Popup(_curIdx, _clips.Select(p => p.name).ToArray()); //还原clip状态
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Play Frame", GUILayout.Width(100));
                _timer = EditorGUILayout.Slider(_timer, 0, clip.length);
            }
            GUILayout.EndHorizontal();
            var playAniBtnStr = _isAnimPlaying ? "Pause" : "Play";
            if (GUILayout.Button(playAniBtnStr)) PlayAnim(_timer >= clip.length);

            if (_isAnimPlaying)
            {
                _timer += Time.realtimeSinceStartup - _updateFrameTime;
                if (_timer > clip.length)
                {
                    _isAnimPlaying = false;
                    _timer = clip.length;
                }
            }

            Repaint();
            _updateFrameTime = Time.realtimeSinceStartup;
            if (_isAnimPlaying) clip.SampleAnimation(_animator.gameObject, _timer);
        }

        private void PlayAnim(bool rePlay)
        {
            if (rePlay)
            {
                _timer = 0f;
                _isAnimPlaying = true;
            }
            else
            {
                _isAnimPlaying = !_isAnimPlaying;
            }
        }
    }
}