using System;
using System.Collections.Generic;

public struct ImageData
{
    public Student currentStudent;
    public StudyingOrg organization;

    public ImageData(Student currentStudent, StudyingOrg organization)
    {
        this.currentStudent = currentStudent;
        this.organization = organization;
    }
}
public static class ProcessImageData
{
    public static double GetAveragePointInSubject(List<Test> allTests)
    {
        if (allTests == null)
        {
            return 0;
        }
        double total = GetTotal(allTests);
        return (double)(total / allTests.Count);
    }

    private static double GetTotal(List<Test> allTest)
    {
        double total = 0;
        foreach (Test t in allTest)
        {
            total += t.point;
        }
        return total;
    }

    public static double GetAvPointInMonth(DateTime date, long id, StudyingOrg org)
    {
        List<Test> allTests = org.testRepository.GetStudentsTests(id);
        if (allTests == null)
        {
            return 0;
        }
        double av = CalcPointInMonth(date, allTests);
        return av;
    }

    private static double CalcPointInMonth(DateTime date, List<Test> allTest)
    {
        int total = 0;
        double sum = 0;
        foreach (Test t in allTest)
        {
            if (t.date.Month.ToString() == date.Month.ToString())
            {
                total++;
                sum += t.point;
            }
        }
        if(total == 0)
        {
            return 0;
        }
        return (double) sum / total;
    }
}