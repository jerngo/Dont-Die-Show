using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCont : MonoBehaviour
{

    void Speed(float reducedSpeed) {
        GetComponentInParent<PlayerControl>().SetSpeed(reducedSpeed);
    }
}
