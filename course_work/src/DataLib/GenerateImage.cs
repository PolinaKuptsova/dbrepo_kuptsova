using ScottPlot;
using System;
using System.Collections.Generic;
public static class GenerateImage
{
    // статистика успішності студента
    public static void GenStudentSuccessGraphic(string filepath, ImageData imageData)
    {
        var plt = new ScottPlot.Plot(600, 600);
        string[] months = new string[]{
                "Jan", "Feb",
                "Mar", "Apr",
                "May", "Jun",
                "Jul", "Aug",
                "Sep", "Oct",
                "Nov", "Dec",
            };
        
        int year = DateTime.Now.Year;
        int count = 12;
        double[] xs = DataGen.Consecutive(count);
        double[] avPoints = new double[count];

        for (int i = 0; i < count; i++)
        {
            DateTime date = new DateTime(year, i + 1, 1);
            avPoints[i] = ProcessImageData.GetAvPointInMonth(date, imageData.currentStudent.id, imageData.organization);
        }

        plt.PlotScatter(xs, avPoints, label: "Average point of a student");

        plt.Legend();
        plt.XTicks(xs, months);
        plt.Title($"Statistics for {year}");
        plt.SaveFig(filepath);
    }

    // диаграма з тестами та їх кількістю
    public static void GenTestReportDiagram(string filepath, ImageData imageData)
    {
        var plt = new ScottPlot.Plot(600, 400);
        string [] labels = new string[]{"Music", "Maths", "Informatics", "Psycology","Arts"};
        double[] values = new double[labels.Length] ;
        int i = 0;
        foreach(string l in labels)
        {
            double v = 0;
            List<Test> tests = imageData.organization.testRepository.GetSTestInSubject(l);
            if(tests != null)
            {
                v = tests.Count;
            }
            values[i] = v;
            i++;
        }
        plt.PlotPie(values, labels, showLabels: false);
        plt.Legend();

        plt.Grid(false);
        plt.Frame(false);

        plt.SaveFig(filepath);
    }

    // тести  та середня оцінка студентів 
    public static void ImageOfAvaregeTestPoint(string filepath, ImageData imageData)
    {
        var plt = new ScottPlot.Plot(600, 400);
        string[] labels = new string[] { "Music", "Maths", "Informatics", "Psycology", "Arts" };

        int barCount = labels.Length;

        double[] xs = DataGen.Consecutive(barCount);
        double[] ys = DataGen.RandomNormal(12, barCount, 20, 5);
        double[] yError = DataGen.RandomNormal(12, barCount, 5, 2);

        plt.PlotBar(xs, ys, yError);
        plt.XTicks(xs, labels);
        plt.SaveFig(filepath);
    }

}