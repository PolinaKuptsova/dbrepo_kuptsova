using System;
public class MyExceptionArguments
{
    private string locationOfError;
    private DateTime errorTime;

    public MyExceptionArguments(string locationOfError, DateTime errorTime)
    {
        this.locationOfError = locationOfError;
        this.errorTime = errorTime;
    }
}
