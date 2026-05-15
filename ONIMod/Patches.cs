using HarmonyLib;
using Klei.AI;
using System.Reflection;

namespace RemoveScaleGrowthPenalty
{
	public class Patches
	{
		[HarmonyPatch(typeof(HappinessMonitor))]
		[HarmonyPatch("InitializeStates")]
		public class HappinessMonitor_InitializeStates_Patch
        {
			public static void Postfix(HappinessMonitor __instance)
			{
				//Debug.Log("I execute after HappinessMonitor_InitializeStates!");
                // 通过反射获取私有字段 miserableTameEffect 和 miserableWildEffect
                var tameField = typeof(HappinessMonitor).GetField("miserableTameEffect", BindingFlags.NonPublic | BindingFlags.Instance);
                var wildField = typeof(HappinessMonitor).GetField("miserableWildEffect", BindingFlags.NonPublic | BindingFlags.Instance);

                if (tameField == null || wildField == null)
                {
                    Debug.LogWarning("unknow tame and wild");
                    return;
                }

                var miserableTameEffect = tameField.GetValue(__instance) as Effect;
                var miserableWildEffect = wildField.GetValue(__instance) as Effect;

                foreach (var item in miserableTameEffect.SelfModifiers)
                {
                    //Debug.Log(item.AttributeId);
                    //Debug.Log(item.GetName());
                    if(item.AttributeId == Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id)
                    {
                        //Debug.Log("before change:");
                        //Debug.Log("item.value:" + item.Value);
                        item.SetValue(0f);
                        //Debug.Log("after change:");
                        //Debug.Log("item.value:" + item.Value);
                        miserableTameEffect.SelfModifiers.Remove(item);
                        break;
                    }
                }
                //tameField.SetValue(__instance, miserableTameEffect);
                foreach (var item in miserableWildEffect.SelfModifiers)
                {
                    //Debug.Log(item.AttributeId);
                    //Debug.Log(item.GetName());
                    if (item.AttributeId == Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id)
                    {
                        //Debug.Log("before change:");
                        //Debug.Log("item.value:" + item.Value);
                        item.SetValue(0f);
                        //Debug.Log("after change:");
                        //Debug.Log("item.value:" + item.Value);
                        miserableTameEffect.SelfModifiers.Remove(item);
                        break;
                    }
                }
                wildField.SetValue(__instance, miserableWildEffect);
                Debug.Log("execute successful");
            }
		}
	}
}
