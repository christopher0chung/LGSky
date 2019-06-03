using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Debug_InControlTest : MonoBehaviour {

    public Model_Game gameModel;

    public GameObject bullet;
    public List<GameObject> _bullets;
    public List<float> _bulletTimes;

    public float moveVel;

    public float xBoundClose;
    public float xBoundFar;
    public float zBoundClose;
    public float zBoundFar;

    public Material shieldMat;

    public float relativeBearing;
    public float declination;

    private Transform swivel;
    private Transform pitcher;
    private Transform gun;
    private float leftStickY;
    private float leftStickX;

    private float rightStickY;
    private float rightStickX;

    //private bool shootButtonIsDown;
    //private bool shootButtonOnUp;
    //private bool shootButtonOnDown;
    private float shootTimer;

    public float fireTimeInterval;
    public float fireFirstShotDelay;

    public AudioClip gunShot;

    private AudioSource myAS;

    public Vector3 leftBoundVector;
    public Vector3 rightBoundVector;
    public Vector3 leftInVector;
    public Vector3 rightInVector;

    public Transform swordPivot;
    private Transform swordPivot2;

    private bool leftTriggerIsDown;
    private bool leftTriggerOnDown;
    private bool leftTriggerOnUp;

    private bool rightTriggerIsDown;
    private bool rightTriggerOnDown;
    private bool rightTriggerOnUp;

    private bool leftBumperOnDown;
    private bool rightBumperOnDown;

    private Transform rocketAim;
    private Transform rocketPitch;

    public List<Stations> unassignedStations = new List<Stations>();

    void Start()
    {
        leftBoundVector = Vector3.Normalize(new Vector3(-xBoundFar - -xBoundClose, 0, zBoundFar - zBoundClose));
        leftInVector = Vector3.Cross(leftBoundVector, Vector3.up);

        rightBoundVector = Vector3.Normalize(new Vector3(xBoundFar - xBoundClose, 0, zBoundFar - zBoundClose));
        rightInVector = Vector3.Cross(rightBoundVector, Vector3.down);

        swivel = transform.GetChild(0);
        pitcher = transform.GetChild(0).GetChild(0);
        gun = transform.GetChild(1);
        rocketAim = transform.GetChild(2);
        rocketPitch = transform.GetChild(2).GetChild(0);
        _bullets = new List<GameObject>();
        _bulletTimes = new List<float>();
        myAS = GetComponent<AudioSource>();


        swordPivot2 = swordPivot.GetChild(0);
        swd = swordPivot2.GetChild(0);

    }

	void Update () {
        var inputDevice = InputManager.ActiveDevice;
        leftStickX = inputDevice.LeftStickX;
        leftStickY = inputDevice.LeftStickY;

        rightStickX = inputDevice.RightStickX;
        rightStickY = inputDevice.RightStickY;

        //shootButtonIsDown = inputDevice.Action1.IsPressed;
        //shootButtonOnUp = inputDevice.Action1.WasReleased;
        //shootButtonOnDown = inputDevice.Action1.WasPressed;

        leftTriggerIsDown = inputDevice.LeftTrigger.IsPressed;
        leftTriggerOnDown = inputDevice.LeftTrigger.WasPressed;
        leftTriggerOnUp = inputDevice.LeftTrigger.WasReleased;

        rightTriggerIsDown = inputDevice.RightTrigger.IsPressed;
        rightTriggerOnDown = inputDevice.RightTrigger.WasPressed;
        rightTriggerOnUp = inputDevice.RightTrigger.WasReleased;

        leftBumperOnDown = inputDevice.LeftBumper.WasPressed;
        rightBumperOnDown = inputDevice.RightBumper.WasPressed;

        _MaintainStations(leftBumperOnDown, rightBumperOnDown);
        _HookUpStations();
        _BulletsUpdate();

        //_CalculateAndMoveGunPointer();
        //_FiringController();

        //_Pilot();

        //_ShieldCalculateAndMove();
        //_BulletsUpdate();

        //_Rockets();

        //_Sword();
    }

    #region Switchers
    private void _MaintainStations(bool left, bool right)
    {
        if (left || right)
        {
            // populate list
            unassignedStations = new List<Stations>();
            unassignedStations.Add(Stations.Guns);
            unassignedStations.Add(Stations.Pilot);
            unassignedStations.Add(Stations.Rockets);
            unassignedStations.Add(Stations.Shield);
            unassignedStations.Add(Stations.Sword);

            unassignedStations.Remove(gameModel.leftStation);
            unassignedStations.Remove(gameModel.rightStation);

            if (left)
            {
                int next = (int)gameModel.leftStation + 1;
                if (next >= 5)
                    next = 0;

                if (unassignedStations.Contains((Stations)next))
                    gameModel.leftStation = (Stations)next;
                else
                {
                    next++;
                    if (next >= 5)
                        next = 0;
                    if (unassignedStations.Contains((Stations)next))
                        gameModel.leftStation = (Stations)next;
                }
            }
            else if (right)
            {
                int next = (int)gameModel.rightStation + 1;
                if (next >= 5)
                    next = 0;

                if (unassignedStations.Contains((Stations)next))
                    gameModel.rightStation = (Stations)next;
                else
                {
                    next++;
                    if (next >= 5)
                        next = 0;
                    if (unassignedStations.Contains((Stations)next))
                        gameModel.rightStation = (Stations)next;
                }
            }
        }
    }

    private void _HookUpStations()
    {
        if(gameModel.leftStation == Stations.Guns)
        {
            _CalculateAndMoveGunPointer(leftStickX, leftStickY);
            _FiringController(leftTriggerIsDown, leftTriggerOnUp);
        }
        else if (gameModel.leftStation == Stations.Pilot)
        {
            _Pilot(leftStickX, leftStickY, leftTriggerOnDown);
        }
        else if (gameModel.leftStation == Stations.Rockets)
        {
            _RocketAim(leftStickX, leftStickY);
            _Rockets(leftTriggerOnDown);
        }
        else if (gameModel.leftStation == Stations.Shield)
        {
            _ShieldCalculateAndMove(leftStickX, leftStickY);
        }
        else if(gameModel.leftStation == Stations.Sword)
        {
            _Sword(leftStickX, leftStickY, leftTriggerIsDown);
        }

        if (gameModel.rightStation == Stations.Guns)
        {
            _CalculateAndMoveGunPointer(rightStickX, rightStickY);
            _FiringController(rightTriggerIsDown, rightTriggerOnUp);
        }
        else if (gameModel.rightStation == Stations.Pilot)
        {
            _Pilot(rightStickX, rightStickY, rightTriggerOnDown);
        }
        else if (gameModel.rightStation == Stations.Rockets)
        {
            _RocketAim(rightStickX, rightStickY);
            _Rockets(rightTriggerOnDown);
        }
        else if (gameModel.rightStation == Stations.Shield)
        {
            _ShieldCalculateAndMove(rightStickX, rightStickY);
        }
        else if (gameModel.rightStation == Stations.Sword)
        {
            _Sword(rightStickX, rightStickY, rightTriggerIsDown);
        }
    }
    #endregion

    #region Pilot
    Vector3 inputDirRaw;
    Vector3 inputDirNorm;
    Vector3 moveDir;
    float xBoundCalc;
    Vector3 limitPos;

    private void _Pilot(float inputX, float inputY, bool jump)
    {
        xBoundCalc = Mathf.Lerp(xBoundClose, xBoundFar, (transform.position.z - zBoundClose) / (zBoundFar - zBoundClose));

        inputDirRaw.x = inputX;
        inputDirRaw.z = inputY;

        inputDirNorm = Vector3.Normalize(inputDirRaw);

        if (transform.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
            moveDir = inputDirRaw;
        else if (transform.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
            moveDir = inputDirRaw;

        else if (transform.position.z <= zBoundClose && Vector3.Dot(inputDirNorm, Vector3.forward) > 0)
            moveDir = inputDirRaw;
        else if (transform.position.z >= zBoundFar && Vector3.Dot(inputDirNorm, Vector3.back) > 0)
            moveDir = inputDirRaw;

        else moveDir = inputDirRaw;

        transform.position += moveDir * Time.deltaTime * moveVel;

        if (jump)
            transform.position += moveDir * 3;


        limitPos = transform.position;
        if (transform.position.x < -xBoundCalc)
            limitPos.x = -xBoundCalc;
        if (transform.position.x > xBoundCalc)
            limitPos.x = xBoundCalc;
        if (transform.position.z < zBoundClose)
            limitPos.z = zBoundClose;
        if (transform.position.z > zBoundFar)
            limitPos.z = zBoundFar;

        transform.position = limitPos;
    }
    #endregion

    #region Guns
    private void _CalculateAndMoveGunPointer(float inputX, float inputY)
    {
        relativeBearing = (Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg + 630) % 360;
        declination = Mathf.Sqrt(inputX * inputX + inputY * inputY) * 90;

        swivel.eulerAngles = new Vector3(0, -relativeBearing, 0);
        pitcher.localEulerAngles = new Vector3(declination, 0, 0);

        gun.rotation = Quaternion.RotateTowards(gun.rotation, pitcher.GetChild(0).rotation, 85 * Time.deltaTime);
    }

    private bool leftRightBarrel;

    private void _FiringController(bool shoot, bool release)
    {
        if (shoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer - fireFirstShotDelay >= fireTimeInterval)
            {
                shootTimer -= fireTimeInterval;
                if (leftRightBarrel)
                    _bullets.Add(Instantiate(bullet, transform.position + Vector3.left * .1f, Quaternion.identity, null));
                else
                    _bullets.Add(Instantiate(bullet, transform.position + Vector3.right * .1f, Quaternion.identity, null));
                leftRightBarrel = !leftRightBarrel;
                _bulletTimes.Add(0);
                _bullets[_bullets.Count - 1].GetComponent<Rigidbody>().AddForce(gun.forward * 30, ForceMode.Impulse);
                myAS.PlayOneShot(gunShot);
            }
        }
        if (release)
            shootTimer = 0;
    }
    private void _BulletsUpdate()
    {
        for (int i = 0; i < _bulletTimes.Count; i++)
        {
            _bulletTimes[i] += Time.deltaTime;
            if (_bulletTimes[i] >= 2.2f)
            {
                _bulletTimes.RemoveAt(i);
                GameObject bulletToDestroy = _bullets[i];
                _bullets.RemoveAt(i);
                Destroy(bulletToDestroy);
            }
        }
    }
    #endregion

    #region Rockets

    public GameObject rocket;

    private int rocketIncrementor;

    private float rBRG;
    private float rDEC;

    private void _RocketAim(float inputX, float inputY)
    {
        rBRG = (Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg + 630) % 360;
        rDEC = Mathf.Sqrt(inputX * inputX + inputY * inputY) * 90;

        if (rDEC != 0)
            rocketAim.eulerAngles = new Vector3(0, -rBRG, 0);
        else
            rocketAim.eulerAngles = Vector3.zero;
        rocketPitch.localEulerAngles = new Vector3(rDEC, 0, 0);
    }

    private void _Rockets(bool shoot)
    {
        if (shoot)
            rocketIncrementor = 1;

        if (rocketIncrementor > 0)
        {
            GameObject g = Instantiate(rocket, transform.position, Quaternion.identity);
            g.GetComponent<Debug_RocketFlight>().ultimatePath = rocketPitch.up;
            rocketIncrementor++;
            if (rocketIncrementor >= 40)
                rocketIncrementor = 0;
        }
    }
    #endregion

    #region Shield
    public Vector4 fwdShield;
    private void _ShieldCalculateAndMove(float inputX, float inputY)
    {
        fwdShield.x = inputX;
        fwdShield.z = inputY;
        fwdShield.y = 1 - (Mathf.Sqrt(inputX * inputX + inputY * inputY));
        shieldMat.SetVector("_Forward", fwdShield);
    }
    #endregion

    #region Sword
    private float brg;
    private float dec;
    private Transform swd;

    private Vector3 bigScale = new Vector3(.2f, 15, .2f);
    private Vector3 bigPos = new Vector3(0, 21.5f, 0);

    private Vector3 smallScale = new Vector3(.2f, 5, .2f), currentScale = new Vector3(.2f, 5, .2f);
    private Vector3 smallPos = new Vector3(0, 12, 0), currentPos = new Vector3(0, 12, 0);

    private void _Sword(float inputX, float inputY, bool extend)
    {
        brg = (Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg + 630) % 360;
        dec = Mathf.Sqrt(inputX * inputX + inputY * inputY) * 90;

        swordPivot.eulerAngles = new Vector3(0, -brg, 0);
        swordPivot2.localEulerAngles = new Vector3(dec, 0, 0);

        if (extend)
        {
            currentScale = Vector3.Lerp(currentScale, bigScale, 0.8f);
            currentPos = Vector3.Lerp(currentPos, bigPos, 0.8f);
        }
        else
        {
            currentScale = Vector3.Lerp(currentScale, smallScale, 0.8f);
            currentPos = Vector3.Lerp(currentPos, smallPos, 0.8f);
        }

        swd.localScale = currentScale;
        swd.localPosition = currentPos;
    }
    #endregion
}