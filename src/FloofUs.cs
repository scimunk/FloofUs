using System;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using InnerNet;
using Reactor;
using Reactor.Extensions;
using Reactor.Networking;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnhollowerBaseLib.Attributes;
using UnityEngine;



namespace FloofUs
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class FloofUsPlugin : BasePlugin
    {
        public const string Id = "com.scimunk.floofus";

        public static Sprite Kill;

    
		

        public Harmony Harmony { get; } = new Harmony(Id);


        public override void Load()
        {
            RegisterInIl2CppAttribute.Register();
            RegisterCustomRpcAttribute.Register(this);

            var gameObject = new GameObject(nameof(ReactorPlugin)).DontDestroy();
            gameObject.AddComponent<FloofUsComponent>().Plugin = this;

            Kill = CreateSprite("FloofUs.Resources.Kill.png");

            RolesManager.InitializeRoles();


            Harmony.PatchAll();
        }

        [RegisterInIl2Cpp]
        public class FloofUsComponent : MonoBehaviour
        {
            [HideFromIl2Cpp]
            public FloofUsPlugin Plugin { get; internal set; }

            public FloofUsComponent(IntPtr ptr) : base(ptr)
            {
            }

            private void Update()
            {
                
            }
        }

        
        public static Sprite CreateSprite(string name, bool hat=false)
        {
	        var pixelsPerUnit = hat ? 225f : 100f;
	        var pivot = hat ? new Vector2(0.5f, 0.8f) : new Vector2(0.5f, 0.5f);
			
			var assembly = Assembly.GetExecutingAssembly();
			var tex = GUIExtensions.CreateEmptyTexture();
			var imageStream = assembly.GetManifestResourceStream(name);
			var img = imageStream.ReadFully();
			LoadImage(tex, img, true);
			tex.DontDestroy();
			var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, (float) tex.width, (float) tex.height), pivot, pixelsPerUnit);
			sprite.DontDestroy();
			return sprite;
		}
		
		private static void LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
		{
			_iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");
			var il2CPPArray = (Il2CppStructArray<byte>) data;
			_iCallLoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, markNonReadable);
		}

		private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);

		private static DLoadImage _iCallLoadImage;
		
		
    }
}
