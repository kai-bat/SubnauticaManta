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
        }
        
        void RegisterCommands(Scene scene, LoadSceneMode mode)
        {
            DevConsole.RegisterConsoleCommand(this, "manta");
            Console.WriteLine("[MantaMod] Created and registered new 'manta' command object");

            obj = Utils.SpawnPrefabAt(MantaInject.bundle.LoadAsset<GameObject>("MantaPrefab.prefab"), null, Vector3.one * 10000f);

            var largeworld = obj.AddOrGetComponent<LargeWorldEntity>();

            largeworld.cellLevel = LargeWorldEntity.CellLevel.VeryFar;

            var rb = obj.AddOrGetComponent<Rigidbody>();
            rb.angularDrag = 1f;
            rb.mass = 100000f;
            obj.transform.localScale *= 3f;

            var sky = obj.AddOrGetComponent<SkyApplier>();
            sky.anchorSky = Skies.Auto;
            sky.renderers = GetComponentsInChildren<Renderer>();

            WorldForces forces = obj.AddOrGetComponent<WorldForces>();
            forces.underwaterDrag = 1f;
            forces.underwaterGravity = -9.8f;
        }
        public GameObject obj;
        public static MantaConsole main;
    }
}
