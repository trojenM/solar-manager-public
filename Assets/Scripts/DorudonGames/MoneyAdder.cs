using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DorudonGames
{
    public class MoneyAdder : MonoBehaviour
    {
        public void SetScaleZ(float z)
        {
            var scl = transform.localScale;
            scl.z = z;
            transform.localScale = scl;
        }
    }
}
