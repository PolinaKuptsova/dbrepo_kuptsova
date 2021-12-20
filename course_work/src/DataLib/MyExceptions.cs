using System;
public class MyException : Exception
{
    public MyExceptionArguments exceptionArgs;
    public MyException(string message, MyExceptionArguments args) : base(message)
    {
        this.exceptionArgs = args;
    }

}