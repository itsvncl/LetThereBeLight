using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LightSwitchLevelTest
{
    [UnityTest]
    public IEnumerator WinTriggerTest()
    {
        GameObject triggerObject = new();
        triggerObject.AddComponent<BoxCollider2D>();
        WinTrigger trigger = triggerObject.AddComponent<WinTrigger>();
        
        Assert.IsFalse(trigger.win);

        GameObject obj = new();
        obj.AddComponent<BoxCollider2D>();
        obj.AddComponent<Rigidbody2D>();
        obj.transform.position = new Vector3(100, 100, 100);
        
        trigger.SetWinObjects(new GameObject[] { obj });
        triggerObject.transform.position = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        obj.transform.position = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(trigger.win);
    }
}
