using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class VolumeButtonGameLevelTest
{
    [UnityTest]
    public IEnumerator VolumeUpTest() {
        GameObject go = new();
        VolumeButtonGame game = go.AddComponent<VolumeButtonGame>();

        GameObject paddingObj = new();

        game.progress = paddingObj;

        yield return new WaitForSeconds(0.1f);
        
        Assert.IsTrue(game.alphaTarget == 0);
        
        game.IncrementAlphaUp();
        Assert.IsTrue(game.alphaTarget == game.incrementValue);
    }

    [UnityTest]
    public IEnumerator VolumeDownTest() {
        GameObject go = new();
        VolumeButtonGame game = go.AddComponent<VolumeButtonGame>();

        GameObject paddingObj = new();

        game.progress = paddingObj;

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(game.alphaTarget == 0);

        game.IncrementAlphaUp();
        game.IncrementAlphaDown();

        Assert.IsTrue(game.alphaTarget == 0);
    }

    [UnityTest]
    public IEnumerator VolumeDownTestNotNegative() {
        GameObject go = new();
        VolumeButtonGame game = go.AddComponent<VolumeButtonGame>();

        GameObject paddingObj = new();

        game.progress = paddingObj;

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(game.alphaTarget == 0);

        game.IncrementAlphaDown();

        Assert.IsTrue(game.alphaTarget == 0);
    }

    [UnityTest]
    public IEnumerator CanWin() {
        GameObject go = new();
        VolumeButtonGame game = go.AddComponent<VolumeButtonGame>();

        GameObject paddingObj = new();

        game.progress = paddingObj;

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(game.alphaTarget == 0);
        Assert.IsFalse(game.win);

        for (int i = 0; i < Mathf.Ceil(game.winTarget / game.incrementValue) + 1; i++) {
            game.IncrementAlphaUp();
        }

        Assert.IsTrue(game.win);
    }
}
