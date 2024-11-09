

using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ClockControllerTests
{
    private GameObject clockControllerGameObject;
    private ClockController clockController;
    private GameObject clockKeyMock;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject for the ClockController
        clockControllerGameObject = new GameObject();
        clockController = clockControllerGameObject.AddComponent<ClockController>();

        // Instantiate and assign the clockKey GameObject
        clockKeyMock = new GameObject("ClockKey");
        clockController.ClockKey = clockKeyMock;
    }

    [Test]
    public void ActivateClockKey_ActivatesClockKey()
    {
        // Act
        clockController.ActivateClockKey();

        // Assert
        Assert.IsTrue(clockController.ClockKey.activeSelf, "Clock key should be active after calling ActivateClockKey.");
    }

    //[Test]
    //public void DeactivateClockKey_DeactivatesClockKey()
    //{
    //    // First, ensure the clock key is active
    //    clockController.ClockKey.SetActive(true);

    //    // Act
    //    clockController.DeactivateClockKey();

    //    // Assert
    //    Assert.IsFalse(clockController.ClockKey.activeSelf, "Clock key should be inactive after calling DeactivateClockKey.");
    //}

    [TearDown]
    public void TearDown()
    {
        if (clockControllerGameObject != null)
            Object.DestroyImmediate(clockControllerGameObject);
        // No need to explicitly destroy clockKeyMock as it will be destroyed with clockControllerGameObject
    }
}