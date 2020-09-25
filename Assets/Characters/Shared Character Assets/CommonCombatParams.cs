using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class contains code to be called from the base animation controller
// that each character will use to get the parameters for the attack that is
// being performed.

public class CommonCombatParams : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField]
    private PunchInfo punchInfo = default;
    [SerializeField]
    private SidePunchInfo sidePunchInfo = default;
    [SerializeField]
    private UpPunchInfo upPunchInfo = default;
    [SerializeField]
    private DownPunchInfo downPunchInfo = default;
    [SerializeField]
    private AirPunchInfo airPunchInfo = default;
    [SerializeField]
    private AirPunchDownInfo airPunchDownInfo = default;
    [SerializeField]
    private AirPunchUpInfo airPunchUpInfo = default;
    [SerializeField]
    private ShieldInfo shieldInfo = default;
    [SerializeField]
    private DodgeInfo dodgeInfo = default;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        punchInfo.SetCharCtrl(characterController);
        sidePunchInfo.SetCharCtrl(characterController);
        upPunchInfo.SetCharCtrl(characterController);
        downPunchInfo.SetCharCtrl(characterController);
        airPunchInfo.SetCharCtrl(characterController);
        airPunchDownInfo.SetCharCtrl(characterController);
        airPunchUpInfo.SetCharCtrl(characterController);
        shieldInfo.SetCharCtrl(characterController);
        dodgeInfo.SetCharCtrl(characterController);
    }

    public PunchInfo GetPunchInfo()
    {
        return punchInfo;
    }

    public SidePunchInfo GetSidePunchInfo()
    {
        return sidePunchInfo;
    }

    public UpPunchInfo GetUpPunchInfo()
    {
        return upPunchInfo;
    }

    public DownPunchInfo GetDownPunchInfo()
    {
        return downPunchInfo;
    }

    public AirPunchInfo GetAirPunchInfo()
    {
        return airPunchInfo;
    }

    public AirPunchDownInfo GetAirPunchDownInfo()
    {
        return airPunchDownInfo;
    }

    public AirPunchUpInfo GetAirPunchUpInfo()
    {
        return airPunchUpInfo;
    }

    public ShieldInfo GetShieldInfo()
    {
        return shieldInfo;
    }

    public DodgeInfo GetDodgeInfo()
    {
        return dodgeInfo;
    }



}


// ------------------------ INFO CLASSES ----------------------
// Most of these are nearly identical, it is just that some
// of the default values have changed. This is the reason for them
// being all different classes

[System.Serializable]
public class PunchInfo
{
    private CharacterController charCtrl;
    public float damageAmount = 3;
    public float launchFactor = 0.1f;
    public float attackHeight = 1.16f;
    public float attackRange = 0.8f;
    public float attackAngle = 0;
    public float launchAngle = 0.55f;

    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class SidePunchInfo
{
    private CharacterController charCtrl;
    public float damageAmount = 6;
    public float launchFactor = 0.2f;
    public float attackHeight = 1.16f;
    public float attackRange = 0.8f;
    public float attackAngle = 0;
    public float launchAngle = 0.75f;
    public float moveSpeed = 2f;
    public bool attack = true;

    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class UpPunchInfo
{
    private CharacterController charCtrl;
    public bool attackTwice = true;
    public float damageAmount = 6;
    public float launchFactor = 0.2f;
    public float attackHeight = 1.1f;
    public float attackRange = 0.8f;
    public float firstAttackAngle = 0;
    public float secondAttackAngle = 1.5f;
    public float launchAngle = 1.5f;

    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class DownPunchInfo
{
    private CharacterController charCtrl;
    public bool attackTwice = true;
    public float firstDamageAmount = 6;
    public float secondDamageAmount = 6;
    public float firstLaunchFactor = 0.2f;
    public float secondLaunchFactor = 0.2f;
    public float firstAttackHeight = 0.1f;
    public float secondAttackHeight = 0.1f;
    public float firstAttackRange = 1.5f;
    public float secondAttackRange = 0.8f;
    public float firstAttackAngle = 0;
    public float secondAttackAngle = 3.14f;
    public float firstLaunchAngle = 1.5f;
    public float secondLaunchAngle = 1.74f;

    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class AirPunchInfo
{
    private CharacterController charCtrl;
    public bool attackTwice = true;
    public float firstDamageAmount = 3;
    public float secondDamageAmount = 3;
    public float firstLaunchFactor = 0.1f;
    public float secondLaunchFactor = 0.1f;
    public float firstAttackHeight = 0.7f;
    public float secondAttackHeight = 1f;
    public float firstAttackRange = 0.6f;
    public float secondAttackRange = 0.6f;
    public float firstAttackAngle = 0;
    public float secondAttackAngle = 0;
    public float firstLaunchAngle = 0.7f;
    public float secondLaunchAngle = -0.7f;

    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class AirPunchDownInfo
{
    private CharacterController charCtrl;
    public bool attackTwice = true;
    public bool doubleAttackTwice = true;
    public float firstDamageAmount = 3;
    public float secondDamageAmount = 3;
    public float firstLaunchFactor = 0.1f;
    public float secondLaunchFactor = 0.1f;
    public float firstAttackHeight = 0.45f;
    public float secondAttackHeight = 0.45f;
    public float firstAttackRange = 0.65f;
    public float secondAttackRange = 0.65f;
    public float firstAttackAngle = -1.9f;
    public float secondAttackAngle = -1.4f;
    public float firstLaunchAngle = -1.75f;
    public float secondLaunchAngle = -1.4f;

    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class AirPunchUpInfo
{
    private CharacterController charCtrl;
    public bool attackTwice = true;
    public float firstDamageAmount = 3;
    public float secondDamageAmount = 3;
    public float firstLaunchFactor = 0.1f;
    public float secondLaunchFactor = 0.1f;
    public float firstAttackHeight = 1.1f;
    public float secondAttackHeight = 1.1f;
    public float firstAttackRange = 0.7f;
    public float secondAttackRange = 0.8f;
    public float firstAttackAngle = 0;
    public float secondAttackAngle = 1.5f;
    public float firstLaunchAngle = 1.5f;
    public float secondLaunchAngle = 1.5f;

    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class ShieldInfo
{
    private CharacterController charCtrl;
    public float damageAmount = 3;
    public float launchFactor = 0.01f;
    public float launchAngle = 1.4f;
    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}

[System.Serializable]
public class DodgeInfo
{
    private CharacterController charCtrl;
    public float dodgeRate = 2;
    public void SetCharCtrl(CharacterController charCtrl)
    {
        this.charCtrl = charCtrl;
    }

    public CharacterController GetCharCtrl()
    {
        return charCtrl;
    }
}