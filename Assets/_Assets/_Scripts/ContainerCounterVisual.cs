using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // get event 
        containerCounter.OnPlayerGrabbedObject += Container_Counter_OnPlayerGrabbedObject;
    }

    private void Container_Counter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        // trigger actions for event 
        animator.SetTrigger(OPEN_CLOSE);
    }

}
