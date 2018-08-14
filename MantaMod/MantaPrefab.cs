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

namespace MantaMod
{
    class MantaPrefab : ModPrefab
    {
        public MantaPrefab(string classId, string prefabFileName, TechType techType = TechType.None) : base(classId, prefabFileName, techType)
        {
            ClassID = classId;
            PrefabFileName = prefabFileName;
            TechType = techType;

            GameObject model = MantaInject.bundle.LoadAsset<GameObject>("MantaPrefab");

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

            Utility.AddBasicComponents(ref model, "MantaModel");
            BoxCollider box = model.AddComponent<BoxCollider>();
            box.size = new Vector3(5, 5, 10);
            model.AddComponent<TechTag>().type = MantaInject.mantaTech;
            Fixer fixer = model.AddComponent<Fixer>();
            fixer.ClassId = ClassID;
            fixer.techType = TechType;

            this.model = model;
        }

        public override GameObject GetGameObject()
        {
            return model;
        }

        public GameObject model;
    }
}
