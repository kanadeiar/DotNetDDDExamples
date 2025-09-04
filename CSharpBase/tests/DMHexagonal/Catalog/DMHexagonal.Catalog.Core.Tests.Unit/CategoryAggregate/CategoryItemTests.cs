using AutoFixture.Xunit2;
using DMHexagonal.Catalog.Core.CategoryAggregate;
using DMHexagonal.Catalog.Core.CategoryAggregate.Events;
using DMHexagonal.Catalog.Core.CategoryAggregate.Values;
using DMHexagonal.Catalog.Core.Entries;
using FluentAssertions;
using Kanadeiar.Common.Tests;

namespace DMHexagonal.Catalog.Core.Tests.Unit.CategoryAggregate;

public class CategoryItemTests
{
    [Theory(DisplayName = "Проверка создания новой категории товаров")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Категория")]
    public void TestCreate(Guid id, string name)
    {
        var actual = CategoryItem.Create(new CategoryId(id), new CategoryNameValue(name));

        var entry = actual.Entry();
        entry.Id.Should().Be(id);
        entry.Name.Should().Be(name);
        var events = actual.Changes();
        events.Count().Should().Be(1);
        (events.Last() as CategoryCreated).Id.Value.Should().Be(id);
        (events.Last() as CategoryCreated).Name.Value.Should().Be(name);
    }

    [Theory(DisplayName = "Проверка нарушения инвариантов категории товаров")]
    [InlineAutoMoqData("00000000-0000-0000-0000-000000000000", "Тест")]
    [InlineAutoMoqData("5C60F693-BEF5-E011-A485-80EE7300C695", "Т")]
    public void TestCreate_WhenInvariantError(Guid id, string name)
    {
        var act = () =>
        {
            _ = CategoryItem.Create(new CategoryId(id), new CategoryNameValue(name));
        };

        act.Should().Throw<ApplicationException>();
    }

    [Theory(DisplayName = "Проверка возможности изменения названия элемента")]
    [AutoMoqData]
    public void TestRename(CategoryEntry entry)
    {
        var expected = new CategoryNameValue("Новое имя");
        var sut = CategoryItem.Restore(entry);

        sut.Rename(expected);

        var actualEntry = sut.Entry();
        actualEntry.Name.Should().Be(expected.Value);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as CategoryRenamed).Name.Should().Be(expected);
    }

    [Theory(DisplayName = "Проверка возможности удаления элемента")]
    [AutoData]
    public void TestDeleteItem(CategoryEntry entry)
    {
        var expected = Guid.Parse("5C60F693-BEF5-E011-A485-80EE7300C695");
        entry = new CategoryEntry { Id = expected, Name = entry.Name, IsDeleted = false };
        var sut = CategoryItem.Restore(entry);

        sut.Delete();

        var actualEntry = sut.Entry();
        actualEntry.IsDeleted.Should().Be(true);
        var events = sut.Changes();
        events.Count().Should().Be(1);
        (events.Last() as CategoryDeleted).Id.Value.Should().Be(expected);
    }
}