using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackCollider 
{
    [SerializeField]
    private Transform colliderObject;
    [SerializeField]
    private Transform followTransform;

    public AttackCollider(Transform colliderObject, Transform followTransform)
    {
        this.colliderObject = colliderObject;
        this.followTransform = followTransform;
    }

    public void UpdatePosition() {
        colliderObject.position = new Vector3(followTransform.position.x, followTransform.position.y, 0);
    }
}

public class CharacterColliderControl : MonoBehaviour
{
    public GameObject standardAttackColliderPrefab;
    public AttackCollider[] customAttackColliders;
    private string[] standardAttackColliderPositionObjectNames = new string[] {
        "mixamorig:LeftToeBase", 
        "mixamorig:RightToeBase",
        "mixamorig:LeftHandMiddle1",
        "mixamorig:RightHandMiddle1"};
    private AttackCollider[] allAttackColliders;

    // Start is called before the first frame update
    void Start()
    {
        // Create parent for standard attack colliders
        GameObject attackCollidersParent = Instantiate(new GameObject(), transform);
        attackCollidersParent.name = "Standard Attack Colliders";

        // combine both attack collider arrays
        allAttackColliders = new AttackCollider[standardAttackColliderPositionObjectNames.Length + customAttackColliders.Length];
        for (int i = 0; i < customAttackColliders.Length; i++)
        {
            allAttackColliders[i] = customAttackColliders[i];
        }
        // Instantiate standard attack colliders and add them to the array
        for (int i = 0; i < standardAttackColliderPositionObjectNames.Length; i++)
        {
            Transform newStandardCollider = Instantiate(standardAttackColliderPrefab, attackCollidersParent.transform).transform;
            allAttackColliders[customAttackColliders.Length + i] = new AttackCollider(newStandardCollider,
                                                                FindChildRecursively(transform, standardAttackColliderPositionObjectNames[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update all attack collider positions once every frame
        foreach(AttackCollider attackCollider in allAttackColliders)
        {
            Debug.Log("yeet " + attackCollider);
            attackCollider.UpdatePosition();
        }
    }

    private Transform FindChildRecursively(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform result = FindChildRecursively(child, childName);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return null;
    }
}
