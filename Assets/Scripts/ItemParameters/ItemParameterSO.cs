using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    [CreateAssetMenu]
    public class ItemParameterSO : ScriptableObject
    {
        [field: SerializeField]
        public string ParameterName { get; private set; }
    }
}
