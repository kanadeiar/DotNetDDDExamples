using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Kanadeiar.Common.Tests;

public class AutoMoqDataAttribute() : AutoDataAttribute(() => new Fixture()
    .Customize(new AutoMoqCustomization { ConfigureMembers = true }));