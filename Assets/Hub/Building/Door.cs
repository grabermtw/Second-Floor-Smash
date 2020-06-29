using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : HubInteractor
{
    private Animator hinge;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hinge.GetCurrentAnimatorStateInfo(0).IsTag("Actionable"))
        {
            base.UpdateStatus();
        }
        
    }

    public void Open()
    {
        base.playerControl.UnsetInteract();
        hinge.SetBool("DoorOpen", true);
    }

    public void Close()
    {
        base.playerControl.UnsetInteract();
        hinge.SetBool("DoorOpen", false);
    }

    public void OpenAndClose()
    {
        base.playerControl.UnsetInteract();
        hinge.SetTrigger("OpenAndClose");
    }

    public override void Interact()
    {
        OpenAndClose();
    }
}
