using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int x;
    public int y;
    public Vector3 fpos;
    public Vector3 fdir = Vector3.forward;

    public int tx;
    public int ty;
    public Vector3 tpos;
    public Vector3 tdir = Vector3.forward;

    public int depth;
    public float pc;
    public float corridorwidth=0.8f;

    public Vector3 nodedir = Vector3.forward;

    public float movelength = 1.0f;
    public float turnlength = 0.3f;
    public bool turnin = false;
    public Camera cam;
    // Use this for initialization
    void Start () {
        fpos = transform.position;
        tpos = transform.position;
        pc = 1.0f;		
	}

    public int lastdir=-1;

    public void DoMove(int dir){
        if (pc>=0.9f)
        {
            if (dir == 0)
            {
                var r = new Ray(transform.position, transform.forward);
                RaycastHit hitInfo;
                Physics.Raycast(r, out hitInfo);


                var dv = hitInfo.point - transform.position;
                if (dv.magnitude>1.0f){

                    Vector3 halb = transform.position+((hitInfo.point - transform.forward * corridorwidth)-transform.position) / 2;
                    Ray rl = new Ray(halb, Vector3.Cross(transform.forward, Vector3.up));
                    Ray rr = new Ray(halb, -Vector3.Cross(transform.forward, Vector3.up));

                    RaycastHit hitInfo_r;
                    RaycastHit hitInfo_l;
                    Physics.Raycast(rr, out hitInfo_r);
                    Physics.Raycast(rl, out hitInfo_l);

                    Debug.DrawRay(rr.origin, rr.direction * 10, Color.green,5.0f);
                    Debug.DrawRay(rl.origin, rl.direction * 10, Color.cyan, 5.0f);

                    movelength = Mathf.Log(dv.magnitude);
                    fpos = tpos;
                    Debug.Log((hitInfo_l.distance) + "," + (hitInfo_r.distance));


                    if (hitInfo_l.distance > 1 || hitInfo_r.distance > 1)
                    {
                        tpos = halb;
                    } else {
                        tpos = hitInfo.point - transform.forward * corridorwidth;
                    }

                    fdir = tdir;
                    turnin = false;
                    pc = 0;
                    fixedpos = false;
                    lastdir=dir;
                }

            }
            else if (dir == 1)
            {


                var r = new Ray(transform.position, -transform.forward);
                RaycastHit hitInfo;
                Physics.Raycast(r, out hitInfo);


                var dv = hitInfo.point - transform.position;
                if (dv.magnitude > 1.0f)
                {

                    Vector3 halb = transform.position + ((hitInfo.point + transform.forward * corridorwidth) - transform.position) / 2;
                    Ray rl = new Ray(halb, Vector3.Cross(transform.forward, Vector3.up));
                    Ray rr = new Ray(halb, -Vector3.Cross(transform.forward, Vector3.up));

                    RaycastHit hitInfo_r;
                    RaycastHit hitInfo_l;
                    Physics.Raycast(rr, out hitInfo_r);
                    Physics.Raycast(rl, out hitInfo_l);

                    Debug.DrawRay(rr.origin, rr.direction * 10, Color.green, 5.0f);
                    Debug.DrawRay(rl.origin, rl.direction * 10, Color.cyan, 5.0f);

                    movelength = Mathf.Log(dv.magnitude);
                    fpos = tpos;
                    Debug.Log((hitInfo_l.distance) + "," + (hitInfo_r.distance));


                    if (hitInfo_l.distance > 1 || hitInfo_r.distance > 1)
                    {
                        tpos = halb;
                    }
                    else
                    {
                        tpos = hitInfo.point + transform.forward * corridorwidth;
                    }

                    fdir = tdir;
                    turnin = false;
                    pc = 0;
                    fixedpos = false;
                    lastdir=dir;

                }

            }
            else if (dir == 2)
            {
                fpos = tpos;
                fdir = tdir;
                tdir = Vector3.Cross(fdir, Vector3.up);
                turnin = true;
                pc = 0;
                fixedpos = false;
                lastdir=dir;
            }
            else if (dir == 3)
            {
                fpos = tpos;
                fdir = tdir;
                tdir = -Vector3.Cross(fdir, Vector3.up);
                turnin = true;
                pc = 0;
                fixedpos = false;
                lastdir=dir;
            }
        } else if ((lastdir==0 && dir==1) || (lastdir==1 && dir==0)){
            pc=1-pc;
            var t = fpos;
            fpos=tpos;
            tpos=t;
            lastdir=dir;
        } else if ((lastdir==2 && dir==3) || (lastdir==3 && dir==2)){
            pc=1-pc;
            var t = fdir;
            fdir=tdir;
            tdir=t;
            lastdir=dir;
        }
    }
    static float easeInOut(float t) {
        t = Mathf.Clamp01(t);
        return t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }

    private bool fixedpos = false;

    public UnityEngine.Audio.AudioMixer audioMixer;

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.R)){
            UnityEngine.SceneManagement.SceneManager.LoadScene("MeshGen");
        }
        if (turnin)
        {
            pc += Time.deltaTime / turnlength;
        }
        else
        {
            pc += Time.deltaTime / movelength;
        }

        var pp = easeInOut(pc);

        
        float amv = 1-2*Mathf.Clamp01(Mathf.Abs(0.5f-pc));
        if (turnin){
            amv=0;
        }
        audioMixer.SetFloat("FlangeRate",amv*10);

        transform.position = Vector3.Lerp(fpos, tpos, pp);
        transform.rotation = Quaternion.Slerp( Quaternion.LookRotation(fdir), Quaternion.LookRotation(tdir), pc);

        if (pc >= 1.0f)
        {
            if (fixedpos == false)
            {
                fixedpos = true;
                Vector3 v = transform.localPosition;
                v.x = Mathf.Round(v.x);
                v.z = Mathf.Round(v.z);
                transform.localPosition = v;
                fpos = transform.position;
                tpos = transform.position;

                Vector3 ee = transform.localEulerAngles;
                ee.x = Mathf.Round(ee.x / 90.0f) * 90;
                ee.y = Mathf.Round(ee.y / 90.0f) * 90;
                ee.z = Mathf.Round(ee.z / 90.0f) * 90;
                transform.localEulerAngles = ee;
                fdir = transform.forward;
                tdir = transform.forward;
            }
        }

        if (Input.GetButtonDown("left")){
            DoMove(2);
        }
        
        if (Input.GetButtonDown("right")){
            DoMove(3);
        }
        
        if (Input.GetButtonDown("up")){
            DoMove(0);
        }
        
        if (Input.GetButtonDown("down")){
            DoMove(1);
        }
    }
}
