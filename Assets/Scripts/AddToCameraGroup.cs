using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToCameraGroup : MonoBehaviour
{
    CinemachineTargetGroup targetGroup;
    // Start is called before the first frame update
    void Start()
    {
        targetGroup = GameObject.FindWithTag("GroupCam").GetComponent<CinemachineTargetGroup>();
        targetGroup.AddMember(this.transform, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
