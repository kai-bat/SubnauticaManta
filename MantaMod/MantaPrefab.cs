using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMLHelper;
using SMLHelper.V2.Assets;
using SMLHelper.V2.MonoBehaviours;
using SMLHelper.V2.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MantaMod
{
    class MantaPrefab : ModPrefab
    {
        public MantaPrefab(string classId, string prefabFileName, TechType techType = TechType.None) : base(classId, prefabFileName, techType)
        {
            ClassID = classId;
            PrefabFileName = prefabFileName;
            TechType = techType;
        }

        public override GameObject GetGameObject()
        {
            GameObject model = null;
            try
            {
                model = Utils.SpawnPrefabAt(MantaInject.bundle.LoadAsset<GameObject>("MantaPrefab.prefab"), null, Vector3.one * 100000f);

                foreach (MeshRenderer rend in model.GetComponentsInChildren<MeshRenderer>())
                {
                    rend.material.shader = Shader.Find("MarmosetUBER");
                }
                model.transform.localScale *= 0.1f;

                Constructable cons = model.AddComponent<Constructable>();
                cons.allowedInBase = true;
                cons.allowedInSub = true;
                cons.allowedOnWall = false;
                cons.allowedOnGround = true;
                cons.allowedOnConstructables = true;
                cons.allowedOnCeiling = false;
                cons.allowedOutside = true;
                cons.deconstructionAllowed = true;
                cons.model = model.transform.GetChild(0).gameObject;
                cons.techType = TechType;

                model.AddComponent<ConstructableBounds>();
                foreach (MeshCollider col in model.GetComponentsInChildren<MeshCollider>())
                {
                    col.convex = true;
                    col.inflateMesh = true;
                }

                BoxCollider box = model.AddComponent<BoxCollider>();
                box.size = new Vector3(5, 2, 12);
                box.center = new Vector3(0, 0.5f, 2f);
                model.AddComponent<TechTag>().type = MantaInject.mantaTech;
                Fixer fixer = model.AddComponent<Fixer>();
                fixer.ClassId = ClassID;
                fixer.techType = TechType;

                Utility.AddBasicComponents(ref model, ClassID);
            }
            catch (Exception e)
            {
                Console.WriteLine("[MantaMod] Manta Model loading failed: " + e.Message + e.StackTrace);
            }
            return model;
        }
    }
}
