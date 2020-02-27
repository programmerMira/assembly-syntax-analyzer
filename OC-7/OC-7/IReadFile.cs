using System;
using System.Collections.Generic;

namespace OC_7
{
    public interface IReadFile
    {
        List<String> openFileToRead(String path);
        Dictionary<Int32, String> checkDataForExs(List<String> data);
        Boolean fileIsOpened { get; }
        Boolean fileIsRead { get; }
        Boolean fileIsChecked { get; }
        Boolean fileIsCorrect { get; }
    }
}
