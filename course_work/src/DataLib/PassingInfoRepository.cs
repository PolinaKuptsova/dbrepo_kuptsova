using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class PassingInfoRepository
{
    private SqliteConnection connection;

    public PassingInfoRepository(SqliteConnection connection)
    {
        this.connection = connection;

    }

    public List<PassingInfo> GetAll()
    {
        List<PassingInfo> passingInfos = new List<PassingInfo>();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM passingInfos";
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            passingInfos.Add(GetPassingInfo(reader));
        }
        reader.Close();
        return passingInfos;
    }


    public bool Update(long id, PassingInfo passingInfo)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE passingInfos SET studentId = $studentId, testId = $testId, 
            classroomNumber = $classroomNumber WHERE id = $id";

        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$studentId", passingInfo.studentId);
        command.Parameters.AddWithValue("$testId", passingInfo.testId);
        command.Parameters.AddWithValue("$classroomNumber", passingInfo.classroomNumber);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public PassingInfo GetById(long id)
    {
        PassingInfo passingInfo = new PassingInfo();
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM passingInfos WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            passingInfo = GetPassingInfo(reader);
        }
        else
        {
            return null;
        }
        reader.Close();
        return passingInfo;
    }

    public int DeleteById(long id)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM passingInfos WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        int nChanged = command.ExecuteNonQuery();
        if (nChanged == 0)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public int Insert(PassingInfo passingInfo)
    {
        SqliteCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO customers (studentId, testId, classroomNumber) 
            VALUES ($studentId, $testId, $classroomNumber);
            
            SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$studentId", passingInfo.studentId);
        command.Parameters.AddWithValue("$testId", passingInfo.testId);
        command.Parameters.AddWithValue("$classroomNumber", passingInfo.classroomNumber); 
        long newId = (long)command.ExecuteScalar();
        if (newId == 0)
        {
            return 0;
        }
        else
        {
            return (int)newId; ;
        }

    }

    public PassingInfo GetPassingInfo(SqliteDataReader reader)
    {
        PassingInfo passingInfo = new PassingInfo(); 
        passingInfo.id = long.Parse(reader.GetString(0));
        passingInfo.studentId = long.Parse(reader.GetString(1));
        passingInfo.testId = long.Parse(reader.GetString(2));
        passingInfo.classroomNumber = int.Parse(reader.GetString(3));

        return passingInfo;
    }
}
