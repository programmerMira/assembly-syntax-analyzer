using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace OC_7.Tests
{
    public class ReadFileTesting
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FileFoundAndOpened()
        {
            IReadFile Iread = new ReadFile();
            string path = "D:\\VisualStudio2019\\test.asm";
            Iread.openFileToRead(path);
            Assert.IsTrue(Iread.fileIsOpened);
        }

        //[Test]
        //public void FileNotFound()
        //{
        //    IReadFile Iread = new ReadFile();
        //    string path = "D:\\VisualStudio2019\\est.txt";
        //    Iread.openFileToRead(path);
        //    Assert.Throws<FileNotFoundException>(()=> throw new FileNotFoundException(path));
        //}

        [Test]
        public void CanReadData()
        {
            IReadFile Iread = new ReadFile();
            string path = "D:\\VisualStudio2019\\test.asm";
            Iread.openFileToRead(path);
            Assert.IsTrue(Iread.fileIsRead);
        }

        //[Test]
        //public void ReadCorrectData()
        //{
        //    IReadFile Iread = new ReadFile();
        //    string path = "D:\\VisualStudio2019\\text.asm";
        //    List<String> tmp_data = Iread.openFileToRead(path);
        //    List<String> expected_data = new List<String>();
        //    expected_data.Add("temp db 13");
        //    expected_data.Add("right dw 14h");
        //    expected_data.Add("add ax, 13");
        //    Assert.AreEqual(expected_data, tmp_data);
        //}

        [Test]
        public void FileChecked()
        {
            IReadFile Iread = new ReadFile();
            string path = "D:\\VisualStudio2019\\text.asm";
            List<String> tmp_data = Iread.openFileToRead(path);
            Dictionary<Int32, String> exceptions = Iread.checkDataForExs(tmp_data);
            Assert.IsTrue(Iread.fileIsChecked);
        }

        [Test]
        public void FileCorrect()
        {
            IReadFile Iread = new ReadFile();
            string path = "D:\\VisualStudio2019\\text.asm";
            List<String> tmp_data = Iread.openFileToRead(path);
            Dictionary<Int32, String> exceptions = Iread.checkDataForExs(tmp_data);
            Assert.IsEmpty(exceptions);
            Assert.IsTrue(Iread.fileIsCorrect);
        }

        [Test]
        public void LineCheck()
        {
            ReadFile reader = new ReadFile();
            var res = reader.checkCommand("test db 0101b");
            Assert.IsTrue(res);
        }
    }
}