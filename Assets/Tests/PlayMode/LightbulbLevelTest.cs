using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LightbulbLevelTest
{
    [UnityTest]
    public IEnumerator ClickIsWin() {
        GameObject go = new();
        LightbulbGame game = go.AddComponent<LightbulbGame>();

        yield return new WaitForSeconds(0.1f);

        Assert.IsFalse(game.win);
        game.onClick();
        Assert.IsTrue(game.win);
    }
}
