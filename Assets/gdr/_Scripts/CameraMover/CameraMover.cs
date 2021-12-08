using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    #region Singleton Init
    private static CameraMover _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
            Destroy(gameObject);
    }

    public static CameraMover Instance // Init not in order
    {
        get
        {
            if (_instance == null)
                Init();
            return _instance;
        }
        private set { _instance = value; }
    }

    static void Init() // Init script
    {
        _instance = FindObjectOfType<CameraMover>();
        _instance.Initialize();
    }
    #endregion

    public static bool isDebug;
    public static Color debugColor;

    [HideInInspector] public Vector3 defaultPosition;
    [HideInInspector] public Quaternion defaultRotation;
    public bool isAlwaysMoving = true;
    public float smooth = 1f;
    public float reachDistance = 0.05f;
    [NaughtyAttributes.Required]
    public Transform defaultViewTr;

    public bool isMove;
    public bool isMoveToDefaultPosition;

    [Header("Current Target Transform")]
    public Transform currentTarget;

    private Transform cameraTr;

    private float defaultSmooth;


    void Initialize()
    {
        cameraTr = transform;
        currentTarget = defaultViewTr;
        defaultSmooth = smooth;
        SetAsDefaultPosition(transform); // Current pos as default
        // Init data here
        enabled = true;
    }

    public void ChangeDefaultSpeed(float speed)
    {
        if (isDebug) DLog.D($"ChangeDefaultSpeed {speed}");
        defaultSmooth = speed;
    }

    public void Disable()
    {
        if (isDebug) DLog.D($"Disable");
        gameObject.SetActive(false);
    }
    
    public void Enable()
    {
        if (isDebug) DLog.D($"Enable");
        gameObject.SetActive(true);   
    }

    public void AttachToPlayerCameraTr(Transform playerCameraTr)
    {
        if (isDebug) DLog.D($"AttachToPlayerCameraTr {playerCameraTr}");
        SetAndMoveToTarget(playerCameraTr, true);
    }

    public void SetAndMoveToTarget(Transform _targetT, bool isInstantMove = false, float speed = -1f)
    {
        if (isDebug) DLog.D($"SetAndMoveToTarget {_targetT}");
        if (speed != -1f)
            smooth = speed;
        else
            smooth = defaultSmooth;

        currentTarget = _targetT;
        isMove = true;
        isMoveToDefaultPosition = false;

        if (isInstantMove)
        {
            // Debug.Log("isInstantMove");
            cameraTr.position = currentTarget.position;
            cameraTr.rotation = currentTarget.rotation;
        }
    }

    public void SetAsDefaultPosition(Transform view)
    {
        if (isDebug) DLog.D($"SetAsDefaultPosition");
        transform.rotation = view.rotation;
        transform.position = view.position;

        defaultPosition = view.position;
        defaultRotation = view.rotation;
    }

    public void MoveAtDefaultPosition()
    {
        if (isDebug) DLog.D($"MoveAtDefaultPosition");
        isMove = true;
        isMoveToDefaultPosition = true;
    }

    private void LateUpdate()
    {
        if (isMove)
        {
            if (currentTarget == null)
                isMove = false;
            else if (isMoveToDefaultPosition)
            {
                if (Vector3.Distance(cameraTr.position, defaultPosition) > reachDistance)
                {
                    cameraTr.position = Vector3.Lerp(cameraTr.position, defaultPosition, Time.deltaTime * smooth);
                    cameraTr.rotation = Quaternion.Lerp(cameraTr.rotation, defaultRotation, Time.deltaTime * smooth);
                }
                else
                {
                    isMove = false;
                }
            }
            else
            {
                if (!IsTargetReached())
                {
                    cameraTr.position = Vector3.Lerp(cameraTr.position, currentTarget.position, Time.deltaTime * smooth);
                    cameraTr.rotation = Quaternion.Lerp(cameraTr.rotation, currentTarget.rotation, Time.deltaTime * smooth);
                }
                else
                {
                    isMove = false;
                }
            }
        }
    }

    private bool IsTargetReached()
    {
        if (isAlwaysMoving)
            return false;
        else
            return (cameraTr.position - currentTarget.position).sqrMagnitude < reachDistance;
    }
}
