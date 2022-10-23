using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField, Range(0.01f, 0.4f)] float moveDuration = 0.4f; //field
    [SerializeField, Range(0.01f, 0.5f)] float jumpHeight = 0.5f;
    [SerializeField] ParticleSystem dieParticle;
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }
    public bool IsDie { get => this.enabled == false; }

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    void Update()
    {
        var moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
        }

        if (moveDir != Vector3.zero && IsJumping() == false)
        {
            Jump(moveDir);
        }
    }

    private void Jump(Vector3 targetDirection)
    {
        // atur rotasi
        var TargetPosition = transform.position + targetDirection;
        transform.LookAt(TargetPosition);

        // loncat ke atas
        var movSeq = DOTween.Sequence(transform);

        movSeq.Append(transform.DOMoveY(jumpHeight, moveDuration / 2));
        movSeq.Append(transform.DOMoveY(0, moveDuration / 2));

        if (TargetPosition.z <= backBoundary || TargetPosition.x <= leftBoundary || TargetPosition.x >= rightBoundary)
        {
            return;
        }

        // mencegah menembus pohon
        if (Tree.AllPositions.Contains(TargetPosition))
        {
            return;
        }

        // gerak maju/mundur/samping
        transform.DOMoveX(TargetPosition.x, moveDuration);
        transform
        .DOMoveZ(TargetPosition.z, moveDuration)
        .OnComplete(UpdateTravel);

        //SFX loncat
        if (SoundManager.Instance.asSFXJump.mute != true)
        {
            SoundManager.Instance.asSFXJump.Play();
        }
    }

    private void UpdateTravel()
    {
        currentTravel = (int)this.transform.position.z;

        if (currentTravel > maxTravel)
        {
            maxTravel = currentTravel;
        }

        if (currentTravel >= 0)
        {
            stepText.text = $"STEP : {currentTravel}";
        }
        else
        {
            stepText.text = "STEP : 0";
        }
    }
    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
        {
            return;
        }
        // di jalankan sekali pada frame ketika nempel pertama kali
        // var car = other.GetComponent<Car>();
        // if (car != null)
        // {
        //     Debug.Log("Hit " + car.name);
        // }

        if (other.tag == "Car")
        {
            AnimateDieCrash();
        }
    }

    private void AnimateDieCrash()
    {
        transform.DOScaleY(0.1f, 0.5f);
        transform.DOScaleX(3, 0.5f);
        transform.DOScaleZ(2, 0.5f);

        this.enabled = false; //komponen script dimatikan
        dieParticle.Play();

        if (SoundManager.Instance.asSFXDie.mute != true)
        {
            SoundManager.Instance.asSFXDie.Play(); //sound efek meninggal
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // di jalankan setiap frame selama masih nempel
    }

    private void OnTriggerExit(Collider other)
    {
        // di jalankan sekali pada frame tidak nempel
    }
}