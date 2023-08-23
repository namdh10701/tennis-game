using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Racket : MonoBehaviour
    {
        [SerializeField] private CPU _cpu;
        public void ProcessHit()
        {
            _cpu.ProcessHit();
        }
    }
}
