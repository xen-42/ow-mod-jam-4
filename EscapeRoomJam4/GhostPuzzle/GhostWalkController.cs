using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static GhostEffects;

namespace EscapeRoomJam4.GhostPuzzle;

public class GhostWalkController : MonoBehaviour
{
    private Animator _animator;

    private Vector3 _destination;

    private Vector2 _smoothedMoveSpeed;
    private DampedSpring2D _moveSpeedSpring = new(50f, 1f);
    private bool _isWalking;
    private Vector2 _targetValue;

    private OWRenderer[] _ditherRenderers;
    private OWRenderer[] _dissolveRenderers;
    private bool _playingDeathSequence;

    private bool _deathAnimComplete;

    private readonly int _propID_DissolveProgress = Shader.PropertyToID("_DissolveProgress");

    private float _speed = 2f;
    private float _turnSpeed = 360f;

    private OWAudioSource _audioSource;


    public void Awake()
    {
        _animator = this.GetRequiredComponent<Animator>();

        // SkinnedMeshRenderers dither to vanish whereas regular ones dissolve
        _ditherRenderers = this.GetComponentsInChildren<OWRenderer>().Where(x => x.GetComponent<SkinnedMeshRenderer>() != null).ToArray();
        _dissolveRenderers = this.GetComponentsInChildren<OWRenderer>().Where(x => x.GetComponent<SkinnedMeshRenderer>() == null).ToArray();

        gameObject.AddComponent<AudioSource>();
        _audioSource = gameObject.AddComponent<OWAudioSource>();

        _audioSource.SetTrack(OWAudioMixer.TrackName.Environment);
    }

    public void WalkTo(Vector3 destination)
    {
        _destination = destination;
        _isWalking = true;
        _audioSource.PlayOneShot(AudioType.Ghost_Identify_Curious);
    }

    public void PressButton()
    {
        _animator.SetTrigger(GhostEffects.AnimatorKeys.Trigger_Grab);
        _audioSource.PlayOneShot(AudioType.Ghost_Laugh);
    }

    public virtual void PlayDeathAnimation()
    {
        var deathAnimStyle = GhostEffects.DeathAnimStyle.Reaching;
        _animator.SetInteger(GhostEffects.AnimatorKeys.Int_DeathType, (int)deathAnimStyle);
        _animator.SetTrigger(GhostEffects.AnimatorKeys.Trigger_Death);
        _deathAnimComplete = false;
        _playingDeathSequence = true;
        _isWalking = false;
        _audioSource.PlayOneShot(AudioType.Ghost_DeathSingle);
    }

    private void Anim_Death_Complete()
    {
        _deathAnimComplete = true;
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (_isWalking)
        {
            UpdateWalk();
        }
        if (this._playingDeathSequence)
        {
            UpdateDeathEffects();
        }

        _smoothedMoveSpeed = _moveSpeedSpring.Update(this._smoothedMoveSpeed, _isWalking ? _targetValue : Vector2.zero, Time.deltaTime);
        _animator.SetFloat(GhostEffects.AnimatorKeys.Float_MoveDirectionX, this._smoothedMoveSpeed.x);
        _animator.SetFloat(GhostEffects.AnimatorKeys.Float_MoveDirectionY, this._smoothedMoveSpeed.y);
    }

    private void UpdateDeathEffects()
    {
        var deathFade = _animator.GetFloat(GhostEffects.AnimatorKeys.AnimCurve_DeathFade);
        for (var i = 0; i < _dissolveRenderers.Length; i++)
        {
            _dissolveRenderers[i].SetMaterialProperty(this._propID_DissolveProgress, deathFade);
        }
        for (int j = 0; j < _ditherRenderers.Length; j++)
        {
            _ditherRenderers[j].SetDitherFade(deathFade);
        }
        if (_deathAnimComplete)
        {
            _playingDeathSequence = false;
            for (var k = 0; k < _dissolveRenderers.Length; k++)
            {
                _dissolveRenderers[k].SetMaterialProperty(_propID_DissolveProgress, 0f);
            }
            for (var l = 0; l < _ditherRenderers.Length; l++)
            {
                _ditherRenderers[l].SetDitherFade(0f);
            }
            this.gameObject.SetActive(false);
        }
    }

    private void UpdateWalk()
    {
        var disp = _destination - transform.localPosition;
        var relativeVelocity = disp.normalized * _speed;

        var rotation = Vector3.Angle(disp, transform.forward);
        var dtheta = Mathf.Sign(rotation) * _turnSpeed * Time.deltaTime;
        transform.localRotation *= Quaternion.AngleAxis(Mathf.Min(dtheta, rotation), transform.up);

        var stepSize = relativeVelocity * Time.deltaTime;
        if (stepSize.magnitude > disp.magnitude)
        {
            transform.localPosition = _destination;
            _isWalking = false;
            return;
        }

        transform.localPosition += stepSize;

        // Moves in x/z plane
        _targetValue = new Vector2(0, 1f);
    }
}
