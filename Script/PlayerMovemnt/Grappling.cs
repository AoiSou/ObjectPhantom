using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMove pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;

    private void Start()
    {
        pm = GetComponent<PlayerMove>();
        
        // coolDownText.gameObject.SetActive(false);
        // coolDownImage.fillAmount = 0.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
        
        if (coolDown)
        {
            ApplyCoolDown();
        }
    }

    private void LateUpdate()
    {
         if (grappling)
            lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        UseCoolDown();
        
        if (grapplingCdTimer > 0) return;

        grappling = true;

        //pm.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
    
    
    // [SerializeField]
    // private Image coolDownImage;
    // [SerializeField]
    // private TMP_Text coolDownText;
    
    private bool coolDown;
    private float coolDownTime=3.0f;
    private float coolDownTimer=0.0f;

    void ApplyCoolDown()
    {
        coolDownTimer -= Time.deltaTime;

        if (coolDownTimer <= 0.0f)
        {
            coolDown = false;
            // coolDownText.gameObject.SetActive(false);
            // coolDownImage.fillAmount = 0.0f;
        }
        else
        {
            // coolDownText.text = Mathf.RoundToInt(coolDownTimer).ToString();
            // coolDownImage.fillAmount = coolDownTimer / coolDownTime;
        }
    }

    public void UseCoolDown()
    {
        if (coolDown)
        {
            //return false;
        }
        else
        {
            coolDown=true;
            // coolDownText.gameObject.SetActive(true);
            coolDownTimer=coolDownTime;
            //return true;
        }
    }
}
