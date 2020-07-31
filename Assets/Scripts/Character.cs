using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : SingletonBehaviour<Character>
{
    [Header("Body Parts")]
    public BodyPart head;
    public BodyPart body;
    public BodyPart LeftArm;
    public BodyPart RightArm;
    public BodyPart LeftLeg;
    public BodyPart RightLeg;

    // for debugging
    public int forTest = 0;

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion

    public void SendTime(int time)
    {

    }
}

