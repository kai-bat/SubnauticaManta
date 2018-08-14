using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MantaMod
{
    public class MantaConsole : MonoBehaviour
    {
        public void Awake()
        {
            main = this;
            DevConsole.RegisterConsoleCommand(this, "manta");
            Console.WriteLine("[MantaMod] Created and registered new 'manta' command object");
        }

        public void OnConsoleCommand_manta(NotificationCenter.Notification n)
        {
            Console.WriteLine("[MantaMod] manta command triggered, creating new manta gameobject");
            Vector3 pos = Player.main.transform.position + (Player.main.camRoot.GetAimingTransform().forward * 30f);

            Instantiate(obj, pos, Quaternion.identity);
        }

        public GameObject obj;
        public static MantaConsole main;
    }
}
