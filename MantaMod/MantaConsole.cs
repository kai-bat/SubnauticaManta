using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MantaMod
{
    public class MantaConsole : MonoBehaviour
    {
        public void Awake()
        {
            main = this;
            DevConsole.RegisterConsoleCommand(this, "manta");
        }

        public void OnConsoleCommand_manta (NotificationCenter.Notification n)
        {
            Vector3 pos = Player.main.transform.position + (Player.main.camRoot.GetAimingTransform().forward * 30f);

            Instantiate<GameObject>(obj, pos, Quaternion.Euler(Vector3.zero));
        }

        public GameObject obj;
        public static MantaConsole main;
    }
}
