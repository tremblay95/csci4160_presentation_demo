using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public Transform muzzleFlash;
    Material flashMat;
    public AnimationCurve flashTransCurve;
    public AnimationCurve flashSizeCurve;
    public float flashDuration = 0.1f;
    public GameObject bulletHolePrefab;

    public int totalAmmo = 120;
    public int maxMag = 30;
    public int curMag = 30;
    public float range = 50.0f;

    public int bulletDmg = 15;

    public Animator anim;
    public Transform body;
 
    public float rpm = 60.0f;
    float fireRate;
    float fireCounter = 0;

    public float recoilAmount = 0.25f;
    public float recoilAimAmount = 0.1f;
    public float recoilRecoverTime = 0.2f;

    float recoilPos = 0;
    float recoilVel;

    bool _gunDown = false;
    public bool gunDown
    {
        set
        {
            _gunDown = value;
            anim.SetBool("IsGunDown", _gunDown);
        }
        get
        {
            return _gunDown;
        }
    }


    private void Start()
    {
        fireRate = 60.0f / rpm;
    }

    private void Update()
    {
        if (_gunDown) return;

        anim.SetBool("IsAimed", Input.GetButton("Fire2"));

        if(Input.GetButton("Fire1"))
        {
            if(fireCounter <= 0)
            {
                if (curMag > 0)
                {
                    //fire
                    curMag--;
                    StartCoroutine(MuzzleFlash());
                    RaycastHit hit;
                    if(Physics.Raycast(muzzleFlash.position, muzzleFlash.forward, out hit, range))
                    {
                        GameObject bh = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
                        bh.transform.SetParent(hit.transform, true);
                        bh.GetComponent<BulletHole>().Init(hit.transform.tag, transform.forward);

                        hit.transform.SendMessageUpwards("ApplyDamage", bulletDmg, SendMessageOptions.DontRequireReceiver);
                    }

                    //do recoil
                    recoilPos = Input.GetButton("Fire2") ? recoilAimAmount : recoilAmount;

                    fireCounter = fireRate;
                }
                else
                {
                    anim.SetTrigger("Reload");
                }
                
            }
            else
            {
                fireCounter -= Time.deltaTime;
            }
        }
        if (Input.GetButtonUp("Fire1")) fireCounter = 0;


        recoilPos = Mathf.SmoothDamp(recoilPos, 0, ref recoilVel, recoilRecoverTime);
        body.localPosition = Vector3.back * recoilPos;

        if (Input.GetButtonDown("Reload") && curMag != maxMag)
            anim.SetTrigger("Reload");

    }

    void Reload()
    {
        int diff = maxMag - curMag;
        curMag = maxMag;
        totalAmmo -= diff;
        anim.ResetTrigger("Reload");
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.localEulerAngles = Vector3.forward * Random.Range(0, 360);
        if (!flashMat) flashMat = muzzleFlash.GetComponent<MeshRenderer>().material;

        Color c = Color.white;
        float t = 0;

        do
        {
            t += Time.deltaTime / flashDuration;

            muzzleFlash.localScale = Vector3.one * flashSizeCurve.Evaluate(t);
            c.a = flashTransCurve.Evaluate(t);
            flashMat.color = c;
            yield return null;

        } while (t <= 1.0f);
    }
}
