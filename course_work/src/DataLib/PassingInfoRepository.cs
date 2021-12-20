using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

public class PassingInfoRepository
{
    private MySqlConnection connection;

    public PassingInfoRepository(MySqlConnection connection)
    {
        this.connection = connection;
    }
    public long GetCount()
    {
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT COUNT(*) FROM passingInfos";
        long count = (long)command.ExecuteScalar();
        return count;
    }

    public List<PassingInfo> GetAll()
    {
        List<PassingInfo> passingInfos = new List<PassingInfo>();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM passingInfos";
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            passingInfos.Add(GetPassingInfo(reader));
        }
        reader.Close();
        return passingInfos;
    }


    public bool Update(long id, PassingInfo passingInfo)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"UPDATE passingInfos SET studentId = @studentId, testId = @testId, 
            classroomNumber = @classroomNumber WHERE id = @id";

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@studentId", passingInfo.studentId);
        command.Parameters.AddWithValue("@testId", passingInfo.testId);
        command.Parameters.AddWithValue("@classroomNumber", passingInfo.classroomNumber);

        int nChanged = command.ExecuteNonQuery();
        return nChanged == 1;

    }


    public PassingInfo GetById(long id)
    {
        PassingInfo passingInfo = new PassingInfo();
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"SELECT * FROM passingInfos WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        MySqlDataReader reader = command.ExecuteReader();
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
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText = @"DELETE FROM passingInfos WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
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

    public void Insert(PassingInfo passingInfo)
    {
        MySqlCommand command = this.connection.CreateCommand();
        command.CommandText =
        @"INSERT INTO passingInfos (studentId, testId, classroomNumber) 
            VALUES (@studentId, @testId, @classroomNumber);
            SELECT last_insert_id();";
        command.Parameters.AddWithValue("@studentId", passingInfo.studentId);
        command.Parameters.AddWithValue("@testId", passingInfo.testId);
        command.Parameters.AddWithValue("@classroomNumber", passingInfo.classroomNumber); 
        command.ExecuteScalar();
    }

    public PassingInfo GetPassingInfo(MySqlDataReader reader)
    {
        PassingInfo passingInfo = new PassingInfo(); 
        passingInfo.id = long.Parse(reader.GetString(0));
        passingInfo.studentId = long.Parse(reader.GetString(1));
        passingInfo.testId = long.Parse(reader.GetString(2));
        passingInfo.classroomNumber = reader.GetString(3);

        return passingInfo;
    }
}
