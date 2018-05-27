using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SMLHelper;
using SMLHelper.Patchers;
using Harmony;
using System.Reflection;
using UWE;
using Object = UnityEngine.Object;

namespace MantaMod
{
    public static class MantaInject
    {
        public static void Inject()
        {
            try
            {
                AssetBundle bundle = AssetBundle.LoadFromFile("./QMods/Manta/mantabundle");

                GameObject manta = new GameObject("mantaconsole");
                GameObject.DontDestroyOnLoad(manta);

                MantaConsole mant = manta.AddComponent<MantaConsole>();

                mant.obj = bundle.LoadAsset<GameObject>("MantaPrefab");

                foreach(MeshRenderer rend in mant.obj.GetComponentsInChildren<MeshRenderer>())
                {
                    rend.material.shader = Shader.Find("MarmosetUBER");
                }

                //make the model
                mantaTech = TechTypePatcher.AddTechType("MantaModel", "Miniature Manta Submarine", "A model of the concept Manta Sub");

                GameObject model = bundle.LoadAsset<GameObject>("MantaPrefab");

                foreach (MeshRenderer rend in model.GetComponentsInChildren<MeshRenderer>())
                {
                    rend.material.shader = Shader.Find("MarmosetUBER");
                }
                Utility.AddBasicComponents(ref model, "MantaModel");
                model.transform.localScale *= 0.05f;

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
                cons.techType = mantaTech;

                model.AddComponent<ConstructableBounds>();
                foreach(MeshCollider col in model.GetComponentsInChildren<MeshCollider>())
                {
                    col.convex = true;
                    col.inflateMesh = true;
                }

                model.AddComponent<BoxCollider>();
                model.AddComponent<Rigidbody>();
                model.AddComponent<TechTag>().type = mantaTech;

                CustomPrefabHandler.customPrefabs.Add(new CustomPrefab("MantaModel", "Submarine/Build/MantaModel", model, mantaTech));

                TechDataHelper helper = new TechDataHelper()
                {
                    _ingredients = new List<IngredientHelper>()
                    {
                        new IngredientHelper(TechType.Titanium, 1)
                    },
                    _techType = mantaTech,
                    _craftAmount = 1
                };
                CraftDataPatcher.customTechData.Add(mantaTech, helper);

                CraftDataPatcher.customBuildables.Add(mantaTech);
                CraftDataPatcher.AddToCustomGroup(TechGroup.InteriorModules, TechCategory.InteriorModule, mantaTech);
            }
            catch (Exception e)
            {
                Console.WriteLine("[MantaMod] Mod Loading Failed: " + e.StackTrace);
            }
        }

        static T CopyComponent<T>(T original, ref GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst as T;
        }

        public static TechType mantaTech;
    }
}
