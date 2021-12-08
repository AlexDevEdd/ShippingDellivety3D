using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IndependedMoveSystem : MonoBehaviour
{
    #region Singleton Init
    private static IndependedMoveSystem _instance;

    void Awake() // Init in order
    {
        if (_instance == null)
            Init();
        else if (_instance != this)
        {
            Debug.Log($"Destroying {gameObject.name}, caused by one singleton instance");
            Destroy(gameObject);
        }
    }

    public static IndependedMoveSystem Instance // Init not in order
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
        _instance = FindObjectOfType<IndependedMoveSystem>();
        if (_instance != null)
            _instance.Initialize();
    }
    #endregion

    public class MoveData
    {
        public Transform target;
        public Vector3 start;
        public Vector3 end;
        public float duration;
        public float elapsed;
        public bool isSmooth;
        public UnityAction actionOnEnd;
    }

    public List<MoveData> moveRequests;
    public AnimationCurve smoothCurve;

    public class RotationData
    {
        public Transform target;
        public Quaternion startRotation;
        public Quaternion targetRotation;
        public float duration;
        public float elapsed;
        public bool isSmooth;
        public UnityAction actionOnEnd;
    }
    public List<RotationData> rotationRequests;
    public AnimationCurve rotationSmoothCurve;

    public class PathData
    {
        public Transform target;
        public int currentPointId;
        public List<Vector3> points;
        public float rotationSpeed;
        public float moveSpeed;
        public bool isSmoothRotation;
        public UnityAction actionOnEnd;
        public bool isRotateX;
        public bool isRotate;

        public bool IsLastReached()
        {
            if (points.Count < 2)
            {
                return true;
            }
            if (currentPointId >= points.Count)
                return true;
            return false;
        }

        public void RecalcPointId()
        {
            var currentPos = target.position;
            var targetPos = points[currentPointId];
            if ((currentPos - targetPos).sqrMagnitude < 0.001f)
            {
                target.position = points[currentPointId];
                currentPointId++;
            }
        }
    }
    public List<PathData> pathRequests;

    public class RepeativeMoveData
    {
        public Transform target;
        public Vector2 randomTimeRepeat;
        public float duration;
        public float elapsed;
        public float rolledRepeat;
        public float rotationSpeed;
        public bool isSmooth;
        public bool isAlsoRotate;
        public float fullPathLength;
        public List<Vector3> path;
        public UnityAction actionOnEnd;

        public void CalcFullPath()
        {
            float len = 0;
            for(int i = 1; i < path.Count; i++)
            {
                len += (path[i] - path[i - 1]).magnitude;
            }
            fullPathLength = len;
        }

        public void Roll()
        {
            rolledRepeat = Random.Range(randomTimeRepeat.x, randomTimeRepeat.y);
        }

        public void TryRepeat()
        {
            if (elapsed > rolledRepeat)
            {
                InitPlace();
                Roll();
                if (actionOnEnd != null)
                    actionOnEnd.Invoke();
            }
        }

        public void InitPlace()
        {
            target.transform.position = path[0];
            elapsed = 0f;
            if (isAlsoRotate)
                target.transform.rotation = Quaternion.LookRotation((path[1] - path[0]).normalized, Vector3.up);
        }

        public Vector3 GetPosition(float passedDistance)
        {
            var pos = path[0];
            var passed = 0f;
            for(int i = 1; i < path.Count; i++)
            {
                pos = path[i];
                passed += (path[i] - path[i - 1]).magnitude;
                if (passed > passedDistance)
                {
                    pos = path[i - 1]; // Go back
                    var target = path[i]; // Target found
                    var vector = target - pos; // Vector found
                    var dir = vector.normalized; // Direction found
                    var length = vector.magnitude; // Length to pass from [i-1] to [i]
                    var initPass = passed - length; // Length to pass all way to [i-1]
                    // passed - length to pass all way to [i]
                    var overcome = passedDistance - initPass; // Overcome length
                    var percent = overcome / length; // Vector length
                    var value = pos + vector * percent;
                    return value;
                }
            }
            return path[path.Count - 1]; // Last
        }
    }
    public List<RepeativeMoveData> repeativeMoveData;


    public class LocalMoveData
    {
        public Transform target;
        public Vector3 start;
        public Vector3 end;
        public float duration;
        public float elapsed;
        public bool isSmooth;
        public UnityAction actionOnEnd;
    }

    public List<LocalMoveData> localMoveRequests;


    public class LocalRotationData
    {
        public Transform target;
        public Quaternion startRotation;
        public Quaternion targetRotation;
        public float duration;
        public float elapsed;
        public bool isSmooth;
        public UnityAction actionOnEnd;
    }
    public List<LocalRotationData> localRotationRequests;

    void Initialize()
    {
        moveRequests = new List<MoveData>();
        rotationRequests = new List<RotationData>();
        pathRequests = new List<PathData>();
        repeativeMoveData = new List<RepeativeMoveData>();
        localMoveRequests = new List<LocalMoveData>();
        localRotationRequests = new List<LocalRotationData>();
        // Init data here
        enabled = true;
    }

    private void Update()
    {
        for(int i = 0; i < moveRequests.Count; i++)
        {
            var mr = moveRequests[i];
            if (mr.target == null)
            {
                moveRequests.Remove(mr);
                i--;
            }
            else if (mr.elapsed >= mr.duration)
            {
                moveRequests.Remove(mr);
                i--;
                if (mr.actionOnEnd != null)
                    mr.actionOnEnd.Invoke();
            }
            else
            {
                mr.elapsed += Time.deltaTime;
                var start = mr.start;
                var end = mr.end;
                var percent = Mathf.Clamp01(mr.elapsed / mr.duration);
                if (mr.isSmooth)
                    mr.target.position = Vector3.Lerp(start, end, smoothCurve.Evaluate(percent));
                else
                    mr.target.position = Vector3.Lerp(start, end, percent);
            }
        }

        for (int i = 0; i < rotationRequests.Count; i++)
        {
            var rr = rotationRequests[i];
            if (rr.target == null)
            {
                rotationRequests.Remove(rr);
                i--;
            }
            else if (rr.elapsed >= rr.duration)
            {
                rotationRequests.Remove(rr);
                i--;
                if (rr.actionOnEnd != null)
                    rr.actionOnEnd.Invoke();
            }
            else
            {
                rr.elapsed += Time.deltaTime;
                var start = rr.startRotation;
                var end = rr.targetRotation;
                var percent = Mathf.Clamp01(rr.elapsed / rr.duration);
                if (rr.isSmooth)
                    rr.target.rotation = Quaternion.Lerp(start, end, smoothCurve.Evaluate(percent));
                else
                    rr.target.rotation = Quaternion.Lerp(start, end, percent);
            }
        }

        for (int i = 0; i < pathRequests.Count; i++)
        {
            var pr = pathRequests[i];
            if (pr.target == null)
            {
                pathRequests.Remove(pr);
                i--;
            }
            else if (pr.IsLastReached()) // Special case
            {
                pathRequests.Remove(pr);
                i--;
                if (pr.actionOnEnd != null)
                    pr.actionOnEnd.Invoke();
            }
            else
            {
                pr.target.position = Vector3.MoveTowards(pr.target.position, pr.points[pr.currentPointId], pr.moveSpeed * Time.deltaTime);
                var vector = (pr.points[pr.currentPointId] - pr.target.position);
                if (vector != Vector3.zero && pr.isRotate)
                {
                    if (!pr.isRotateX)
                        vector.y = 0f;
                    Quaternion rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
                    pr.target.rotation = Quaternion.Lerp(pr.target.rotation, rotation, pr.rotationSpeed * Time.deltaTime);
                }
                pr.RecalcPointId();
                if (pr.IsLastReached())
                {
                    pathRequests.Remove(pr);
                    i--;
                    if (pr.actionOnEnd != null)
                        pr.actionOnEnd.Invoke();
                }
            }
        }

        for (int i = 0; i < repeativeMoveData.Count; i++)
        {
            var rmd = repeativeMoveData[i];
            if (rmd.target == null)
            {
                repeativeMoveData.Remove(rmd);
                i--;
            }
            else if (rmd.elapsed >= rmd.duration) // Special case
            {
                rmd.elapsed += Time.deltaTime;
                rmd.TryRepeat();
            }
            else
            {
                rmd.elapsed += Time.deltaTime;
                // Percent
                var percent = Mathf.Clamp01(rmd.elapsed / rmd.duration);
                float transformedPercent;
                if (rmd.isSmooth)
                    transformedPercent = smoothCurve.Evaluate(percent);
                else
                    transformedPercent = percent;
                // Pos
                var targetLength = rmd.fullPathLength * transformedPercent;
                var pos = rmd.GetPosition(targetLength);
                rmd.target.transform.position = pos;

                // Rotation
                var curPos = rmd.target.transform.position;
                var dir = pos - curPos;
                if (rmd.isAlsoRotate && dir != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
                    rmd.target.rotation = Quaternion.Lerp(rmd.target.rotation, rotation, rmd.rotationSpeed * Time.deltaTime);
                }

            }
        }

        for (int i = 0; i < localMoveRequests.Count; i++)
        {
            var lmr = localMoveRequests[i];
            if (lmr.target == null)
            {
                localMoveRequests.Remove(lmr);
                i--;
            }
            else if (lmr.elapsed >= lmr.duration)
            {
                localMoveRequests.Remove(lmr);
                i--;
                if (lmr.actionOnEnd != null)
                    lmr.actionOnEnd.Invoke();
            }
            else
            {
                lmr.elapsed += Time.deltaTime;
                var start = lmr.start;
                var end = lmr.end;
                var percent = Mathf.Clamp01(lmr.elapsed / lmr.duration);
                if (lmr.isSmooth)
                    lmr.target.localPosition = Vector3.Lerp(start, end, smoothCurve.Evaluate(percent));
                else
                    lmr.target.localPosition = Vector3.Lerp(start, end, percent);
            }
        }

        for (int i = 0; i < localRotationRequests.Count; i++)
        {
            var lrr = localRotationRequests[i];
            if (lrr.target == null)
            {
                localRotationRequests.Remove(lrr);
                i--;
            }
            else if (lrr.elapsed >= lrr.duration)
            {
                localRotationRequests.Remove(lrr);
                i--;
                if (lrr.actionOnEnd != null)
                    lrr.actionOnEnd.Invoke();
            }
            else
            {
                lrr.elapsed += Time.deltaTime;
                var start = lrr.startRotation;
                var end = lrr.targetRotation;
                var percent = Mathf.Clamp01(lrr.elapsed / lrr.duration);
                if (lrr.isSmooth)
                    lrr.target.localRotation = Quaternion.Lerp(start, end, smoothCurve.Evaluate(percent));
                else
                    lrr.target.localRotation = Quaternion.Lerp(start, end, percent);
            }
        }
    }

    public void Move(Transform target, Vector3 position, float duration, bool isSmooth, UnityAction actionOnEnd = null)
    {
        if (target == null)
            return;
        var item = IsAlreadyAdded(target);
        if (item != null)
        {
            item.start = target.position;
            item.end = position;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
        }
        else
        {
            item = new MoveData();
            item.target = target;
            item.start = target.position;
            item.end = position;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
            moveRequests.Add(item);
        }
    }

    public void MoveWithSpeed(Transform target, Vector3 position, float speed, bool isSmooth, UnityAction actionOnEnd = null)
    {
        if (speed <= 0f)
        {
            Debug.LogError($"Cant move with 0 or negative speed");
            return;
        }
        if (target == null)
            return;

        float distance = (target.position - position).magnitude;
        float duration = distance / speed;

        var item = IsAlreadyAdded(target);
        if (item != null)
        {
            item.start = target.position;
            item.end = position;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
        }
        else
        {
            item = new MoveData();
            item.target = target;
            item.start = target.position;
            item.end = position;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
            moveRequests.Add(item);
        }
    }

    private MoveData IsAlreadyAdded(Transform target)
    {
        foreach(var item in moveRequests)
        {
            if (item.target == target)
            {
                return item;
            }
        }
        return null;
    }

    public void Rotate(Transform target, Quaternion targetRotation, float duration, bool isSmooth, UnityAction actionOnEnd = null)
    {
        if (target == null)
            return;

        var item = IsAlreadyAddedRotation(target);
        if (item != null)
        {
            item.startRotation = target.rotation;
            item.targetRotation = targetRotation;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
        }
        else
        {
            item = new RotationData();
            item.startRotation = target.rotation;
            item.targetRotation = targetRotation;
            item.target = target;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
            rotationRequests.Add(item);
        }
    }

    private RotationData IsAlreadyAddedRotation(Transform target)
    {
        foreach (var item in rotationRequests)
        {
            if (item.target == target)
            {
                return item;
            }
        }
        return null;
    }

    public void GoPath(Transform target, List<Transform> path, float moveSpeed, float rotationSpeed, bool isRotateX, bool isRotate, UnityAction actionOnEnd = null)
    {
        if (moveSpeed <= 0f)
            return;
        if (target == null)
            return;

        var item = IsAlreadyAddedPath(target);
        if (item != null)
        {
            List<Quaternion> rotations = new List<Quaternion>();
            List<Vector3> points = new List<Vector3>();

            // Init
            rotations.Add(target.rotation);
            points.Add(target.position);

            for (int i = 0; i < path.Count; i++)
            {
                rotations.Add(path[i].rotation);
                points.Add(path[i].position);
            }
            item.target = target;
            item.currentPointId = 1;
            item.points = points;
            item.rotationSpeed = rotationSpeed;
            item.moveSpeed = moveSpeed;
            item.actionOnEnd = actionOnEnd;
            item.isRotateX = isRotateX;
            item.isRotate = isRotate;
        }
        else
        {
            item = new PathData();
            List<Quaternion> rotations = new List<Quaternion>();
            List<Vector3> points = new List<Vector3>();

            // Init
            rotations.Add(target.rotation);
            points.Add(target.position);

            for (int i = 0; i < path.Count; i++)
            {
                rotations.Add(path[i].rotation);
                points.Add(path[i].position);
            }
            item.target = target;
            item.currentPointId = 1;
            item.points = points;
            item.rotationSpeed = rotationSpeed;
            item.moveSpeed = moveSpeed;
            item.actionOnEnd = actionOnEnd;
            item.isRotateX = isRotateX;
            item.isRotate = isRotate;
            pathRequests.Add(item);
        }
    }

    private PathData IsAlreadyAddedPath(Transform target)
    {
        foreach (var item in pathRequests)
        {
            if (item.target == target)
            {
                return item;
            }
        }
        return null;
    }

    public void Attach(Transform tr, Transform target)
    {
        tr.SetParent(target);
        tr.localRotation = Quaternion.identity;
        tr.localPosition = Vector3.zero;
    }

    public void DoRepeativeMove(Transform target, List<Vector3> path, float duration, Vector2 randomTimeRepeat, bool isSmooth, bool isAlsoRotate, float rotationSpeed = 0f, UnityAction actionOnEnd = null)
    {
        if (target == null)
            return;

        var item = IsAlreadyAddedRepeativeMove(target);
        if (item != null)
        {
            item.target = target;
            item.randomTimeRepeat = randomTimeRepeat;
            item.duration = duration;
            item.elapsed = 0;
            item.isSmooth = isSmooth;
            item.isAlsoRotate = isAlsoRotate;
            item.rotationSpeed = rotationSpeed;
            item.path = path;
            item.actionOnEnd = actionOnEnd;
            item.CalcFullPath();
            item.Roll();
            item.InitPlace();
        }
        else
        {
            item = new RepeativeMoveData();
            item.target = target;
            item.randomTimeRepeat = randomTimeRepeat;
            item.duration = duration;
            item.elapsed = 0;
            item.isSmooth = isSmooth;
            item.isAlsoRotate = isAlsoRotate;
            item.rotationSpeed = rotationSpeed;
            item.path = path;
            item.actionOnEnd = actionOnEnd;
            item.CalcFullPath();
            item.Roll();
            item.InitPlace();
            repeativeMoveData.Add(item);
        }
    }

    private RepeativeMoveData IsAlreadyAddedRepeativeMove(Transform target)
    {
        foreach (var item in repeativeMoveData)
        {
            if (item.target == target)
            {
                return item;
            }
        }
        return null;
    }

    public void MoveLocal(Transform target, Vector3 position, float duration, bool isSmooth, UnityAction actionOnEnd = null)
    {
        if (target == null)
            return;
        var item = IsAlreadyAddedLocalMove(target);
        if (item != null)
        {
            item.start = target.localPosition;
            item.end = position;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
        }
        else
        {
            item = new LocalMoveData();
            item.target = target;
            item.start = target.localPosition;
            item.end = position;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
            localMoveRequests.Add(item);
        }
    }

    private LocalMoveData IsAlreadyAddedLocalMove(Transform target)
    {
        foreach (var item in localMoveRequests)
        {
            if (item.target == target)
            {
                return item;
            }
        }
        return null;
    }

    public void LocalRotate(Transform target, Quaternion targetRotation, float duration, bool isSmooth, UnityAction actionOnEnd = null)
    {
        if (target == null)
            return;

        var item = IsAlreadyAddedLocalRotation(target);
        if (item != null)
        {
            item.startRotation = target.localRotation;
            item.targetRotation = targetRotation;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
        }
        else
        {
            item = new LocalRotationData();
            item.startRotation = target.localRotation;
            item.targetRotation = targetRotation;
            item.target = target;
            item.duration = duration;
            item.isSmooth = isSmooth;
            item.elapsed = 0f;
            item.actionOnEnd = actionOnEnd;
            localRotationRequests.Add(item);
        }
    }

    private LocalRotationData IsAlreadyAddedLocalRotation(Transform target)
    {
        foreach (var item in localRotationRequests)
        {
            if (item.target == target)
            {
                return item;
            }
        }
        return null;
    }
}
