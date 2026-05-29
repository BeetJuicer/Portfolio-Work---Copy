using NUnit.Framework.Constraints;
using UnityEngine;

public class FastIKFabric : MonoBehaviour
{
    public int ChainLength = 2;
    public Transform Target;
    public Transform Pole;


    private float CompleteLength;
    private float[] BonesLength;
    private Transform[] Bones;
    private Vector3[] BonePositions;

    private Vector3[] StartDirectionSucc;
    private Quaternion[] StartRotationBone;
    private Quaternion StartRotationTarget;
    private Quaternion StartRotationRoot;

    [SerializeField] private int iterations;
    [SerializeField] private float deltaSquared = 0.002f;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Bones = new Transform[ChainLength + 1];
        BonePositions = new Vector3[ChainLength + 1];
        BonesLength = new float[ChainLength];
        StartDirectionSucc = new Vector3[ChainLength + 1];
        StartRotationBone = new Quaternion[ChainLength + 1];

        CompleteLength = 0;

        if (Target == null)
        {
            Debug.LogWarning("no target!");
        }
        StartRotationTarget = Target.rotation;


        var current = transform;
        for (int i = Bones.Length - 1; i >= 0; i--)
        {
            Bones[i] = current;
            StartRotationBone[i] = current.rotation;

            if (i == Bones.Length - 1) //skip thje frist one
            {
                
                StartDirectionSucc[i] = Target.position - current.position; 
            }
            else
            {
                StartDirectionSucc[i] = Bones[i + 1].position - current.position;
                BonesLength[i] = StartDirectionSucc[i].magnitude;
                CompleteLength += BonesLength[i];
            }

            current = current.parent;
        }

        if (Bones[0] == null)
        {
            Debug.LogError("The chain value is longer than the ancestor chain!");
        }
    }

    //private void LateUpdate()
    //{
    //    ResolveIK();
    //}

    public void ResolveIK()
    {

        if (Target == null)
            return;

        if(BonesLength.Length != ChainLength)
            Init();

        //get
        for (int i = 0; i < Bones.Length; i++)
        {
            BonePositions[i] = Bones[i].position;
        }

        var rootRot = (Bones[0].parent != null) ? Bones[0].parent.rotation : Quaternion.identity;
        var rootRotDiff = rootRot * Quaternion.Inverse(StartRotationRoot);

        //calculations
        if (CompleteLength * CompleteLength <= (Target.position - BonePositions[0]).sqrMagnitude)
        {
            //unreachable. just stretch
            Vector3 direction = (Target.position - BonePositions[0]).normalized;
            for (int i = 1; i < BonePositions.Length; i++)
            {
                BonePositions[i] = BonePositions[i - 1] + direction * BonesLength[i - 1];
            }
        }
        else 
        {
            for (int iteration = 0; iteration < iterations; iteration++)
            {

                //Work backward from the target.
                for (int j = BonePositions.Length - 1; j > 0; j--)
                {
                    //for the "head" bone, just place it directly where the target is.
                    if (j == BonePositions.Length - 1)
                        BonePositions[j] = Target.position;
                    // Working backwards, set all previous bones to where they should be if the head bone is moved.
                    else
                        BonePositions[j] = BonePositions[j + 1] + (BonePositions[j] - BonePositions[j + 1]).normalized * BonesLength[j];
                }

                // Now starting from the root, we adjust all the other ones where they should be, if the root stays in place.
                for (int i = 1; i < BonePositions.Length; i++)
                {
                    BonePositions[i] = BonePositions[i - 1] + (BonePositions[i] - BonePositions[i - 1]).normalized * BonesLength[i - 1];
                }

                // We're close enough
                if ((BonePositions[BonePositions.Length - 1] - Target.position).sqrMagnitude < deltaSquared)
                {
                    break;
                }
            }
        }

        if(Pole != null)
        {
            for(int i = 1; i < BonePositions.Length - 1; i++)
            {
                var plane = new Plane(BonePositions[i + 1] - BonePositions[i - 1], BonePositions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(Pole.position);
                var projectedBone = plane.ClosestPointOnPlane(BonePositions[i]);
                var angle = Vector3.SignedAngle(projectedBone - BonePositions[i - 1], projectedPole - BonePositions[i - 1], plane.normal);
                BonePositions[i] = Quaternion.AngleAxis(angle, plane.normal) * (BonePositions[i] - BonePositions[i - 1]) + BonePositions[i - 1];
            }
        }

        //set
        for (int i = 0; i < Bones.Length; i++)
        {
            // Set Rotation
            if (i == BonePositions.Length - 1)
                Bones[i].rotation = Target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[i];
            else
                Bones[i].rotation = Quaternion.FromToRotation(StartDirectionSucc[i], BonePositions[i + 1] - BonePositions[i]) * StartRotationBone[i];

            // Set Position
            Bones[i].position = BonePositions[i];
        }
    }

    private void OnDrawGizmos()
    {
        var current = this.transform;
        for (int i = 0; i < ChainLength; i++) {
            
            if (current == null || current.parent == null)
                return;

            Gizmos.DrawLine(current.transform.position, current.parent.transform.position);
            current = current.parent;
        }
    }
}
