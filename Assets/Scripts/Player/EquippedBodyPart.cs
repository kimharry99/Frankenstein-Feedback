using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquippedBodyPart : ScriptableObject
{
    public BodyPart[] bodyParts = new BodyPart[6];

    #region BodyPart Property
    public BodyPart Head
    {
        get
        {
            return bodyParts[(int)BodyPartType.Head];
        }
        set
        {
            if(value.bodyPartType != BodyPartType.Head)
            {
                Debug.Log("BodyPartType mismatched");
            }
            else
            {
                bodyParts[(int)BodyPartType.Head] = value;
            }
        }
    }
    public BodyPart Body
    {
        get
        {
            return bodyParts[(int)BodyPartType.Body];
        }
        set
        {
            if (value.bodyPartType != BodyPartType.Body)
            {
                Debug.Log("BodyPartType mismatched");
            }
            else
            {
                bodyParts[(int)BodyPartType.Body] = value;
            }
        }
    }
    public BodyPart LeftArm
    {
        get
        {
            return bodyParts[(int)BodyPartType.LeftArm];
        }
        set
        {
            if (value.bodyPartType != BodyPartType.LeftArm)
            {
                Debug.Log("BodyPartType mismatched");
            }
            else
            {
                bodyParts[(int)BodyPartType.LeftArm] = value;
            }
        }
    }
    public BodyPart RightArm
    {
        get
        {
            return bodyParts[(int)BodyPartType.RightArm];
        }
        set
        {
            if (value.bodyPartType != BodyPartType.RightArm)
            {
                Debug.Log("BodyPartType mismatched");
            }
            else
            {
                bodyParts[(int)BodyPartType.RightArm] = value;
            }
        }
    }
    public BodyPart LeftLeg
    {
        get
        {
            return bodyParts[(int)BodyPartType.LeftLeg];
        }
        set
        {
            if (value.bodyPartType != BodyPartType.LeftLeg)
            {
                Debug.Log("BodyPartType mismatched");
            }
            else
            {
                bodyParts[(int)BodyPartType.LeftLeg] = value;
            }
        }
    }
    public BodyPart RightLeg
    {
        get
        {
            return bodyParts[(int)BodyPartType.RightLeg];
        }
        set
        {
            if (value.bodyPartType != BodyPartType.RightLeg)
            {
                Debug.Log("BodyPartType mismatched");
            }
            else
            {
                bodyParts[(int)BodyPartType.RightLeg] = value;
            }
        }
    }

    public int GetCntOfBodyPart(Race race)
    {
        int cnt = 0;
        for(int i=0;i<bodyParts.Length;i++)
        {
            if (bodyParts[i] != null)
                if (bodyParts[i].race == race)
                    cnt++;
        }
        return cnt;
    }
    #endregion
    
}
