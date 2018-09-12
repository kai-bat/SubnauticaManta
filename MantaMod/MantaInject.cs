using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
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
                bundle = AssetBundle.LoadFromFile("./QMods/Manta/mantabundle.assets");

                GameObject manta = new GameObject("mantaconsole");
                Object.DontDestroyOnLoad(manta);

                MantaConsole mant = manta.AddComponent<MantaConsole>();
                mant.obj = bundle.LoadAsset<GameObject>("MantaPrefab");

                foreach(MeshRenderer rend in mant.obj.GetComponentsInChildren<MeshRenderer>())
                {
                    rend.material.shader = Shader.Find("MarmosetUBER");
                }

                //make the model
                mantaTech = TechTypeHandler.AddTechType("MantaModel", "Miniature Manta Submarine", "A model of the concept Manta Submarine");
                MantaPrefab prefab = new MantaPrefab("MantaModel", "Submarine/Build/MantaModel", mantaTech);
                PrefabHandler.RegisterPrefab(prefab);

                TechData helper = new TechData()
                {
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient(TechType.Titanium, 2)
                    }
                };
                CraftDataHandler.SetTechData(mantaTech, helper);
                
                CraftDataHandler.AddBuildable(mantaTech);
                CraftDataHandler.AddToGroup(TechGroup.InteriorModules, TechCategory.InteriorModule, mantaTech);
            }
            catch (Exception e)
            {
                Console.WriteLine("[MantaMod] Mod Loading Failed: "+e.Message + e.StackTrace);
                StackTrace st = new StackTrace(e, true);
                int line = st.GetFrame(st.FrameCount-1).GetFileLineNumber();
                Console.WriteLine("[MantaMod] Error occurred in line: " + line);
            }
        }

        static T CopyComponent<T>(T original, ref GameObject destination) where T : Component
        {
            Type type = original.GetType();
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
        public static AssetBundle bundle;
    }
}
