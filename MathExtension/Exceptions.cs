using System;

public class SyntaxException : Exception
{
    public SyntaxException(int position)
        : base($"Syntax error at {position}.")
    {
    }

    public SyntaxException(string what, int position)
        : base($"{what} syntax error at {position}.")
    {
    }
}

public class CoefficientSyntaxException : SyntaxException
{
    public CoefficientSyntaxException(int position)
        : base("Coefficient", position)
    {
    }
}

public class ExponentSyntaxException : SyntaxException
{
    public ExponentSyntaxException(int position)
        : base("Exponent", position)
    {
    }
}