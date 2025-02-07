using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float input_xMovement;
    public bool input_Jump;

    private InputActionMap inputActions;
    // Start is called before the first frame update
    void Start()
    {
       // inputActions = GetComponent<InputActionMap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Inputs_Movement(InputAction.CallbackContext ctx)
    {
         input_xMovement = ctx.ReadValue<float>();  //-1 for left, 1 for right. 
    }

    public void Imputs_Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.started) { input_Jump = true;}
        if(ctx.canceled) { input_Jump = false;}
    }
}
