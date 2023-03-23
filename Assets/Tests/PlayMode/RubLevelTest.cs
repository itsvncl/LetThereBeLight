using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class RubLevelTest
{
    [UnityTest]
    public IEnumerator BeginDirection() {
        GameObject gameGo = new();
        RubGame game = gameGo.AddComponent<RubGame>();

        yield return new WaitForSeconds(0.1f);

        TouchData init = new TouchData(Vector3.zero, 0, gameGo, 1);
        game.OnTouchBegin(init);

        yield return new WaitForSeconds(0.1f);
        
        TouchData moveUp = new TouchData(new Vector3(0.0f, 1.0f, 0.0f), 0, gameGo, 1);
        game.OnDrag(moveUp);
        Assert.IsTrue(game.lightLevel > 0.0f);
    }
    [UnityTest]
    public IEnumerator CorrectRub() {
        GameObject gameGo = new();
        RubGame game = gameGo.AddComponent<RubGame>();

        yield return new WaitForSeconds(0.1f);

        TouchData init = new TouchData(Vector3.zero, 0, gameGo, 1);
        game.OnTouchBegin(init);

        yield return new WaitForSeconds(0.1f);

        TouchData moveUp = new TouchData(new Vector3(0.0f, 1.0f, 0.0f), 0, gameGo, 1);
        game.OnDrag(moveUp);
        game.OnDrag(init);
        game.OnDrag(moveUp);
        game.OnDrag(init);

        Assert.IsTrue(game.lightLevel == 4* game.increaseSpeed);
    }

    [UnityTest]
    public IEnumerator IncorrectRub() {
        GameObject gameGo = new();
        RubGame game = gameGo.AddComponent<RubGame>();

        yield return new WaitForSeconds(0.1f);

        TouchData init = new TouchData(Vector3.zero, 0, gameGo, 1);
        game.OnTouchBegin(init);

        yield return new WaitForSeconds(0.1f);

        TouchData moveUp = new TouchData(new Vector3(0.0f, 1.0f, 0.0f), 0, gameGo, 1);
        game.OnDrag(moveUp);
        game.OnDrag(moveUp);
        game.OnDrag(moveUp);
        game.OnDrag(moveUp);

        Assert.IsTrue(game.lightLevel == game.increaseSpeed);
    }
}
