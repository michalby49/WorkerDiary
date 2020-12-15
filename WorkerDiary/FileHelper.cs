using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WorkerDiary
{
    class FileHelper<T> where T : new()
    {
        private string _filePath;

        public FileHelper(string filePath)
        {
            _filePath = filePath;
        }
        public void SerializeToFile(T students)
        {
            var serialize = new XmlSerializer(typeof(T));
            using (var streamWriter = new StreamWriter(_filePath))
            {
                serialize.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public T DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
                return new T();

            var serialize = new XmlSerializer(typeof(List<Employee>));
            using (var streamReader = new StreamReader(_filePath))
            {
                var employees = (T)serialize.Deserialize(streamReader);
                streamReader.Close();
                return employees;
            }
        }
    }
}



