using UnityEngine;
using NUnit.Framework;
using NSubstitute;

public class AutoTest {

    
	[Test]
	public void HealthDeathTrue()
	{
        var ih = Substitute.For<IHealth>();
        HealthController hc = new HealthController();
        hc.SetHealth(ih);

        ih.ClearReceivedCalls();
        hc.DecreaseHealth(true, 10, 5, 0, 0, 0, 0);
        ih.Received().SetHealth(Arg.Is<float>(x => x == 5));
	}

    [Test]
    public void HealthDeathFalse()
    {
        var ih = Substitute.For<IHealth>();
        HealthController hc = new HealthController();
        hc.SetHealth(ih);

        ih.ClearReceivedCalls();
        hc.DecreaseHealth(false, 10, 5, 0, 0, 0, 0);
        ih.DidNotReceiveWithAnyArgs().SetHealth(0);
    }

    [Test]
    public void HealthMaxTrue()
    {
        var ih = Substitute.For<IHealth>();
        HealthController hc = new HealthController();
        hc.SetHealth(ih);

        ih.ClearReceivedCalls();
        hc.IncreaseHealth(10, 15, 20);
        ih.Received().SetHealth(Arg.Is<float>(x => x == 20));
    }

    [Test]
    public void HealthMaxFalse()
    {
        var ih = Substitute.For<IHealth>();
        HealthController hc = new HealthController();
        hc.SetHealth(ih);

        ih.ClearReceivedCalls();
        hc.IncreaseHealth(10, 15, 30);
        ih.Received().SetHealth(Arg.Is<float>(x => x == 25));
    }
    
}
