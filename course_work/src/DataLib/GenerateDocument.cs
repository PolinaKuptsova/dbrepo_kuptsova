using System.IO.Compression;
using System.Xml.Linq;
using System.Xml;
using System.IO;

public static class GenerateDocument
{
    public static void GenerateDoc(Student student, StudyingOrg org)
    {
        const string filepath = @"./../data/Report.docx";
        student.classTeacher = org.teacherRepository.GetById(student.classTeacherId);
        ExtractZipFile(filepath, "./Report");
    
        string [] data = SetData(student);
        
        XElement root = XElement.Load("./Report/word/document.xml");
        FindAndReplace(root, data);
        ImageData d = new ImageData(student, org);
        ReplaceImages(d);
        root.Save("./Report/word/document.xml");

        CreateZipFile(@"./../data/Report2.docx", "./Report");
        Directory.Delete("./Report", true);
    }

    private static string [] SetData(Student student)
    {
        string [] data = new string [5];
        data[0] = student.Name;
        data[1] = student.age.ToString();
        data[2] = student.averagePoint.ToString();
        data[3] = student.classTeacher.Name;
        data[4] = student.ClassNumber;
        return data;
    }

    static void FindAndReplace(XElement node, string [] data)
    {
        if (node.FirstNode != null
            && node.FirstNode.NodeType == XmlNodeType.Text)
        {
            switch (node.Value)
            {
                case "{{title}}": { node.Value = "Sudent`s success report"; break; }
                case "{{name}}": { node.Value = data[0]; break; }
                case "{{age}}": { node.Value = data[1]; break; }
                case "{{avpoint}}": { node.Value = data[2]; break; }
                case "{{classTeacher}}": { node.Value = data[3]; break; }
                case "{{class}}": { node.Value = data[4]; break; }
                case "{{imagetitle1}}": { node.Value = "Graphic for students success over a year"; break; }
            }
        }

        foreach (XElement el in node.Elements())
        {
            FindAndReplace(el, data);
        }
    }

    public static void CreateZipFile(string zipPath, string startPath)
    {
        ZipFile.CreateFromDirectory(startPath, zipPath);
    }

    public static void ExtractZipFile(string zipPath, string extractPath)
    {
        ZipFile.ExtractToDirectory(zipPath, extractPath);
    }

    public static void ReplaceImages(ImageData d)
    {
        GenerateImage.GenStudentSuccessGraphic("Report/word/media/image1.png", d );
    }

}