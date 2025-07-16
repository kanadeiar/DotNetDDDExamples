namespace Kanadeiar.Common.Functionals;

public abstract record ExampleCode
{
    public Result<int> CreateWithOption(ExampleCode code)
    {
        return code.Require(code != null) switch
        {
            FirstCode => Result.Ok(1),
            SecondCode => Result.Ok(2),
            _ => Result.Fail<int>("none"),
        };
    }

    public string Create(ExampleCode code)
    {
        return code.RequireNot(code is null) switch
        {
            FirstCode => "first",
            SecondCode { name: "one" } => "number one",
            SecondCode(var name) => "second " + name,
            _ => "none",
        };
    }

    public Result ExampleAction(bool value)
    {
        return value
            ? Result.Ok()
            : Result.Fail("Ничего нет");
    }

    public static ExampleCode First => new FirstCode();
    public static ExampleCode Second(string name) => new SecondCode(name);

    private record FirstCode : ExampleCode;
    private record SecondCode(string name) : ExampleCode;
}