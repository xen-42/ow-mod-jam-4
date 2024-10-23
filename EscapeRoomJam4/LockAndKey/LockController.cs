using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomJam4
{
    public class LockController : MonoBehaviour
    {
        [HideInInspector]
        public static LockController instance;

        // I would prefer to use a Dictionary for this but Unity can't Serialize those,
        // so a blind array it is
        [Tooltip("Layer1 = 0\nLayer2 = 1\nLayer3 = 2\nFinal = 3")]
        public UnityEvent[] openEvents;

        private Dictionary<LockType, Dictionary<Lock, bool>> locks;

        private void Awake()
        {
            instance = this;
            locks = new Dictionary<LockType, Dictionary<Lock, bool>>();
        }

        public void RegisterLock(Lock @lock, LockType type)
        {
            if (!locks.ContainsKey(type)) locks.Add(type, new Dictionary<Lock, bool>());
            locks[type].Add(@lock, false);
        }

        public void OpenLock(Lock @lock, LockType type)
        {
            locks[type][@lock] = true;
            TestLocks(type);
        }

        private void TestLocks(LockType type)
        {
            foreach(bool isUnlocked in locks[type].Values)
            {
                if (!isUnlocked) return;
            }
            openEvents[(int)type]?.Invoke();
        }
    }
}
