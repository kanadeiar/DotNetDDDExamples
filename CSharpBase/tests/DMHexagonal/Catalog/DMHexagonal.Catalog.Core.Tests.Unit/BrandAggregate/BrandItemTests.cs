using AutoFixture.Xunit2;
using DMHexagonal.Catalog.Core.BrandAggregate;
using DMHexagonal.Catalog.Core.BrandAggregate.Events;
using DMHexagonal.Catalog.Core.BrandAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;
using FluentAssertions;
using Kanadeiar.Common.Tests;

namespace DMHexagonal.Catalog.Core.Tests.Unit.BrandAggregate;

public class BrandItemTests
{
    [Theory(DisplayName = "Проверка создания нового бренда товаров")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Бренд")]
    public void TestCreate(Guid id, string name)
    {
        var actual = BrandItem.Create(new BrandId(id), new BrandNameValue(name));

        var entry = actual.Entry();
        entry.Id.Should().Be(id);
        entry.Name.Should().Be(name);
        var events = actual.Changes();
        events.Count().Should().Be(1);
        (events.Last() as BrandCreated).Id.Value.Should().Be(id);
        (events.Last() as BrandCreated).Name.Value.Should().Be(name);
    }

    [Theory(DisplayName = "Проверка нарушения инвариантов брендов товаров")]
    [InlineAutoMoqData("00000000-0000-0000-0000-000000000000", "Тест")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Т")]
    public void TestCreate_WhenInvariantError(Guid id, string name)
    {
        var act = () =>
        {
            _ = BrandItem.Create(new BrandId(id), new BrandNameValue(name));
        };

        act.Should().Throw<ApplicationException>();
    }

    [Theory(DisplayName = "Проверка возможности изменения названия элемента")]
    [AutoMoqData]
    public void TestRename(BrandEntry entry)
    {
        var expected = new BrandNameValue("Новое имя");
        var sut = BrandItem.Restore(entry);

        sut.Rename(expected);

        var actualEntry = sut.Entry();
        actualEntry.Name.Should().Be(expected.Value);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as BrandRenamed).Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности удаления элемента")]
    [AutoData]
    public void TestDeleteItem(BrandEntry entry)
    {
        var expected = Guid.Parse("5C60F693-BEF5-E011-A485-80EE7300C695");
        entry = new BrandEntry { Id = expected, Name = entry.Name, IsDeleted = false };
        var sut = BrandItem.Restore(entry);

        sut.Delete();

        var actualEntry = sut.Entry();
        actualEntry.IsDeleted.Should().Be(true);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as BrandDeleted).Id.Value.Should().Be(expected);
    }
}