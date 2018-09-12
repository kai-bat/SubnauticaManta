using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

namespace MantaMod
{
    public class MantaConsole : MonoBehaviour
    {
        public void Awake()
        {
            main = this;
            SceneManager.sceneLoaded += RegisterCommands;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= RegisterCommands;
        }

        public void OnConsoleCommand_manta(NotificationCenter.Notification n)
        {
            Console.WriteLine("[MantaMod] manta command triggered, creating new manta gameobject");
            ErrorMessage.AddMessage("[MantaMod] Creating sub w/ command");
            Vector3 pos = Player.main.transform.position + (Player.main.camRoot.GetAimingTransform().forward * 30f);

            GameObject clone = Utils.SpawnPrefabAt(obj, null, pos);

            SMLHelper.Utility.AddBasicComponents(ref clone, "MantaSubmarine");

            clone.GetComponent<Rigidbody>().angularDrag = 1f;
            clone.GetComponent<Rigidbody>().mass = 5000f;

            WorldForces forces = clone.GetComponent<WorldForces>();
            if(!forces)
            {
                forces = clone.AddComponent<WorldForces>();
            }
            forces.underwaterDrag = 1f;
            forces.underwaterGravity = -9.8f;

            MeshCollider collider = clone.transform.GetChild(0).GetComponent<MeshCollider>();
            if(!collider)
            {
                collider = clone.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            }
            collider.convex = true;
            collider.inflateMesh = true;
        }
        
        void RegisterCommands(Scene scene, LoadSceneMode mode)
        {
            DevConsole.RegisterConsoleCommand(this, "manta");
            Console.WriteLine("[MantaMod] Created and registered new 'manta' command object");
        }
        public GameObject obj;
        public static MantaConsole main;
    }
}
